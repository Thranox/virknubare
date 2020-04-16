using System;
using System.Linq;
using System.Threading.Tasks;
using Application.Dtos;
using Application.Interfaces;
using AutoMapper;
using Domain;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Services
{
    public class GetTravelExpenseService : IGetTravelExpenseService
    {
        private readonly IMapper _mapper;
        private readonly IServiceProvider _serviceProvider;

        public GetTravelExpenseService(IServiceProvider serviceProvider, IMapper mapper)
        {
            _serviceProvider = serviceProvider;
            _mapper = mapper;
        }

        public async Task<TravelExpenseGetResponse> GetAsync()
        {
            using (var unitOfWork = _serviceProvider.GetService<IUnitOfWork>())
            {
                return await Task.FromResult(new TravelExpenseGetResponse
                {
                    Result = unitOfWork
                        .Repository
                        .List<TravelExpenseEntity>()
                        .Select(x => _mapper.Map<TravelExpenseDto>(x))
                });
            }
        }

        public async Task<TravelExpenseGetByIdResponse> GetByIdAsync(Guid id)
        {
            using (var unitOfWork = _serviceProvider.GetService<IUnitOfWork>())
            {
                var travelExpenseEntity = unitOfWork
                    .Repository
                    .GetById<TravelExpenseEntity>(id);

                if (travelExpenseEntity == null)
                    throw new ItemNotFoundException(id.ToString(),"TravelExpense");

                return await Task.FromResult(
                    new TravelExpenseGetByIdResponse
                    {
                        Result = _mapper.Map<TravelExpenseDto>(travelExpenseEntity)
                    });
            }
        }
    }
}