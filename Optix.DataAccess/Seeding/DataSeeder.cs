using System.Globalization;
using CsvHelper;
using Optix.DataAccess.DbModels;
using Microsoft.EntityFrameworkCore;

namespace Optix.DataAccess.Seeding;

public static class DataSeeder
{
    /// <summary>
    /// Used to seed the data 
    /// </summary>
    /// <param name="modelBuilder"></param>
    public static void SeedMovies(ModelBuilder modelBuilder)
    {
        using var reader = new StreamReader($"{AppDomain.CurrentDomain.BaseDirectory}/Seeding/SeedData/mymoviedb.csv");
        using var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);
        csvReader.Context.RegisterClassMap<MovieMapping>();

        // no checks here as we trust it.
        var movies = csvReader.GetRecords<Movie>();
        modelBuilder.Entity<Movie>()
            .HasData(PrepareMoviesForSeeding(movies));
    }
    
    private static List<Movie> PrepareMoviesForSeeding(IEnumerable<Movie> movies)
    {
        var count = 1;
        return movies.Select(m => new Movie
        {
            Id = count++,
            Title = m.Title,
            Overview = m.Overview,
            ReleaseDate = m.ReleaseDate,
            Popularity = m.Popularity,
            VoteCount = m.VoteCount,
            VoteAverage = m.VoteAverage,
            OriginalLanguage = m.OriginalLanguage,
            Genre = m.Genre,
            PosterUrl = m.PosterUrl
        }).ToList();
    }
    
}     