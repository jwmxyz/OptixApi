using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using Optix.Api.DTO;
using Optix.Api.Responses;
using Optix.ErrorManagement.Exceptions;

namespace Optix.Api.Tests;

[TestFixture]
public class ResponseFactoryTests
{
    private ResponseFactory _factory;

    [SetUp]
    public void Setup()
    {
        _factory = new ResponseFactory();
    }

    [Test]
    public void CreateResponse_WithPagingResult_ReturnsOkObjectResult()
    {
        // Arrange
        var pagingResult = new PagingResult<string>(
            new List<string> { "item1", "item2" },
            2, 10);

        // Act
        var result = _factory.CreateResponse(pagingResult);

        // Assert
        Assert.That(result, Is.TypeOf<OkObjectResult>());
        var okResult = (OkObjectResult)result;
        Assert.That(okResult.Value, Is.TypeOf<OptixStandardResult>());
        
        var standardResult = (OptixStandardResult)okResult.Value;
        Assert.That(standardResult.HasErrors, Is.False);
        Assert.That(standardResult.Data, Is.EqualTo(pagingResult));
        Assert.That(standardResult.Errors, Is.Null);
    }

    [Test]
    public void CreateResponse_WithInvalidSearchParametersException_ReturnsBadRequest()
    {
        // Arrange
        var exception = new InvalidSearchParametersException("Invalid search parameters");

        // Act
        var result = _factory.CreateResponse(exception);

        // Assert
        Assert.That(result, Is.TypeOf<BadRequestObjectResult>());
        var badRequestResult = (BadRequestObjectResult)result;
        Assert.That(badRequestResult.StatusCode, Is.EqualTo(StatusCodes.Status400BadRequest));
        
        var standardResult = (OptixStandardResult)badRequestResult.Value;
        Assert.That(standardResult.HasErrors, Is.True);
        Assert.That(standardResult.Errors.ToList()[0], Is.EqualTo("Invalid search parameters"));
        Assert.That(standardResult.Data, Is.Null);
    }

    [Test]
    public void CreateResponse_WithMovieNotFoundException_ReturnsNotFound()
    {
        // Arrange
        var exception = new MovieNotFoundException("Movie not found");

        // Act
        var result = _factory.CreateResponse(exception);

        // Assert
        Assert.That(result, Is.TypeOf<NotFoundObjectResult>());
        var notFoundResult = (NotFoundObjectResult)result;
        
        var standardResult = (OptixStandardResult)notFoundResult.Value;
        Assert.That(standardResult.HasErrors, Is.True);
        Assert.That(standardResult.Errors.ToList()[0], Is.EqualTo("Movie not found"));
        Assert.That(standardResult.Data, Is.Null);
    }

    [Test]
    public void CreateResponse_WithUnhandledException_ReturnsInternalServerError()
    {
        // Arrange
        var exception = new Exception("Unexpected error");

        // Act
        var result = _factory.CreateResponse(exception);

        // Assert
        Assert.That(result, Is.TypeOf<ObjectResult>());
        var objectResult = (ObjectResult)result;
        Assert.That(objectResult.StatusCode, Is.EqualTo(StatusCodes.Status500InternalServerError));
        
        var standardResult = (OptixStandardResult)objectResult.Value;
        Assert.That(standardResult.HasErrors, Is.True);
        Assert.That(standardResult.Errors.ToList()[0], Is.EqualTo("Unexpected error"));
        Assert.That(standardResult.Data, Is.Null);
    }

    [Test]
    public void CreateResponse_WithEmptyPagingResult_ReturnsOkWithEmptyResult()
    {
        // Arrange
        var pagingResult = new PagingResult<string>(
            new List<string>(),
            2, 10);

        // Act
        var result = _factory.CreateResponse(pagingResult);

        // Assert
        Assert.That(result, Is.TypeOf<OkObjectResult>());
        var okResult = (OkObjectResult)result;
        var standardResult = (OptixStandardResult)okResult.Value;
        
        Assert.Multiple(() =>
        {
            Assert.That(standardResult.HasErrors, Is.False);
            Assert.That(standardResult.Data, Is.EqualTo(pagingResult));
            Assert.That(((PagingResult<string>)standardResult.Data).Results, Is.Empty);
            Assert.That(((PagingResult<string>)standardResult.Data).Results.Count(), Is.EqualTo(0));
        });
    }

    [Test]
    public void CreateResponse_WithNullPagingResult_ReturnsOkWithNull()
    {
        // Arrange
        PagingResult<string> pagingResult = null;

        // Act
        var result = _factory.CreateResponse(pagingResult);

        // Assert
        Assert.That(result, Is.TypeOf<OkObjectResult>());
        var okResult = (OkObjectResult)result;
        var standardResult = (OptixStandardResult)okResult.Value;
        
        Assert.That(standardResult.Data, Is.Null);
        Assert.That(standardResult.HasErrors, Is.False);
    }
}