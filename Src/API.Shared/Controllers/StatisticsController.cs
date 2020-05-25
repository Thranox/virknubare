using System.Threading.Tasks;
using API.Shared.Services;
using Application.Dtos;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Shared.Controllers
{
    [ApiController]
    [Route("statistics")]
    public class StatisticsController : ControllerBase
    {
        private readonly IGetStatisticsService _getStatisticsService;
        private readonly ISubManagementService _subManagementService;

        public StatisticsController(ISubManagementService subManagementService,
            IGetStatisticsService getStatisticsService)
        {
            _subManagementService = subManagementService;
            _getStatisticsService = getStatisticsService;
        }

        [HttpPost]
        public async Task<ActionResult<SubmissionPostResponse>> Post()
        {
            var sub = _subManagementService.GetSub(User, HttpContext);
            var statisticsGetResponse = await _getStatisticsService.GetAsync(sub);

            return Ok(statisticsGetResponse);
        }
    }

}