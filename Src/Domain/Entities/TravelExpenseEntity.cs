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

        public void Update(string description)
        {
            Description = description;
        }

        public void Certify()
        {
            IsCertified = true;
        }

        public void ReportDone()
        {
            IsReportedDone = true;
        }
    }
}