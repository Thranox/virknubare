using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Dtos;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Web.Controllers
{
    [ApiController]
    [Route("travelexpenses")]
    public class TravelExpenseController : ControllerBase
    {
        private readonly IServiceProvider _serviceProvider;

        public TravelExpenseController(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        [HttpPost("{id}/ProcessStep/{processStepKey}")]
        public async Task<ActionResult<TravelExpenseProcessStepResponse>> Process(Guid id, string processStepKey)
        {
            var travelExpenseProcessStepDto=new TravelExpenseProcessStepDto(){TravelExpenseId = id,ProcessStepKey = processStepKey};
            var travelExpenseDtos = await _serviceProvider
                .GetService<IProcessStepTravelExpenseService>()
                .ProcessStepAsync(travelExpenseProcessStepDto);

            return Ok(travelExpenseDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<TravelExpenseDto>>> GetById(Guid id)
        {
            var travelExpenseDto = await _serviceProvider
                .GetService<IGetTravelExpenseService>()
                .GetByIdAsync(id);

            return Ok(travelExpenseDto);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TravelExpenseDto>>> Get()
        {
            var travelExpenseDtos = await _serviceProvider
                .GetService<IGetTravelExpenseService>()
                .GetAsync();

            return Ok(travelExpenseDtos);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<TravelExpenseUpdateResponse>> Put(Guid id, TravelExpenseUpdateDto travelExpenseUpdateDto)
        {
            var travelExpenseDtos = await _serviceProvider
                .GetService<IUpdateTravelExpenseService>()
                .UpdateAsync(id, travelExpenseUpdateDto);

            return Ok(travelExpenseDtos);
        }

        [HttpPost]
        public async Task<ActionResult<TravelExpenseCreateResponse>> Post(TravelExpenseCreateDto travelExpenseCreateDto)
        {
            var travelExpenseDtos = await _serviceProvider
                .GetService<ICreateTravelExpenseService>()
                .CreateAsync(travelExpenseCreateDto);

            return Created(nameof(GetById), travelExpenseDtos);
        }
    }
}