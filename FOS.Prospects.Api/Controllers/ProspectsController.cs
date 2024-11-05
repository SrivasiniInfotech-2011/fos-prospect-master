using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FOS.Prospects.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProspectsController : FOSControllerBase
    {
        public ProspectsController(IMediator mediator, ILogger<ProspectsController> logger) : base(mediator, logger)
        {

        }
    }
}
