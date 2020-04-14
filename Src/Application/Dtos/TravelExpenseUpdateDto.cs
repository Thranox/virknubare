using System;
using Domain.SharedKernel;

namespace Application.Dtos
{
    public class TravelExpenseUpdateDto : ValueObject
    {
        public string Description { get; set; }
        public Guid Id { get; set; }
    }
}