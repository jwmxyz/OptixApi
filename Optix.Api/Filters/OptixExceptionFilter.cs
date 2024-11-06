using Microsoft.AspNetCore.Mvc.Filters;
using Optix.Api.Responses;

namespace Optix.Api.Filters;

public class OptixExceptionFilter : ExceptionFilterAttribute
{
    /// <summary>
    /// An exception filter that will catch any exceptions thrown within the application and mask them.
    /// </summary>
    /// <param name="exceptionContext"></param>
    public override void OnException(ExceptionContext exceptionContext)
    {
        
        //todo Exceptions can be logged here - but they should really be loged at the source of the exception.
        //This is just nice place to catch any unhandled exceptions and presents them nicely. 
        var responseFactory = exceptionContext.HttpContext.RequestServices.GetService<IResponseFactory>();

        exceptionContext.Result = responseFactory?.CreateResponse(exceptionContext.Exception);
    }
}