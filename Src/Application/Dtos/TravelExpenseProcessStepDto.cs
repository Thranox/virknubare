using System;

namespace Application.Dtos
{
    public class TravelExpenseProcessStepDto
    {
        public Guid TravelExpenseId { get; set; }
        public string ProcessStepKey { get; set; }
    }
}