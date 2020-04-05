using System;
using Domain.SharedKernel;

namespace Web.ApiModels
{
    public class TravelExpenseUpdateDto : ValueObject
    {
        public string Description { get; set; }
        public Guid PublicId { get; set; }
    }
}