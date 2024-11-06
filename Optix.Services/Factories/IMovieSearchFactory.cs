using System.Linq.Expressions;
using Optix.DataAccess.DbModels;
using Optix.Services.Models.DTO;

namespace Optix.Services.Factories;

public interface IMovieSearchFactory
{
    /// <summary>
    /// Builds a search expression for filtering movies by title and genre.
    /// </summary>
    /// <param name="searchParams">Search parameters containing title and genre criteria</param>
    /// <returns>Expression to filter movies based on provided search parameters</returns>
    public Expression<Func<Movie, bool>> GetMovieSearchParameter(MovieSearchParams searchParams);
}