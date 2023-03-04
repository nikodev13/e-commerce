namespace ECommerce.ApplicationCore.Shared.Models;

public class PaginatedList<T>
{
    public List<T> Items { get; }
    public uint PageNumber { get; }
    public uint PageSize { get; }
    public uint TotalItems { get; }
    public uint TotalPages { get; }
    
    public PaginatedList(List<T> items, uint pageNumber, uint pageSize, uint totalItems)
    {
        Items = items;
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalItems = totalItems;
        TotalPages = (uint)double.Ceiling((double)totalItems / pageSize);
    }
}