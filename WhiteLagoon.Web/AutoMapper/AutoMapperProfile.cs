using AutoMapper;
using WhiteLagoon.Domain.Entities;
using WhiteLagoon.Web.Models;

namespace WhiteLagoon.Web.AutoMapper
{
    public class AutoMapperProfile:Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Villa, VillaIndexViewModel>().ReverseMap();
            CreateMap<VillaCreateViewModel, Villa>().ReverseMap();
            CreateMap<VillaUpdateViewModel, Villa>().ReverseMap();
        }
    }
}
