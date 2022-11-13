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
                .ForMember(gm => gm.GenreIds, g
                => g.MapFrom(src => src.GameGenres.Select(x => x.GenreId)))
                .ForMember(gm => gm.ImageIds, g
                    => g.MapFrom(src => src.GameImages.Select(x => x.Id)))
                .ReverseMap();

            CreateMap<GameImage, ImageModel>()
                .ReverseMap();

            CreateMap<Genre, GenreModel>()
                .ReverseMap();

            CreateMap<RefreshToken, RefreshTokenModel>()
                .ReverseMap();

            CreateMap<Comment, CommentModel>()
                .ForMember(cm => cm.CreatedDate, c => 
                    c.MapFrom(src => src.Created))
                .ForMember(cm => cm.ReplyId, c =>
                    c.MapFrom(src => src.ReplieId))
                    .ReverseMap();
                    
            CreateMap<OrderDetails, OrderDetailsModel>()
                .ForMember(odm => odm.Phone, 
                    od => od.MapFrom(src => src.UserPhone))
                .ForMember(odm => odm.OrderedGames, 
                    god => god.MapFrom(src => src.GameOrderDetails))
                .ReverseMap();

            CreateMap<GameOrderDetails, GameOrderDetailsModel>()
                .ReverseMap();
        }
    }
}
