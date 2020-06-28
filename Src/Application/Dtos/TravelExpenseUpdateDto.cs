﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Domain.SharedKernel;

namespace Application.Dtos
{
    public class TravelExpenseUpdateDto : ValueObject
    {
        [Required]
        public string Description { get; set; }
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
            yield return ArrivalDateTime;
            yield return DepartureDateTime;
            yield return CommitteeId;
            yield return Purpose;
            yield return IsEducationalPurpose;
            yield return Expenses;
            yield return IsAbsenceAllowance;
            yield return DestinationPlace;
            yield return TransportSpecification;
            yield return DailyAllowanceAmount;
            yield return FoodAllowances;
        }
    }
}