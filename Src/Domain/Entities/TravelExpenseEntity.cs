using Domain.Events;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.SharedKernel;

namespace Domain.Entities
{
    public class TravelExpenseEntity : BaseEntity
    {
        private TravelExpenseEntity()
        {
        }

        public TravelExpenseEntity(string description, UserEntity userEntity) : this()
        {
            Description = description;
            Stage = TravelExpenseStage.Initial;
            OwnedByUserEntity = userEntity;
        }

        public TravelExpenseStage Stage { get; private set; }

        public UserEntity OwnedByUserEntity { get; private set; }

        public string Description { get; private set; }
        public bool IsCertified { get; private set; }
        public bool IsReportedDone { get; private set; }
        public bool IsAssignedPayment { get; private set; }

        public void Update(string description)
        {
            //BR: Can't be updated if reported done:
            if (Stage!=TravelExpenseStage.Initial)
                throw new BusinessRuleViolationException(Id, "Rejseafregning kan ikke ændres når den er færdigmeldt.");

            Description = description;

            Events.Add(new TravelExpenseUpdatedDomainEvent());
        }

        public void ApplyProcessStep(IProcessFlowStep processFlowStep)
        {
            var travelExpenseStage = processFlowStep.GetResultingStage(this);
            Stage = travelExpenseStage;

            Events.Add(new TravelExpenseUpdatedDomainEvent());
        }
    }
}