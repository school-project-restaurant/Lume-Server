using System.Diagnostics;

namespace Lume.Middlewares;

public class RequestTimeLoggingMiddleware(ILogger<RequestTimeLoggingMiddleware> logger) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var stopwatch = Stopwatch.StartNew();

        await next.Invoke(context);

        stopwatch.Stop();
        if (stopwatch.ElapsedMilliseconds >= 100)
            logger.LogInformation("[{Protocol}] [{Verb}] at {Path} responded {StatusCode} {StatusName} took {Time}ms",
                context.Request.Protocol, context.Request.Method,
                context.Request.Path, context.Response.StatusCode, GetStatusCodeName(context.Response.StatusCode),
                stopwatch.ElapsedMilliseconds);
    }

    private string GetStatusCodeName(int statusCode)
    {
        return statusCode switch
        {
            StatusCodes.Status100Continue => "Continue",
            StatusCodes.Status101SwitchingProtocols => "Switching Protocols",
            StatusCodes.Status200OK => "OK",
            StatusCodes.Status201Created => "Created",
            StatusCodes.Status202Accepted => "Accepted",
            203 => "Non-Authoritative Information", //StatusCodes.Status203NonAuthoritativeInformation
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
            505 => "HTTP Version Not Supported", // StatusCodes.Status505HttpVersionNotSupported
            _ => "Unknown Status Code"
        };
    }
}