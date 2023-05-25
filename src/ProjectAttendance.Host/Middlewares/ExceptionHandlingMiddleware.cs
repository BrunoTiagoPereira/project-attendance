using ProjectAttendance.Core.Exceptions;
using ProjectAttendance.Core.Extensions;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ProjectAttendance.Host.Middlewares
{
    public class ExceptionHandlingMiddleware : IMiddleware
    {
        private const int DEFAULT_STATUS_CODE = 400;
        private const int NOT_FOUND_STATUS_CODE = 404;
        private const int FORBIDDEN_STATUS_CODE = 403;

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            ApiErrorResponse response = null;
            int statusCode = DEFAULT_STATUS_CODE;

            try
            {
                await next(context);
                return;
            }
            catch (DomainException e)
            {
                response = ApiErrorResponse.Error(e.ValidationFailuresMessages);
            }
            catch (Exception e)
            {
                statusCode = GetStatusCodeFromException(e);

                var exceptionsMessages = GetExceptionMessages(e);
                response = ApiErrorResponse.Error(exceptionsMessages);
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;
            await context.Response.WriteAsync(JsonSerializer.Serialize(response, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }));
        }

        private static IEnumerable<string> GetExceptionMessages(Exception e)
        {
            var exceptionMessages = new List<string>();

            exceptionMessages.Add(e.Message);

            if (e.InnerException is not null)
            {
                exceptionMessages.Add(e.InnerException.Message);
            }

            return exceptionMessages;
        }

        private int GetStatusCodeFromException(Exception e)
        {
            switch (e)
            {
                case NotFoundException:
                    return NOT_FOUND_STATUS_CODE;

                case NotAuthorizedException:
                    return FORBIDDEN_STATUS_CODE;

                default:
                    return (int)HttpStatusCode.InternalServerError;
            }
        }
    }

    public class ApiErrorResponse
    {
        [JsonPropertyName("message")]
        public string Message { get; set; }

        private ApiErrorResponse(string message)
        {
            Message = message;
        }

        public static ApiErrorResponse Error(IEnumerable<string> errorMessages) => new(errorMessages.ToErrorMessage());
    }
}