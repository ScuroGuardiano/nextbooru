namespace Nextbooru.Core.SearchParser;

public class ParseException : Exception
{
    public ParseException(string message)
        : base(message)
    {
    }
}
