using System;
using System.Threading.Tasks;
using Application.Dtos;
using Application.Interfaces;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.ValueObjects;

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
            if (travelExpenseCreateDto.DailyAllowanceAmount == null)
                throw new ArgumentNullException(nameof(travelExpenseCreateDto.DailyAllowanceAmount));
            if (travelExpenseCreateDto.DestinationPlace == null)
                throw new ArgumentNullException(nameof(travelExpenseCreateDto.DestinationPlace));
            if (travelExpenseCreateDto.ArrivalPlace == null)
                throw new ArgumentNullException(nameof(travelExpenseCreateDto.ArrivalPlace));
            if (travelExpenseCreateDto.DeparturePlace == null)
                throw new ArgumentNullException(nameof(travelExpenseCreateDto.DeparturePlace));
            if (travelExpenseCreateDto.FoodAllowances == null)
                throw new ArgumentNullException(nameof(travelExpenseCreateDto.FoodAllowances));
            if (travelExpenseCreateDto.TransportSpecification == null)
                throw new ArgumentNullException(nameof(travelExpenseCreateDto.TransportSpecification));

            var owningCustomer = _unitOfWork.Repository.GetById<CustomerEntity>(travelExpenseCreateDto.CustomerId);

            if (owningCustomer == null)
                throw new ItemNotFoundException(travelExpenseCreateDto.CustomerId.ToString(), "CustomerEntity");

            var travelExpenseEntity = _travelExpenseFactory.Create(travelExpenseCreateDto.Description,
                polApiContext.CallingUser, owningCustomer, travelExpenseCreateDto.ArrivalDateTime,
                travelExpenseCreateDto.DepartureDateTime, travelExpenseCreateDto.CommitteeId,
                travelExpenseCreateDto.Purpose, travelExpenseCreateDto.IsEducationalPurpose, travelExpenseCreateDto.Expenses,
                travelExpenseCreateDto.IsAbsenceAllowance,
                new Place
                {
                    Street = travelExpenseCreateDto.DestinationPlace.Street,
                    StreetNumber = travelExpenseCreateDto.DestinationPlace.StreetNumber,
                    ZipCode = travelExpenseCreateDto.DestinationPlace.ZipCode
                }, new TransportSpecification
                {
                    KilometersCalculated = travelExpenseCreateDto.TransportSpecification.KilometersCalculated,
                    KilometersCustom = travelExpenseCreateDto.TransportSpecification.KilometersCustom,
                    KilometersOverTaxLimit = travelExpenseCreateDto.TransportSpecification.KilometersOverTaxLimit,
                    KilometersTax = travelExpenseCreateDto.TransportSpecification.KilometersTax,
                    Method = travelExpenseCreateDto.TransportSpecification.Method,
                    NumberPlate = travelExpenseCreateDto.TransportSpecification.NumberPlate
                },
                new DailyAllowanceAmount
                {
                    DaysLessThan4hours = travelExpenseCreateDto.DailyAllowanceAmount.DaysLessThan4hours,
                    DaysMoreThan4hours = travelExpenseCreateDto.DailyAllowanceAmount.DaysMoreThan4hours,
                },
                new FoodAllowances
                {
                    Morning = travelExpenseCreateDto.FoodAllowances.Morning,
                    Lunch = travelExpenseCreateDto.FoodAllowances.Lunch,
                    Dinner = travelExpenseCreateDto.FoodAllowances.Dinner
                },
                new LossOfEarningEntity[] { },
                new Place
                {
                    Street = travelExpenseCreateDto.ArrivalPlace.Street,
                    StreetNumber = travelExpenseCreateDto.ArrivalPlace.StreetNumber,
                    ZipCode = travelExpenseCreateDto.ArrivalPlace.ZipCode
                },
                new Place
                {
                    Street = travelExpenseCreateDto.DeparturePlace.Street,
                    StreetNumber = travelExpenseCreateDto.DeparturePlace.StreetNumber,
                    ZipCode = travelExpenseCreateDto.DeparturePlace.ZipCode
                }
                );

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