namespace Nextbooru.Core.SearchParser;

public abstract class Node;
public abstract class MetatagValue;

public class EmptyNode : Node;

public class TagNode : Node
{
    public required string Value { get; set; }
}

public class StringValue : MetatagValue
{
    public required string Value { get; set; }
}

public class ArrayValue : MetatagValue
{
    public required string[] Value { get; set; }
}

public class RangeValue : MetatagValue
{
    public required string From { get; set; }
    public required string To { get; set; }
}

public enum FilterOperator
{
    Equals,
    GreaterThan,
    GreaterThanOrEqual,
    LessThan,
    LessThanOrEqual
}

public class MetatagFilterNode : Node
{
    public required string Field { get; set; }
    public FilterOperator Operator { get; set; }
    public required MetatagValue Value { get; set; }
}

/// <summary>
/// Age node has special rules:
/// <ul>
///     <li><see cref="FilterOperator.Equals"/> should be treaten <see cref="FilterOperator.LessThanOrEqual"/></li>
///     <li>
///         If <see cref="AgeFilterNode.To" /> value is null then <see cref="AgeFilterNode.From"/> Value it should be
///         matched using <see cref="AgeFilterNode.Operator" />. 
///     </li>
///     <li>If <see cref="AgeFilterNode.To" /> is set then it should be treaten as range and matched with between.</li>
/// </ul>
/// </summary>
public class AgeFilterNode : Node
{
    public FilterOperator Operator { get; set; }
    public TimeSpan From { get; set; }
    public TimeSpan? To { get; set; }
}

public class MetatagSortingNode : Node
{
    public required string Field { get; set; }
    public string Direction { get; set; } = DescendingOrder;
    
    public const string AscendingOrder = "asc";
    public const string DescendingOrder = "desc";
}

public class NegationNode : Node
{
    public required Node Value { get; set; }
}

public class AndNode : Node
{
    public required Node[] Value { get; set; }
}

public class OrNode : Node
{
    public required Node[] Value { get; set; }
}

// cat
// TagNode(cat)
// cat summer
// AndNode[ TagNode(cat), TagNode(summer) ]
// (cat summer) or dog
// OrNode[ AndNode[ TagNode(cat), TagNode(summer) ], TagNode(dog) ]
