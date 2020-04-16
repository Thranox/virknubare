using System;
using System.Linq;
using System.Threading.Tasks;
using Application.Dtos;
using Application.Interfaces;
using Domain;
using Domain.Interfaces;
using Domain.Specifications;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Services
{
    public class UpdateTravelExpenseService : IUpdateTravelExpenseService
    {
        private readonly IServiceProvider _serviceProvider;

        public UpdateTravelExpenseService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<TravelExpenseUpdateResponse> UpdateAsync(TravelExpenseUpdateDto travelExpenseUpdateDto)
        {
            using (var unitOfWork = _serviceProvider.GetService<IUnitOfWork>())
            {
                var travelExpenseEntity = unitOfWork
                    .Repository
                    .List(new TravelExpenseById(travelExpenseUpdateDto.Id))
                    .SingleOrDefault();

                if (travelExpenseEntity == null)
                    throw new ItemNotFoundException( travelExpenseUpdateDto.Id.ToString(), "TravelExpense");
                
                travelExpenseEntity.Update(travelExpenseUpdateDto.Description);

                unitOfWork
                    .Repository
                    .Update(travelExpenseEntity);

                unitOfWork.Commit();
                return await Task.FromResult(new TravelExpenseUpdateResponse());
            }
        }
    }
}