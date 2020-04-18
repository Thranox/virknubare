using Application.Dtos;
using AutoMapper;
using Domain.Entities;

namespace Web.MapperProfiles
{
    public class EntityDtoProfile : Profile
    {
        public EntityDtoProfile()
        {
            CreateMap<TravelExpenseEntity, TravelExpenseDto>();
            CreateMap<FlowStepEntity, FlowStepDto>();
        }
    }
}