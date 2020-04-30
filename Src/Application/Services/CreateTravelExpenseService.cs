using System.Linq;
using System.Threading.Tasks;
using Application.Dtos;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Specifications;

namespace Application.Services
{
    public class CreateTravelExpenseService : ICreateTravelExpenseService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITravelExpenseFactory _travelExpenseFactory;

        public CreateTravelExpenseService(IUnitOfWork unitOfWork, ITravelExpenseFactory travelExpenseFactory)
        {
            _unitOfWork = unitOfWork;
            _travelExpenseFactory = travelExpenseFactory;
        }

        public async Task<TravelExpenseCreateResponse> CreateAsync(TravelExpenseCreateDto travelExpenseCreateDto,
            string sub)
        {
            var creatingUser = _unitOfWork.Repository.List(new UserBySub(sub)).FirstOrDefault();
            var owningCustomer = _unitOfWork.Repository.GetById<CustomerEntity>(travelExpenseCreateDto.CustomerId);
            var travelExpenseEntity =_travelExpenseFactory.Create(travelExpenseCreateDto.Description, creatingUser,owningCustomer);

            _unitOfWork
                .Repository
                .Add(travelExpenseEntity);

            _unitOfWork.Commit();

            return await Task.FromResult(new TravelExpenseCreateResponse
            {
                Id = travelExpenseEntity.Id
            });
        }
    }
}