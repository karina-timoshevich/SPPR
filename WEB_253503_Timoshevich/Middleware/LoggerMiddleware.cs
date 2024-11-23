namespace WEB_253503_Timoshevich.UI.Middleware;

using Serilog;
using System.Diagnostics;

public class LoggerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<LoggerMiddleware> _logger;

    public LoggerMiddleware(RequestDelegate next, ILogger<LoggerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        _logger.LogInformation($"Request: {context.Request.Method} {context.Request.Path}");

        await _next(context);

        var statusCode = context.Response.StatusCode;

        if (statusCode % 200 < 100)
        {
            _logger.LogInformation(
                $"Response: {context.Request.Method} {context.Request.Path} responded with {context.Response.StatusCode}");
        }
        else
        {
            _logger.LogError(
                $"Bad response: {context.Request.Method} {context.Request.Path} responded with {context.Response.StatusCode}");
        }
    }
}