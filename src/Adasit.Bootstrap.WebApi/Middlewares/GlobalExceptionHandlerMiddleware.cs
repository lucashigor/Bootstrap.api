using System.Net;
using System.Net.Mime;
using Adasit.Bootstrap.Application.Dto;
using Adasit.Bootstrap.Application.Dto.Models;
using Adasit.Bootstrap.Application.Dto.Models.Errors;
using Newtonsoft.Json;

namespace Adasit.Bootstrap.WebApi.Middlewares;

public class GlobalExceptionHandlerMiddleware : IMiddleware
{
    private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

    public GlobalExceptionHandlerMiddleware(ILogger<GlobalExceptionHandlerMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (BusinessException ex)
        {
            _logger.LogError(ex, $"Ocorreu um erro de processamento | {context.Request.Path.Value}");
            await HandleExceptionAsync(context, ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Ocorreu um erro inesperado | {context.Request.Path.Value}");
            await HandleExceptionAsync(context);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, BusinessException exception)
    {
        return HandleErrorResponseAsync(context,
            exception.StatusCode,
            exception.ContentType,
            exception.ErrorCode);
    }

    private Task HandleExceptionAsync(HttpContext context)
    {
        return HandleErrorResponseAsync(context,
            HttpStatusCode.InternalServerError,
            MediaTypeNames.Application.Json,
            ErrorCodeConstant.Generic());
    }

    private Task HandleErrorResponseAsync(
        HttpContext context,
        HttpStatusCode httpStatusCode,
        string mediaType,
        ErrorModel errorCode)
    {
        context.Response.StatusCode = (int)httpStatusCode;
        context.Response.ContentType = mediaType;

        var response = new DefaultResponseDto<object>();

        response.Errors.Add(errorCode);
        return context.Response.WriteAsync(JsonConvert.SerializeObject(response));
    }
}