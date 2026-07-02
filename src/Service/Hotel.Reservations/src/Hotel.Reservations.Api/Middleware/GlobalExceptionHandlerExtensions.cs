using Hotel.Reservations.Application.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Hotel.Reservations.Api.Middleware;

public static class GlobalExceptionHandlerExtensions
{
    public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(new ExceptionHandlerOptions
        {
            ExceptionHandler = async context =>
            {
                var exceptionFeature = context.Features.Get<IExceptionHandlerFeature>();
                var exception = exceptionFeature?.Error;

                if (exception == null)
                    return;

                var environment = context.RequestServices.GetRequiredService<IHostEnvironment>();
                var logger = context.RequestServices.GetRequiredService<ILogger<GlobalExceptionHandler>>();

                logger.LogError(exception, "Unhandled exception occurred. TraceId: {TraceId}", Activity.Current?.Id);

                var traceId = Activity.Current?.Id ?? context.TraceIdentifier;
                var isDevelopment = environment.IsDevelopment();

                var (statusCode, problemDetails) = exception switch
                {
                    FluentValidation.ValidationException validationException => (
                        StatusCodes.Status400BadRequest,
                        CreateValidationProblemDetails(validationException, traceId, isDevelopment)
                    ),
                    NotFoundException notFoundException => (
                        StatusCodes.Status404NotFound,
                        CreateProblemDetails(
                            type: "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                            title: "Resource not found.",
                            status: StatusCodes.Status404NotFound,
                            detail: notFoundException.Message,
                            traceId: traceId,
                            isDevelopment: isDevelopment
                        )
                    ),
              
                    _ => (
                        StatusCodes.Status500InternalServerError,
                        CreateProblemDetails(
                            type: "https://tools.ietf.org/html/rfc7231#section-6.6.1",
                            title: "An unexpected error occurred.",
                            status: StatusCodes.Status500InternalServerError,
                            detail: isDevelopment ? exception.Message : "An unexpected error occurred. Please try again later.",
                            traceId: traceId,
                            isDevelopment: isDevelopment
                        )
                    )
                };

                context.Response.StatusCode = statusCode;
                context.Response.ContentType = "application/problem+json";

                await context.Response.WriteAsJsonAsync(problemDetails, new System.Text.Json.JsonSerializerOptions
                {
                    PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase
                });
            }
        });

        return app;
    }

    private static ProblemDetails CreateProblemDetails(
        string type,
        string title,
        int status,
        string? detail = null,
        string? traceId = null,
        bool isDevelopment = false,
        Dictionary<string, string[]>? errors = null)
    {
        var problemDetails = new ProblemDetails
        {
            Type = type,
            Title = title,
            Status = status,
            Detail = isDevelopment ? detail : null
        };

        if (traceId != null)
            problemDetails.Extensions["traceId"] = traceId;

        if (errors != null)
            problemDetails.Extensions["errors"] = errors;

        return problemDetails;
    }

    private static ProblemDetails CreateValidationProblemDetails(FluentValidation.ValidationException exception, string traceId, bool isDevelopment)
    {
        var errors = exception.Errors
            .GroupBy(e => e.PropertyName)
            .ToDictionary(
                g => g.Key,
                g => g.Select(e => e.ErrorMessage).ToArray()
            );

        return CreateProblemDetails(
            type: "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            title: "One or more validation errors occurred.",
            status: StatusCodes.Status400BadRequest,
            detail: isDevelopment ? $"Validation failed for {errors.Count} field(s)." : null,
            traceId: traceId,
            isDevelopment: isDevelopment,
            errors: errors
        );
    }
}

public class GlobalExceptionHandler
{
}