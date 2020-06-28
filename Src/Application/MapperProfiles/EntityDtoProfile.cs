using API.Shared.Controllers;
using Application.Dtos;
using AutoMapper;
using Domain;
using Domain.Entities;
using Domain.ValueObjects;

namespace Application.MapperProfiles
{
    public class EntityDtoProfile : Profile
    {
        public EntityDtoProfile()
        {
            CreateMap<TravelExpenseEntity, TravelExpenseInListDto>()
                .ForMember(x => x.StageId, xx => xx.MapFrom(xxx => xxx.Stage.Id))
                .ForMember(x => x.StageText, xx => xx.MapFrom(xxx => Globals.StageNamesDanish[xxx.Stage.Value]))
                .ForMember(x => x.AllowedFlows, xx => xx.Ignore())
                ;
            CreateMap<TravelExpenseEntity, TravelExpenseSingleDto>()
                .ForMember(x => x.StageId, xx => xx.MapFrom(xxx => xxx.Stage.Id))
                .ForMember(x => x.StageText, xx => xx.MapFrom(xxx => Globals.StageNamesDanish[xxx.Stage.Value]))
                .ForMember(x => x.AllowedFlows, xx => xx.Ignore())
                .ForMember(x => x.PayoutTable, xx => xx.Ignore())
                ;
            CreateMap<FlowStepEntity, FlowStepDto>()
                .ForMember(x => x.FromStageId, xx => xx.MapFrom(xxx => xxx.From.Id))
                .ForMember(x => x.FromStageText, xx => xx.MapFrom(xxx => Globals.StageNamesDanish[xxx.From.Value]))
                .ForMember(x => x.FlowStepId, xx => xx.MapFrom(xxx => xxx.Id))
                ;
            CreateMap<CustomerUserPermissionEntity, UserPermissionDto>()
                .ForMember(x => x.UserId, xx => xx.MapFrom(xxx => xxx.UserId))
                .ForMember(x => x.UserStatus, xx => xx.MapFrom(xxx => (int) xxx.UserStatus))
                ;
            CreateMap<Place, PlaceDto>()
                ;
            CreateMap<TransportSpecification, TransportSpecificationDto>()
                ;
            CreateMap<DailyAllowanceAmount, DailyAllowanceAmountDto>()
                ;
            CreateMap<FoodAllowances, FoodAllowancesDto>()
                ;
            CreateMap<PayoutTable, PayoutTableDto>()
                ;
            CreateMap<LossOfEarningSpecEntity, LossOfEarningSpecDto>()
                ;
        }
    }
}