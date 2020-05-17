using System.Collections.Generic;
using System.Linq;
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
            CreateMap<TravelExpenseEntity, TravelExpenseDto>()
                .ForMember(x => x.StageId, xx => xx.MapFrom(xxx => xxx.Stage.Id))
                .ForMember(x => x.StageText, xx => xx.MapFrom(xxx => Globals.StageNamesDanish[xxx.Stage.Value]))
                .ForMember(x=>x.AllowedFlows, xx=>xx.Ignore())
                ;

            CreateMap<FlowStepEntity, FlowStepDto>()
                .ForMember(x => x.FromStageId, xx => xx.MapFrom(xxx => xxx.From.Id))
                .ForMember(x => x.FromStageText, xx => xx.MapFrom(xxx => Globals.StageNamesDanish[xxx.From.Value]))
                .ForMember(x => x.FlowStepId, xx => xx.MapFrom(xxx => xxx.Id))
                ;
        }
    }
}