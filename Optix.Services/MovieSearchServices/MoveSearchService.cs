using System.Linq.Expressions;
using Optix.DataAccess.DbModels;
using Optix.DataAccess.Repositories;
using Optix.ErrorManagement.Exceptions;
using Optix.Services.Interfaces;
using Optix.Services.Models.DTO;
using Optix.Services.Validation;

namespace Optix.Services.SearchServices;

public class MoveSearchService :  IMoveSearchService
{
    private readonly IRepository<Movie> _moviesRepository;
    private readonly IValidator<MovieSearchParams> _searchParamValidator;
    
    public MoveSearchService(IRepository<Movie> moviesRepository, IValidator<MovieSearchParams> searchParamValidator)
    {
        _moviesRepository = moviesRepository;
        _searchParamValidator = searchParamValidator;
    }
    public async Task<(IEnumerable<Movie> Results, int TotalPages)> PagedSearch(MovieSearchParams searchOptions, Expression<Func<Movie, bool>> searchService)
    {
        if (!_searchParamValidator.Validate(searchOptions, out var validationErrors))
        {
            throw new InvalidSearchParametersException(validationErrors);
        }

        var orderByDesc =
            searchOptions.OrderBy?.Equals(Constants.SearchConstants.DESC, StringComparison.InvariantCultureIgnoreCase) ?? false;

        var sortBySelector = searchOptions.SortBy switch
        {
            Constants.MovieSearchConstants.SORT_BY_TITLE => (Expression<Func<Movie, object>>)(m => m.Title),
            Constants.MovieSearchConstants.SORT_BY_RELEASE_DATE => m => m.ReleaseDate,
            Constants.MovieSearchConstants.SORT_BY_VOTE_COUNT => m => m.VoteCount,
            Constants.MovieSearchConstants.SORT_BY_VOTE_AVERAGE => m => m.VoteAverage,
            _ => null
        };

        var result = sortBySelector != null 
            ? await _moviesRepository.Get(searchService, sortBySelector, searchOptions.Limit, searchOptions.SkipAmount, orderByDesc)
            : await _moviesRepository.Get(searchService, searchOptions.Limit, searchOptions.SkipAmount);
        
        if (result.Results == null || result.TotalCount == 0)
        {
            throw new MovieNotFoundException("No results were found for those parameters.");
        }

        var pageCount = GetPageCount(result.TotalCount, searchOptions.Limit);

        if (searchOptions.Page > pageCount)
        {
            throw new InvalidSearchParametersException($"The request page count of {searchOptions.Page} is greater than the total returned pages: {pageCount}");
        }
        
        return (result.Results, pageCount);
    }

    private static int GetPageCount(int totalCount, int limit)
    {
        return (int)Math.Ceiling((double)totalCount / limit);
    }
}

