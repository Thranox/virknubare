using System;
using System.Threading.Tasks;
using Application.Dtos;
using Application.Interfaces;
using Domain.Interfaces;
using Domain.Specifications;

namespace Application.Services
{
    public class CreateSubmissionService : ICreateSubmissionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateSubmissionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<SubmissionPostResponse> CreateAsync(PolApiContext polApiContext)
        {
            var travelExpenses = _unitOfWork.Repository.List(new TravelExpensesReadyToSend());
            throw new NotImplementedException();
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