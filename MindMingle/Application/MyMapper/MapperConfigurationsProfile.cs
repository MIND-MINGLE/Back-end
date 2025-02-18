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
using Application.Request.Patient;

namespace Application.MyMapper
{
    public class MapperConfigurationsProfile : Profile
    {
        public MapperConfigurationsProfile()
        {
            //Account
            CreateMap<Account, ResponseAccount>();
            CreateMap<Account, RequestAccount>();
            //Role
            CreateMap<Role, ResponseRole>();

			//Patient
			CreateMap<CreateNewPatientRequest, Patient>()
			.ForMember(dest => dest.PatientId, opt => opt.MapFrom(src => Guid.NewGuid())); // Tạo GUID mới
			CreateMap<Patient, ResponsePatient>();


		}
	}
}
