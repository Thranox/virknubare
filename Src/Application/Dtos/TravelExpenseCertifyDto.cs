using System;
using Domain.SharedKernel;

namespace Application.Dtos
{
    public class TravelExpenseCertifyDto:ValueObject
    {
        public Guid Id { get; set; }
    }
}