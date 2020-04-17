using System;
using System.Linq;
using System.Threading.Tasks;
using Application.Dtos;
using Application.Interfaces;
using AutoMapper;
using Domain;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Specifications;

namespace Application.Services
{
    public class GetTravelExpenseService : IGetTravelExpenseService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public GetTravelExpenseService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<TravelExpenseGetResponse> GetAsync(string sub)
        {
            var userEntities = _unitOfWork
                .Repository
                .List<UserEntity>(new UserBySubSpecification(sub));
            var userEntity = userEntities
                .SingleOrDefault();

            if(userEntity==null)
                throw new ItemNotFoundException(sub, "UserEntity");

            var travelExpenseStages = userEntity.FlowStepUserPermissions.Select(x => x.FlowStep.From).Distinct().ToList();
            var customer = userEntity.Customer;

            return await Task.FromResult(new TravelExpenseGetResponse
            {
                Result = customer
                    .TravelExpenses
                    .Where(x=>
                        travelExpenseStages.Contains( x.Stage) || 
                        x.OwnedByUserEntity == userEntity)
                    .Select(x => _mapper.Map<TravelExpenseDto>(x))
            });
        }

        public async Task<TravelExpenseGetByIdResponse> GetByIdAsync(Guid id, string sub)
        {
            var travelExpenseEntity = _unitOfWork
                .Repository
                .GetById<TravelExpenseEntity>(id);

            if (travelExpenseEntity == null)
                throw new ItemNotFoundException(id.ToString(), "TravelExpense");

            return await Task.FromResult(
                new TravelExpenseGetByIdResponse
                {
                    Result = _mapper.Map<TravelExpenseDto>(travelExpenseEntity)
                });
        }
    }
}