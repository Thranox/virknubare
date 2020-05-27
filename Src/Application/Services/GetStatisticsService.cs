using System;
using System.Threading.Tasks;
using Application.Dtos;
using Application.Interfaces;
using Domain.Interfaces;

namespace Application.Services
{
    public class GetStatisticsService : IGetStatisticsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetStatisticsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<StatisticsGetResponse> GetAsync(PolApiContext polApiContext)
        {
            throw new NotImplementedException();

            // Gather statistics
            // Insert into response
        }
    }
}