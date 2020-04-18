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
    public class GetFlowStepService : IGetFlowStepService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public GetFlowStepService(IUnitOfWork unitOfWork, ILogger logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<FlowStepGetResponse> GetAsync(string sub)
        {
            // Get user by sub
            var userEntities = _unitOfWork
                .Repository
                .List(new UserBySubSpecification(sub));
            var userEntity = userEntities
                .SingleOrDefault();

            if (userEntity == null)
                throw new ItemNotFoundException(sub, "UserEntity");

            var flowStepEntities = _unitOfWork.Repository.List(new AllFlowStepsByCustomerId(userEntity.Customer.Id));

            return await Task.FromResult(new FlowStepGetResponse
            {
                Result = flowStepEntities
                    .Select(x => _mapper.Map<FlowStepDto>(x))
            });
        }
    }
}