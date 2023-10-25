using AutoMapper;
using Relaxinema.Core.Domain.Entities;
using Relaxinema.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Relaxinema.Core.MappingProfile
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<UserDto,User>().ReverseMap();
            CreateMap<User, User>();
        }
    }
}
