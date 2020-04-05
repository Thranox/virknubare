using System;
using Domain.SharedKernel;

namespace Web.ApiModels
{
    public class TravelExpenseCreateDto : ValueObject
    {
        public string Description { get; set; }
    }
}