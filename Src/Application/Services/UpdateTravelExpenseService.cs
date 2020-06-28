using System;
using System.Linq;
using System.Threading.Tasks;
using Application.Dtos;
using Application.Interfaces;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.Specifications;
using Domain.ValueObjects;

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
            if (travelExpenseUpdateDto.DailyAllowanceAmount == null)
                throw new ArgumentNullException(nameof(travelExpenseUpdateDto.DailyAllowanceAmount));
            if (travelExpenseUpdateDto.DestinationPlace == null)
                throw new ArgumentNullException(nameof(travelExpenseUpdateDto.DestinationPlace));
            if (travelExpenseUpdateDto.FoodAllowances == null)
                throw new ArgumentNullException(nameof(travelExpenseUpdateDto.FoodAllowances));
            if (travelExpenseUpdateDto.TransportSpecification == null)
                throw new ArgumentNullException(nameof(travelExpenseUpdateDto.TransportSpecification));

            var travelExpenseEntity = _unitOfWork
                .Repository
                .List(new TravelExpenseById(id))
                .SingleOrDefault();

            if (travelExpenseEntity == null)
                throw new ItemNotFoundException(id.ToString(), "TravelExpense");

            travelExpenseEntity.Update(travelExpenseUpdateDto.Description,
                travelExpenseUpdateDto.ArrivalDateTime,
                travelExpenseUpdateDto.DepartureDateTime, travelExpenseUpdateDto.CommitteeId,
                travelExpenseUpdateDto.Purpose, travelExpenseUpdateDto.IsEducationalPurpose, travelExpenseUpdateDto.Expenses,
                travelExpenseUpdateDto.IsAbsenceAllowance,
                new Place
                {
                    Street = travelExpenseUpdateDto.DestinationPlace.Street,
                    StreetNumber = travelExpenseUpdateDto.DestinationPlace.StreetNumber,
                    ZipCode = travelExpenseUpdateDto.DestinationPlace.ZipCode
                }, new TransportSpecification
                {
                    KilometersCalculated = travelExpenseUpdateDto.TransportSpecification.KilometersCalculated,
                    KilometersCustom = travelExpenseUpdateDto.TransportSpecification.KilometersCustom,
                    KilometersOverTaxLimit = travelExpenseUpdateDto.TransportSpecification.KilometersOverTaxLimit,
                    KilometersTax = travelExpenseUpdateDto.TransportSpecification.KilometersTax,
                    Method = travelExpenseUpdateDto.TransportSpecification.Method,
                    NumberPlate = travelExpenseUpdateDto.TransportSpecification.NumberPlate
                },
                new DailyAllowanceAmount
                {
                    DaysLessThan4hours = travelExpenseUpdateDto.DailyAllowanceAmount.DaysLessThan4hours,
                    DaysMoreThan4hours = travelExpenseUpdateDto.DailyAllowanceAmount.DaysMoreThan4hours,
                },
                new FoodAllowances
                {
                    Morning = travelExpenseUpdateDto.FoodAllowances.Morning,
                    Lunch = travelExpenseUpdateDto.FoodAllowances.Lunch,
                    Dinner = travelExpenseUpdateDto.FoodAllowances.Dinner
                });

            _unitOfWork
                .Repository
                .Update(travelExpenseEntity);

            await _unitOfWork.CommitAsync();

            return await Task.FromResult(new TravelExpenseUpdateResponse());
        }
    }
}