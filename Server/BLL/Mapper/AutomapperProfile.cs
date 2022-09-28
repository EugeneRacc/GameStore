
using AutoMapper;
using BLL.Models;
using DAL.Entities;

namespace BLL.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Game, GameModel>()
                .ForMember(x => x.Price,
                    u => u.MapFrom(y => y.Price))
                .ForMember(x => x.Id,
                    u => u.MapFrom(y => y.Id))
                .ForMember(x => x.Title,
                    u => u.MapFrom(y => y.Title))
                .ForMember(x => x.Description,
                    u => u.MapFrom(y => y.Description))
                .ReverseMap();

            CreateMap<GameModel, GameModel>()
                .ReverseMap();
        }
    }
}
