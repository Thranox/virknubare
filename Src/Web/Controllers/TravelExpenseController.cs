using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using Domain;
using Domain.Entities;
using Domain.Specifications;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Web.ApiModels;

namespace Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TravelExpenseController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public TravelExpenseController(ILogger logger, IMapper mapper, IUnitOfWork unitOfWork)
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
                    Result = _unitOfWork
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
                var travelExpenseEntity = _unitOfWork
                    .Repository
                    .List(new TravelExpenseById(travelExpenseUpdateDto.Id))
                    .SingleOrDefault();

                if (travelExpenseEntity == null)
                {
                    _logger.Error("TravelExpense not found by Id: " + travelExpenseUpdateDto.Id);
                    return NotFound(new TravelExpenseIdDto {Id = travelExpenseUpdateDto.Id});
                }

                travelExpenseEntity.Update(travelExpenseUpdateDto.Description);

                _unitOfWork
                    .Repository
                    .Update(travelExpenseEntity);

                _unitOfWork.Commit();
                return await Task.FromResult(Ok(new TravelExpenseUpdateResponse()));
            }
            catch (BusinessRuleViolationException brve)
            {
                _logger.Error(brve, "BRVE During " + MethodBase.GetCurrentMethod().Name);
                return await Task.FromResult(BadRequest(new BusinessRuleViolationResponseDto
                    {EntityId = brve.EntityId, Message = brve.Message}));
            }
            catch (Exception e)
            {
                _logger.Error(e, "During " + MethodBase.GetCurrentMethod().Name);
                return await Task.FromResult(BadRequest());
            }
        }

        [HttpPost]
        [Route("Post")]
        public async Task<ActionResult<TravelExpenseCreateResponse>> Post(TravelExpenseCreateDto travelExpenseCreateDto)
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
            catch (BusinessRuleViolationException brve)
            {
                _logger.Error(brve, "BRVE During " + MethodBase.GetCurrentMethod().Name);
                return await Task.FromResult(BadRequest(new BusinessRuleViolationResponseDto
                    {EntityId = brve.EntityId, Message = brve.Message}));
            }
            catch (Exception e)
            {
                _logger.Error(e, "During " + MethodBase.GetCurrentMethod().Name);
                return await Task.FromResult(BadRequest());
            }
        }

        [HttpPost]
        [Route("Certify")]
        public async Task<ActionResult<TravelExpenseCertifyResponse>> Certify(
            TravelExpenseCertifyDto travelExpenseCertifyDto)
        {
            try
            {
                var travelExpenseEntity = _unitOfWork
                    .Repository
                    .List(new TravelExpenseById(travelExpenseCertifyDto.Id))
                    .SingleOrDefault();

                if (travelExpenseEntity == null)
                {
                    _logger.Error("TravelExpense not found by Id: " + travelExpenseCertifyDto.Id);
                    return NotFound(new TravelExpenseIdDto {Id = travelExpenseCertifyDto.Id});
                }

                travelExpenseEntity.Certify();
                _unitOfWork
                    .Repository
                    .Update(travelExpenseEntity);

                _unitOfWork.Commit();

                return await Task.FromResult(Ok(new TravelExpenseCertifyResponse()));
            }
            catch (BusinessRuleViolationException brve)
            {
                _logger.Error(brve, "BRVE During " + MethodBase.GetCurrentMethod().Name);
                return await Task.FromResult(BadRequest(new BusinessRuleViolationResponseDto
                    {EntityId = brve.EntityId, Message = brve.Message}));
            }
            catch (Exception e)
            {
                _logger.Error(e, "During " + MethodBase.GetCurrentMethod().Name);
                return await Task.FromResult(BadRequest());
            }
        }

        [HttpPost]
        [Route("ReportDone")]
        public async Task<ActionResult<TravelExpenseReportDoneResponse>> ReportDone(
            TravelExpenseReportDoneDto travelExpenseReportDoneDto)
        {
            try
            {
                var travelExpenseEntity = _unitOfWork
                    .Repository
                    .List(new TravelExpenseById(travelExpenseReportDoneDto.Id))
                    .SingleOrDefault();

                if (travelExpenseEntity == null)
                {
                    _logger.Error("TravelExpense not found by Id: " + travelExpenseReportDoneDto.Id);
                    return NotFound(new TravelExpenseIdDto {Id = travelExpenseReportDoneDto.Id});
                }

                travelExpenseEntity.ReportDone();
                _unitOfWork
                    .Repository
                    .Update(travelExpenseEntity);

                _unitOfWork.Commit();

                return await Task.FromResult(Ok(new TravelExpenseReportDoneResponse()));
            }
            catch (BusinessRuleViolationException brve)
            {
                _logger.Error(brve, "BRVE During " + MethodBase.GetCurrentMethod().Name);
                return await Task.FromResult(BadRequest(new BusinessRuleViolationResponseDto
                    {EntityId = brve.EntityId, Message = brve.Message}));
            }
            catch (Exception e)
            {
                _logger.Error(e, "During " + MethodBase.GetCurrentMethod().Name);
                return await Task.FromResult(BadRequest());
            }
        }

        [HttpPost]
        [Route("AssignPayment")]
        public async Task<ActionResult<TravelExpenseAssignPaymentResponse>> AssignPayment(
            TravelExpenseAssignPaymentDto travelExpenseAssignPaymentDto)
        {
            try
            {
                var travelExpenseEntity = _unitOfWork
                    .Repository
                    .List(new TravelExpenseById(travelExpenseAssignPaymentDto.Id))
                    .SingleOrDefault();

                if (travelExpenseEntity == null)
                {
                    _logger.Error("TravelExpense not found by Id: " + travelExpenseAssignPaymentDto.Id);
                    return NotFound(new TravelExpenseIdDto {Id = travelExpenseAssignPaymentDto.Id});
                }

                travelExpenseEntity.AssignPayment();
                _unitOfWork
                    .Repository
                    .Update(travelExpenseEntity);

                _unitOfWork.Commit();

                return await Task.FromResult(Ok(new TravelExpenseAssignPaymentResponse()));
            }
            catch (BusinessRuleViolationException brve)
            {
                _logger.Error(brve, "BRVE During " + MethodBase.GetCurrentMethod().Name);
                return await Task.FromResult(BadRequest(new BusinessRuleViolationResponseDto
                    {EntityId = brve.EntityId, Message = brve.Message}));
            }
            catch (Exception e)
            {
                _logger.Error(e, "During " + MethodBase.GetCurrentMethod().Name);
                return await Task.FromResult(BadRequest());
            }
        }
    }
}