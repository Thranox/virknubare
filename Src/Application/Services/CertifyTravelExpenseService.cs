using System;
using System.Linq;
using System.Threading.Tasks;
using Application.Dtos;
using Application.Interfaces;
using Domain;
using Domain.Interfaces;
using Domain.Specifications;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Services
{
    public class CertifyTravelExpenseService : ICertifyTravelExpenseService
    {
        private readonly IServiceProvider _serviceProvider;

        public CertifyTravelExpenseService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<TravelExpenseCertifyResponse> CertifyAsync(TravelExpenseCertifyDto travelExpenseCertifyDto)
        {
            using (var unitOfWork = _serviceProvider.GetService<IUnitOfWork>())
            {
                var travelExpenseEntity = unitOfWork
                    .Repository
                    .List(new TravelExpenseById(travelExpenseCertifyDto.Id))
                    .SingleOrDefault();

                if (travelExpenseEntity == null)
                    throw new TravelExpenseNotFoundByIdException(travelExpenseCertifyDto.Id);

                travelExpenseEntity.Certify();

                unitOfWork
                    .Repository
                    .Update(travelExpenseEntity);

                unitOfWork.Commit();

                return await Task.FromResult(new TravelExpenseCertifyResponse());
            }
        }
    }
}