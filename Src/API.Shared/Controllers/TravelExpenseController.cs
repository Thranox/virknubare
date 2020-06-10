using System;
using System.Threading.Tasks;
using API.Shared.Services;
using Application.Dtos;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Shared.Controllers
{
    [Authorize]
    [ApiController]
    [Route("travelexpenses")]
    public class TravelExpenseController : ControllerBase
    {
        private readonly ICreateTravelExpenseService _createTravelExpenseService;
        private readonly IGetTravelExpenseService _getTravelExpenseService;
        private readonly IFlowStepTravelExpenseService _flowStepTravelExpenseService;
        private readonly ISubManagementService _subManagementService;
        private readonly IUpdateTravelExpenseService _updateTravelExpenseService;

        public TravelExpenseController(IFlowStepTravelExpenseService flowStepTravelExpenseService,
            IGetTravelExpenseService getTravelExpenseService,
            IUpdateTravelExpenseService updateTravelExpenseService,
            ICreateTravelExpenseService createTravelExpenseService,
            ISubManagementService subManagementService)
        {
            _flowStepTravelExpenseService = flowStepTravelExpenseService;
            _getTravelExpenseService = getTravelExpenseService;
            _updateTravelExpenseService = updateTravelExpenseService;
            _createTravelExpenseService = createTravelExpenseService;
            _subManagementService = subManagementService;
        }

        [HttpPost("{id}/FlowStep/{flowStepId}")]
        public async Task<ActionResult<TravelExpenseProcessStepResponse>> Process(Guid id, Guid flowStepId)
        {
            var travelExpenseProcessStepDto = new TravelExpenseFlowStepDto
                {TravelExpenseId = id, FlowStepId = flowStepId};

            var polApiContext = await _subManagementService
                .GetPolApiContext(HttpContext);

            var travelExpenseProcessStepResponse = await _flowStepTravelExpenseService
                .ProcessStepAsync(travelExpenseProcessStepDto, polApiContext);

            return Ok(travelExpenseProcessStepResponse);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TravelExpenseGetByIdResponse>> GetById(Guid id)
        {
            var polApiContext = await _subManagementService.GetPolApiContext(HttpContext);

            var travelExpenseDto = await _getTravelExpenseService.GetByIdAsync(polApiContext, id);

            return Ok(travelExpenseDto);
        }

        [HttpGet]
        public async Task<ActionResult<TravelExpenseGetResponse>> Get()
        {
            var polApiContext = await _subManagementService.GetPolApiContext(HttpContext);

            var travelExpenseDtos = await _getTravelExpenseService.GetAsync(polApiContext);

            return Ok(travelExpenseDtos);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<TravelExpenseUpdateResponse>> Put(Guid id,
            [FromBody] TravelExpenseUpdateDto travelExpenseUpdateDto)
        {
            var polApiContext = await _subManagementService.GetPolApiContext(HttpContext);
            var travelExpenseDtos =
                await _updateTravelExpenseService.UpdateAsync(polApiContext, id, travelExpenseUpdateDto);

            return Ok(travelExpenseDtos);
        }

        [HttpPost]
        public async Task<ActionResult<TravelExpenseCreateResponse>> Post(TravelExpenseCreateDto travelExpenseCreateDto)
        {
            var polApiContext = await _subManagementService.GetPolApiContext(HttpContext);
            var travelExpenseCreateResponse = await _createTravelExpenseService.CreateAsync(polApiContext, travelExpenseCreateDto);

            return Created(nameof(GetById), travelExpenseCreateResponse);
        }
    }
}