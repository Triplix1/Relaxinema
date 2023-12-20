using AutoMapper;
using Relaxinema.Core.Domain.Entities;
using Relaxinema.Core.DTO.Authorization;
using Relaxinema.Core.DTO.Film;
using Relaxinema.Core.DTO.Genre;
using Relaxinema.Core.DTO.User;
using Relaxinema.Core.DTO.Comment;
using Relaxinema.Core.DTO.Rating;
using Relaxinema.Core.DTO.Subscribe;
using Relaxinema.Core.Extentions;

namespace Relaxinema.Core.MappingProfile
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<User, User>();
            CreateMap<User, UserResponse>();
            CreateMap<RegisterDto, User>();
            CreateMap<LoginDto,  User>();
            CreateMap<User, AccountInfoResponse>();

            CreateMap<Role, Role>();

            CreateMap<Film, Film>();
            CreateMap<FilmAddRequest, Film>()
                .ForMember(f => f.Sources, 
                    fad => fad
                        .MapFrom(f => f.SourceNames.Split(",", StringSplitOptions.None)));
            CreateMap<FilmUpdateRequest, Film>()
                .ForMember(f => f.Sources, f => f
                    .MapFrom(fr => fr.SourceNames.Split(",",StringSplitOptions.None)));
            CreateMap<Film, FilmResponse>()
                .ForMember(fr => fr.GenreNames, 
                    f => f
                        .MapFrom(x => x.Genres.Select(g => g.Name)));
            CreateMap<Film, FilmCardResponse>()
                .ForMember(fr => fr.GenreNames, 
                    f => f
                        .MapFrom(x => x.Genres.Select(g => g.Name)));
            CreateMap<Film, TrailerResponse>();
            

            CreateMap<Genre, Genre>();
            CreateMap<GenreAddRequest, Genre>();
            CreateMap<GenreUpdateRequest, Genre>();
            CreateMap<Genre, GenreResponse>();

            CreateMap<Rating, Rating>();
            CreateMap<RatingRequest, Rating>();
            CreateMap<Rating, RatingResponse>();

            CreateMap<Comment, Comment>();
            CreateMap<CommentAddRequest, Comment>()
                .ForMember(c => c.Created, 
                    car => car
                        .MapFrom(_ => DateTime.Now.SetKindUtc()));
            CreateMap<CommentUpdateRequest, Comment>();
            CreateMap<Comment, CommentResponse>()
                .ForMember(c => c.UserNickname, 
                    cr => 
                        cr.MapFrom(c => c.User.Nickname)
                        )
                .ForMember(c => c.UserPhotoUrl, 
                    co => 
                    co.MapFrom(c => c.User.PhotoUrl)
                    );

            CreateMap<Trailer, TrailerResponse>()
                .ForMember(t => t.Trailer, 
                    tr => tr
                        .MapFrom(t => t.Frame));

            CreateMap<SubscribeAddRequest, Subscription>();
            CreateMap<Subscription, SubscribeResponse>();
        }
    }
}
