using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Dtos;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Web.Services;

namespace Web.Controllers
{
    [ApiController]
    [Route("travelexpenses")]
    public class TravelExpenseController : ControllerBase
    {
        private readonly IGetTravelExpenseService _getTravelExpenseService;
        private readonly IUpdateTravelExpenseService _updateTravelExpenseService;
        private readonly ICreateTravelExpenseService _createTravelExpenseService;
        private readonly ISubManagementService _subManagementService;
        private readonly IProcessStepTravelExpenseService _processStepTravelExpenseService;

        public TravelExpenseController(IProcessStepTravelExpenseService processStepTravelExpenseService,
            IGetTravelExpenseService getTravelExpenseService,
            IUpdateTravelExpenseService updateTravelExpenseService,
            ICreateTravelExpenseService createTravelExpenseService,
            ISubManagementService subManagementService)
        {
            _processStepTravelExpenseService = processStepTravelExpenseService;
            _getTravelExpenseService = getTravelExpenseService;
            _updateTravelExpenseService = updateTravelExpenseService;
            _createTravelExpenseService = createTravelExpenseService;
            _subManagementService = subManagementService;
        }

        [HttpPost("{id}/ProcessStep/{processStepKey}")]
        public async Task<ActionResult<TravelExpenseProcessStepResponse>> Process(Guid id, string processStepKey)
        {
            var travelExpenseProcessStepDto = new TravelExpenseProcessStepDto {TravelExpenseId = id, ProcessStepKey = processStepKey};
            var sub = _subManagementService.GetSub(User);

            var travelExpenseDtos = await _processStepTravelExpenseService.ProcessStepAsync(travelExpenseProcessStepDto, sub);

            return Ok(travelExpenseDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<TravelExpenseDto>>> GetById(Guid id)
        {
            var sub = _subManagementService.GetSub(User);
            var travelExpenseDto = await _getTravelExpenseService.GetByIdAsync(id, sub);

            return Ok(travelExpenseDto);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TravelExpenseDto>>> Get()
        {
            var sub = _subManagementService.GetSub(User);
            var travelExpenseDtos = await _getTravelExpenseService.GetAsync(sub);

            return Ok(travelExpenseDtos);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<TravelExpenseUpdateResponse>> Put(Guid id,
            TravelExpenseUpdateDto travelExpenseUpdateDto)
        {
            var sub = _subManagementService.GetSub(User);
            var travelExpenseDtos = await _updateTravelExpenseService.UpdateAsync(id, travelExpenseUpdateDto, sub);

            return Ok(travelExpenseDtos);
        }

        [HttpPost]
        public async Task<ActionResult<TravelExpenseCreateResponse>> Post(TravelExpenseCreateDto travelExpenseCreateDto)
        {
            var sub = _subManagementService.GetSub(User);
            var travelExpenseDtos = await _createTravelExpenseService.CreateAsync(travelExpenseCreateDto,sub);

            return Created(nameof(GetById), travelExpenseDtos);
        }
    }
}