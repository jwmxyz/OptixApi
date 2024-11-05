using System.Linq.Expressions;
using Optix.DataAccess.DbModels;
using Optix.Services.Models.DTO;

namespace Optix.Services;

public interface ISearchService<T, TK> where TK : PagedRequest
{
    Task<(IEnumerable<T> Results, int TotalPages)> PagedSearch(TK searchOptions, Expression<Func<T, bool>> searcbExpression);
}