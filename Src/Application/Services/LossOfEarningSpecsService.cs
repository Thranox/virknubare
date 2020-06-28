using System.Collections.Generic;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Specifications;

namespace API.Shared.Controllers
{
    public class LossOfEarningSpecsService : ILossOfEarningSpecsService
    {
        private readonly IRepository _repository;

        public LossOfEarningSpecsService(IRepository repository)
        {
            _repository = repository;
        }
        public IEnumerable<LossOfEarningSpecEntity> Get()
        {
            return _repository.List(new GetAllLossOfEarningSpec());
        }
    }
}