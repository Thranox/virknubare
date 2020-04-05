using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Domain;
using Domain.Interfaces;
using Domain.Specifications;
using Microsoft.AspNetCore.Mvc;
using Web.ApiModels;

namespace Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TravelExpenseController : ControllerBase
    {
        private readonly Serilog.ILogger _logger;
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public TravelExpenseController(Serilog.ILogger logger, IRepository repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task< IEnumerable<TravelExpenseDto>> Get()
        {
            _logger.Information("TravelExpense.Get() called.");

            await Task.CompletedTask;
            return _repository
                .List<TravelExpenseEntity>()
                .Select(x=>_mapper.Map<TravelExpenseDto>(x));
        }

        [HttpPut]
        public async Task<ActionResult<TravelExpenseUpdateResponse>> Put(TravelExpenseUpdateDto travelExpenseUpdateDto)
        {
            var travelExpenseEntity = _repository
                .List(new TravelExpenseByPublicId(travelExpenseUpdateDto.PublicId))
                .SingleOrDefault();
            if (travelExpenseEntity == null)
                throw new ArgumentException("Travel expense not found by PublicId: " + travelExpenseUpdateDto.PublicId);
            travelExpenseEntity.Update(travelExpenseUpdateDto.Description);
            _repository.Update(travelExpenseEntity);
            
            return await Task.FromResult(Ok(new TravelExpenseUpdateResponse { Result = true }));
        }

        public async Task<ActionResult< TravelExpenseCreateResponse>> Post(TravelExpenseCreateDto travelExpenseCreateDto)
        {
            var travelExpenseEntity = new TravelExpenseEntity( travelExpenseCreateDto.Description);
            _repository.Add(travelExpenseEntity);

            return await Task.FromResult(Ok(new TravelExpenseCreateResponse
            {
                PublicId=travelExpenseEntity.PublicId
            }));
        }
    }

}
