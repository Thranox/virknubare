using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Entities;
using Domain.Interfaces;
using Domain.ValueObjects;

namespace Domain
{
    public class TravelExpenseFactory : ITravelExpenseFactory
    {
        private readonly IUnitOfWork _unitOfWork;
        private List<StageEntity> _stages;

        public TravelExpenseFactory(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public TravelExpenseEntity Create(string description, UserEntity userEntityPol, CustomerEntity customer,
            DateTime arrivalDateTime, DateTime departureDateTime, int committeeId, string purpose,
            bool isEducationalPurpose, double expenses, bool isAbsenceAllowance, Place destinationPlace,
            TransportSpecification transportSpecification, DailyAllowanceAmount dailyAllowanceAmount,
            FoodAllowances foodAllowances, LossOfEarningEntity[] lossOfEarningEntities)
        {
            if (_stages == null) _stages = _unitOfWork.Repository.List<StageEntity>();

            var stageEntity = _stages.Single(x => x.Value == TravelExpenseStage.Initial);
            return new TravelExpenseEntity(description, userEntityPol, customer, stageEntity, arrivalDateTime,
                departureDateTime, committeeId, purpose, isEducationalPurpose, expenses, isAbsenceAllowance,
                destinationPlace, transportSpecification, dailyAllowanceAmount,foodAllowances,lossOfEarningEntities);
        }
    }
}