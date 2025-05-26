using System.Diagnostics;
using Microsoft.Extensions.Options;

namespace Lume.Middlewares;

public class RequestTimeLoggingOptions
{
    public int MaxBodyLogSize { get; set; } = 500;
    public bool LogSuccessResponseBody { get; set; } = true;
    public bool LogErrorResponseBody { get; set; } = true;
    public string[] ExcludePaths { get; set; } = [];
}

public class RequestTimeLoggingMiddleware(
    ILogger<RequestTimeLoggingMiddleware> logger,
    IOptions<RequestTimeLoggingOptions>? options = null)
    : IMiddleware
{
    private readonly RequestTimeLoggingOptions _options = options?.Value ?? new RequestTimeLoggingOptions();

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        // Skip logging for excluded paths
        if (_options.ExcludePaths.Any(p => context.Request.Path.StartsWithSegments(p)))
        {
            await next(context);
            return;
        }

        var stopwatch = Stopwatch.StartNew();
        var correlationId = context.TraceIdentifier;
        var protocol = context.Request.Protocol;
        var method = context.Request.Method;
        var path = context.Request.Path;

        // Capture request body for POST, PUT, PATCH
        var requestBody = string.Empty;
        if (HttpMethods.IsPost(method) || HttpMethods.IsPut(method) || HttpMethods.IsPatch(method))
        {
            context.Request.EnableBuffering();
            using var reader = new StreamReader(context.Request.Body, leaveOpen: true);
            requestBody = await reader.ReadToEndAsync();
            
            // Truncate if needed
            if (requestBody.Length > _options.MaxBodyLogSize)
                requestBody = requestBody.Substring(0, _options.MaxBodyLogSize) + "... [truncated]";
                
            context.Request.Body.Position = 0;
        }

        // Replace response stream with buffered stream
        var originalBodyStream = context.Response.Body;
        using var responseBodyStream = new MemoryStream();
        context.Response.Body = responseBodyStream;

        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            logger.LogError(ex,
                "[UnhandledException] [{Protocol}] [{Method}] {Path} failed after {Elapsed}ms with CorrelationId {CorrelationId}",
                protocol, method, path, stopwatch.ElapsedMilliseconds, correlationId);
            throw;
        }
        finally
        {
            stopwatch.Stop();
        }

        var statusCode = context.Response.StatusCode;
        var statusName = GetStatusCodeName(statusCode);
        var statusClass = GetStatusClass(statusCode);
        
        // Capture response body for success or error responses if enabled
        string responseBody = string.Empty;
        if ((statusClass == "Success" && _options.LogSuccessResponseBody) || 
            ((statusClass == "Client Error" || statusClass == "Server Error") && _options.LogErrorResponseBody))
        {
            responseBodyStream.Position = 0;
            responseBody = await new StreamReader(responseBodyStream).ReadToEndAsync();
            
            // Truncate if needed
            if (responseBody.Length > _options.MaxBodyLogSize)
                responseBody = responseBody.Substring(0, _options.MaxBodyLogSize) + "... [truncated]";
        }

        // Copy the response to the original stream
        responseBodyStream.Position = 0;
        await responseBodyStream.CopyToAsync(originalBodyStream);
        context.Response.Body = originalBodyStream;

        logger.LogInformation(
            "[{StatusClass:l}] [{Protocol}] [{Method:u}] {Path} responded {StatusCode} {StatusName:l} in {Elapsed}ms CorrelationId={CorrelationId} {RequestBody} {ResponseBody}",
            statusClass, protocol, ColorizeMethod(method), path, statusCode, ColorizeStatusName(statusName, statusCode), stopwatch.ElapsedMilliseconds, correlationId,
            string.IsNullOrWhiteSpace(requestBody) ? string.Empty : $"RequestBody={requestBody}",
            string.IsNullOrWhiteSpace(responseBody) ? string.Empty : $"ResponseBody={responseBody}");
    }

    private static string GetStatusClass(int statusCode) =>
        statusCode switch
        {
            >= 200 and < 300 => "\u001b[32mSuccess\u001b[0m",
            >= 400 and < 500 => "\u001b[33mClient Error\u001b[0m",
            >= 500 => "\u001b[31mServer Error\u001b[0m",
            _ => "Other"
        };
    
    private string ColorizeMethod(string method) => method switch
    {
        "GET" => $"\u001b[34m{method}\u001b[0m", // Blue
        "POST" => $"\u001b[32m{method}\u001b[0m", // Green
        "PUT" => $"\u001b[33m{method}\u001b[0m", // Yellow
        "PATCH" => $"\u001b[35m{method}\u001b[0m", // Magenta
        "DELETE" => $"\u001b[31m{method}\u001b[0m", // Red
        _ => method
    };
    
    private string ColorizeStatusName(string statusName, int statusCode)
    {
        var statusClass = GetStatusClass(statusCode);
        return statusClass.Contains("Success") ? $"\u001b[32m{statusName}\u001b[0m" : // Green for success
               statusClass.Contains("Client Error") ? $"\u001b[33m{statusName}\u001b[0m" : // Yellow for client error
               statusClass.Contains("Server Error") ? $"\u001b[31m{statusName}\u001b[0m" : // Red for server error
               statusName;
    }

    private static string GetStatusCodeName(int statusCode)
    {
        return statusCode switch
        {
            StatusCodes.Status100Continue => "Continue",
            StatusCodes.Status101SwitchingProtocols => "Switching Protocols",
            StatusCodes.Status200OK => "OK",
            StatusCodes.Status201Created => "Created",
            StatusCodes.Status202Accepted => "Accepted",
            203 => "Non-Authoritative Information",
            StatusCodes.Status204NoContent => "No Content",
            StatusCodes.Status205ResetContent => "Reset Content",
            StatusCodes.Status206PartialContent => "Partial Content",
            StatusCodes.Status300MultipleChoices => "Multiple Choices",
            StatusCodes.Status301MovedPermanently => "Moved Permanently",
            StatusCodes.Status302Found => "Found",
            StatusCodes.Status303SeeOther => "See Other",
            StatusCodes.Status304NotModified => "Not Modified",
            StatusCodes.Status305UseProxy => "Use Proxy",
            StatusCodes.Status307TemporaryRedirect => "Temporary Redirect",
            StatusCodes.Status308PermanentRedirect => "Permanent Redirect",
            StatusCodes.Status400BadRequest => "Bad Request",
            StatusCodes.Status401Unauthorized => "Unauthorized",
            StatusCodes.Status402PaymentRequired => "Payment Required",
            StatusCodes.Status403Forbidden => "Forbidden",
            StatusCodes.Status404NotFound => "Not Found",
            StatusCodes.Status405MethodNotAllowed => "Method Not Allowed",
            StatusCodes.Status406NotAcceptable => "Not Acceptable",
            StatusCodes.Status407ProxyAuthenticationRequired => "Proxy Authentication Required",
            StatusCodes.Status408RequestTimeout => "Request Timeout",
            StatusCodes.Status409Conflict => "Conflict",
            StatusCodes.Status410Gone => "Gone",
            StatusCodes.Status411LengthRequired => "Length Required",
            StatusCodes.Status412PreconditionFailed => "Precondition Failed",
            StatusCodes.Status413PayloadTooLarge => "Payload Too Large",
            StatusCodes.Status414UriTooLong => "URI Too Long",
            StatusCodes.Status415UnsupportedMediaType => "Unsupported Media Type",
            StatusCodes.Status416RangeNotSatisfiable => "Range Not Satisfiable",
            StatusCodes.Status417ExpectationFailed => "Expectation Failed",
            StatusCodes.Status426UpgradeRequired => "Upgrade Required",
            StatusCodes.Status451UnavailableForLegalReasons => "Unavailable For Legal Reasons",
            StatusCodes.Status500InternalServerError => "Internal Server Error",
            StatusCodes.Status501NotImplemented => "Not Implemented",
            StatusCodes.Status502BadGateway => "Bad Gateway",
            StatusCodes.Status503ServiceUnavailable => "Service Unavailable",
            StatusCodes.Status504GatewayTimeout => "Gateway Timeout",
            505 => "HTTP Version Not Supported",
            _ => "Unknown"
        };
    }
}