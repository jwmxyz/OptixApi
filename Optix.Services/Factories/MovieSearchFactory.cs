using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Optix.DataAccess.DbModels;
using Optix.Services.Models.DTO;

namespace Optix.Services.Factories;

public class MovieSearchFactory : IMovieSearchFactory
{
    public MovieSearchFactory()
    {
    }

    /// <inheritdoc cref="IMovieSearchFactory.GetMovieSearchParameter"/>
    public Expression<Func<Movie, bool>> GetMovieSearchParameter(MovieSearchParams searchParams)
    {
        if (!string.IsNullOrEmpty(searchParams.Title) && string.IsNullOrEmpty(searchParams.Genre))
        {
            return movies => 
                EF.Functions.Like(movies.Title, $"%{searchParams.Title}%");
        }

        return movies => 
            EF.Functions.Like(movies.Title, $"%{searchParams.Title}%") && 
            EF.Functions.Like(movies.Genre, $"%{searchParams.Genre}%");
    }
}