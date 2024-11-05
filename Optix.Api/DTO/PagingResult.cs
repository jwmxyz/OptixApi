namespace Optix.Api.DTO;

public class PagingResult<T>(IEnumerable<T> results, int pageNumber, int totalPages)
{
    //todo include HATEOAS properties such as prevPage and nextPage urls.
    public IEnumerable<T>? Results { get; set; } = results;
    public int PageNumber { get; set; } = pageNumber;
    public int TotalPages { get; set; } = totalPages;
}