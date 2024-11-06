using System.Linq.Expressions;

namespace Optix.DataAccess.Repositories;

public interface IRepository<TEntity>
{
    /// <summary>
    /// Used to GetAll TEntity filtered based on a predicate
    /// </summary>
    /// <param name="predicate">The filtering mechanism</param>
    /// <returns>A List of TEntity based on </returns>
    Task<(List<TEntity> Results, int TotalCount)> Get(Expression<Func<TEntity, bool>> predicate, int limit, int skip);

    /// <summary>
    /// Retrieves a sorted and paged list of TEntity matching the predicate
    /// </summary>
    /// <typeparam name="TKey">Type of sorting key</typeparam>
    /// <returns>Tuple containing sorted TEntity and total count</returns>
    Task<(List<TEntity> Results, int TotalCount)> Get<TKey>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TKey>> orderBy,
        int limit, int skip, bool orderByDesc = false);
    

}