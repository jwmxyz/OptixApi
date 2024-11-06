using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Optix.DataAccess.DbModels;

namespace Optix.DataAccess.Repositories;

public class MovieRepository : IRepository<Movie>
{
    private readonly OptixDbContext _dbContext;

    public MovieRepository(OptixDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Retrieves a paged list of movies matching the predicate
    /// </summary>
    /// <returns>Tuple containing matching movies and total count</returns>
    public async Task<(List<Movie> Results, int TotalCount)> Get(Expression<Func<Movie, bool>> predicate, int limit,
        int skip)
    {
        var query = _dbContext.Movies.Where(predicate);
        return await GetPagedResultsAsync(query, limit, skip);
    }

    /// <summary>
    /// Retrieves a sorted and paged list of movies matching the predicate
    /// </summary>
    /// <typeparam name="TKey">Type of sorting key</typeparam>
    /// <returns>Tuple containing sorted movies and total count</returns>
    public async Task<(List<Movie> Results, int TotalCount)> Get<TKey>(
        Expression<Func<Movie, bool>> predicate,
        Expression<Func<Movie, TKey>> orderBy,
        int limit,
        int skip,
        bool orderByDesc = false)
    {
        var query = _dbContext.Movies.Where(predicate);
        var orderedQuery = orderByDesc ? query.OrderByDescending(orderBy) : query.OrderBy(orderBy);
        return await GetPagedResultsAsync(orderedQuery, limit, skip);
    }

    /// <summary>
    /// Applies pagination to a movie query and returns results with total count
    /// </summary>
    /// <returns>Tuple containing paged movies and total count</returns>
    private async Task<(List<Movie> Results, int TotalCount)> GetPagedResultsAsync(IQueryable<Movie> query, int limit,
        int skip)
    {
        var totalCount = await query.CountAsync();
        var results = await query.Skip(skip).Take(limit).ToListAsync();
        return (results, totalCount);
    }
}