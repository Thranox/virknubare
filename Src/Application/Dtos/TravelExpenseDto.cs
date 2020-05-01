using System;
using Domain.SharedKernel;
using Newtonsoft.Json;

namespace Application.Dtos
{
    public class TravelExpenseDto : ValueObject
    {
        public string Description { get; set; }
        public Guid Id { get; set; }
        public bool IsCertified { get; set; }
        public bool IsReportedDone { get; set; }
        public bool IsAssignedPayment { get; set; }
        public string StageId { get; set; }
        public string StageText { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

    }
}