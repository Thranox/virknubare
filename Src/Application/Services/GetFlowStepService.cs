using System.Linq;
using System.Threading.Tasks;
using API.Shared.Services;
using Application.Dtos;
using Application.Interfaces;
using AutoMapper;
using Domain.Interfaces;
using Domain.Specifications;
using Serilog;

namespace Application.Services
{
    public class GetFlowStepService : IGetFlowStepService
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public GetFlowStepService(IUnitOfWork unitOfWork, ILogger logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<FlowStepGetResponse> GetAsync(PolApiContext polApiContext)
        {
            var flowStepEntities = _unitOfWork.Repository.List(new AllFlowStepsByUserId(polApiContext.CallingUser.Id));

            return await Task.FromResult(new FlowStepGetResponse
            {
                Result = flowStepEntities
                    .Select(x => _mapper.Map<FlowStepDto>(x))
            });
        }
    }
}