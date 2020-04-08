using System;
using Domain.Entities;
using Web.ApiModels;

namespace Web.Validation.Adapters
{
    public class AssignPaymentValidationItemAdapter : IValidationInput
    {
        public AssignPaymentValidationItemAdapter(TravelExpenseAssignPaymentDto travelExpenseAssignPaymentDto, TravelExpenseEntity travelExpenseEntity)
        {
            TravelExpenseEntity = travelExpenseEntity;
        }

        public ValidationInputContextEnum Context { get; }
        public TravelExpenseEntity TravelExpenseEntity { get; }
    }
}