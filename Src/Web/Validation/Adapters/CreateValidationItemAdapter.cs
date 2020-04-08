using System;
using Domain.Entities;
using Web.ApiModels;

namespace Web.Validation.Adapters
{
    public class CreateValidationItemAdapter : IValidationInput
    {
        public CreateValidationItemAdapter(TravelExpenseCreateDto travelExpenseCreateDto, TravelExpenseEntity travelExpenseEntity)
        {
            Context = ValidationInputContextEnum.Create;
            TravelExpenseEntity = travelExpenseEntity;
        }

        public ValidationInputContextEnum Context { get; }
        public TravelExpenseEntity TravelExpenseEntity { get; }
    }
}