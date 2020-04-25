using System;
using System.Threading.Tasks;
using Application.Dtos;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Web.Services;

namespace Web.Controllers
{
    [ApiController]
    [Route("submissions")]
    public class SubmissionController : ControllerBase
    {
        private readonly ICreateSubmissionService _createSubmissionService;
        private readonly ISubManagementService _subManagementService;

        public SubmissionController(ISubManagementService subManagementService,
            ICreateSubmissionService createSubmissionService)
        {
            _subManagementService = subManagementService;
            _createSubmissionService = createSubmissionService;
        }

        [HttpPost]
        public async Task<ActionResult<SubmissionPostResponse>> Post()
        {
            var sub = _subManagementService.GetSub(User);
            var travelExpenseDtos = await _createSubmissionService.CreateAsync(sub);

            return Ok(travelExpenseDtos);
        }
    }

}