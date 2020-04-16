using System;
using System.Linq;
using System.Threading.Tasks;
using Application.Dtos;
using Application.Interfaces;
using AutoMapper;
using Domain;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class GetTravelExpenseService : IGetTravelExpenseService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public GetTravelExpenseService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<TravelExpenseGetResponse> GetAsync()
        {
            return await Task.FromResult(new TravelExpenseGetResponse
            {
                Result = _unitOfWork
                    .Repository
                    .List<TravelExpenseEntity>()
                    .Select(x => _mapper.Map<TravelExpenseDto>(x))
            });
        }

        public async Task<TravelExpenseGetByIdResponse> GetByIdAsync(Guid id)
        {
            var travelExpenseEntity = _unitOfWork
                .Repository
                .GetById<TravelExpenseEntity>(id);

            if (travelExpenseEntity == null)
                throw new ItemNotFoundException(id.ToString(), "TravelExpense");

            return await Task.FromResult(
                new TravelExpenseGetByIdResponse
                {
                    Result = _mapper.Map<TravelExpenseDto>(travelExpenseEntity)
                });
        }
    }
}