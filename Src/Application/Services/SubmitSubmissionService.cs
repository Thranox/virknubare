using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Interfaces;
using Domain.Specifications;

namespace Application.Services
{
    public class SubmitSubmissionService : ISubmitSubmissionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SubmitSubmissionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<object> SubmitAsync(PolApiContext polApiContext)
        {
            var submissions = _unitOfWork.Repository.List(new SubmissionsReady());

            // Submit all or just the first?!
            foreach (var submissionEntity in submissions)
            {
                // Ftp upload file.

                // Record current time in entity to mark it done.
            }

            throw new System.NotImplementedException();
            await _unitOfWork.CommitAsync();
        }
    }
}