using System;
using System.Collections.Generic;
using Domain.Events;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.SharedKernel;
using Domain.ValueObjects;

namespace Domain.Entities
{
    public class TravelExpenseEntity : BaseEntity, IMessageValueEnricher
    {
        private TravelExpenseEntity()
        {
        }

        public TravelExpenseEntity(string description, UserEntity user, CustomerEntity customer, StageEntity stage) : this()
        {
            if(string.IsNullOrEmpty( description))
                throw new ArgumentNullException(nameof(description));

            Description = description;
            Stage = stage??throw new ArgumentNullException(nameof(stage));
            OwnedByUser = user ?? throw new ArgumentNullException(nameof(user));
            Customer = customer ?? throw new ArgumentNullException(nameof(customer));
        }

        public StageEntity Stage { get; private set; }

        public UserEntity OwnedByUser { get; private set; }
        public CustomerEntity Customer { get; private set; }

        public string Description { get; private set; }

        public void Update(string description)
        {
            //BR: Can't be updated if reported done:
            if (Stage.Value!=TravelExpenseStage.Initial)
                throw new BusinessRuleViolationException(Id, "Rejseafregning kan ikke ændres når den er færdigmeldt.");

            Description = description;

            Events.Add(new TravelExpenseUpdatedDomainEvent());
        }

        public void ApplyProcessStep(IProcessFlowStep processFlowStep)
        {
            var stageBefore = Stage;
            var travelExpenseStage = processFlowStep.GetResultingStage(this);
            Stage = travelExpenseStage;

            Events.Add(new TravelExpenseUpdatedDomainEvent());
        }

        public void Enrich(Dictionary<string, string> messageValues)
        {
        }
    }
}