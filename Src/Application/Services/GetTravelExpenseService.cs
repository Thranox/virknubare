using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Dtos;
using Application.Interfaces;
using AutoMapper;
using Domain;
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
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;

        public GetTravelExpenseService(IMapper mapper, IUnitOfWork unitOfWork, ILogger logger)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<TravelExpenseGetResponse> GetAsync(string sub)
        {
            _logger.Debug("Getting em all");

            // Get user by sub
            var userEntities = _unitOfWork
                .Repository
                .List(new UserBySub(sub));
            var userEntity = userEntities
                .SingleOrDefault();

            if (userEntity == null)
                throw new ItemNotFoundException(sub, "UserEntity");

            // Users can access own travelexpenses plus those owned by others but in a state accessible for user.
            var ownTravelExpenses = _unitOfWork.Repository.List(new TravelExpenseByUserId(userEntity.Id));


            // Get the stages for which the user may manipulate travel expenses
            var travelExpenseStages =
                userEntity.FlowStepUserPermissions.Select(x => x.FlowStep.From.Value).Distinct().ToList();

            var customersVisibleByUser = userEntity.CustomerUserPermissions
                .Where(x =>x.UserStatus == UserStatus.Registered || x.UserStatus == UserStatus.UserAdministrator)
                .Select(x=>x.CustomerId);

            var travelExpenseEntities = _unitOfWork.Repository.List(new TravelExpenseByCustomerList(customersVisibleByUser)).Where(x=>travelExpenseStages.Contains( x.Stage.Value));

            var rees = ownTravelExpenses.Concat(travelExpenseEntities).Distinct();
            //// The user may see the travel expense if
            //// 1) owned by the user or if
            //// 2) user may manipulate travel expense stage

            var travelExpenseGetResponse = new TravelExpenseGetResponse
            {
                Result = rees
                    .Select(x =>
                    {
                        var travelExpenseDto = _mapper.Map<TravelExpenseDto>(x);
                        travelExpenseDto.AllowedFlows = GetAllowedFlows(x, sub).ToArray();
                        return travelExpenseDto;
                    }).ToArray()
            };
            return await Task.FromResult(travelExpenseGetResponse);
        }

        private IEnumerable< AllowedFlowDto> GetAllowedFlows(TravelExpenseEntity travelExpenseEntity, string sub)
        {
            foreach (var VARIABLE in travelExpenseEntity.OwnedByUser.FlowStepUserPermissions.Where(x=>x.FlowStep.From==travelExpenseEntity.Stage && x.FlowStep.Customer==travelExpenseEntity.Customer))
            {
                yield return new AllowedFlowDto()
                {
                    FlowStepId=VARIABLE.FlowStepId,
                    Description = VARIABLE.FlowStep.Description
                };
               
            }

        }

        public async Task<TravelExpenseGetByIdResponse> GetByIdAsync(Guid id, string sub)
        {
            // Get user by sub
            var userEntities = _unitOfWork
                .Repository
                .List(new UserBySub(sub));
            var userEntity = userEntities
                .SingleOrDefault();

            if (userEntity == null)
                throw new ItemNotFoundException(sub,nameof(UserEntity));

            // Get the stages for which the user may manipulate travel expenses
            var travelExpenseStages =
                userEntity.FlowStepUserPermissions.Select(x => x.FlowStep.From).Distinct().ToList();

            var travelExpenseEntity = _unitOfWork.Repository.List(new TravelExpenseById(id)).SingleOrDefault();

            if(travelExpenseEntity==null)
                throw new ItemNotFoundException(id.ToString(), nameof(TravelExpenseEntity));

            var travelExpensesTheUserCanSee = await this.GetAsync(sub);

            var travelExpenseDto = travelExpensesTheUserCanSee.Result.SingleOrDefault(x => x.Id == id);

            if (travelExpenseDto == null)
                throw new ItemNotAllowedException(id.ToString(), nameof(TravelExpenseEntity));

            return await Task.FromResult(
                new TravelExpenseGetByIdResponse
                {
                    Result = travelExpenseDto
                });
        }
    }

}