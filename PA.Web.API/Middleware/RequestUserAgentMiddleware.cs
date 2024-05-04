using MyCSharp.HttpUserAgentParser;
using MyCSharp.HttpUserAgentParser.Providers;

namespace PA.Web.API.Middleware
{

    //  See: https://schwabencode.com/blog/2021/06/18/aspnetcore-parse-http-user-agent
    public class RequestUserAgentMiddleware
    {
        private readonly RequestDelegate _next;
        private IHttpUserAgentParserProvider _parserProvider;

        //  mmust include minimum public constructor with a parameter of type RequestDelegate.
        public RequestUserAgentMiddleware(RequestDelegate next, IHttpUserAgentParserProvider parserProvider)
        {
            _next = next;
            _parserProvider = parserProvider;
        }

        //  Must include a public method named Invoke or InvokeAsync. This method must:
        //  Return a Task.
        //  Accept a first parameter of type HttpContext.
        public async Task InvokeAsync(HttpContext context)
        {
            var userAgent = context.Request.Headers["User-Agent"].ToString();
            if(!string.IsNullOrEmpty(userAgent))
            {
                HttpUserAgentInformation? info = _parserProvider.Parse(userAgent);
                if(info != null)
                {
                    // Add the OS
                    context.Items["Device-Type"] = info.Value.MobileDeviceType;
                    context.Items["Device-OS"] = info.Value.Platform;
                }                
            }

            // Call the next delegate/middleware in the pipeline.
            await _next(context);

        }

    }

    //  An extension method is created to expose the middleware through IApplicationBuilder:
    //  See Program.cs
    public static class RequestUserAgentMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestUserAgent(
       this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestUserAgentMiddleware>();
        }
    }
}
