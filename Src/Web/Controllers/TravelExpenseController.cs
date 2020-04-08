using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Entities;
using Domain.Specifications;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Web.ApiModels;

namespace Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TravelExpenseController : ControllerBase
    {
        private readonly Serilog.ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public TravelExpenseController(Serilog.ILogger logger, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("Get")]
        public async Task<ActionResult<IEnumerable<TravelExpenseDto>>> Get()
        {
            try
            {
                _logger.Information("TravelExpense.Get() called.");

                return await Task.FromResult(Ok(new TravelExpenseGetResponse
                {
                    Result =_unitOfWork
                        .Repository
                        .List<TravelExpenseEntity>()
                        .Select(x => _mapper.Map<TravelExpenseDto>(x))
                }));
            }
            catch (Exception e)
            {
                _logger.Error(e, "During " + MethodBase.GetCurrentMethod().Name);
                return await Task.FromResult(BadRequest());
            }
        }

        [HttpPut]
        [Route("Put")]
        public async Task<ActionResult<TravelExpenseUpdateResponse>> Put(TravelExpenseUpdateDto travelExpenseUpdateDto)
        {
            try
            {
                var travelExpenseEntity =_unitOfWork
                    .Repository
                    .List(new TravelExpenseById(travelExpenseUpdateDto.Id))
                    .SingleOrDefault();
                if (travelExpenseEntity == null)
                    throw new ArgumentException("Travel expense not found by Id: " + travelExpenseUpdateDto.Id);
                travelExpenseEntity.Update(travelExpenseUpdateDto.Description);
               _unitOfWork
                   .Repository
                   .Update(travelExpenseEntity);

                _unitOfWork.Commit();
                return await Task.FromResult(Ok(new TravelExpenseUpdateResponse { }));
            }
            catch (Exception e)
            {
                _logger.Error(e, "During " + MethodBase.GetCurrentMethod().Name);
                return await Task.FromResult(BadRequest());
            }
        }

        [HttpPost]
        [Route("Post")]
        public async Task<ActionResult< TravelExpenseCreateResponse>> Post(TravelExpenseCreateDto travelExpenseCreateDto)
        {
            try
            {
                var travelExpenseEntity = new TravelExpenseEntity(travelExpenseCreateDto.Description);
                _unitOfWork
                    .Repository
                    .Add(travelExpenseEntity);

                _unitOfWork.Commit();

                return await Task.FromResult(Ok(new TravelExpenseCreateResponse
                {
                    Id = travelExpenseEntity.Id
                }));
            }
            catch (Exception e)
            {
                _logger.Error(e, "During " + MethodBase.GetCurrentMethod().Name);
                return await Task.FromResult(BadRequest());
            }
        }

        [HttpPost]
        [Route("Approve")]
        public async Task<ActionResult<TravelExpenseApproveResponse>> Approve(TravelExpenseApproveDto travelExpenseApproveDto)
        {
            try
            {
                var travelExpenseEntity = _unitOfWork
                    .Repository
                    .List(new TravelExpenseById(travelExpenseApproveDto.Id))
                    .SingleOrDefault();
                if (travelExpenseEntity == null)
                    throw new ArgumentException("Travel expense not found by Id: " + travelExpenseApproveDto.Id);
                travelExpenseEntity.Approve();
                _unitOfWork
                    .Repository
                    .Update(travelExpenseEntity);

                _unitOfWork.Commit();

                return await Task.FromResult(Ok(new TravelExpenseApproveResponse { }));
            }
            catch (Exception e)
            {
                _logger.Error(e, "During " + MethodBase.GetCurrentMethod().Name);
                return await Task.FromResult(BadRequest());
            }
        }

        [HttpPost]
        [Route("ReportDone")]
        public async Task<ActionResult<TravelExpenseReportDoneResponse>> ReportDone(TravelExpenseReportDoneDto travelExpenseReportDoneDto)
        {
            try
            {
                var travelExpenseEntity = _unitOfWork
                    .Repository
                    .List(new TravelExpenseById(travelExpenseReportDoneDto.Id))
                    .SingleOrDefault();
                if (travelExpenseEntity == null)
                    throw new ArgumentException("Travel expense not found by Id: " + travelExpenseReportDoneDto.Id);
                travelExpenseEntity.ReportDone();
                _unitOfWork
                    .Repository
                    .Update(travelExpenseEntity);

                _unitOfWork.Commit();

                return await Task.FromResult(Ok(new TravelExpenseReportDoneResponse { }));
            }
            catch (Exception e)
            {
                _logger.Error(e, "During " + MethodBase.GetCurrentMethod().Name);
                return await Task.FromResult(BadRequest());
            }
        }
    }
}
