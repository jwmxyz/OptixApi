namespace Optix.Services.Models.DTO;

public class MovieSearchParams : PagedRequest
{
    public string Title { get; set; }
    public string Genre { get; set; }
    public override List<string> ValidSortByOptions { get; } =
    [
        Constants.MovieSearchConstants.SORT_BY_TITLE,
        Constants.MovieSearchConstants.SORT_BY_RELEASE_DATE,
        Constants.MovieSearchConstants.SORT_BY_VOTE_COUNT,
        Constants.MovieSearchConstants.SORT_BY_VOTE_AVERAGE

    ];
}