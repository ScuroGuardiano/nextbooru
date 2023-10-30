namespace Nextbooru.Core.Dto;

public class ListResponse<T>
{
    public required List<T> Data { get; init; }
    public int Page { get; init; }
    public int TotalPages { get; init; }
    public int TotalRecords { get; init; }
    public int RecordsPerPage { get; init; }
    
    /// <summary>
    /// Will be used for key based pagination.
    /// </summary>
    public long LastRecordId { get; init; }
}
