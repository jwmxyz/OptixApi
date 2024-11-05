using System.Linq.Expressions;
using Optix.DataAccess.DbModels;
using Optix.Services.Models.DTO;

namespace Optix.Services.Factories;

public interface IMovieSearchFactory
{
    public Expression<Func<Movie, bool>> GetMovieSearchParameter(MovieSearchParams searchParams);
}