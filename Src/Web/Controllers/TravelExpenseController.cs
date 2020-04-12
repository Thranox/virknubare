using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Application.Dtos;
using Application.Interfaces;
using AutoMapper;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TravelExpenseController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IServiceProvider _serviceProvider;
        private readonly IUnitOfWork _unitOfWork;

        public TravelExpenseController(ILogger logger, IMapper mapper, IUnitOfWork unitOfWork,
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _serviceProvider = serviceProvider;
        }

        [HttpGet]
        [Route("GetById")]
        public async Task<ActionResult<IEnumerable<TravelExpenseDto>>> GetById(Guid id)
        {
            _logger.Information(MethodBase.GetCurrentMethod().Name + " called.");

            var travelExpenseDto = await _serviceProvider
                .GetService<IGetTravelExpenseService>()
                .GetByIdAsync(id);

            return Ok(travelExpenseDto);
        }
        [HttpGet]
        [Route("Get")]
        public async Task<ActionResult<IEnumerable<TravelExpenseDto>>> Get()
        {
            _logger.Information(MethodBase.GetCurrentMethod().Name + " called.");

            var travelExpenseDtos = await _serviceProvider
                .GetService<IGetTravelExpenseService>()
                .GetAsync();

            return Ok(travelExpenseDtos);
        }

        [HttpPut]
        [Route("Put")]
        public async Task<ActionResult<TravelExpenseUpdateResponse>> Put(TravelExpenseUpdateDto travelExpenseUpdateDto)
        {
            _logger.Information(MethodBase.GetCurrentMethod().Name + " called.");
            
            var travelExpenseDtos = await _serviceProvider
                .GetService<IUpdateTravelExpenseService>()
                .UpdateAsync(travelExpenseUpdateDto);

            return Ok(travelExpenseDtos);
        }

        [HttpPost]
        [Route("Post")]
        public async Task<ActionResult<TravelExpenseCreateResponse>> Post(TravelExpenseCreateDto travelExpenseCreateDto)
        {
            _logger.Information(MethodBase.GetCurrentMethod().Name + " called.");

            var travelExpenseDtos = await _serviceProvider
                .GetService<ICreateTravelExpenseService>()
                .CreateAsync(travelExpenseCreateDto);

            return Created(nameof(GetById), travelExpenseDtos);
        }

        [HttpPost]
        [Route("Certify")]
        public async Task<ActionResult<TravelExpenseCertifyResponse>> Certify(TravelExpenseCertifyDto travelExpenseCertifyDto)
        {
            _logger.Information(MethodBase.GetCurrentMethod().Name + " called.");

            var travelExpenseDtos = await _serviceProvider
                .GetService<ICertifyTravelExpenseService>()
                .CertifyAsync(travelExpenseCertifyDto);
            
            return Ok(travelExpenseDtos);
        }

        [HttpPost]
        [Route("ReportDone")]
        public async Task<ActionResult<TravelExpenseReportDoneResponse>> ReportDone(
            TravelExpenseReportDoneDto travelExpenseReportDoneDto)
        {
            _logger.Information(MethodBase.GetCurrentMethod().Name + " called.");

            var travelExpenseDtos = await _serviceProvider
                .GetService<IReportDoneTravelExpenseService>()
                .ReportDoneAsync(travelExpenseReportDoneDto);
            
            return Ok(travelExpenseDtos);
        }

        [HttpPost]
        [Route("AssignPayment")]
        public async Task<ActionResult<TravelExpenseAssignPaymentResponse>> AssignPayment(
            TravelExpenseAssignPaymentDto travelExpenseAssignPaymentDto)
        {
            _logger.Information(MethodBase.GetCurrentMethod().Name + " called.");

            var travelExpenseDtos = await _serviceProvider
                .GetService<IAssignPaymentTravelExpenseService>()
                .AssignPaymentAsync(travelExpenseAssignPaymentDto);

            return Ok(travelExpenseDtos);
        }
    }
}