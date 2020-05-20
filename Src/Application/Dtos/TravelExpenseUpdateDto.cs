using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CSharpFunctionalExtensions;
using Domain.SharedKernel;

namespace Application.Dtos
{
    public class TravelExpenseUpdateDto : ValueObject
    {
        [Required]
        public string Description { get; set; }

        public override IEnumerable<object> GetEqualityComponents()
        {
            yield return Description;
        }
    }
}