using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Dtos;
using Application.Interfaces;
using Domain;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class ProcessStepTravelExpenseService : IProcessStepTravelExpenseService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEnumerable<IProcessFlowStep> _processFlowSteps;

        public ProcessStepTravelExpenseService(IUnitOfWork unitOfWork, IEnumerable<IProcessFlowStep> processFlowSteps)
        {
            _unitOfWork = unitOfWork;
            _processFlowSteps = processFlowSteps;
        }

        public async Task<TravelExpenseProcessStepResponse> ProcessStepAsync(
            TravelExpenseProcessStepDto travelExpenseProcessStepDto)
        {
            var travelExpenseEntity = _unitOfWork
                .Repository
                .GetById<TravelExpenseEntity>(travelExpenseProcessStepDto.TravelExpenseId);

            if (travelExpenseEntity == null)
                throw new ItemNotFoundException(travelExpenseProcessStepDto.TravelExpenseId.ToString(),
                    "TravelExpense");

            var processFlowStep = _processFlowSteps
                .SingleOrDefault(x => x.CanHandle(travelExpenseProcessStepDto.ProcessStepKey));

            if (processFlowStep == null)
                throw new ItemNotFoundException(travelExpenseProcessStepDto.ProcessStepKey, "ProcessFlowStep");

            travelExpenseEntity.ApplyProcessStep(processFlowStep);

            _unitOfWork
                .Repository
                .Update(travelExpenseEntity);

            _unitOfWork.Commit();

            return await Task.FromResult(new TravelExpenseProcessStepResponse());
        }
    }
}