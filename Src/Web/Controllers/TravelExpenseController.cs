using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Dtos;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [ApiController]
    [Route("travelexpenses")]
    public class TravelExpenseController : ControllerBase
    {
        private readonly IGetTravelExpenseService _getTravelExpenseService;
        private readonly IUpdateTravelExpenseService _updateTravelExpenseService;
        private readonly ICreateTravelExpenseService _createTravelExpenseService;
        private readonly IProcessStepTravelExpenseService _processStepTravelExpenseService;

        public TravelExpenseController(IProcessStepTravelExpenseService processStepTravelExpenseService,
            IGetTravelExpenseService getTravelExpenseService,
            IUpdateTravelExpenseService updateTravelExpenseService,
            ICreateTravelExpenseService createTravelExpenseService)
        {
            _processStepTravelExpenseService = processStepTravelExpenseService;
            _getTravelExpenseService = getTravelExpenseService;
            _updateTravelExpenseService = updateTravelExpenseService;
            _createTravelExpenseService = createTravelExpenseService;
        }

        [HttpPost("{id}/ProcessStep/{processStepKey}")]
        public async Task<ActionResult<TravelExpenseProcessStepResponse>> Process(Guid id, string processStepKey)
        {
            var travelExpenseProcessStepDto = new TravelExpenseProcessStepDto {TravelExpenseId = id, ProcessStepKey = processStepKey};
            var travelExpenseDtos = await _processStepTravelExpenseService.ProcessStepAsync(travelExpenseProcessStepDto);

            return Ok(travelExpenseDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<TravelExpenseDto>>> GetById(Guid id)
        {
            var travelExpenseDto = await _getTravelExpenseService.GetByIdAsync(id);

            return Ok(travelExpenseDto);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TravelExpenseDto>>> Get()
        {
            var travelExpenseDtos = await _getTravelExpenseService.GetAsync();

            return Ok(travelExpenseDtos);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<TravelExpenseUpdateResponse>> Put(Guid id,
            TravelExpenseUpdateDto travelExpenseUpdateDto)
        {
            var travelExpenseDtos = await _updateTravelExpenseService.UpdateAsync(id, travelExpenseUpdateDto);

            return Ok(travelExpenseDtos);
        }

        [HttpPost]
        public async Task<ActionResult<TravelExpenseCreateResponse>> Post(TravelExpenseCreateDto travelExpenseCreateDto)
        {
            var travelExpenseDtos = await _createTravelExpenseService.CreateAsync(travelExpenseCreateDto);

            return Created(nameof(GetById), travelExpenseDtos);
        }
    }
}