using AutoMapper;
using Domain.Entities;
using Web.ApiModels;

namespace Web.Controllers
{
    public class TravelExpenseProfile:Profile
    {
        public TravelExpenseProfile()
        {
            CreateMap<TravelExpenseEntity, TravelExpenseDto>();
        }
    }
}