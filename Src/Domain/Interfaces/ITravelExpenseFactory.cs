using System;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface ITravelExpenseFactory
    {
        TravelExpenseEntity Create(string description, UserEntity userEntityPol, CustomerEntity customer,
            DateTime arrivalDateTime);
    }
}