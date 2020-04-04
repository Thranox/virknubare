using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Domain;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Web.ApiModels;

namespace Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TravelExpenseController : ControllerBase
    {
        private readonly ILogger<TravelExpenseController> _logger;
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public TravelExpenseController(ILogger<TravelExpenseController> logger, IRepository repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task< IEnumerable<TravelExpenseDto>> Get()
        {
            _logger.LogInformation("TravelExpense.Get() called.");
            await Task.CompletedTask;
            return _repository
                .List<TravelExpenseEntity>()
                .Select(x=>_mapper.Map<TravelExpenseDto>(x));
        }
    }
}
