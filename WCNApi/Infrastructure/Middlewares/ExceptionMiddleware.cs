
using System.Net;
using WCNApi.Infrastructure.Errors;

namespace WCNApi.Infrastructure.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            string controllerName = Convert.ToString(context.GetRouteData().Values["controller"]);
            ErrorCode errorCode = string.IsNullOrEmpty(controllerName) ? ErrorCode.NoError : (ErrorCode)Enum.Parse(typeof(ErrorCode), controllerName);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            var response = new ErrorResponse(errorCode, exception.Message);
            return context.Response.WriteAsJsonAsync(response);
        }
    }
}
