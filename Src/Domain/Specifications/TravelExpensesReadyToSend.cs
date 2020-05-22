using System;
using System.Linq.Expressions;
using Domain.Entities;
using Domain.Interfaces;
using Domain.ValueObjects;

namespace Domain.Specifications
{
    public class TravelExpensesReadyToSend : ISpecification<TravelExpenseEntity>
    {
        public TravelExpensesReadyToSend()
        {
            Criteria = e => e.Stage.Value == TravelExpenseStage.AssignedForPayment;
        }

        public Expression<Func<TravelExpenseEntity, bool>> Criteria { get; }
    }
}