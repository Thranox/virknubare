using System;
using System.Linq;
using System.Threading.Tasks;
using Application.Dtos;
using Application.Interfaces;
using Domain;
using Domain.Interfaces;
using Domain.Specifications;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Application.Services
{
    public class AssignPaymentTravelExpenseService : IAssignPaymentTravelExpenseService
    {
        private readonly IServiceProvider _serviceProvider;
        private ILogger _logger;

        public AssignPaymentTravelExpenseService(IServiceProvider serviceProvider, ILogger logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public async Task<TravelExpenseAssignPaymentResponse> AssignPaymentAsync(
            TravelExpenseAssignPaymentDto travelExpenseAssignPaymentDto)
        {
            using (var unitOfWork = _serviceProvider.GetService<IUnitOfWork>())
            {
                var travelExpenseEntity = unitOfWork
                    .Repository
                    .List(new TravelExpenseById(travelExpenseAssignPaymentDto.Id))
                    .SingleOrDefault();

                if (travelExpenseEntity == null)
                    throw new TravelExpenseNotFoundByIdException(travelExpenseAssignPaymentDto.Id);

                travelExpenseEntity.AssignPayment();

                unitOfWork
                    .Repository
                    .Update(travelExpenseEntity);
                unitOfWork.Commit();

                return await Task.FromResult(new TravelExpenseAssignPaymentResponse());
            }
        }
    }
}