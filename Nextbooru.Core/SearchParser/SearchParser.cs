using System.Text.RegularExpressions;
using Microsoft.IdentityModel.Tokens;

namespace Nextbooru.Core.SearchParser;

public class SearchParser
{
    private SearchParserOptions options = new SearchParserOptions();
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
        if (code.IsNullOrEmpty())
        {
            return new EmptyNode();
        }

        throw new NotImplementedException();
    }

    private Node NextNode(ReadOnlySpan<char> code)
    {
        throw new NotImplementedException();
    }

    private Node NextTagNode(ReadOnlySpan<char> code)
    {
        throw new NotImplementedException();
    }

    private (string op, int length) NextOperator(ReadOnlySpan<char> code)
    {
        throw new NotImplementedException();
        // if (logicalOperators["or"].EnumerateMatches(code))
        // {
        //     return "or";
        // }
        //
        // if (logicalOperators["and"].IsMatch(code))
        // {
        //     return "and";
        // }
    }

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

// cat dog
