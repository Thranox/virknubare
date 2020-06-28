using System;
using Application.Dtos;
using Domain.Entities;
using Domain.ValueObjects;

namespace Tests.Domain.Services
{
    public static class TestDataHelper
    {
        public static TravelExpenseEntity GetValidTravelExpense(StageEntity stageEntity, UserEntity userEntity = null,
            CustomerEntity customerEntity = null)
        {
            return new TravelExpenseEntity("Description", userEntity ?? new UserEntity("", ""),
                customerEntity ?? new CustomerEntity(""), stageEntity,
                DateTime.Now, DateTime.Now, 42, "Purpose", true, 3.14, true, new Place(), new TransportSpecification(),
                new DailyAllowanceAmount(), new FoodAllowances());
        }

        public static TravelExpenseCreateDto GetValidTravelExpenseCreateDto(string newDescription, Guid customerId)
        {
            return new TravelExpenseCreateDto
            {
                Description = newDescription,
                CustomerId = customerId,
                DailyAllowanceAmount = new DailyAllowanceAmountDto
                {
                    DaysLessThan4hours = 2, DaysMoreThan4hours = 3
                },
                DestinationPlace = new PlaceDto
                {
                    Street = "Jegstrupvænget",
                    StreetNumber = "269",
                    ZipCode = "8310"
                },
                FoodAllowances = new FoodAllowancesDto
                {
                    Morning = 1,
                    Lunch = 1,
                    Dinner = 1
                },
                TransportSpecification = new TransportSpecificationDto
                {
                    KilometersCustom = 42,
                    KilometersCalculated = 43,
                    KilometersOverTaxLimit = 5,
                    KilometersTax = 7,
                    Method = "Method",
                    NumberPlate = "AX68276"
                },
                Purpose = "Purpose"
            };
        }

        public static TravelExpenseUpdateDto GetValidTravelExpenseUpdateDto(TravelExpenseEntity existing, string description=null)
        {
            return new TravelExpenseUpdateDto
            {
                Description = description?? existing.Description + "_" + DateTime.Now.TimeOfDay,
                DailyAllowanceAmount = new DailyAllowanceAmountDto
                {
                    DaysLessThan4hours = existing.DailyAllowanceAmount.DaysLessThan4hours + 1,
                    DaysMoreThan4hours = existing.DailyAllowanceAmount.DaysMoreThan4hours + 1
                },
                DestinationPlace = new PlaceDto
                {
                    Street = existing.DestinationPlace.Street + "_A",
                    StreetNumber = existing.DestinationPlace.StreetNumber + "0",
                    ZipCode = existing.DestinationPlace.ZipCode + "0"
                },
                FoodAllowances = new FoodAllowancesDto
                {
                    Morning = existing.FoodAllowances.Morning + 1,
                    Lunch = existing.FoodAllowances.Lunch + 1,
                    Dinner = existing.FoodAllowances.Dinner + 1
                },
                TransportSpecification = new TransportSpecificationDto
                {
                    KilometersCustom = existing.TransportSpecification.KilometersCustom + 1,
                    KilometersCalculated = existing.TransportSpecification.KilometersCalculated + 1,
                    KilometersOverTaxLimit = existing.TransportSpecification.KilometersOverTaxLimit + 1,
                    KilometersTax = existing.TransportSpecification.KilometersTax + 1,
                    NumberPlate = existing.TransportSpecification.NumberPlate + "X",
                    Method = existing.TransportSpecification.Method + "Y"
                }
            };
        }
    }
}