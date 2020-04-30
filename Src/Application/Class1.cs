using System.Collections.Generic;
using System.Linq;
using Domain;
using Domain.Entities;
using Domain.Interfaces;

namespace Application
{
    public class TravelExpenseFactory : ITravelExpenseFactory
    {
        private readonly IUnitOfWork _unitOfWork;
        private List<StageEntity> _stages;

        public TravelExpenseFactory(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public TravelExpenseEntity Create(string description, UserEntity userEntityPol, CustomerEntity customer)
        {
            if (_stages==null) _stages = _unitOfWork.Repository.List<StageEntity>();

            return new TravelExpenseEntity(description, userEntityPol, customer,
                _stages.Single(x => x.Value == TravelExpenseStage.Initial));
        }
    }
}