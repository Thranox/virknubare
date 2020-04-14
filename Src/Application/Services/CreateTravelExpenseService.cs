using System;
using System.Threading.Tasks;
using Application.Dtos;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Services
{
    public class CreateTravelExpenseService : ICreateTravelExpenseService
    {
        private readonly IServiceProvider _serviceProvider;

        public CreateTravelExpenseService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<TravelExpenseCreateResponse> CreateAsync(TravelExpenseCreateDto travelExpenseCreateDto)
        {
            using (var unitOfWork = _serviceProvider.GetService<IUnitOfWork>())
            {
                var travelExpenseEntity = new TravelExpenseEntity(travelExpenseCreateDto.Description);

                unitOfWork
                    .Repository
                    .Add(travelExpenseEntity);

                unitOfWork.Commit();

                return await Task.FromResult(new TravelExpenseCreateResponse
                {
                    Id = travelExpenseEntity.Id
                });
            }
        }
    }
}