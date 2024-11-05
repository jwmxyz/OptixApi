using NUnit.Framework;
using Optix.Services.Models.DTO;
using Optix.Services.Validation;

namespace Optix.Services.Tests;

[TestFixture]
public class MovieSearchParamValidatorTests
{
    private MovieSearchParamValidator _validator;
    private List<string> _validationErrors;

    [SetUp]
    public void Setup()
    {
        _validator = new MovieSearchParamValidator();
        _validationErrors = new List<string>();
    }

    [Test]
    public void Validate_WithValidParameters_ReturnsTrue()
    {
        // Arrange
        var searchParams = new MovieSearchParams
        {
            Title = "Test Movie",
            SortBy = "title",
            OrderBy = "asc",
            Limit = 10,
            Page = 1
        };

        // Act
        bool result = _validator.Validate(searchParams, out _validationErrors);

        // Assert
        Assert.That(result, Is.True);
        Assert.That(_validationErrors, Is.Empty);
    }

    [Test]
    public void Validate_WithEmptyTitle_ReturnsFalse()
    {
        // Arrange
        var searchParams = new MovieSearchParams
        {
            Title = "",
            Limit = 10,
            Page = 1
        };

        // Act
        bool result = _validator.Validate(searchParams, out _validationErrors);

        // Assert
        Assert.That(result, Is.False);
        Assert.That(_validationErrors, Has.Exactly(1).Items);
        Assert.That(_validationErrors[0], Is.EqualTo("Movie title cannot be null or empty."));
    }

    [Test]
    public void Validate_WithNullTitle_ReturnsFalse()
    {
        // Arrange
        var searchParams = new MovieSearchParams
        {
            Title = null,
            Limit = 10,
            Page = 1
        };

        // Act
        bool result = _validator.Validate(searchParams, out _validationErrors);

        // Assert
        Assert.That(result, Is.False);
        Assert.That(_validationErrors, Has.Exactly(1).Items);
        Assert.That(_validationErrors[0], Is.EqualTo("Movie title cannot be null or empty."));
    }

    [Test]
    public void Validate_WithInvalidSortBy_ReturnsFalse()
    {
        // Arrange
        var searchParams = new MovieSearchParams
        {
            Title = "Test Movie",
            SortBy = "invalid_sort",
            Limit = 10,
            Page = 1
        };

        // Act
        bool result = _validator.Validate(searchParams, out _validationErrors);

        // Assert
        Assert.That(result, Is.False);
        Assert.That(_validationErrors, Has.Exactly(1).Items);
        Assert.That(_validationErrors[0], Does.StartWith("Sortby can only contain"));
    }

    [Test]
    public void Validate_WithInvalidOrderBy_ReturnsFalse()
    {
        // Arrange
        var searchParams = new MovieSearchParams
        {
            Title = "Test Movie",
            OrderBy = "invalid_order",
            Limit = 10,
            Page = 1
        };

        // Act
        bool result = _validator.Validate(searchParams, out _validationErrors);

        // Assert
        Assert.That(result, Is.False);
        Assert.That(_validationErrors, Has.Exactly(1).Items);
        Assert.That(_validationErrors[0], Does.StartWith("OrderBy can only contain"));
    }

    [Test]
    public void Validate_WithZeroLimit_ReturnsFalse()
    {
        // Arrange
        var searchParams = new MovieSearchParams
        {
            Title = "Test Movie",
            Limit = 0,
            Page = 1
        };

        // Act
        bool result = _validator.Validate(searchParams, out _validationErrors);

        // Assert
        Assert.That(result, Is.False);
        Assert.That(_validationErrors, Has.Exactly(1).Items);
        Assert.That(_validationErrors[0], Is.EqualTo("Limit must be a positive integer."));
    }

    [Test]
    public void Validate_WithNegativeLimit_ReturnsFalse()
    {
        // Arrange
        var searchParams = new MovieSearchParams
        {
            Title = "Test Movie",
            Limit = -1,
            Page = 1
        };

        // Act
        bool result = _validator.Validate(searchParams, out _validationErrors);

        // Assert
        Assert.That(result, Is.False);
        Assert.That(_validationErrors, Has.Exactly(1).Items);
        Assert.That(_validationErrors[0], Is.EqualTo("Limit must be a positive integer."));
    }

    [Test]
    public void Validate_WithZeroPage_ReturnsFalse()
    {
        // Arrange
        var searchParams = new MovieSearchParams
        {
            Title = "Test Movie",
            Limit = 10,
            Page = 0
        };

        // Act
        bool result = _validator.Validate(searchParams, out _validationErrors);

        // Assert
        Assert.That(result, Is.False);
        Assert.That(_validationErrors, Has.Exactly(1).Items);
        Assert.That(_validationErrors[0], Is.EqualTo("Page must be a positive integer."));
    }

    [Test]
    public void Validate_WithNegativePage_ReturnsFalse()
    {
        // Arrange
        var searchParams = new MovieSearchParams
        {
            Title = "Test Movie",
            Limit = 10,
            Page = -1
        };

        // Act
        bool result = _validator.Validate(searchParams, out _validationErrors);

        // Assert
        Assert.That(result, Is.False);
        Assert.That(_validationErrors, Has.Exactly(1).Items);
        Assert.That(_validationErrors[0], Is.EqualTo("Page must be a positive integer."));
    }

    [Test]
    public void Validate_WithMultipleValidationErrors_ReturnsFalse()
    {
        // Arrange
        var searchParams = new MovieSearchParams
        {
            Title = "",
            SortBy = "invalid_sort",
            OrderBy = "invalid_order",
            Limit = 0,
            Page = 0
        };

        // Act
        bool result = _validator.Validate(searchParams, out _validationErrors);

        // Assert
        Assert.That(result, Is.False);
        Assert.That(_validationErrors, Has.Exactly(5).Items);
    }
}