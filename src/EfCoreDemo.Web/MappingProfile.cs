using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EfCoreDemo.Core.Dto;
using EfCoreDemo.Core.Entities;

namespace EfCoreDemo.Web
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, SimpleUser>(); //Map from User Object to SimpleUser Object
        }
    }
}
