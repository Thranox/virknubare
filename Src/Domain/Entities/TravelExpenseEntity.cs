using System;
using CleanArchitecture.Core.SharedKernel;

namespace Domain
{
    public class TravelExpenseEntity: BaseEntity
    {
        public string Description { get; set; }
    }
}
