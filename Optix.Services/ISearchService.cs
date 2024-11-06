using System.Linq.Expressions;
using Optix.DataAccess.DbModels;
using Optix.Services.Models.DTO;

namespace Optix.Services;

public interface ISearchService<T, TK> where TK : PagedRequest
{
    /// <summary>
    /// Performs paginated search for T with optional sorting and filtering
    /// </summary>
    /// <param name="searchOptions">Search parameters including pagination, sorting, and filtering options</param>
    /// <param name="searchExpression">Expression defining the search criteria</param>
    /// <returns>Tuple containing matching movies and total page count</returns>
    Task<(IEnumerable<T> Results, int TotalPages)> PagedSearch(TK searchOptions, Expression<Func<T, bool>> searchExpression);
}