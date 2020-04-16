using System;
using System.Linq;
using System.Threading.Tasks;
using Application.Dtos;
using Application.Interfaces;
using Domain;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Services
{
    public class ProcessStepTravelExpenseService : IProcessStepTravelExpenseService
    {
        private readonly IServiceProvider _serviceProvider;

        public ProcessStepTravelExpenseService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<TravelExpenseProcessStepResponse> ProcessStepAsync(
            TravelExpenseProcessStepDto travelExpenseProcessStepDto)
        {
            using (var unitOfWork = _serviceProvider.GetService<IUnitOfWork>())
            {
                var travelExpenseEntity = unitOfWork
                    .Repository
                    .GetById<TravelExpenseEntity>(travelExpenseProcessStepDto.TravelExpenseId);

                if (travelExpenseEntity == null)
                    throw new ItemNotFoundException(travelExpenseProcessStepDto.TravelExpenseId.ToString(),
                        "TravelExpense");

                var processFlowStep = _serviceProvider
                    .GetServices<IProcessFlowStep>()
                    .SingleOrDefault(x => x.CanHandle(travelExpenseProcessStepDto.ProcessStepKey));

                if (processFlowStep == null)
                    throw new ItemNotFoundException(travelExpenseProcessStepDto.ProcessStepKey, "ProcessFlowStep");

                processFlowStep.Process(travelExpenseEntity);

                unitOfWork
                    .Repository
                    .Update(travelExpenseEntity);

                unitOfWork.Commit();

                return await Task.FromResult(new TravelExpenseProcessStepResponse());
            }
        }
    }
}