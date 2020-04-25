using System.Collections.Generic;
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
        public async Task<ActionResult<IEnumerable<FlowStepDto>>> Get()
        {
            var sub = _subManagementService.GetSub(User);
            var travelExpenseDtos = await _getFlowStepService.GetAsync(sub);

            return Ok(travelExpenseDtos);
        }
    }
}