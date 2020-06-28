using System.Linq;
using System.Threading.Tasks;
using API.Shared.Services;
using Application.Dtos;
using AutoMapper;
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
                    .Select(x=>_mapper.Map<LossOfEarningSpecDto>(x))
                    .ToArray() 
            });
        }

    }
}