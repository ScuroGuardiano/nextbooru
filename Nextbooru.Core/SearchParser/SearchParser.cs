using System.Text.RegularExpressions;

namespace Nextbooru.Core.SearchParser;

public class SearchParser
{
    private readonly SearchParserOptions options = new SearchParserOptions();
    private string? currentOperator;
    private List<Node> currentNodes = [];
    
    public SearchParser()
    {
    }

    public SearchParser(Action<SearchParserOptions> configurationAction)
    {
        configurationAction(this.options);
    }
    
    /// <summary>
    /// Will parse given string input and return AST.
    /// </summary>
    /// <param name="code"></param>
    /// <returns></returns>
    public Node Parse(string code)
    {
        if (string.IsNullOrEmpty(code))
        {
            return new EmptyNode();
        }

        return NextNode(code);
    }

    private Node NextNode(ReadOnlySpan<char> code)
    {
        // if (code[0] == '-')
        // {
        //     // Many `-` at the beginning will translate to negation
        //     var i = 1;
        //     while (code[i] == '-') i++;
        //     
        //     return new NegationNode(NextNode(code[i..]));
        // }

        List<KeyValuePair<Node, string>> operatorTagList = [];

        while (!code.IsEmpty)
        {
            // Get next tag
            var (tagNode, tagLength) = NextTagNode(code);
            code = code[tagLength..];

            // Get next operator
            var (op, opLength) = NextLogicalOperator(code);
            code = code[opLength..];
            operatorTagList.Add(new(tagNode, op));
        }

        return BuildNodeFromNodeOperatorPairList(operatorTagList);

        throw new NotImplementedException();
    }

    private (Node node, int length) NextTagNode(ReadOnlySpan<char> code)
    {
        // Read until first white character
        var enumerator = whitespaceRegex.EnumerateMatches(code);
        if (enumerator.MoveNext())
        {
            code = code[..enumerator.Current.Index];
        }

        return (new TagNode(code.ToString()), code.Length);
    }

    private (string op, int length) NextLogicalOperator(ReadOnlySpan<char> code)
    {
        var orMatchEnumer = logicalOperators["or"].EnumerateMatches(code);
        if (orMatchEnumer.MoveNext())
        {
            return ("or", orMatchEnumer.Current.Length);
        }

        var andMatchEnumer = logicalOperators["and"].EnumerateMatches(code);
        if (andMatchEnumer.MoveNext())
        {
            return ("and", andMatchEnumer.Current.Length);
        }

        return ("eof", 0);
    }

    private Node BuildNodeFromNodeOperatorPairList(IList<KeyValuePair<Node, string>> list)
    {
        switch (list.Count)
        {
            case 0:
                return new EmptyNode();
            case 1:
                return list[0].Key;
            case 2:
                return list[0].Value switch
                {
                    "and" => new AndNode([list[0].Key, list[1].Key]),
                    "or" => new OrNode([list[0].Key, list[1].Key]),
                    _ => throw new ParseException($"Operator ${list[0].Value} is not valid operator.")
                };
        }

        // How does this work:
        // Consider example list looking like this:
        // 1 and 2 or 3 or 4 and 5 and 6 or 9 and 10
        // 'and' operator have precedence over 'or' operator, so we must evaluate them first
        // So we need to make our new list to look like this:
        // (1 and 2) or 3 or (4 and 5 and 6) or (9 and 10)
        // AndNode(1, 2) or 3 or AndNode(4, 5, 6) or AndNode(9, 10)
        // Now we have only or operators left so we can merge them into `OrNode`
        // OrNode(AndNode(1, 2), Node(3), AndNode(4, 5, 6), AndNode(9, 10))
        
        // Nodes that will go into AndNode
        List<Node> andedNodes = [];
        // Nodes that will go into OrNode
        List<Node> oredNodes = [];
        bool lastAnd = list[0].Value == "and";

        for (int i = 0; i < list.Count; i++)
        {
            Node currNode = list[i].Key;
            string currOp = list[i].Value;

            if (currOp == "and")
            {
                andedNodes.Add(currNode);
                lastAnd = true;
                continue;
            }

            // In this care operator is either 'or' or 'eof', we do the same for them
            if (lastAnd)
            {
                andedNodes.Add(currNode);
                oredNodes.Add(new AndNode(andedNodes));
                andedNodes = [];
                lastAnd = false;
                continue;
            }
            
            oredNodes.Add(currNode);
        }

        // Now if in oredNodes we have only one element we can just return it because we don't have second element to or them
        // Otherwise we need to construct it.
        return oredNodes.Count == 1 ? oredNodes[0] : new OrNode(oredNodes);
    }
    
    private readonly Regex whitespaceRegex = new Regex(@"\s");
    
    private readonly Dictionary<string, Regex> logicalOperators = new()
    {
        { "or", new Regex(@"^\s+or\s+") },
        { "and", new Regex(@"^\s+") }
    };

    private readonly Dictionary<string, FilterOperator> operators = new()
    {
        { "", FilterOperator.Equals },
        { "<", FilterOperator.LessThan },
        { ">", FilterOperator.GreaterThan },
        { "<=", FilterOperator.LessThanOrEqual },
        { ">=", FilterOperator.GreaterThanOrEqual }
    };
}

public class SearchParserOptions
{
    /// <summary>
    /// Which fields parser should treat as age fields?
    /// </summary>
    public IEnumerable<string>? AgeFields { get; set; }
}
