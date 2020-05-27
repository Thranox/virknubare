using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Shared.Services;
using Application.Dtos;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Shared.Controllers
{
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

            var sub = _subManagementService.GetSub(User, HttpContext);

            var travelExpenseProcessStepResponse =
                await _flowStepTravelExpenseService.ProcessStepAsync(travelExpenseProcessStepDto, sub);

            return Ok(travelExpenseProcessStepResponse);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TravelExpenseGetByIdResponse>> GetById(Guid id)
        {
            var sub = _subManagementService.GetSub(User, HttpContext);
            var travelExpenseDto = await _getTravelExpenseService.GetByIdAsync(id, sub);

            return Ok(travelExpenseDto);
        }

        [HttpGet]
        public async Task<ActionResult<TravelExpenseGetResponse>> Get()
        {
            var sub = _subManagementService.GetSub(User, HttpContext);
            var travelExpenseDtos = await _getTravelExpenseService.GetAsync(sub);

            return Ok(travelExpenseDtos);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<TravelExpenseUpdateResponse>> Put(Guid id,
            [FromBody] TravelExpenseUpdateDto travelExpenseUpdateDto)
        {
            var sub = _subManagementService.GetSub(User, HttpContext);
            var travelExpenseDtos =
                await _updateTravelExpenseService.UpdateAsync(id, travelExpenseUpdateDto, sub);

            return Ok(travelExpenseDtos);
        }

        [HttpPost]
        public async Task<ActionResult<TravelExpenseCreateResponse>> Post(TravelExpenseCreateDto travelExpenseCreateDto)
        {
            var sub = _subManagementService.GetSub(User, HttpContext);
            var travelExpenseCreateResponse = await _createTravelExpenseService.CreateAsync(travelExpenseCreateDto, sub);

            return Created(nameof(GetById), travelExpenseCreateResponse);
        }
    }
}