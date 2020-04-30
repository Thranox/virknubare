using System;
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

        public TravelExpenseEntity(string description, UserEntity user, CustomerEntity customer) : this()
        {
            Description = description;
            Stage = TravelExpenseStage.Initial;
            OwnedByUser = user ?? throw new ArgumentNullException(nameof(user));
            Customer = customer ?? throw new ArgumentNullException(nameof(customer));
        }

        public TravelExpenseStage Stage { get; private set; }

        public UserEntity OwnedByUser { get; private set; }
        public CustomerEntity Customer { get; private set; }

        public string Description { get; private set; }

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