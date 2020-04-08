using System;
using Domain.SharedKernel;

namespace Web.ApiModels
{
    public class TravelExpenseCertifyDto:ValueObject
    {
        public Guid Id { get; set; }
    }
}