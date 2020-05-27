using System.Threading.Tasks;
using API.Shared.Services;
using Application.Dtos;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Shared.Controllers
{
    [ApiController]
    [Route("flowsteps")]
    public class FlowStepController : ControllerBase
    {
        private readonly IGetFlowStepService _getFlowStepService;
        private readonly ISubManagementService _subManagementService;

        public FlowStepController(ISubManagementService subManagementService, IGetFlowStepService getFlowStepService)
        {
            _subManagementService = subManagementService;
            _getFlowStepService = getFlowStepService;
        }

        [HttpGet]
        public async Task<ActionResult<FlowStepGetResponse>> Get()
        {
            var sub = _subManagementService.GetSub(User, HttpContext);
            var flowStepGetResponse = await _getFlowStepService.GetAsync(sub);

            return Ok(flowStepGetResponse);
        }
    }
}