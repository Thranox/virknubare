using System;
using System.Threading.Tasks;
using Domain.Interfaces;
using Domain.Responses;

namespace Application.Services
{
    public class CreateCustomerService : ICreateCustomerService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateCustomerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<CreateCustomerResponse> CreateAsync(string name)
        {
            throw new NotImplementedException();

            //var travelExpenses = _unitOfWork.Repository.List(new TravelExpensesReadyToSend());
            //CreateCsv(travelExpenses);
            //foreach (var travelExpense in travelExpenses)
            //{
            //    travelExpense.Stage = TravelExpenseStage.Final
            //}
            //_unitOfWork.Commit();

            // ftpService.UploadAllWaiting();
        }
    }
}