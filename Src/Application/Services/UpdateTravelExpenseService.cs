using System;
using System.Linq;
using System.Threading.Tasks;
using Application.Dtos;
using Application.Interfaces;
using Domain;
using Domain.Interfaces;
using Domain.Specifications;

namespace Application.Services
{
    public class UpdateTravelExpenseService : IUpdateTravelExpenseService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateTravelExpenseService(IServiceProvider serviceProvider, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<TravelExpenseUpdateResponse> UpdateAsync(Guid id,
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

            _unitOfWork.Commit();
            return await Task.FromResult(new TravelExpenseUpdateResponse());
        }
    }
}