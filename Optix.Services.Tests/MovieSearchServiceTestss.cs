using System.Linq.Expressions;
using Moq;
using NUnit.Framework;
using Optix.DataAccess.DbModels;
using Optix.DataAccess.Repositories;
using Optix.ErrorManagement.Exceptions;
using Optix.Services.Models.DTO;
using Optix.Services.SearchServices;
using Optix.Services.Validation;

namespace Optix.Services.Tests;

[TestFixture]
public class MovieSearchServiceTests
{
    private Mock<IRepository<Movie>> _mockRepository;
    private Mock<IValidator<MovieSearchParams>> _mockValidator;
    private MoveSearchService _searchService;
    private Expression<Func<Movie, bool>> _defaultSearchExpression;

    [SetUp]
    public void Setup()
    {
        _mockRepository = new Mock<IRepository<Movie>>();
        _mockValidator = new Mock<IValidator<MovieSearchParams>>();
        _searchService = new MoveSearchService(_mockRepository.Object, _mockValidator.Object);
        _defaultSearchExpression = m => true; // Default expression for testing
    }

    [Test]
    public async Task PagedSearch_WhenValidationFails_ThrowsInvalidSearchParametersException()
    {
        // Arrange
        var searchParams = new MovieSearchParams();
        List<string> validationErrors = new List<string> { "Validation failed" };
        _mockValidator.Setup(v => v.Validate(searchParams, out validationErrors))
            .Returns(false);

        // Act & Assert
        var ex = Assert.ThrowsAsync<InvalidSearchParametersException>(() =>
            _searchService.PagedSearch(searchParams, _defaultSearchExpression));
        Assert.That(ex.Message, Is.EqualTo(validationErrors[0]));
    }

    [Test]
    public async Task PagedSearch_WhenNoMoviesFound_ThrowsMovieNotFoundException()
    {
        // Arrange
        var searchParams = new MovieSearchParams { Limit = 10, Page = 1 };
        _mockValidator.Setup(v => v.Validate(searchParams, out It.Ref<List<string>>.IsAny))
            .Returns(true);
        _mockRepository.Setup(r => r.Get(
                It.IsAny<Expression<Func<Movie, bool>>>(),
                It.IsAny<int>(),
                It.IsAny<int>()))
            .ReturnsAsync((new List<Movie>(), 0));

        // Act & Assert
        Assert.ThrowsAsync<MovieNotFoundException>(() =>
            _searchService.PagedSearch(searchParams, _defaultSearchExpression));
    }

    [Test]
    public async Task PagedSearch_WhenPageExceedsTotalPages_ThrowsInvalidSearchParametersException()
    {
        // Arrange
        var searchParams = new MovieSearchParams { Limit = 10, Page = 3 };
        var movies = Enumerable.Range(1, 5).Select(i => new Movie()).ToList();

        _mockValidator.Setup(v => v.Validate(searchParams, out It.Ref<List<string>>.IsAny))
            .Returns(true);
        _mockRepository.Setup(r => r.Get(
                It.IsAny<Expression<Func<Movie, bool>>>(),
                It.IsAny<int>(),
                It.IsAny<int>()))
            .ReturnsAsync((movies, movies.Count));

        // Act & Assert
        var ex = Assert.ThrowsAsync<InvalidSearchParametersException>(() =>
            _searchService.PagedSearch(searchParams, _defaultSearchExpression));
        Assert.That(ex.Message, Contains.Substring("request page count"));
    }

    [TestCase(Constants.MovieSearchConstants.SORT_BY_TITLE, false)]
    [TestCase(Constants.MovieSearchConstants.SORT_BY_RELEASE_DATE, true)]
    [TestCase(Constants.MovieSearchConstants.SORT_BY_VOTE_COUNT, false)]
    [TestCase(Constants.MovieSearchConstants.SORT_BY_VOTE_AVERAGE, true)]
    public async Task PagedSearch_WithValidSortParameters_CallsRepositoryWithCorrectParameters(string sortBy,
        bool orderByDesc)
    {
        // Arrange
        var searchParams = new MovieSearchParams
        {
            Limit = 10,
            Page = 1,
            SortBy = sortBy,
            OrderBy = orderByDesc ? Constants.SearchConstants.DESC : Constants.SearchConstants.ASC
        };
        var movies = Enumerable.Range(1, 5).Select(i => new Movie()).ToList();

        _mockValidator.Setup(v => v.Validate(searchParams, out It.Ref<List<string>>.IsAny))
            .Returns(true);
        _mockRepository.Setup(r => r.Get(
                It.IsAny<Expression<Func<Movie, bool>>>(),
                It.IsAny<Expression<Func<Movie, object>>>(),
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<bool>()))
            .ReturnsAsync((movies, movies.Count));

        // Act
        var result = await _searchService.PagedSearch(searchParams, _defaultSearchExpression);

        // Assert
        _mockRepository.Verify(r => r.Get(
            It.IsAny<Expression<Func<Movie, bool>>>(),
            It.IsAny<Expression<Func<Movie, object>>>(),
            searchParams.Limit,
            searchParams.SkipAmount,
            orderByDesc), Times.Once);
        Assert.That(result.Results, Is.EqualTo(movies));
        Assert.That(result.TotalPages, Is.EqualTo(1));
    }

    [Test]
    public async Task PagedSearch_WithoutSortParameters_UsesDefaultSort()
    {
        // Arrange
        var searchParams = new MovieSearchParams { Limit = 10, Page = 1 };
        var movies = Enumerable.Range(1, 5).Select(i => new Movie()).ToList();

        _mockValidator.Setup(v => v.Validate(searchParams, out It.Ref<List<string>>.IsAny))
            .Returns(true);
        _mockRepository.Setup(r => r.Get(
                It.IsAny<Expression<Func<Movie, bool>>>(),
                It.IsAny<int>(),
                It.IsAny<int>()))
            .ReturnsAsync((movies, movies.Count));

        // Act
        var result = await _searchService.PagedSearch(searchParams, _defaultSearchExpression);

        // Assert
        _mockRepository.Verify(r => r.Get(
            It.IsAny<Expression<Func<Movie, bool>>>(),
            searchParams.Limit,
            searchParams.SkipAmount), Times.Once);
        Assert.That(result.Results, Is.EqualTo(movies));
        Assert.That(result.TotalPages, Is.EqualTo(1));
    }

    [TestCase(10, 20, 2)] // 20 items, limit 10 = 2 pages
    [TestCase(5, 11, 3)] // 11 items, limit 5 = 3 pages
    [TestCase(3, 3, 1)] // 3 items, limit 3 = 1 page
    public async Task PagedSearch_CalculatesCorrectPageCount(int limit, int totalItems, int expectedPages)
    {
        // Arrange
        var searchParams = new MovieSearchParams { Limit = limit, Page = 1 };
        var movies = Enumerable.Range(1, totalItems).Select(i => new Movie()).ToList();

        _mockValidator.Setup(v => v.Validate(searchParams, out It.Ref<List<string>>.IsAny))
            .Returns(true);
        _mockRepository.Setup(r => r.Get(
                It.IsAny<Expression<Func<Movie, bool>>>(),
                It.IsAny<int>(),
                It.IsAny<int>()))
            .ReturnsAsync((movies, totalItems));

        // Act
        var result = await _searchService.PagedSearch(searchParams, _defaultSearchExpression);

        // Assert
        Assert.That(result.TotalPages, Is.EqualTo(expectedPages));
    }
}