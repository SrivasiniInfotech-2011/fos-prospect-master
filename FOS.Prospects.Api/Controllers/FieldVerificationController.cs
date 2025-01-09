using AutoMapper;
using FOS.Infrastructure.Commands;
using FOS.Infrastructure.Queries;
using FOS.Models.Constants;
using FOS.Models.Entities;
using FOS.Models.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static FOS.Models.Constants.Constants;

namespace FOS.Prospects.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FieldVerificationController(IMediator mediator, ILogger<FieldVerificationController> logger, IMapper mapper) : FOSControllerBase(mediator, logger, mapper)
    {

        /// <summary>
        /// Gets the FVR Hirer Lookup.
        /// </summary>
        /// <returns>A <see cref="Task{IActionResult}"/> representing the result of the asynchronous operation.</returns>
        /// <response code="200">Returns the user's requests as a byte array.</response>
        /// <response code="400">If the query is invalid or the message handler response status is not OK.</response>
        /// <response code="401">Returns if the user is unauthorized.</response>
        /// <response code="500">If an internal server error occurs.</response>
        [HttpGet]
        [Route("GetFvrHirerLookup")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(byte[]), StatusCodes.Status200OK, Web.ContentType.Json)]
        [ProducesResponseType(typeof(FOSBaseResponse), StatusCodes.Status400BadRequest, Web.ContentType.Json)]
        [ProducesResponseType(typeof(FOSBaseResponse), StatusCodes.Status500InternalServerError, Web.ContentType.Json)]

        public async Task<IActionResult> GetFvrHirerLookup(int? companyId, int? userId)
        {
            try
            {
                var query = new GetFvrHirerLookup.Query(userId, companyId);
                var lookup = await FOSMediator.Send(query);

                return Ok(new FOSResponse
                {
                    Status = Status.Success,
                    Message = lookup
                });
            }
            catch (Exception ex)
            {
                return ErrorResponse(new Models.Responses.FOSMessageResponse
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Error = new FOSErrorResponse { Exception = ex }

                });
            }
        }

        /// <summary>
        /// Gets the FVR Neighbour Lookup.
        /// </summary>
        /// <returns>A <see cref="Task{IActionResult}"/> representing the result of the asynchronous operation.</returns>
        /// <response code="200">Returns the user's requests as a byte array.</response>
        /// <response code="400">If the query is invalid or the message handler response status is not OK.</response>
        /// <response code="401">Returns if the user is unauthorized.</response>
        /// <response code="500">If an internal server error occurs.</response>
        [HttpGet]
        [Route("GetFvrNeighbourLookup")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(byte[]), StatusCodes.Status200OK, Web.ContentType.Json)]
        [ProducesResponseType(typeof(FOSBaseResponse), StatusCodes.Status400BadRequest, Web.ContentType.Json)]
        [ProducesResponseType(typeof(FOSBaseResponse), StatusCodes.Status500InternalServerError, Web.ContentType.Json)]

        public async Task<IActionResult> GetFvrNeighbourLookup(int? companyId, int? userId)
        {
            try
            {
                var query = new GetFvrNeighbourLookup.Query(userId, companyId);
                var lookup = await FOSMediator.Send(query);

                return Ok(new FOSResponse
                {
                    Status = Status.Success,
                    Message = lookup
                });
            }
            catch (Exception ex)
            {
                return ErrorResponse(new Models.Responses.FOSMessageResponse
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Error = new FOSErrorResponse { Exception = ex }

                });
            }
        }

        /// <summary>
        /// Gets the Branch Locations.
        /// </summary>
        /// <param name="branchLocationRequest">Branch Location Request Object.</param>
        /// <returns>A <see cref="Task{IActionResult}"/> representing the result of the asynchronous operation.</returns>
        /// <response code="200">Returns the user's requests as a byte array.</response>
        /// <response code="400">If the query is invalid or the message handler response status is not OK.</response>
        /// <response code="401">Returns if the user is unauthorized.</response>
        /// <response code="500">If an internal server error occurs.</response>
        [HttpGet]
        [Route("GetFvrAssetLookup")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(byte[]), StatusCodes.Status200OK, Web.ContentType.Json)]
        [ProducesResponseType(typeof(FOSBaseResponse), StatusCodes.Status400BadRequest, Web.ContentType.Json)]
        [ProducesResponseType(typeof(FOSBaseResponse), StatusCodes.Status500InternalServerError, Web.ContentType.Json)]

        public async Task<IActionResult> GetFvrAssetLookup(int? companyId, int? userId)
        {
            try
            {
                var query = new GetFvrAssetLookup.Query(userId, companyId);
                var lookup = await FOSMediator.Send(query);

                return Ok(new FOSResponse
                {
                    Status = Status.Success,
                    Message = lookup
                });
            }
            catch (Exception ex)
            {
                return ErrorResponse(new Models.Responses.FOSMessageResponse
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Error = new FOSErrorResponse { Exception = ex }

                });
            }
        }

        /// <summary>
        /// Gets the Prospect Detail.
        /// <param name="prospectRequest"></param>
        /// </summary>
        /// <returns>A <see cref="Task{IActionResult}"/> representing the result of the asynchronous operation.</returns>
        /// <response code="200">Returns the user's requests as a byte array.</response>
        /// <response code="400">If the query is invalid or the message handler response status is not OK.</response>
        /// <response code="401">Returns if the user is unauthorized.</response>
        /// <response code="500">If an internal server error occurs.</response>
        [HttpGet]
        [Route("GetFvrNeighbourHoodDetails")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(byte[]), StatusCodes.Status200OK, Web.ContentType.Json)]
        [ProducesResponseType(typeof(FOSBaseResponse), StatusCodes.Status400BadRequest, Web.ContentType.Json)]
        [ProducesResponseType(typeof(FOSBaseResponse), StatusCodes.Status500InternalServerError, Web.ContentType.Json)]

        public async Task<IActionResult> GetFvrNeighbourHoodDetails(int? companyId, int? userId, int? leadId, int? fieldVerificationId)
        {
            try
            {
                var query = new GetFvrNeighbourHoodDetails.Query(userId, companyId, fieldVerificationId, leadId);
                var fvrDetail = await FOSMediator.Send(query);

                if (fvrDetail != null && fvrDetail.FvrNeighbourHood != null)
                {
                    return ErrorResponse(new Models.Responses.FOSMessageResponse
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Error = new FOSErrorResponse { Message = Constants.Messages.NO_RECORDS_FOUND, ValidationErrors = new Dictionary<string, string[]>() },

                    });
                }
                return Ok(new FOSResponse
                {
                    Status = Status.Success,
                    Message = fvrDetail
                });
            }
            catch (Exception ex)
            {
                return ErrorResponse(new Models.Responses.FOSMessageResponse
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Error = new FOSErrorResponse { Exception = ex }

                });
            }
        }

        /// <summary>
        /// Gets the Prospect Detail.
        /// <param name="prospectRequest"></param>
        /// </summary>
        /// <returns>A <see cref="Task{IActionResult}"/> representing the result of the asynchronous operation.</returns>
        /// <response code="200">Returns the user's requests as a byte array.</response>
        /// <response code="400">If the query is invalid or the message handler response status is not OK.</response>
        /// <response code="401">Returns if the user is unauthorized.</response>
        /// <response code="500">If an internal server error occurs.</response>
        [HttpGet]
        [Route("GetLeadAssetDetails")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(byte[]), StatusCodes.Status200OK, Web.ContentType.Json)]
        [ProducesResponseType(typeof(FOSBaseResponse), StatusCodes.Status400BadRequest, Web.ContentType.Json)]
        [ProducesResponseType(typeof(FOSBaseResponse), StatusCodes.Status500InternalServerError, Web.ContentType.Json)]

        public async Task<IActionResult> GetLeadAssetDetails(int? companyId, int? userId, string? leadNumber, string? vehicleNumber)
        {
            try
            {
                var query = new GetLeadAssetDetails.Query(userId, companyId, leadNumber, vehicleNumber);
                var fvrAsset = await FOSMediator.Send(query);

                if (fvrAsset == null)
                {
                    return ErrorResponse(new Models.Responses.FOSMessageResponse
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Error = new FOSErrorResponse { Message = Constants.Messages.NO_RECORDS_FOUND, ValidationErrors = new Dictionary<string, string[]>() },

                    });
                }
                return Ok(new FOSResponse
                {
                    Status = Status.Success,
                    Message = fvrAsset
                });
            }
            catch (Exception ex)
            {
                return ErrorResponse(new Models.Responses.FOSMessageResponse
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Error = new FOSErrorResponse { Exception = ex }

                });
            }
        }

        /// <summary>
        /// Gets the Prospect Detail.
        /// <param name="prospectRequest"></param>
        /// </summary>
        /// <returns>A <see cref="Task{IActionResult}"/> representing the result of the asynchronous operation.</returns>
        /// <response code="200">Returns the user's requests as a byte array.</response>
        /// <response code="400">If the query is invalid or the message handler response status is not OK.</response>
        /// <response code="401">Returns if the user is unauthorized.</response>
        /// <response code="500">If an internal server error occurs.</response>
        [HttpGet]
        [Route("GetLeadHirerDetails")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(byte[]), StatusCodes.Status200OK, Web.ContentType.Json)]
        [ProducesResponseType(typeof(FOSBaseResponse), StatusCodes.Status400BadRequest, Web.ContentType.Json)]
        [ProducesResponseType(typeof(FOSBaseResponse), StatusCodes.Status500InternalServerError, Web.ContentType.Json)]

        public async Task<IActionResult> GetLeadHirerDetails(int? companyId, int? userId, string? mode, string? leadNumber, string? vehicleNumber)
        {
            try
            {
                var query = new GetLeadHirerDetails.Query(userId, companyId, mode, leadNumber, vehicleNumber);
                var fvrDetail = await FOSMediator.Send(query);

                if (fvrDetail == null)
                {
                    return ErrorResponse(new Models.Responses.FOSMessageResponse
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Error = new FOSErrorResponse { Message = Constants.Messages.NO_RECORDS_FOUND, ValidationErrors = new Dictionary<string, string[]>() },

                    });
                }
                return Ok(new FOSResponse
                {
                    Status = Status.Success,
                    Message = fvrDetail
                });
            }
            catch (Exception ex)
            {
                return ErrorResponse(new Models.Responses.FOSMessageResponse
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Error = new FOSErrorResponse { Exception = ex }

                });
            }
        }


        /// <summary>
        /// Creates a New Prospect.
        /// </summary>
        /// <param name="prospectRequest">Prospect Request.</param>
        /// <returns>A <see cref="Task{IActionResult}"/> representing the result of the asynchronous operation.</returns>
        /// <response code="200">Returns the user's requests as a byte array.</response>
        /// <response code="400">If the query is invalid or the message handler response status is not OK.</response>
        /// <response code="401">Returns if the user is unauthorized.</response>
        /// <response code="500">If an internal server error occurs.</response>
        [HttpPost]
        [Route("AddFvrHirerDetail")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(byte[]), StatusCodes.Status200OK, Web.ContentType.Json)]
        [ProducesResponseType(typeof(FOSBaseResponse), StatusCodes.Status400BadRequest, Web.ContentType.Json)]
        [ProducesResponseType(typeof(FOSBaseResponse), StatusCodes.Status500InternalServerError, Web.ContentType.Json)]

        public async Task<IActionResult> AddFvrHirerDetail([FromQuery]int? companyId, [FromQuery]int? leadId, [FromBody]FvrDetail fvrDetail)
        {
            try
            {
                var command = new AddFvrHirerDetail.Command(companyId, leadId, fvrDetail);
                var response = await FOSMediator.Send(command);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return ErrorResponse(new Models.Responses.FOSMessageResponse
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Error = new FOSErrorResponse { Exception = ex }
                });
            }
        }

        /// <summary>
        /// Creates a New Prospect.
        /// </summary>
        /// <param name="prospectRequest">Prospect Request.</param>
        /// <returns>A <see cref="Task{IActionResult}"/> representing the result of the asynchronous operation.</returns>
        /// <response code="200">Returns the user's requests as a byte array.</response>
        /// <response code="400">If the query is invalid or the message handler response status is not OK.</response>
        /// <response code="401">Returns if the user is unauthorized.</response>
        /// <response code="500">If an internal server error occurs.</response>
        [HttpPost]
        [Route("AddFvrAssetDetail")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(byte[]), StatusCodes.Status200OK, Web.ContentType.Json)]
        [ProducesResponseType(typeof(FOSBaseResponse), StatusCodes.Status400BadRequest, Web.ContentType.Json)]
        [ProducesResponseType(typeof(FOSBaseResponse), StatusCodes.Status500InternalServerError, Web.ContentType.Json)]

        public async Task<IActionResult> AddFvrAssetDetail([FromQuery] int? companyId, [FromQuery] int? userId, [FromQuery] int? leadId, [FromBody] FvrAssetDetail fvrAssetDetail)
        {
            try
            {
                var command = new AddFvrAssetDetail.Command(companyId, userId, leadId, fvrAssetDetail);
                var response = await FOSMediator.Send(command);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return ErrorResponse(new Models.Responses.FOSMessageResponse
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Error = new FOSErrorResponse { Exception = ex }
                });
            }
        }


        /// <summary>
        /// Gets the Fvr Guarantor Details.
        /// </summary>
        /// <returns>A <see cref="Task{IActionResult}"/> representing the result of the asynchronous operation.</returns>
        /// <response code="200">Returns the user's requests as a byte array.</response>
        /// <response code="400">If the query is invalid or the message handler response status is not OK.</response>
        /// <response code="401">Returns if the user is unauthorized.</response>
        /// <response code="500">If an internal server error occurs.</response>
        [HttpGet]
        [Route("GetFvrGuarantorDetails")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(byte[]), StatusCodes.Status200OK, Web.ContentType.Json)]
        [ProducesResponseType(typeof(FOSBaseResponse), StatusCodes.Status400BadRequest, Web.ContentType.Json)]
        [ProducesResponseType(typeof(FOSBaseResponse), StatusCodes.Status500InternalServerError, Web.ContentType.Json)]

        public async Task<IActionResult> GetFvrGuarantorDetails(int? companyId, int? userId, int? leadId, int? personType)
        {
            try
            {
                var query = new GetFvrDetails.Query(userId, companyId, leadId,personType);
                var fvrDetail = await FOSMediator.Send(query);

                if (fvrDetail == null)
                {
                    return ErrorResponse(new Models.Responses.FOSMessageResponse
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Error = new FOSErrorResponse { Message = Constants.Messages.NO_RECORDS_FOUND, ValidationErrors = new Dictionary<string, string[]>() },

                    });
                }
                return Ok(new FOSResponse
                {
                    Status = Status.Success,
                    Message = fvrDetail
                });
            }
            catch (Exception ex)
            {
                return ErrorResponse(new Models.Responses.FOSMessageResponse
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Error = new FOSErrorResponse { Exception = ex }

                });
            }
        }
    }
}
