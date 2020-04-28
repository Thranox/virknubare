using System;
using System.Threading.Tasks;
using Application.Dtos;
using Application.Interfaces;
using Domain.Interfaces;

namespace Application.Services
{
    public class CreateUserService : ICreateUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateUserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<SubmissionPostResponse> CreateAsync(string name)
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