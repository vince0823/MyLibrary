using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MyLibrary.Business.Errors;
using MyLibrary.Business.Filter;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Data.SqlClient;
using System.Net;
using System.Threading.Tasks;

namespace MyLibrary.WebApi.Middleware
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

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, _logger);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception ex, ILogger<ExceptionMiddleware> logger)
        {
            ApiResult resultRetrrn = new ApiResult();

            switch (ex)
            {
                case RestException re:


                    resultRetrrn = resultRetrrn.SetFailedResult(((int)re.ErrorCode).ToString(), re.ErrorMessage);
                    logger.LogError("REST ERROR: {error}. {ex}", re.ErrorMessage, ex);
                    context.Response.StatusCode = (int)re.ErrorCode;
                    break;

                case Exception e:
                    var receivedException = e.InnerException ?? e;
                    logger.LogError("SERVER ERROR: {receivedException.Message}. {ex}", receivedException.Message, ex);
                    var isSqlError = receivedException is SqlException;
                    var sqlError = receivedException as SqlException;
                    var errorCode = (int)HttpStatusCode.InternalServerError;
                    var errorType = HttpStatusCode.InternalServerError.ToString();
                    var errorMessage = HttpStatusCode.InternalServerError.ToString();

                    if (!string.IsNullOrWhiteSpace(receivedException.Message))
                        errorMessage = receivedException.Message;

                    if (isSqlError && sqlError!.Number == 547)
                    {
                        errorCode = (int)HttpStatusCode.Conflict;
                        errorType = HttpStatusCode.Conflict.ToString();
                        errorMessage = "This item cannot be deleted.";
                        context.Response.StatusCode = (int)HttpStatusCode.Conflict;
                    }
                    else
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                    resultRetrrn = resultRetrrn.SetFailedResult(errorCode.ToString(), errorMessage);
                    break;
            }

            context.Response.ContentType = "application/json";
            if (resultRetrrn != null)
            {
                var result = JsonConvert.SerializeObject(resultRetrrn, Formatting.Indented, new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });
                await context.Response.WriteAsync(result);
            }
        }
    }
}
