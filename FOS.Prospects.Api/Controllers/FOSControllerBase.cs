using AutoMapper;
using FOS.Models.Exceptions;
using FOS.Models.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FOS.Prospects.Api.Controllers;

/// <summary>Base class from which all other controllers are derived</summary>
/// <summary>
/// Base class for all controllers, providing common functionality.
/// </summary>
/// <param name="mediator">The mediator instance for handling requests.</param>
/// <param name="logger">The logger instance for logging information.</param>
public class FOSControllerBase(IMediator mediator, ILogger<FOSControllerBase> logger) : ControllerBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FOSControllerBase"/> class.
    /// </summary>
    /// <param name="mediator">The mediator instance for handling requests.</param>
    /// <param name="logger">The logger instance for logging information.</param>
    /// <param name="mapper">The mapper instance for object mapping.</param>
    public FOSControllerBase(IMediator mediator, ILogger<FOSControllerBase> logger, IMapper mapper) : this(mediator, logger) => HLObjectMapper = mapper;

    /// <summary>Gets the <see cref="IMediator"/> instance</summary>
    protected IMediator FOSMediator => mediator;

    /// <summary>
    /// Gets the <see cref="IMapper"/> instance
    /// </summary>
    private IMapper HLObjectMapper { get; } = default!;

    /// <summary>
    /// Generates an error response based on the provided command response.
    /// </summary>
    /// <param name="response">The response from the handler containing error details.</param>
    /// <returns>An <see cref="IActionResult"/> representing the error response.</returns>
    protected IActionResult ErrorResponse(FOSMessageResponse response)
    {
        var methodName = $"{GetType().Name}.{nameof(ErrorResponse)}";

        logger.LogInformation("{MethodName}: Message handler response status is not OK", methodName);
        logger.LogDebug("{MethodName}: Error in message handler: {Error}", methodName,JsonConvert.SerializeObject(response.Error));

        if (response.IsRequestValid) return BuildErrorResponse(new FOSMessageHandlerException(response));

        logger.LogInformation("{MethodName}: Request validation failed.", methodName);
        logger.LogDebug("{MethodName}: Validation errors: {ValidationErrors}", methodName, response.Error.ValidationErrors);

        return BuildErrorResponse(new FOSValidationException(response.Error.ValidationErrors));
    }

    /// <summary>
    /// Builds an error response based on the provided exception.
    /// </summary>
    /// <param name="exception">The exception to process.</param>
    /// <returns>An <see cref="IActionResult"/> containing the error response.</returns>
    private ObjectResult BuildErrorResponse(FOSException exception)
    {
        // Get the current method name for logging purposes
        var methodName = $"{GetType().Name}.{nameof(BuildErrorResponse)}";

        // Map the exception to a base response object
        var response = HLObjectMapper.Map<FOSBaseResponse>(exception);

        // Handle different types of exceptions
        switch (exception)
        {
            case FOSValidationException validationEx:
                // Create a response for validation exceptions
                response = new FOSBaseResponse
                {
                    StatusCode = validationEx.StatusCode,
                    Error = new FOSErrorResponse { Message = validationEx.Message, ValidationErrors = validationEx.ValidationErrors }
                };
                break;
            case FOSMessageHandlerException messageHandlerEx:
                // Log the error for message handler exceptions
                logger.LogDebug("{MethodName}: Error in message handler: {Error}", methodName, JsonConvert.SerializeObject(messageHandlerEx));

                // Map the message handler exception to a response
                response = HLObjectMapper.Map<FOSBaseResponse>(messageHandlerEx);
                response.Error.AdditionalData = new Dictionary<string, object>
                {
                    { "requestType", messageHandlerEx.Response.RequestType?.Name ?? String.Empty },
                    { "request", JsonConvert.SerializeObject(messageHandlerEx.Response.Request)}
                };
                break;
            default:
                // Map the exception to an inner error response for other exception types
                response.Error.InnerError = HLObjectMapper.Map<FOSErrorResponse>(exception);
                break;
        }

        // Return the error response with the appropriate status code
        return StatusCode((int)response.StatusCode, response);
    }
}