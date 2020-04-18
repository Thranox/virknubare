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

        public CreateTravelExpenseService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<TravelExpenseCreateResponse> CreateAsync(TravelExpenseCreateDto travelExpenseCreateDto,
            string sub)
        {
            var creatingUser = _unitOfWork.Repository.List(new UserBySubSpecification(sub)).FirstOrDefault();
            var travelExpenseEntity = new TravelExpenseEntity(travelExpenseCreateDto.Description, creatingUser);

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