namespace Lume.Application.Common;

public class PagedResult<T>
{
    public IEnumerable<T> Items { get; set; }
    public int TotalPages { get; set; }
    public int TotalItemsCount { get; set; }
    public int ItemsFrom { get; set; }
    public int ItemsTo { get; set; }
    
    public PagedResult(IEnumerable<T> items, int totalItemsCount, int pageSize, int pageIndex)
    {
        Items = items;
        TotalItemsCount = totalItemsCount;
        TotalPages = (int)Math.Ceiling(totalItemsCount / (double)pageSize);
        ItemsFrom = pageSize * (pageIndex - 1) + 1;
        ItemsTo = ItemsFrom + pageSize - 1;
    }
}