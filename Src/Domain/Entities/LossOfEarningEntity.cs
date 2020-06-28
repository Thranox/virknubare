using System;
using Domain.SharedKernel;

namespace Domain.Entities
{
    public class LossOfEarningEntity:BaseEntity
    {
        public int NumberOfHours { get; set; }
        public DateTime Date { get; set; }
        public TravelExpenseEntity TravelExpense { get; set; }
        public LossOfEarningSpecEntity LossOfEarningSpec { get; set; }
    }
}