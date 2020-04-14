using Application.Dtos;
using AutoMapper;
using Domain.Entities;

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