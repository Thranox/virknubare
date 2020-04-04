using AutoMapper;
using Domain;
using Web.ApiModels;

namespace Web.Controllers
{
    public class TravelExpenseProfile:Profile
    {
        public TravelExpenseProfile()
        {
            CreateMap<TravelExpenseEntity, TravelExpenseDto>()
                //.ForMember(dest =>
                //        dest.FName,
                //    opt => opt.MapFrom(src => src.FirstName))
                //.ForMember(dest =>
                //        dest.LName,
                //    opt => opt.MapFrom(src => src.LastName))
                ;
        }
    }
}