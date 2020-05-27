using System.Threading.Tasks;
using API.Shared.Services;
using Application.Dtos;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Shared.Controllers
{
    [ApiController]
    [Route("submissions")]
    public class SubmissionController : ControllerBase
    {
        private readonly ICreateSubmissionService _createSubmissionService;
        private readonly ISubManagementService _subManagementService;

        public SubmissionController(ISubManagementService subManagementService,
            ICreateSubmissionService createSubmissionService)
        {
            _subManagementService = subManagementService;
            _createSubmissionService = createSubmissionService;
        }

        [HttpPost]
        public async Task<ActionResult<SubmissionPostResponse>> Post()
        {
            var polApiContext = await _subManagementService.GetPolApiContext(HttpContext);

            var travelExpenseDtos = await _createSubmissionService.CreateAsync(polApiContext);

            return Ok(travelExpenseDtos);
        }
    }

}