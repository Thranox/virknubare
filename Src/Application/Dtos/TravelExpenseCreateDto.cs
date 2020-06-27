using System;
using System.Collections.Generic;
using Domain.SharedKernel;

namespace Application.Dtos
{
    public class TravelExpenseCreateDto : ValueObject
    {
        public string Description { get; set; }
        public Guid CustomerId { get; set; }
        public DateTime ArrivalDateTime { get; set; }
        public DateTime DepartureDateTime { get; set; }
        public int CommitteeId { get; set; }
        public string Purpose { get; set; }
        public bool IsEducationalPurpose { get; set; }
        public double Expenses { get; set; }
        public bool IsAbsenceAllowance { get; set; }
        public PlaceDto DestinationPlace { get; set; }
        public TransportSpecificationDto TransportSpecification { get; set; }
        public DailyAllowanceAmountDto DailyAllowanceAmount { get; set; }
        public FoodAllowancesDto FoodAllowances { get; set; }

        public override IEnumerable<object> GetEqualityComponents()
        {
            yield return Description;
            yield return CustomerId;
        }
    }

    public class FoodAllowancesDto:ValueObject
    {
        public override IEnumerable<object> GetEqualityComponents()
        {
            throw new NotImplementedException();
        }

        public int Morning { get; set; }
        public int Lunch { get; set; }
        public int Dinner { get; set; }
    }
}