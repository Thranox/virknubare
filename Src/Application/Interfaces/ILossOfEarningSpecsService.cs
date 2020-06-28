using System.Collections.Generic;
using Domain.Entities;

namespace API.Shared.Controllers
{
    public interface ILossOfEarningSpecsService
    {
        IEnumerable<LossOfEarningSpecEntity> Get();
    }
}