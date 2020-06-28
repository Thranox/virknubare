using System;

namespace Application.Dtos
{
    public class LossOfEarningDto
    {
        public Guid Id { get; set; }
        public int NumberOfHours { get; set; }
        public DateTime Date { get; set; }
        public Guid LossOfEarningSpecId { get; set; }
    }
}