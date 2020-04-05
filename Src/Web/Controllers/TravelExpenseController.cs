using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Domain;
using Domain.Entities;
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
        public async Task<ActionResult<IEnumerable<TravelExpenseDto>>> Get()
        {
            _logger.Information("TravelExpense.Get() called.");

            await Task.CompletedTask;
            return await Task.FromResult(Ok(new TravelExpenseGetResponse { Result = _repository
                .List<TravelExpenseEntity>()
                .Select(x => _mapper.Map<TravelExpenseDto>(x))
            }));
        }

        [HttpPut]
        public async Task<ActionResult<TravelExpenseUpdateResponse>> Put(TravelExpenseUpdateDto travelExpenseUpdateDto)
        {
            var travelExpenseEntity = _repository
                .List(new TravelExpenseById(travelExpenseUpdateDto.Id))
                .SingleOrDefault();
            if (travelExpenseEntity == null)
                throw new ArgumentException("Travel expense not found by Id: " + travelExpenseUpdateDto.Id);
            travelExpenseEntity.Update(travelExpenseUpdateDto.Description);
            _repository.Update(travelExpenseEntity);
            
            return await Task.FromResult(Ok(new TravelExpenseUpdateResponse { }));
        }

        public async Task<ActionResult< TravelExpenseCreateResponse>> Post(TravelExpenseCreateDto travelExpenseCreateDto)
        {
            var travelExpenseEntity = new TravelExpenseEntity( travelExpenseCreateDto.Description);
            _repository.Add(travelExpenseEntity);

            return await Task.FromResult(Ok(new TravelExpenseCreateResponse
            {
                Id=travelExpenseEntity.Id
            }));
        }

        public async Task<ActionResult<TravelExpenseApproveResponse>> Approve(TravelExpenseApproveDto travelExpenseApproveDto)
        {
            var travelExpenseEntity = _repository
                .List(new TravelExpenseById(travelExpenseApproveDto.Id))
                .SingleOrDefault();
            if (travelExpenseEntity == null)
                throw new ArgumentException("Travel expense not found by Id: " + travelExpenseApproveDto.Id);
            travelExpenseEntity.Approve();
            _repository.Update(travelExpenseEntity);

            return await Task.FromResult(Ok(new TravelExpenseApproveResponse { }));
        }

        public async Task<ActionResult<TravelExpenseReportDoneResponse>> ReportDone(TravelExpenseReportDoneDto travelExpenseReportDoneDto)
        {
            var travelExpenseEntity = _repository
                .List(new TravelExpenseById(travelExpenseReportDoneDto.Id))
                .SingleOrDefault();
            if (travelExpenseEntity == null)
                throw new ArgumentException("Travel expense not found by Id: " + travelExpenseReportDoneDto.Id);
            travelExpenseEntity.ReportDone();
            _repository.Update(travelExpenseEntity);

            return await Task.FromResult(Ok(new TravelExpenseReportDoneResponse { }));
        }

    }

}
