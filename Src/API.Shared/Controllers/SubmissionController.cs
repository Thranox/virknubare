using System.Threading.Tasks;
using API.Shared.Services;
using Application;
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
        private ISubmitSubmissionService _submitSubmissionService;

        public SubmissionController(ISubManagementService subManagementService,
            ICreateSubmissionService createSubmissionService, ISubmitSubmissionService submitSubmissionService)
        {
            _subManagementService = subManagementService;
            _createSubmissionService = createSubmissionService;
            _submitSubmissionService = submitSubmissionService;
        }

        [HttpPost("")]
        public async Task<ActionResult<SubmissionPostResponse>> Post()
        {
            var polApiContext = await _subManagementService.GetPolApiContext(HttpContext);

            var submissionPostResponse = await _createSubmissionService.CreateAsync(polApiContext);

            return Ok(submissionPostResponse);
        }

        [HttpPost("Tbd")]
        public async Task<ActionResult<SubmissionPostResponse>> PostTbd()
        {
            var polApiContext = await _subManagementService.GetPolApiContext(HttpContext);

            var travelExpenseDtos = await _submitSubmissionService.SubmitAsync(polApiContext);

            return Ok(travelExpenseDtos);
        }
    }

}
