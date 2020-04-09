using Domain.SharedKernel;

namespace Domain.Entities
{
    public class TravelExpenseEntity : BaseEntity
    {
        private TravelExpenseEntity()
        {
        }

        public TravelExpenseEntity(string description) : this()
        {
            Description = description;
        }

        public string Description { get; private set; }
        public bool IsCertified { get; private set; }
        public bool IsReportedDone { get; private set; }
        public bool IsAssignedPayment { get; private set; }

        public void Update(string description)
        {
            //BR: Can't be updated if reported done:
            if(IsReportedDone) 
                throw new BusinessRuleViolationException(Id, "Rejseafregning kan ikke ændres når den er færdigmeldt.");
            
            Description = description;

            Events.Add(new TravelExpenseUpdatedDomainEvent());
        }

        public void Certify()
        {
            //BR: Can't be certified if not reported done:
            if (!IsReportedDone) 
                throw new BusinessRuleViolationException(Id, "Rejseafregning kan ikke attesteres da den ikke er færdigmeldt.");
            if (IsCertified) 
                throw new BusinessRuleViolationException(Id, "Rejseafregning kan ikke attesteres da den allerede er attesteret.");

            IsCertified = true;

            Events.Add(new TravelExpenseUpdatedDomainEvent());
        }

        public void ReportDone()
        {
            //BR: Can't be reported done unless...:
            //if (IsReportedDone) throw new BusinessRuleViolationException(Id, "Rejseafregning kan ikke ændres når den er færdigmeldt.");
            //BR: Can't be certified if not reported done:
            if (IsReportedDone) 
                throw new BusinessRuleViolationException(Id, "Rejseafregning kan ikke færdigmeldes da den allerede er færdigmeldt.");

            IsReportedDone = true;

            Events.Add(new TravelExpenseUpdatedDomainEvent());
        }

        public void AssignPayment()
        {
            //BR: Can't be assigned payment if not certified:
            if (!IsCertified)
                throw new BusinessRuleViolationException(Id, "Rejseafregning kan ikke anvises til betaling da den ikke er attesteret.");

            //BR: Can't be assigned payment if already assigned payment:
            if (IsAssignedPayment)
                throw new BusinessRuleViolationException(Id, "Rejseafregning kan ikke anvises til betaling da den allerede er anvist til betaling.");

            IsAssignedPayment = true;

            Events.Add(new TravelExpenseUpdatedDomainEvent());
        }
    }
}