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

    public async Task<(List<Movie> Results, int TotalCount)> Get(Expression<Func<Movie, bool>> predicate, int limit,
        int skip)
    {
        var query = _dbContext.Movies.Where(predicate);
        return await GetPagedResultsAsync(query, limit, skip);
    }

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

    private async Task<(List<Movie> Results, int TotalCount)> GetPagedResultsAsync(IQueryable<Movie> query, int limit,
        int skip)
    {
        var totalCount = await query.CountAsync();
        var results = await query.Skip(skip).Take(limit).ToListAsync();
        return (results, totalCount);
    }
}