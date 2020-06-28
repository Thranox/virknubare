using System;
using System.Collections.Generic;
using Domain.SharedKernel;
using Newtonsoft.Json;

namespace Application.Dtos
{
    public class TravelExpenseSingleDto : ValueObject
    {
        public string Description { get; set; }
        public Guid Id { get; set; }
        public bool IsCertified { get; set; }
        public bool IsReportedDone { get; set; }
        public bool IsAssignedPayment { get; set; }
        public string StageId { get; set; }
        public string StageText { get; set; }
        public IEnumerable< AllowedFlowDto> AllowedFlows { get; set; }
        public Guid OwnedByUserId { get; set; }
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

        public LossOfEarningsTableDto LossOfEarningsTable { get; set; }
        public PayoutTableDto PayoutTable { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

        public override IEnumerable<object> GetEqualityComponents()
        {
            yield return Description;
            yield return Id;
            yield return IsCertified;
            yield return IsReportedDone;
            yield return IsAssignedPayment;
            yield return StageId;
            yield return StageText;

            if (AllowedFlows==null)
                yield break;

            foreach (var allowedFlowDto in AllowedFlows)
            {
                foreach (var equalityComponent in allowedFlowDto.GetEqualityComponents())
                {
                    yield return equalityComponent;
                }
            }
        }
    }
}