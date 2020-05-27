using System;
using System.Threading.Tasks;
using API.Shared.Services;
using Application.Dtos;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Shared.Controllers
{
    [ApiController]
    [Route("customer")]
    public class CustomerController : ControllerBase
    {
        private readonly ISubManagementService _subManagementService;
        private readonly ICustomerUserService _customerUserService;

        public CustomerController(ISubManagementService subManagementService, ICustomerUserService customerUserService)
        {
            _subManagementService = subManagementService;
            _customerUserService = customerUserService;
        }

        [HttpGet("{customerId}/Users")]
        public async Task<ActionResult<CustomerUserGetResponse>> GetCustomerUsers(Guid customerId)
        {
            var polApiContext = await _subManagementService.GetPolApiContext(HttpContext);

            var customerUserGetResponse = await _customerUserService.GetAsync(polApiContext, customerId);

            return Ok(customerUserGetResponse);
        }

        [HttpPost("{customerId}/Invitations")]
        public async Task<ActionResult<CustomerInvitationsPostResponse>> PostInvitations(Guid customerId, CustomerInvitationsPostDto customerInvitationsPostDto)
        {
            var polApiContext = await _subManagementService.GetPolApiContext(HttpContext);
            var customerUserGetResponse = await _customerUserService.CreateInvitationsAsync(polApiContext, customerId, customerInvitationsPostDto);

            return Ok(customerUserGetResponse);
        }
    }
}