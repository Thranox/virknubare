﻿using System.Threading.Tasks;
using Application.Dtos;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class CreateTravelExpenseService : ICreateTravelExpenseService
    {
        private readonly ITravelExpenseFactory _travelExpenseFactory;
        private readonly IUnitOfWork _unitOfWork;

        public CreateTravelExpenseService(IUnitOfWork unitOfWork, ITravelExpenseFactory travelExpenseFactory)
        {
            _unitOfWork = unitOfWork;
            _travelExpenseFactory = travelExpenseFactory;
        }

        public async Task<TravelExpenseCreateResponse> CreateAsync(PolApiContext polApiContext,
            TravelExpenseCreateDto travelExpenseCreateDto)
        {
            var owningCustomer = _unitOfWork.Repository.GetById<CustomerEntity>(travelExpenseCreateDto.CustomerId);
            var travelExpenseEntity = _travelExpenseFactory.Create(travelExpenseCreateDto.Description,
                polApiContext.CallingUser, owningCustomer, travelExpenseCreateDto.ArrivalDateTime);

            _unitOfWork
                .Repository
                .Attach(travelExpenseEntity);

            await _unitOfWork.CommitAsync();

            return await Task.FromResult(new TravelExpenseCreateResponse
            {
                Id = travelExpenseEntity.Id
            });
        }
    }
}