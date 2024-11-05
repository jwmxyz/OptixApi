using CsvHelper.Configuration;
using Optix.DataAccess.DbModels;

namespace Optix.DataAccess.Seeding;

public class MovieMapping: ClassMap<Movie>
{
    public MovieMapping()
    {
        Map(model => model.ReleaseDate).Name("Release_Date");
        Map(model => model.Title).Name("Title");
        Map(model => model.Overview).Name("Overview");
        Map(model => model.Popularity).Name("Popularity");
        Map(model => model.VoteCount).Name("Vote_Count");
        Map(model => model.VoteAverage).Name("Vote_Average");
        Map(model => model.OriginalLanguage).Name("Original_Language");
        Map(model => model.Genre).Name("Genre");
        Map(model => model.PosterUrl).Name("Poster_Url");
    }
}