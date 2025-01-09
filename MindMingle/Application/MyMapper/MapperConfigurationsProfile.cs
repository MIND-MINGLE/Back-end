using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Application.Response;
using Application.Request;
using Domain.Entity;
using Application.Request.Account;

namespace Application.MyMapper
{
    public class MapperConfigurationsProfile : Profile
    {
        public MapperConfigurationsProfile()
        {
            //Account
            CreateMap<Account, ResponseAccount>().ReverseMap();
            CreateMap<Account, RequestAccount>().ReverseMap();
            //
        }
    }
}
