using Microsoft.AspNetCore.Mvc;
using Optix.Api.DTO;

namespace Optix.Api.Factory;

public interface IResponseFactory
{
    IActionResult CreateResponse<T>(PagingResult<T> results);
    
    IActionResult CreateResponse<T>(T exception) where T : Exception;
}