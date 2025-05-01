namespace Lume.Application.Common;

public class PagedResult<T>
{
    private IEnumerable<T> Items { get; set; }
    private int PageSize { get; set; }
    private int PageIndex { get; set; }
    private int TotalPages { get; set; }
    private int TotalItemsCount { get; set; }
    public int ItemsFrom { get; set; }
    public int ItemsTo { get; set; }
}