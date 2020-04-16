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
    [Route("[controller]")]
    public class TravelExpenseController : ControllerBase
    {
        private readonly IServiceProvider _serviceProvider;

        public TravelExpenseController(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        [HttpPost]
        [Route("ProcessStep")]
        public async Task<ActionResult<TravelExpenseProcessStepResponse>> Process(TravelExpenseProcessStepDto travelExpenseProcessStepDto)
        {
            var travelExpenseDtos = await _serviceProvider
                .GetService<IProcessStepTravelExpenseService>()
                .ProcessStepAsync(travelExpenseProcessStepDto);

            return Ok(travelExpenseDtos);
        }

        [HttpGet]
        [Route("GetById")]
        public async Task<ActionResult<IEnumerable<TravelExpenseDto>>> GetById(Guid id)
        {
            var travelExpenseDto = await _serviceProvider
                .GetService<IGetTravelExpenseService>()
                .GetByIdAsync(id);

            return Ok(travelExpenseDto);
        }

        [HttpGet]
        [Route("Get")]
        public async Task<ActionResult<IEnumerable<TravelExpenseDto>>> Get()
        {
            var travelExpenseDtos = await _serviceProvider
                .GetService<IGetTravelExpenseService>()
                .GetAsync();

            return Ok(travelExpenseDtos);
        }

        [HttpPut]
        [Route("Put")]
        public async Task<ActionResult<TravelExpenseUpdateResponse>> Put(TravelExpenseUpdateDto travelExpenseUpdateDto)
        {
            var travelExpenseDtos = await _serviceProvider
                .GetService<IUpdateTravelExpenseService>()
                .UpdateAsync(travelExpenseUpdateDto);

            return Ok(travelExpenseDtos);
        }

        [HttpPost]
        [Route("Post")]
        public async Task<ActionResult<TravelExpenseCreateResponse>> Post(TravelExpenseCreateDto travelExpenseCreateDto)
        {
            var travelExpenseDtos = await _serviceProvider
                .GetService<ICreateTravelExpenseService>()
                .CreateAsync(travelExpenseCreateDto);

            return Created(nameof(GetById), travelExpenseDtos);
        }
    }
}