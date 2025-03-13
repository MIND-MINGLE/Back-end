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
using Application.Request.ChatGroupRequest;
using Application.Response.UsersInGroup;
using Application.Request.UsersInGroup;
using Application.Request.ChatMessage;
using Application.Response.ChatMessage;
using Application.Response.ChatGroup;
using Application.Request.Therapist;
using Application.Request.Question;

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
            .ForMember(dest => dest.PatientId, opt => opt.MapFrom(src => Guid.NewGuid().ToString()));  // Tạo GUID mới
            CreateMap<Patient, ResponsePatient>()
                .ForMember(dest => dest.Dob, opt => opt.Ignore()); // ignore, we custom Date
            //Therapist
            CreateMap<AddNewTherapistRequest, Therapist>()
                .ForMember(t => t.TherapistId, opt => opt.MapFrom(src => Guid.NewGuid().ToString()));
            CreateMap<Therapist, ResponseTherapist>()
               .ForMember(dest => dest.Dob, opt => opt.Ignore()); // ignore, we custom Date
            // ChatGroup Mapper
            CreateMap<AddChatGroupRequest, ChatGroup>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src =>
               Guid.NewGuid().ToString()
            ));
            CreateMap<ChatGroup, ChatGroupResponse>()
                .ForMember(dest => dest.AdminName, opt => opt.Ignore())
                .ForMember(dest => dest.ChatGroudId, opt => opt.MapFrom(cg => cg.Id));
            // UsersInGroup
            CreateMap<UsersInGroup, GetAllUserInGroupResponse>()
                .ForMember(des=>des.AccountName, opt=>opt.MapFrom(us=>us.Accounts.AccountName))
                .ForMember(des=>des.UserInGroupId, opt=>opt.MapFrom(us=>us.UsersInGroupId))
                ;
            CreateMap<UsersInGroupRequest, UsersInGroup>()
            .ForMember(dest => dest.UsersInGroupId, opt => opt.MapFrom(src =>
               Guid.NewGuid().ToString()
            ));
            //Chat Message
            CreateMap<ChatMessageRequest, ChatMessage>()
            .ForMember(dest => dest.ChatMessageId, opt => opt.MapFrom(src => Guid.NewGuid().ToString()))
            .ForMember(dest => dest.MessageStatus, opt => opt.MapFrom(src => src.MessageStatus));
            CreateMap<ChatMessage, ChatMessageResponse>();

            CreateMap<Question, ResponseQuestion>();
            CreateMap<QuestionRequest, Question>()
                .ForMember(dest => dest.QuestionId, opt => opt.MapFrom(src => Guid.NewGuid().ToString()));
        }
    }
}
