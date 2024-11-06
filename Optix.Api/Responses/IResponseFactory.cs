using Microsoft.AspNetCore.Mvc;
using Optix.Api.DTO;

namespace Optix.Api.Responses;

public interface IResponseFactory
{
    /// <summary>
    /// Creates successful response with paged results
    /// </summary>
    /// <typeparam name="T">Type of paged data</typeparam>
    /// <returns>OK response containing paged results</returns>
    IActionResult CreateResponse<T>(PagingResult<T> results);
    
    /// <summary>
    /// Creates error response based on exception type
    /// </summary>
    /// <typeparam name="T">Type of exception</typeparam>
    /// <returns>Response with appropriate status code and error details</returns>
    IActionResult CreateResponse<T>(T exception) where T : Exception;
}