namespace Optix.Services.Models.DTO;

public class PagedRequest
{
    public string SortBy { get; set; }
    public int Limit { get; set; }
    public int Page { get; set; }
    public string OrderBy { get; set; }
    public virtual List<string> ValidSortByOptions { get; } = [];
    public int SkipAmount => (Page - 1) * Limit;
}