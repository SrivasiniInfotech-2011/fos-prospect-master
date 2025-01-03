using AutoMapper;
using FOS.Infrastructure;
using FOS.Infrastructure.Commands;
using FOS.Infrastructure.Queries;
using FOS.Models.Constants;
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
    [Authorize]
    public class ProspectsController : FOSControllerBase
    {
        public ProspectsController(IMediator mediator, ILogger<ProspectsController> logger, IMapper mapper) : base(mediator, logger, mapper)
        {
        }

        /// <summary>
        /// Gets the Prospect Lookup.
        /// </summary>
        /// <returns>A <see cref="Task{IActionResult}"/> representing the result of the asynchronous operation.</returns>
        /// <response code="200">Returns the user's requests as a byte array.</response>
        /// <response code="400">If the query is invalid or the message handler response status is not OK.</response>
        /// <response code="401">Returns if the user is unauthorized.</response>
        /// <response code="500">If an internal server error occurs.</response>
        [HttpGet]
        [Route("GetProspectLookup")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(byte[]), StatusCodes.Status200OK, Web.ContentType.Json)]
        [ProducesResponseType(typeof(FOSBaseResponse), StatusCodes.Status400BadRequest, Web.ContentType.Json)]
        [ProducesResponseType(typeof(FOSBaseResponse), StatusCodes.Status500InternalServerError, Web.ContentType.Json)]

        public async Task<IActionResult> GetProspectLookup()
        {
            try
            {
                var query = new GetProspectLookups.Query();
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
        [HttpPost]
        [Route("GetBranchLocations")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(byte[]), StatusCodes.Status200OK, Web.ContentType.Json)]
        [ProducesResponseType(typeof(FOSBaseResponse), StatusCodes.Status400BadRequest, Web.ContentType.Json)]
        [ProducesResponseType(typeof(FOSBaseResponse), StatusCodes.Status500InternalServerError, Web.ContentType.Json)]

        public async Task<IActionResult> GetBranchLocations(BranchLocationRequest branchLocationRequest)
        {
            try
            {
                var query = new GetBranchLocations.Query(branchLocationRequest.UserId, branchLocationRequest.CompanyId, branchLocationRequest.LobId, branchLocationRequest.IsActive);
                var branchLocations = await FOSMediator.Send(query);

                return Ok(new FOSResponse
                {
                    Status = Status.Success,
                    Message = branchLocations
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
        [HttpPost]
        [Route("GetExistingProspectDetailsForCustomer")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(byte[]), StatusCodes.Status200OK, Web.ContentType.Json)]
        [ProducesResponseType(typeof(FOSBaseResponse), StatusCodes.Status400BadRequest, Web.ContentType.Json)]
        [ProducesResponseType(typeof(FOSBaseResponse), StatusCodes.Status500InternalServerError, Web.ContentType.Json)]

        public async Task<IActionResult> GetExistingProspectDetailsForCustomer(GetProspectDetailsRequest prospectRequest)
        {
            try
            {
                var query = new GetExistingProspectCustomerDetails.Query(prospectRequest.UserId, prospectRequest.CompanyId, prospectRequest.ProspectId, prospectRequest.MobileNumber, prospectRequest.AadharNumber, prospectRequest.PanNumber);
                var existingProspectDetail = await FOSMediator.Send(query);

                if (existingProspectDetail != null && string.IsNullOrEmpty(existingProspectDetail.ProspectCode))
                {
                    return ErrorResponse(new Models.Responses.FOSMessageResponse
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Error = new FOSErrorResponse { Message = Constants.Messages.NO_RECORDS_FOUND,ValidationErrors=new Dictionary<string, string[]>() },

                    });
                }
                return Ok(new FOSResponse
                {
                    Status = Status.Success,
                    Message = existingProspectDetail
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
        [Route("CreateNewProspect")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(byte[]), StatusCodes.Status200OK, Web.ContentType.Json)]
        [ProducesResponseType(typeof(FOSBaseResponse), StatusCodes.Status400BadRequest, Web.ContentType.Json)]
        [ProducesResponseType(typeof(FOSBaseResponse), StatusCodes.Status500InternalServerError, Web.ContentType.Json)]

        public async Task<IActionResult> CreateNewProspect(CreateProspectRequestModel prospectRequest)
        {
            try
            {
                var command = new CreateProspectCommand.Command(prospectRequest);
                var response = await FOSMediator.Send(command);
                return GenerateResponse(response);
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
        /// Generate Response based on SaveStatus.
        /// </summary>
        /// <param name="response">Response from the Stored Procedure.</param>
        /// <returns>response of type <see cref="IActionResult"/></returns>
        private IActionResult GenerateResponse(int response)
        {
            if (response == (int)SaveStatus.OK)
                return Ok(new FOSResponse
                {
                    Status = Status.Success,
                    Message = Constants.Messages.PROSPECT_ADDED_SUCCESSFULLY
                });
            else if (response == (int)SaveStatus.AADHARALREADYEXISTS)
                return new BadRequestObjectResult(new Models.Responses.FOSMessageResponse
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Error = new FOSErrorResponse { Message = Constants.Messages.PROSPECT_AADHAR_ALREADY_EXISTS }
                });
            else if (response == (int)SaveStatus.PANALREADYEXISTS)
                return new BadRequestObjectResult(new Models.Responses.FOSMessageResponse
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    Error = new FOSErrorResponse { Message = Constants.Messages.PROSPECT_PAN_ALREADY_EXISTS }
                });
            return new BadRequestResult();
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
        [Route("GetStates")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(byte[]), StatusCodes.Status200OK, Web.ContentType.Json)]
        [ProducesResponseType(typeof(FOSBaseResponse), StatusCodes.Status400BadRequest, Web.ContentType.Json)]
        [ProducesResponseType(typeof(FOSBaseResponse), StatusCodes.Status500InternalServerError, Web.ContentType.Json)]

        public async Task<IActionResult> GetStates()
        {
            try
            {
                var query = new GetStates.Query(1);
                var states = await FOSMediator.Send(query);

                return Ok(new FOSResponse
                {
                    Status = Status.Success,
                    Message = states
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


        [HttpPost]
        [Route("GetCompanyMasterDetails")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(byte[]), StatusCodes.Status200OK, Web.ContentType.Json)]
        [ProducesResponseType(typeof(FOSBaseResponse), StatusCodes.Status400BadRequest, Web.ContentType.Json)]
        [ProducesResponseType(typeof(FOSBaseResponse), StatusCodes.Status500InternalServerError, Web.ContentType.Json)]

        public async Task<IActionResult> GetCompanyMasterDetails(GetCompanyRequestModel customerRequest)    
        {
            try
            {
                var query = new GetCompanyMaster.Query(customerRequest.CompanyId); 
                var existingProspectDetail = await FOSMediator.Send(query);

                return Ok(new FOSResponse
                {
                    Status = Status.Success,
                    Message = existingProspectDetail
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
        [Route("GetDocumentCategories")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(byte[]), StatusCodes.Status200OK, Web.ContentType.Json)]
        [ProducesResponseType(typeof(FOSBaseResponse), StatusCodes.Status400BadRequest, Web.ContentType.Json)]
        [ProducesResponseType(typeof(FOSBaseResponse), StatusCodes.Status500InternalServerError, Web.ContentType.Json)]

        public async Task<IActionResult> GetDocumentCategories(int companyId,int userId)
        {
            try
            {
                var query = new GetDocumentCategories.Query(companyId,userId,(int)DocumentCategoryOptions.LOAN_DOCUMENTS);
                var states = await FOSMediator.Send(query);

                return Ok(new FOSResponse
                {
                    Status = Status.Success,
                    Message = states
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
        [Route("ExportProspectData")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(byte[]), StatusCodes.Status200OK, Web.ContentType.Json)]
        [ProducesResponseType(typeof(FOSBaseResponse), StatusCodes.Status400BadRequest, Web.ContentType.Json)]
        [ProducesResponseType(typeof(FOSBaseResponse), StatusCodes.Status500InternalServerError, Web.ContentType.Json)]

        public async Task<IActionResult> ExportProspectData(string fileOutputType)
        {
            try
            {
                var outputContentType = fileOutputType.Equals("EXCEL", StringComparison.OrdinalIgnoreCase) ? "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" : "application/pdf";
                var extension = fileOutputType.Equals("EXCEL", StringComparison.OrdinalIgnoreCase) ? "xlsx" : "pdf";
                var query = new DownloadProspectReport.Query(fileOutputType);
                var prospectFile = await FOSMediator.Send(query);
                return new FileStreamResult(prospectFile, outputContentType) { FileDownloadName = $"PROSPECT_DATA_{DateTime.Now.ToString("ddMMyyyyhhmmss")}.{extension}" };
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
