using System;
using System.ComponentModel.DataAnnotations;
using Domain.SharedKernel;

namespace Application.Dtos
{
    public class TravelExpenseUpdateDto : ValueObject
    {
        [Required]
        public string Description { get; set; }
        [Required]
        public Guid Id { get; set; }
    }
}