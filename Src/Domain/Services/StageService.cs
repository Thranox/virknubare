using System;
using System.Linq;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Specifications;
using Domain.ValueObjects;

namespace Domain.Services
{
    public class StageService:IStageService
    {
        private readonly IUnitOfWork _unitOfWork;

        public StageService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public StageEntity GetStage(TravelExpenseStage travelExpenseStage)
        {
            var stageEntity = _unitOfWork
                .Repository
                .List(new StageByValue (travelExpenseStage))
                .SingleOrDefault();
            
            if(stageEntity==null) throw new ArgumentNullException(nameof(StageEntity));

            return stageEntity;
        }
    }
}