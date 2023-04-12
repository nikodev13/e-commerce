namespace ECommerce.ApplicationCore.Shared.Models;

public class PaginatedList<T>
{
    public List<T> Items { get; }
    public int PageSize { get; }
    public int PageNumber { get; }
    public int TotalItems { get; }
    public int TotalPages { get; }
    
    public PaginatedList(List<T> items, int pageSize, int pageNumber, int totalItems)
    {
        Items = items;
        PageSize = pageSize;
        PageNumber = pageNumber;
        TotalItems = totalItems;
        TotalPages = (int)double.Ceiling((double)totalItems / pageSize);
    }
}