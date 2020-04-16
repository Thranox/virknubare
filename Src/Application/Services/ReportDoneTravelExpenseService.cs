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
    public class ReportDoneTravelExpenseService : IReportDoneTravelExpenseService
    {
        private readonly IServiceProvider _serviceProvider;

        public ReportDoneTravelExpenseService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<TravelExpenseReportDoneResponse> ReportDoneAsync(TravelExpenseReportDoneDto travelExpenseReportDoneDto)
        {
            using (var unitOfWork = _serviceProvider.GetService<IUnitOfWork>())
            {
                var travelExpenseEntity = unitOfWork
                    .Repository
                    .List(new TravelExpenseById(travelExpenseReportDoneDto.Id))
                    .SingleOrDefault();

                if (travelExpenseEntity == null)
                    throw new ItemNotFoundException(travelExpenseReportDoneDto.Id.ToString(), "TravelExpense");

                travelExpenseEntity.ReportDone();

                unitOfWork
                    .Repository
                    .Update(travelExpenseEntity);

                unitOfWork.Commit();
                return await Task.FromResult(new TravelExpenseReportDoneResponse());
            }
        }
    }
}