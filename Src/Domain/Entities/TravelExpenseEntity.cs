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

        public TravelExpenseEntity(string description, UserEntity user, CustomerEntity customer, StageEntity stage,
            DateTime arrivalDateTime, DateTime departureDateTime, int committeeId, string purpose,
            bool isEducationalPurpose, double expenses, bool isAbsenceAllowance, Place destinationPlace,
            TransportSpecification transportSpecification, DailyAllowanceAmount dailyAllowanceAmount, FoodAllowances foodAllowances) : this()
        {
            Description = description ?? throw new ArgumentNullException(nameof(description));
            Stage = stage ?? throw new ArgumentNullException(nameof(stage));
            ArrivalDateTime = arrivalDateTime ;
            DepartureDateTime = departureDateTime ;
            CommitteeId = committeeId ;
            Purpose = purpose ?? throw new ArgumentNullException(nameof(purpose));
            IsEducationalPurpose = isEducationalPurpose;
            Expenses = expenses;
            IsAbsenceAllowance = isAbsenceAllowance;
            DestinationPlace = destinationPlace ?? throw new ArgumentNullException(nameof(destinationPlace));
            TransportSpecification = transportSpecification ?? throw new ArgumentNullException(nameof(transportSpecification));
            DailyAllowanceAmount = dailyAllowanceAmount ?? throw new ArgumentNullException(nameof(dailyAllowanceAmount));
            FoodAllowances = foodAllowances ?? throw new ArgumentNullException(nameof(foodAllowances));
            OwnedByUser = user ?? throw new ArgumentNullException(nameof(user));
            Customer = customer ?? throw new ArgumentNullException(nameof(customer));
        }

        public StageEntity Stage { get; private set; }
        public UserEntity OwnedByUser { get; set; }
        public CustomerEntity Customer { get; set; }
        public string Description { get; private set; }
        public DateTime DepartureDateTime { get; set; }
        public DateTime ArrivalDateTime { get; set; }
        public int CommitteeId { get; set; }
        public string Purpose { get; set; }
        public Place DestinationPlace { get; set; }
        public bool IsEducationalPurpose { get; set; }
        public TransportSpecification TransportSpecification { get; set; }
        public double Expenses { get; set; }
        public bool IsAbsenceAllowance { get; set; }
        public DailyAllowanceAmount DailyAllowanceAmount { get; set; }
        public FoodAllowances FoodAllowances { get; set; }
        public void Enrich(Dictionary<string, string> messageValues)
        {
        }

        public void Update(string description)
        {
            //BR: Can't be updated if reported done:
            if (Stage.Value != TravelExpenseStage.Initial)
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
    }
}