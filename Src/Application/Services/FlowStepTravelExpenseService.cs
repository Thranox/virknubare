using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Dtos;
using Application.Interfaces;
using Domain;
using Domain.Entities;
using Domain.Events;
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
            TravelExpenseFlowStepDto travelExpenseFlowStepDto, PolApiContext polApiContext)
        {
            var travelExpenseEntity = _unitOfWork
                .Repository
                .List(new TravelExpenseById( travelExpenseFlowStepDto.TravelExpenseId))
                .Single();

            if (travelExpenseEntity == null)
                throw new ItemNotFoundException(travelExpenseFlowStepDto.TravelExpenseId.ToString(),
                    "TravelExpense");

            var stageBefore = travelExpenseEntity.Stage;
            var flowStep = _unitOfWork.Repository.GetById<FlowStepEntity>(travelExpenseFlowStepDto.FlowStepId);
            if (flowStep == null)
                throw new ItemNotFoundException(travelExpenseFlowStepDto.FlowStepId.ToString(),
                    "FlowStep");

            var processFlowStep = _processFlowSteps
                .SingleOrDefault(x => x.CanHandle(flowStep.Key));

            travelExpenseEntity.ApplyProcessStep(processFlowStep);

            travelExpenseEntity.Events.Add(new TravelExpenseChangedStateDomainEvent(stageBefore, travelExpenseEntity, polApiContext.CallingUser, polApiContext.RequestedUrl));

            _unitOfWork
                .Repository
                .Update(travelExpenseEntity);

            await _unitOfWork.CommitAsync();

            return await Task.FromResult(new TravelExpenseProcessStepResponse());
        }
    }
}