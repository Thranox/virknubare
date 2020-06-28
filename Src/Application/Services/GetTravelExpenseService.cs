using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Dtos;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.Specifications;
using Domain.ValueObjects;
using Serilog;

namespace Application.Services
{
    public class GetTravelExpenseService : IGetTravelExpenseService
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public GetTravelExpenseService(IMapper mapper, IUnitOfWork unitOfWork, ILogger logger)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<TravelExpenseGetResponse> GetAsync(PolApiContext polApiContext)
        {
            _logger.Debug("GetTravelExpenseService.GetAsync()");

            // Users can access own travelexpenses plus those owned by others but in a state accessible for user.

            // First get the customers for which the user can see TravelExpenses
            var customersVisibleByUser = polApiContext
                .CallingUser
                .CustomerUserPermissions
                .Where(x => x.UserStatus == UserStatus.Registered || x.UserStatus == UserStatus.UserAdministrator)
                .Select(x => x.CustomerId)
                .ToArray();

            // Get all TravelExpenses
            var all = _unitOfWork.Repository.List(new TravelExpensesByCustomerIdList(customersVisibleByUser));

            // The user may see the travel expense if
            // 1) owned by the user or if
            // 2) user may manipulate travel expense stage

            var travelExpenseDtos = all
                .Select(x =>
                {
                    var travelExpenseDto = _mapper.Map<TravelExpenseInListDto>(x);
                    travelExpenseDto.AllowedFlows = GetAllowedFlows(x, polApiContext.CallingUser).ToArray();
                    return travelExpenseDto;
                })
                .Where(x => x.OwnedByUserId == polApiContext.CallingUser.Id || x.AllowedFlows.Any())
                .ToArray();

            var travelExpenseGetResponse = new TravelExpenseGetResponse
            {
                Result = travelExpenseDtos
            };
            _logger.Debug("GetTravelExpenseService.GetAsync() - "+travelExpenseDtos.Length);
            return await Task.FromResult(travelExpenseGetResponse);
        }

        public async Task<TravelExpenseGetByIdResponse> GetByIdAsync(PolApiContext polApiContext, Guid id)
        {
            // Get the stages for which the user may manipulate travel expenses
            var travelExpenseStages =
                polApiContext.CallingUser.FlowStepUserPermissions.Select(x => x.FlowStep.From).Distinct().ToList();

            var travelExpenseEntity = _unitOfWork.Repository.List(new TravelExpenseById(id)).SingleOrDefault();

            if (travelExpenseEntity == null)
                throw new ItemNotFoundException(id.ToString(), nameof(TravelExpenseEntity));

            var allowedFlowDtos = GetAllowedFlows(travelExpenseEntity, polApiContext.CallingUser).ToArray();

            if (!allowedFlowDtos.Any() && travelExpenseEntity.OwnedByUser.Id != polApiContext.CallingUser.Id)
                throw new ItemNotAllowedException(id.ToString(), nameof(TravelExpenseEntity));

            var travelExpenseSingleDto = _mapper.Map<TravelExpenseSingleDto>(travelExpenseEntity);
            travelExpenseSingleDto.AllowedFlows = allowedFlowDtos;
            travelExpenseSingleDto.PayoutTable = _mapper.Map<PayoutTableDto>(travelExpenseEntity.CalculatePayoutTable());
            return await Task.FromResult(
                new TravelExpenseGetByIdResponse
                {
                    Result = travelExpenseSingleDto
                });
        }

        private IEnumerable<AllowedFlowDto> GetAllowedFlows(TravelExpenseEntity travelExpenseEntity,
            UserEntity userEntity)
        {
            foreach (var flowStepUserPermissionEntity in userEntity.FlowStepUserPermissions.Where(x =>
                x.FlowStep.From == travelExpenseEntity.Stage && x.FlowStep.Customer == travelExpenseEntity.Customer))
                yield return new AllowedFlowDto
                {
                    FlowStepId = flowStepUserPermissionEntity.FlowStepId,
                    Description = flowStepUserPermissionEntity.FlowStep.Description
                };
        }
    }
}