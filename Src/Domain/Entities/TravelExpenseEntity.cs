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
            TransportSpecification transportSpecification, DailyAllowanceAmount dailyAllowanceAmount,
            FoodAllowances foodAllowances, LossOfEarningEntity[] lossOfEarningEntities) : this()
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
            LossOfEarningEntities = lossOfEarningEntities;
        }

        public ICollection< LossOfEarningEntity> LossOfEarningEntities { get; set; }

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

        public void Update(string description, 
            DateTime arrivalDateTime, DateTime departureDateTime, int committeeId, string purpose,
            bool isEducationalPurpose, double expenses, bool isAbsenceAllowance, Place destinationPlace,
            TransportSpecification transportSpecification, DailyAllowanceAmount dailyAllowanceAmount, FoodAllowances foodAllowances)
        {
            //BR: Can't be updated if reported done:
            if (Stage.Value != TravelExpenseStage.Initial)
                throw new BusinessRuleViolationException(Id, "Rejseafregning kan ikke ændres når den er færdigmeldt.");

            Description = description;
            ArrivalDateTime = arrivalDateTime;
            DepartureDateTime = departureDateTime;
            CommitteeId = committeeId;
            Purpose = purpose;
            IsEducationalPurpose = isEducationalPurpose;
            Expenses = expenses;
            IsAbsenceAllowance = isAbsenceAllowance;
            DestinationPlace = destinationPlace;
            TransportSpecification = transportSpecification;
            DailyAllowanceAmount = dailyAllowanceAmount;
            FoodAllowances = foodAllowances;

            Events.Add(new TravelExpenseUpdatedDomainEvent());
        }

        public void ApplyProcessStep(IProcessFlowStep processFlowStep)
        {
            var stageBefore = Stage;
            var travelExpenseStage = processFlowStep.GetResultingStage(this);
            Stage = travelExpenseStage;

            Events.Add(new TravelExpenseUpdatedDomainEvent());
        }

        public PayoutTable CalculatePayoutTable()
        {
            var payoutTable= new PayoutTable(new []{ "Art","Enheder","Sats","Beløb","Lønart"});
            payoutTable.AddRow(PayoutTableRow.Create( new [] {"Fraværsgodtgørelse","","","0,00 kr.","959" }));
            payoutTable.AddRow(PayoutTableRow.Create(new[] { "Fradrag i fraværsopgørelse", "", "", "0,00 kr.", "" }));
            payoutTable.AddRow(PayoutTableRow.Create(new[] { "Fradragsgodtgørelse til udbetaling", "", "", "260,50 kr.", "" }));
            payoutTable.AddRow(PayoutTableRow.Create(new[] { "Diæter over 4 timer", "1", "860,00 kr.", "860,00 kr.", "893" }));
            payoutTable.AddRow(PayoutTableRow.Create(new[] { "KM-godtgørelse", "155", "3,52 kr.", "545,60 kr.", "915" }));
            payoutTable.AddRow(PayoutTableRow.Create(new[] { new[] { "Tabt arbejdsfortjeneste"}, new[] {"0","0","0"}, new[] { "300,00 kr.", "400,00 kr.", "500,00 kr." }, new [] { "0,00 kr.", "0,00 kr.", "0,00 kr." }, new [] { "894", "894", "894" }} ));

            return payoutTable;
        }
    }
}