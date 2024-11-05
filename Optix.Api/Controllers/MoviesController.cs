using Microsoft.AspNetCore.Mvc;
using Optix.Api.DTO;
using Optix.Api.Factory;
using Optix.DataAccess.DbModels;
using Optix.Services.Factories;
using Optix.Services.Interfaces;
using Optix.Services.Models.DTO;

namespace Optix.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class MoviesController : ControllerBase
{
    private readonly IMovieSearchFactory _movieSearchFactory;
    private readonly IMoveSearchService _movieSearchServices;
    private readonly IResponseFactory _responseFactory;

    public MoviesController(IMovieSearchFactory movieSearchFactory, IResponseFactory responseFactory, IMoveSearchService movieSearchServices)
    {
        _movieSearchFactory = movieSearchFactory;
        _responseFactory = responseFactory;
        _movieSearchServices = movieSearchServices;
    }

    [HttpGet]
    public async Task<IActionResult> Search(
        [FromQuery] string? title,
        [FromQuery] string? genre,
        [FromQuery] string? sortBy,
        [FromQuery] string? orderBy,
        [FromQuery] int limit = 10,
        [FromQuery] int page = 1)
    {
        var searchParams = new MovieSearchParams
        {
            Title = title,
            Genre = genre,
            SortBy = sortBy,
            OrderBy = orderBy,
            Limit = limit,
            Page = page
        };

        var movieSearchServicParameter = _movieSearchFactory.GetMovieSearchParameter(searchParams);
        var movies = await _movieSearchServices.PagedSearch(searchParams, movieSearchServicParameter);

        return _responseFactory.CreateResponse(new PagingResult<Movie>(movies.Results, page, movies.TotalPages));
    }
}