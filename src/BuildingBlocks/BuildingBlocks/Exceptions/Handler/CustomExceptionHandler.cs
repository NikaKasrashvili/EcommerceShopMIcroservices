using FluentValidation;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BuildingBlocks.Exceptions.Handler;

public class CustomExceptionHandler(ILogger<CustomExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
    {
        logger.LogError("Error Message:{exceptionMessage}, time of occurence {time}",
            exception.Message, DateTime.UtcNow);

        (string Detail, string Title, int StatusCode) = exception switch
        {
            InternalServerException =>
            (
               exception.Message,
               exception.GetType().Name,
               context.Response.StatusCode = StatusCodes.Status500InternalServerError
            ),
            ValidationException =>
            (
               exception.Message,
               exception.GetType().Name,
               context.Response.StatusCode = StatusCodes.Status400BadRequest
            ),
            BadRequestException =>
            (
               exception.Message,
               exception.GetType().Name,
               context.Response.StatusCode = StatusCodes.Status400BadRequest
            ),
            NotFoundException =>
            (
               exception.Message,
               exception.GetType().Name,
               context.Response.StatusCode = StatusCodes.Status404NotFound
            ),
            _ => (exception.Message,
               exception.GetType().Name,
               context.Response.StatusCode = StatusCodes.Status400BadRequest
            )};

        var problemDetails = new ProblemDetails
        {
            Status = StatusCode,
            Title = Title,
            Detail = Detail,
            Instance = context.Request.Path
        };

        problemDetails.Extensions.Add("traceId", context.TraceIdentifier);

        if(exception is ValidationException validationException)
        {
            problemDetails.Extensions.Add("ValidationErros", validationException.Errors);
        }

        await context.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}
