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
            DateTime arrivalDateTime)
        {
            if (_stages == null) _stages = _unitOfWork.Repository.List<StageEntity>();

            var stageEntity = _stages.Single(x => x.Value == TravelExpenseStage.Initial);
            return new TravelExpenseEntity(description, userEntityPol, customer,stageEntity)
            {
                ArrivalDateTime = arrivalDateTime
            };
        }
    }
}