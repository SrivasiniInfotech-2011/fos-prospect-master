using AutoMapper;
using FOS.Infrastructure.Services.Utils;
using FOS.Models.Exceptions;
using FOS.Models.Responses;
using static FOS.Models.Constants.Constants;

namespace FOS.Prospects.Api.Middleware;

/// <summary>Middleware for handling thrown exceptions</summary>
public sealed class FOSExceptionMiddleware
{
    private readonly ILogger<FOSExceptionMiddleware> _logger;
    private readonly IMapper _mapper;
    private readonly RequestDelegate _next;

    /// <summary>Initializes the middleware</summary>
    /// <param name="next"><see cref="RequestDelegate"/> handler for the request</param>
    /// <param name="logger"><see cref="ILogger"/> to use for logging the details</param>
    /// <param name="mapper"><see cref="IMapper"/> instance to use for mapping the objects</param>
    public FOSExceptionMiddleware(RequestDelegate next, ILogger<FOSExceptionMiddleware> logger, IMapper mapper)
    {
        _next = next;
        _logger = logger;
        _mapper = mapper;
    }

    /// <summary>Processes the request</summary>
    /// <param name="httpContext"><see cref="HttpContent"/> for the request</param>
    /// <returns><see cref="Task"/></returns>
    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    /// <summary>Handles the thrown exception</summary>
    /// <param name="httpContext"><see cref="HttpContext"/> of the request</param>
    /// <param name="sourceEx"><see cref="Exception"/> that was thrown</param>
    /// <returns><see cref="Task"/></returns>
    /// <exception cref="NotImplementedException"></exception>
    private async Task HandleExceptionAsync(HttpContext httpContext, Exception sourceEx)
    {
        var methodName = $"{GetType().Name}.{AppUtil.GetCurrentMethodName()}";

        _logger.LogInformation("{MethodName}: Exception thrown, handling and returning error response", methodName);

        // #Localize
        var response = _mapper.Map<FOSBaseResponse>(sourceEx);

        switch (sourceEx)
        {
            case FOSValidationException validationEx:
                _logger.LogDebug("{MethodName}: Validation error: {Error}", methodName, validationEx.ToJson());

                response.Error.ValidationErrors = validationEx.ValidationErrors;
                break;

            case FOSConfigurationException configurationEx:
                _logger.LogDebug("{MethodName}: Error in configuration: {Error}", methodName, configurationEx.ToJson());
                break;

            case FOSMessageHandlerException messageHandlerEx:
                _logger.LogDebug("{MethodName}: Error in message handler: {Error}", methodName, messageHandlerEx.ToJson());

                response = _mapper.Map<FOSBaseResponse>(messageHandlerEx);
                response.Error.AdditionalData = new Dictionary<string, object>
                {
                    {
                        "requestType", messageHandlerEx.Response.RequestType?.Name ?? String.Empty
                    },
                    {
                        "request", messageHandlerEx.Response.Request.ToJson()
                    }
                };
                break;

            case FOSRestException hlRestEx:
                _logger.LogDebug("{MethodName}: Error in REST service: {Error}", methodName, hlRestEx.ToJson());

                response = _mapper.Map<FOSBaseResponse>(hlRestEx);
                response.Error.InnerError.AdditionalData = new Dictionary<string, object>
                {
                    {
                        "responseData", hlRestEx.ResponseData!
                    }
                };

                break;

            case { }:
                response.Error.InnerError = _mapper.Map<FOSErrorResponse>(sourceEx);
                break;
        }

        httpContext.Response.ContentType = Web.ContentType.Json;
        httpContext.Response.StatusCode = (int)response.StatusCode;

        var contextResponse = response.ToJson();

        _logger.LogDebug("{MethodName}: Error response: {ErrorResponse}", methodName, contextResponse);
        _logger.LogDebug("{MethodName}: Writing error response", methodName);

        await httpContext.Response.WriteAsync(contextResponse);
    }
}