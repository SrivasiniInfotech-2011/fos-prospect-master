using AutoMapper;
using FOS.Infrastructure.Commands;
using FOS.Infrastructure.Queries;
using FOS.Models.Constants;
using FOS.Models.Entities;
using FOS.Models.Requests;
using FOS.Models.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static FOS.Models.Constants.Constants;

namespace FOS.Prospects.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class LeadsController(IMediator mediator, ILogger<LeadsController> logger, IMapper mapper) : FOSControllerBase(mediator, logger, mapper)
    {

        /// <summary>
        /// Gets the List of Statuses.
        /// </summary>
        /// <returns>A <see cref="Task{IActionResult}"/> representing the result of the asynchronous operation.</returns>
        /// <response code="200">Returns the user's requests as a byte array.</response>
        /// <response code="400">If the query is invalid or the message handler response status is not OK.</response>
        /// <response code="401">Returns if the user is unauthorized.</response>
        /// <response code="500">If an internal server error occurs.</response>
        [HttpGet]
        [Route("GetLeadStatuses")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(byte[]), StatusCodes.Status200OK, Web.ContentType.Json)]
        [ProducesResponseType(typeof(FOSBaseResponse), StatusCodes.Status400BadRequest, Web.ContentType.Json)]
        [ProducesResponseType(typeof(FOSBaseResponse), StatusCodes.Status500InternalServerError, Web.ContentType.Json)]

        public async Task<IActionResult> GetLeadStatuses()
        {
            try
            {
                var query = new GetLeadStatuses.Query();
                var leadStatuses = await FOSMediator.Send(query);

                return Ok(new FOSResponse
                {
                    Status = Status.Success,
                    Message = leadStatuses
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
        /// Gets the List of Asset Lookups.
        /// </summary>
        /// <returns>A <see cref="Task{IActionResult}"/> representing the result of the asynchronous operation.</returns>
        /// <response code="200">Returns the user's requests as a byte array.</response>
        /// <response code="400">If the query is invalid or the message handler response status is not OK.</response>
        /// <response code="401">Returns if the user is unauthorized.</response>
        /// <response code="500">If an internal server error occurs.</response>
        [HttpGet]
        [Route("GetAssetLookup")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(byte[]), StatusCodes.Status200OK, Web.ContentType.Json)]
        [ProducesResponseType(typeof(FOSBaseResponse), StatusCodes.Status400BadRequest, Web.ContentType.Json)]
        [ProducesResponseType(typeof(FOSBaseResponse), StatusCodes.Status500InternalServerError, Web.ContentType.Json)]

        public async Task<IActionResult> GetAssetLookup(int companyId, int userId)
        {
            try
            {
                var query = new GetAssetLookup.Query(userId, companyId);
                var assetLookups = await FOSMediator.Send(query);

                return Ok(new FOSResponse
                {
                    Status = Status.Success,
                    Message = assetLookups
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
        /// Gets the Lead Details.
        /// </summary>
        /// <returns>A <see cref="Task{IActionResult}"/> representing the result of the asynchronous operation.</returns>
        /// <response code="200">Returns the user's requests as a byte array.</response>
        /// <response code="400">If the query is invalid or the message handler response status is not OK.</response>
        /// <response code="401">Returns if the user is unauthorized.</response>
        /// <response code="500">If an internal server error occurs.</response>
        [HttpGet]
        [Route("GetLeadDetails")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(byte[]), StatusCodes.Status200OK, Web.ContentType.Json)]
        [ProducesResponseType(typeof(FOSBaseResponse), StatusCodes.Status400BadRequest, Web.ContentType.Json)]
        [ProducesResponseType(typeof(FOSBaseResponse), StatusCodes.Status500InternalServerError, Web.ContentType.Json)]

        public async Task<IActionResult> GetLeadDetails(int companyId, int userId, int? leadId = null, string? vehicleNumber = null, string? leadNumber = null)
        {
            try
            {
                var query = new GetLeadDetails.Query(companyId, userId, leadId.GetValueOrDefault(), vehicleNumber, leadNumber);
                var leadDetail = await FOSMediator.Send(query);

                return Ok(new FOSResponse
                {
                    Status = Status.Success,
                    Message = leadDetail
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
        /// Gets the List of Lookups for the Lead Generation Screen.
        /// </summary>
        /// <returns>A <see cref="Task{IActionResult}"/> representing the result of the asynchronous operation.</returns>
        /// <response code="200">Returns the user's requests as a byte array.</response>
        /// <response code="400">If the query is invalid or the message handler response status is not OK.</response>
        /// <response code="401">Returns if the user is unauthorized.</response>
        /// <response code="500">If an internal server error occurs.</response>
        [HttpGet]
        [Route("GetLeadGenerationLookup/{companyId}/{userId}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(byte[]), StatusCodes.Status200OK, Web.ContentType.Json)]
        [ProducesResponseType(typeof(FOSBaseResponse), StatusCodes.Status400BadRequest, Web.ContentType.Json)]
        [ProducesResponseType(typeof(FOSBaseResponse), StatusCodes.Status500InternalServerError, Web.ContentType.Json)]

        public async Task<IActionResult> GetLeadGenerationLookup(int companyId, int userId)
        {
            try
            {
                var query = new GetLeadGenerationLookup.Query(companyId, userId);
                var leadGenerationLookups = await FOSMediator.Send(query);

                return Ok(new FOSResponse
                {
                    Status = Status.Success,
                    Message = leadGenerationLookups
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
        /// Gets the Prospect Details for the Lead.
        /// </summary>
        /// <returns>A <see cref="Task{IActionResult}"/> representing the result of the asynchronous operation.</returns>
        /// <response code="200">Returns the user's requests as a byte array.</response>
        /// <response code="400">If the query is invalid or the message handler response status is not OK.</response>
        /// <response code="401">Returns if the user is unauthorized.</response>
        /// <response code="500">If an internal server error occurs.</response>
        [HttpPost]
        [Route("GetProspectDetailsForLead")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(byte[]), StatusCodes.Status200OK, Web.ContentType.Json)]
        [ProducesResponseType(typeof(FOSBaseResponse), StatusCodes.Status400BadRequest, Web.ContentType.Json)]
        [ProducesResponseType(typeof(FOSBaseResponse), StatusCodes.Status500InternalServerError, Web.ContentType.Json)]

        public async Task<IActionResult> GetProspectDetailsForLead(GetProspectDetailsRequest request)
        {
            try
            {
                var query = new GetProspectDetailsForLead.Query(request.UserId, request.CompanyId, request.MobileNumber, request.AadharNumber, request.PanNumber);
                var leadProspectDetail = await FOSMediator.Send(query);

                return Ok(new FOSResponse
                {
                    Status = Status.Success,
                    Message = leadProspectDetail
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
        /// Adds the Guarantor Details to the Lead object.
        /// </summary>
        /// <returns>A <see cref="Task{IActionResult}"/> representing the result of the asynchronous operation.</returns>
        /// <response code="200">Returns the user's requests as a byte array.</response>
        /// <response code="400">If the query is invalid or the message handler response status is not OK.</response>
        /// <response code="401">Returns if the user is unauthorized.</response>
        /// <response code="500">If an internal server error occurs.</response>
        [HttpPost]
        [Route("CreateGuarantorData")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(byte[]), StatusCodes.Status200OK, Web.ContentType.Json)]
        [ProducesResponseType(typeof(FOSBaseResponse), StatusCodes.Status400BadRequest, Web.ContentType.Json)]
        [ProducesResponseType(typeof(FOSBaseResponse), StatusCodes.Status500InternalServerError, Web.ContentType.Json)]

        public async Task<IActionResult> CreateGuarantorData([FromBody] Lead lead)
        {
            try
            {
                var command = new CreateGuarantorData.Command(lead);
                var guarantorAddedSuccessfully = await FOSMediator.Send(command);

                return guarantorAddedSuccessfully ?
                Ok(new FOSResponse
                {
                    Status = Status.Success,
                    Message = Constants.Messages.LEAD_GUARANTOR_ADDED_SUCCESSFULLY.Replace(Constants.LeadIdString, Convert.ToString(lead.Header?.LeadId))
                }) :
                ErrorResponse(new Models.Responses.FOSMessageResponse
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Error = new FOSErrorResponse
                    {
                        Message = Constants.Messages.LEAD_GUARANTOR_ADD_FAILURE.Replace(Constants.LeadIdString, Convert.ToString(lead.Header?.LeadId))
                    }
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
        /// Adds the Lead Details for a Lead.
        /// </summary>
        /// <returns>A <see cref="Task{IActionResult}"/> representing the result of the asynchronous operation.</returns>
        /// <response code="200">Returns the user's requests as a byte array.</response>
        /// <response code="400">If the query is invalid or the message handler response status is not OK.</response>
        /// <response code="401">Returns if the user is unauthorized.</response>
        /// <response code="500">If an internal server error occurs.</response>
        [HttpPost]
        [Route("CreateLeadDetails")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(byte[]), StatusCodes.Status200OK, Web.ContentType.Json)]
        [ProducesResponseType(typeof(FOSBaseResponse), StatusCodes.Status400BadRequest, Web.ContentType.Json)]
        [ProducesResponseType(typeof(FOSBaseResponse), StatusCodes.Status500InternalServerError, Web.ContentType.Json)]

        public async Task<IActionResult> CreateLeadDetails([FromQuery] int companyId, [FromQuery] int userId, [FromQuery] int locationId, [FromBody] LeadHeader leadHeader)
        {
            try
            {
                var command = new CreateLeadDetails.Command(userId, companyId, locationId, leadHeader);
                var leadHeaderObject = await FOSMediator.Send(command);

                return Ok(new FOSResponse
                {
                    Status = Status.Success,
                    Message = leadHeaderObject
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
        /// Adds the Lead Individual Details for a Lead.
        /// </summary>
        /// <returns>A <see cref="Task{IActionResult}"/> representing the result of the asynchronous operation.</returns>
        /// <response code="200">Returns the user's requests as a byte array.</response>
        /// <response code="400">If the query is invalid or the message handler response status is not OK.</response>
        /// <response code="401">Returns if the user is unauthorized.</response>
        /// <response code="500">If an internal server error occurs.</response>
        [HttpPost]
        [Route("CreateLeadIndividualDetails")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(byte[]), StatusCodes.Status200OK, Web.ContentType.Json)]
        [ProducesResponseType(typeof(FOSBaseResponse), StatusCodes.Status400BadRequest, Web.ContentType.Json)]
        [ProducesResponseType(typeof(FOSBaseResponse), StatusCodes.Status500InternalServerError, Web.ContentType.Json)]

        public async Task<IActionResult> CreateLeadIndividualDetails([FromQuery] int companyId, [FromQuery] int userId, [FromQuery] int leadId, [FromBody] LeadIndividualDetail leadIndividualDetail)
        {
            try
            {
                var command = new CreateLeadIndividualDetails.Command(companyId, userId, leadId, leadIndividualDetail);
                var response = await FOSMediator.Send(command);

                return response == 0 ?
                 Ok(new FOSResponse
                 {
                     Status = Status.Success,
                     Message = Constants.Messages.LEAD_INDIVIDUAL_ADDED_SUCCESSFULLY.Replace(Constants.LeadIdString, Convert.ToString(leadId))
                 }) :
                 ErrorResponse(new Models.Responses.FOSMessageResponse
                 {
                     StatusCode = System.Net.HttpStatusCode.BadRequest,
                     Error = new FOSErrorResponse
                     {
                         Message = Constants.Messages.LEAD_NONINDIVIDUAL_ADDED_FAILURE.Replace(Constants.LeadIdString, Convert.ToString(leadId))
                     }
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
        /// Adds the Lead Non-Individual Details for a Lead.
        /// </summary>
        /// <returns>A <see cref="Task{IActionResult}"/> representing the result of the asynchronous operation.</returns>
        /// <response code="200">Returns the user's requests as a byte array.</response>
        /// <response code="400">If the query is invalid or the message handler response status is not OK.</response>
        /// <response code="401">Returns if the user is unauthorized.</response>
        /// <response code="500">If an internal server error occurs.</response>
        [HttpPost]
        [Route("CreateNonIndividualDetail")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(byte[]), StatusCodes.Status200OK, Web.ContentType.Json)]
        [ProducesResponseType(typeof(FOSBaseResponse), StatusCodes.Status400BadRequest, Web.ContentType.Json)]
        [ProducesResponseType(typeof(FOSBaseResponse), StatusCodes.Status500InternalServerError, Web.ContentType.Json)]

        public async Task<IActionResult> CreateNonIndividualDetail([FromQuery] int userId, [FromQuery] int leadId, [FromBody] LeadNonIndividualDetail leadNonIndividualDetail)
        {
            try
            {
                var command = new CreateNonIndividualDetail.Command(userId, leadId, leadNonIndividualDetail);
                var response = await FOSMediator.Send(command);

                return response == 0 ?
                 Ok(new FOSResponse
                 {
                     Status = Status.Success,
                     Message = Constants.Messages.LEAD_NONINDIVIDUAL_ADDED_SUCCESSFULLY.Replace(Constants.LeadIdString, Convert.ToString(leadId))
                 }) :
                 ErrorResponse(new Models.Responses.FOSMessageResponse
                 {
                     StatusCode = System.Net.HttpStatusCode.BadRequest,
                     Error = new FOSErrorResponse
                     {
                         Message = Constants.Messages.LEAD_NONINDIVIDUAL_ADDED_FAILURE.Replace(Constants.LeadIdString, Convert.ToString(leadId))
                     }
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
        /// Adds the Lead Header Details for a Lead.
        /// </summary>
        /// <returns>A <see cref="Task{IActionResult}"/> representing the result of the asynchronous operation.</returns>
        /// <response code="200">Returns the user's requests as a byte array.</response>
        /// <response code="400">If the query is invalid or the message handler response status is not OK.</response>
        /// <response code="401">Returns if the user is unauthorized.</response>
        /// <response code="500">If an internal server error occurs.</response>
        [HttpPost]
        [Route("CreatetLeadGenerationHeader")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(byte[]), StatusCodes.Status200OK, Web.ContentType.Json)]
        [ProducesResponseType(typeof(FOSBaseResponse), StatusCodes.Status400BadRequest, Web.ContentType.Json)]
        [ProducesResponseType(typeof(FOSBaseResponse), StatusCodes.Status500InternalServerError, Web.ContentType.Json)]

        public async Task<IActionResult> CreatetLeadGenerationHeader([FromBody] Lead lead)
        {
            try
            {
                var command = new CreatetLeadGenerationHeader.Command(lead);
                var response = await FOSMediator.Send(command);

                return response == 0 ?
                 Ok(new FOSResponse
                 {
                     Status = Status.Success,
                     Message = Constants.Messages.LEAD_HEADER_ADDED_SUCCESSFULLY.Replace(Constants.LeadIdString, Convert.ToString(lead.Header?.LeadId))
                 }) :
                 ErrorResponse(new Models.Responses.FOSMessageResponse
                 {
                     StatusCode = System.Net.HttpStatusCode.BadRequest,
                     Error = new FOSErrorResponse
                     {
                         Message = Constants.Messages.LEAD_HEADER_ADDED_FAILURE.Replace(Constants.LeadIdString, Convert.ToString(lead.Header?.LeadId))
                     }
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
        /// Gets the Lead Translander Details.
        /// </summary>
        /// <returns>A <see cref="Task{IActionResult}"/> representing the result of the asynchronous operation.</returns>
        /// <response code="200">Returns the user's requests as a byte array.</response>
        /// <response code="400">If the query is invalid or the message handler response status is not OK.</response>
        /// <response code="401">Returns if the user is unauthorized.</response>
        /// <response code="500">If an internal server error occurs.</response>
        [HttpPost]
        [Route("GetLeadTranslanderDetails")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(byte[]), StatusCodes.Status200OK, Web.ContentType.Json)]
        [ProducesResponseType(typeof(FOSBaseResponse), StatusCodes.Status400BadRequest, Web.ContentType.Json)]
        [ProducesResponseType(typeof(FOSBaseResponse), StatusCodes.Status500InternalServerError, Web.ContentType.Json)]

        public async Task<IActionResult> GetLeadTranslanderDetails(GetLeadTranslanderRequest request)
        {
            try
            {
                var query = new GetLeadsForTranslander.Query(request.CompanyId, request.UserId, request.CurrentPage, request.PageSize, request.SearchValue, request.VehicleNumber, request.LeadNumber, request.Status);
                var leadTranslanderDetail = await FOSMediator.Send(query);

                return Ok(new FOSResponse
                {
                    Status = Status.Success,
                    Message = leadTranslanderDetail
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
