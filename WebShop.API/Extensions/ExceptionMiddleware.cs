using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using WebShop.API.Models;
using WebShop.Core.Models.Exceptions;

namespace WebShop.API.Extensions
{

    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            this._next = next;
            this._logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something Went wrong while processing {context.Request.Path}");
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var apiError = new ApiError
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Internal Server Error. Please Try Again Later."
            };

            switch (ex)
            {
                case NotFoundException notFoundException:
                    apiError.Status = StatusCodes.Status404NotFound;
                    apiError.Title = "Not Found";
                    apiError.Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4";
                    break;
                case BadRequestException badRequestException:
                    apiError.Status = StatusCodes.Status400BadRequest;
                    apiError.Title = "Bad Request";
                    apiError.Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1";
                    break;
                default:
                    break;
            }

            context.Response.StatusCode = apiError.Status;
            context.Response.ContentType = "application/json";
            return context.Response.WriteAsync(JsonConvert.SerializeObject(apiError));
        }

    }

}