using OrderSystemOCS.Domain.Exceptions;
using System.Text.Json;

namespace OrderSystemOCS.WebApi.Exceptions
{
    public sealed class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                switch (error)
                {
                    case DomainException e:
                        // custom application error
                        response.StatusCode = StatusCodes.Status400BadRequest;
                        break;
                    case WebApiException e:
                        // not found error
                        response.StatusCode = StatusCodes.Status404NotFound;
                        break;
                    default:
                        // unhandled error
                        response.StatusCode = StatusCodes.Status500InternalServerError;
                        break;
                }

                var result = JsonSerializer.Serialize(new { message = error?.Message });
                await response.WriteAsync(result);
            }
        }
    }
}
