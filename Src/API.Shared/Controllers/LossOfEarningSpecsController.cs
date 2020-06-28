using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Shared.Services;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace API.Shared.Controllers
{
    [ApiController]
    [Route("lossofearningspecs")]
    public class LossOfEarningSpecsController : ControllerBase
    {
        private readonly ISubManagementService _subManagementService;
        private readonly ILossOfEarningSpecsService _lossOfEarningSpecsService;
        private readonly IMapper _mapper;

        public LossOfEarningSpecsController(ISubManagementService subManagementService, ILossOfEarningSpecsService lossOfEarningSpecsService, IMapper mapper)
        {
            _subManagementService = subManagementService;
            _lossOfEarningSpecsService = lossOfEarningSpecsService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<LossOfEarningSpecsGetResponse>> Get()
        {
            var polApiContext = await _subManagementService.GetPolApiContext(HttpContext);

            var lossOfEarningSpecEntities = _lossOfEarningSpecsService.Get();
            return Ok(new LossOfEarningSpecsGetResponse
            {
                Items = lossOfEarningSpecEntities
                    .Select(x=>_mapper.Map<LossOfEarningsSpecDto>(x))
                    .ToArray() 
            });
        }

    }

    public interface ILossOfEarningSpecsService
    {
        IEnumerable<LossOfEarningSpecEntity> Get();
    }

    public class LossOfEarningSpecsService : ILossOfEarningSpecsService
    {
        private readonly IRepository _repository;

        public LossOfEarningSpecsService(IRepository repository)
        {
            _repository = repository;
        }
        public IEnumerable<LossOfEarningSpecEntity> Get()
        {
            return _repository.List(new GetAllLossOfEarningSpec());
        }
    }

    public class LossOfEarningsSpecDto
    {
        public int Id { get; set; }
        public int Rate { get; set; }
        public string Description { get; set; }
    }

    public class LossOfEarningSpecsGetResponse
    {
        public LossOfEarningsSpecDto[] Items { get; set; }
    }
}