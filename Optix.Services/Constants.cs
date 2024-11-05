namespace Optix.Services;

public static class Constants
{
    public static class MovieSearchConstants
    {
        public const string SORT_BY_TITLE = "title";
        public const string SORT_BY_RELEASE_DATE = "releasedate";
        public const string SORT_BY_VOTE_COUNT = "votecount";
        public const string SORT_BY_VOTE_AVERAGE = "voteaverage";
    }

    public static class SearchConstants
    {
        public const string ASC = "asc";
        public const string DESC = "desc";

        public static readonly string[] OrderByConsts = new[]
        {
            ASC,
            DESC
        };
    }
}