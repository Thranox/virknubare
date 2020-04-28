using System;
using System.Linq;
using System.Threading.Tasks;
using Application.Dtos;
using Application.Interfaces;
using AutoMapper;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.Specifications;
using Serilog;

namespace Application.Services
{
    public class GetTravelExpenseService : IGetTravelExpenseService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;

        public GetTravelExpenseService(IMapper mapper, IUnitOfWork unitOfWork, ILogger logger)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<TravelExpenseGetResponse> GetAsync(string sub)
        {
            // Get user by sub
            var userEntities = _unitOfWork
                .Repository
                .List(new UserBySubSpecification(sub));
            var userEntity = userEntities
                .SingleOrDefault();

            if (userEntity == null)
                throw new ItemNotFoundException(sub, "UserEntity");

            // Get the stages for which the user may manipulate travel expenses
            var travelExpenseStages =
                userEntity.FlowStepUserPermissions.Select(x => x.FlowStep.From).Distinct().ToList();
            var customer = userEntity.Customer;

            _logger.Debug("During GetAsync -- travelExpenseStages={travelExpenseStages}", travelExpenseStages);

            // The user may see the travel expense if
            // 1) owned by the user or if
            // 2) user may manipulate travel expense stage
            var travelExpenseEntities = customer
                .TravelExpenses
                .Where(x =>
                    travelExpenseStages.Contains(x.Stage) || x.OwnedByUser == userEntity
                );

            return await Task.FromResult(new TravelExpenseGetResponse
            {
                Result = travelExpenseEntities
                    .Select(x => _mapper.Map<TravelExpenseDto>(x))
            });
        }

        public async Task<TravelExpenseGetByIdResponse> GetByIdAsync(Guid id, string sub)
        {
            // Get user by sub
            var userEntities = _unitOfWork
                .Repository
                .List(new UserBySubSpecification(sub));
            var userEntity = userEntities
                .SingleOrDefault();

            if (userEntity == null)
                throw new ItemNotFoundException(sub, "UserEntity");

            // Get the stages for which the user may manipulate travel expenses
            var travelExpenseStages =
                userEntity.FlowStepUserPermissions.Select(x => x.FlowStep.From).Distinct().ToList();
            var customer = userEntity.Customer;

            // Get the travel expenses with the requested id
            var travelExpenseEntities1 = customer
                .TravelExpenses
                .Where(x => x.Id == id).ToList();

            // Respond "not found" if there is no such travel expense
            if (!travelExpenseEntities1.Any())
                throw new ItemNotFoundException(id.ToString(), "TravelExpense");

            // Get travel expense filtered for visibility
            var travelExpenseEntity =
                travelExpenseEntities1.SingleOrDefault(x =>
                    travelExpenseStages.Contains(x.Stage) || x.OwnedByUser == userEntity
                );

            // If not allowed for user (but it existed) we respond Access Denied
            if (travelExpenseEntity == null)
                throw new ItemNotAllowedException(id.ToString(), "TravelExpense");


            return await Task.FromResult(
                new TravelExpenseGetByIdResponse
                {
                    Result = _mapper.Map<TravelExpenseDto>(travelExpenseEntity)
                });
        }
    }
}