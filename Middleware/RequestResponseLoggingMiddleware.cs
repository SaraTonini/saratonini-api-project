using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

public class RequestResponseLoggingMiddleware
{
    private readonly RequestDelegate _next;

    public RequestResponseLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Log request details
        var request = context.Request;
        var method = request.Method;
        var path = request.Path;
        Console.WriteLine($"Request: {method} {path}");

        // Capture the response
        var originalBodyStream = context.Response.Body;
        using (var responseBody = new MemoryStream())
        {
            context.Response.Body = responseBody;
            await _next(context);

            // Log response details
            var statusCode = context.Response.StatusCode;
            Console.WriteLine($"Response: {statusCode}");

            await responseBody.CopyToAsync(originalBodyStream);
        }
    }
}
