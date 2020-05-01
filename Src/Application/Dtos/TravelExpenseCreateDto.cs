using System;
using Domain.SharedKernel;

namespace Application.Dtos
{
    public class TravelExpenseCreateDto : ValueObject
    {
        public string Description { get; set; }
        public Guid CustomerId { get; set; }
    }
}