using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Dtos;
using Application.Interfaces;
using Domain;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.Specifications;

namespace Application.Services
{
    public class FlowStepTravelExpenseService : IFlowStepTravelExpenseService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEnumerable<IProcessFlowStep> _processFlowSteps;

        public FlowStepTravelExpenseService(IUnitOfWork unitOfWork, IEnumerable<IProcessFlowStep> processFlowSteps)
        {
            _unitOfWork = unitOfWork;
            _processFlowSteps = processFlowSteps;
        }

        public async Task<TravelExpenseProcessStepResponse> ProcessStepAsync(
            TravelExpenseFlowStepDto travelExpenseFlowStepDto, string sub)
        {
            // Get user by sub
            var userEntities = _unitOfWork
                .Repository
                .List(new UserBySub(sub));
            var userEntity = userEntities
                .SingleOrDefault();

            if (userEntity == null)
                throw new ItemNotFoundException(sub, "UserEntity");

            var travelExpenseEntity = _unitOfWork
                .Repository
                .GetById<TravelExpenseEntity>(travelExpenseFlowStepDto.TravelExpenseId);

            if (travelExpenseEntity == null)
                throw new ItemNotFoundException(travelExpenseFlowStepDto.TravelExpenseId.ToString(),
                    "TravelExpense");

            var flowStep = _unitOfWork.Repository.GetById<FlowStepEntity>(travelExpenseFlowStepDto.FlowStepId);
            if (flowStep == null)
                throw new ItemNotFoundException(travelExpenseFlowStepDto.FlowStepId.ToString(),
                    "FlowStep");

            var processFlowStep = _processFlowSteps
                .SingleOrDefault(x => x.CanHandle(flowStep.Key));

            travelExpenseEntity.ApplyProcessStep(processFlowStep);

            _unitOfWork
                .Repository
                .Update(travelExpenseEntity);

            _unitOfWork.Commit();

            return await Task.FromResult(new TravelExpenseProcessStepResponse());
        }
    }
}