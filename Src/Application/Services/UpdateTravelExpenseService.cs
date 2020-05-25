using System;
using System.Linq;
using System.Threading.Tasks;
using API.Shared.Services;
using Application.Dtos;
using Application.Interfaces;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.Specifications;

namespace Application.Services
{
    public class UpdateTravelExpenseService : IUpdateTravelExpenseService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateTravelExpenseService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<TravelExpenseUpdateResponse> UpdateAsync(PolApiContext polApiContext, Guid id,
            TravelExpenseUpdateDto travelExpenseUpdateDto)
        {
            var travelExpenseEntity = _unitOfWork
                .Repository
                .List(new TravelExpenseById(id))
                .SingleOrDefault();

            if (travelExpenseEntity == null)
                throw new ItemNotFoundException(id.ToString(), "TravelExpense");

            travelExpenseEntity.Update(travelExpenseUpdateDto.Description);

            _unitOfWork
                .Repository
                .Update(travelExpenseEntity);

            await _unitOfWork.CommitAsync();

            return await Task.FromResult(new TravelExpenseUpdateResponse());
        }
    }
}