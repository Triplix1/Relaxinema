using AutoMapper;
using Relaxinema.Core.Domain.Entities;
using Relaxinema.Core.DTO.Authorization;
using Relaxinema.Core.DTO.Film;
using Relaxinema.Core.DTO.Genre;
using Relaxinema.Core.DTO.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Relaxinema.Core.DTO.Comment;
using Relaxinema.Core.DTO.Rating;

namespace Relaxinema.Core.MappingProfile
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<User, User>();
            CreateMap<UserDto,User>().ReverseMap();
            CreateMap<RegisterDto, User>().ReverseMap();
            CreateMap<LoginDto,  User>().ReverseMap();

            CreateMap<Role, Role>();

            CreateMap<Film, Film>();
            CreateMap<FilmAddRequest, Film>()
                .ForMember(f => f.Sources, 
                    fad => fad
                        .MapFrom(f => f.Sources));
            CreateMap<FilmUpdateRequest, Film>();
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
                        .MapFrom(_ => DateTime.Now));
            CreateMap<CommentUpdateRequest, Comment>();
            CreateMap<Comment, CommentResponse>();

            CreateMap<Trailer, TrailerResponse>()
                .ForMember(t => t.Trailer, 
                    tr => tr
                        .MapFrom(t => t.Frame));
        }
    }
}
