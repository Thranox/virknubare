using System;
using Domain.SharedKernel;

namespace Web.ApiModels
{
    public class TravelExpenseIdDto : ValueObject
    {
        public Guid Id { get; set; }
    }
}