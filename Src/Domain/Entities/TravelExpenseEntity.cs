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
            Description = description;

            Events.Add(new TravelExpenseUpdatedDomainEvent());
        }

        public void Certify()
        {
            IsCertified = true;

            Events.Add(new TravelExpenseUpdatedDomainEvent());
        }

        public void ReportDone()
        {
            IsReportedDone = true;

            Events.Add(new TravelExpenseUpdatedDomainEvent());
        }

        public void AssignPayment()
        {
            IsAssignedPayment = true;

            Events.Add(new TravelExpenseUpdatedDomainEvent());
        }
    }
}