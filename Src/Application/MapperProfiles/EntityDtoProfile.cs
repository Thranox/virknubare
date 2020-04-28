using Application.Dtos;
using AutoMapper;
using Domain;
using Domain.Entities;

namespace Application.MapperProfiles
{
    public class EntityDtoProfile : Profile
    {
        public EntityDtoProfile()
        {
            CreateMap<TravelExpenseEntity, TravelExpenseDto>();
            CreateMap<FlowStepEntity, FlowStepDto>()
                .ForMember(x => x.FromStageId, xx => xx.MapFrom(xxx => xxx.From.Id))
                .ForMember(x => x.FromStageText, xx => xx.MapFrom(xxx =>Globals.StageNamesDanish[ xxx.From.Value]))
                ;
        }
    }
}