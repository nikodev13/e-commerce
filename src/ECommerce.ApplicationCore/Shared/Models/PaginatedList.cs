namespace ECommerce.ApplicationCore.Shared.Models;

public class PaginatedList<T>
{
    public List<T> Items { get; }
    public int PageSize { get; }
    public int PageNumber { get; }
    public int TotalCount { get; }
    public int TotalPages { get; }
    
    public PaginatedList(List<T> items, int pageSize, int pageNumber, int totalCount)
    {
        Items = items;
        PageSize = pageSize;
        PageNumber = pageNumber;
        TotalCount = totalCount;
        TotalPages = (int)double.Ceiling((double)totalCount / pageSize);
    }
}