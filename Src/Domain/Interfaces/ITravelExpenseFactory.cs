using System;
using Domain.Entities;
using Domain.ValueObjects;

namespace Domain.Interfaces
{
    public interface ITravelExpenseFactory
    {
        TravelExpenseEntity Create(string description, UserEntity userEntityPol, CustomerEntity customer,
            DateTime arrivalDateTime, DateTime departureDateTime, int committeeId, string purpose,
            bool isEducationalPurpose, double expenses, bool isAbsenceAllowance, Place destinationPlace,
            TransportSpecification transportSpecification, DailyAllowanceAmount dailyAllowanceAmount,
            FoodAllowances foodAllowances);
    }
}