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
using Application.Request.Category;
using Application.Request.PatientResponse;
using Application.Response.PatientResponse;
using Application.Request.PatientSurvey;
using Application.Response.PatientSurvey;

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
            //PatientResponse
            CreateMap<PatientResRequest, PatientResponse>()
                .ForMember(dest => dest.PatientResponseId, opt => opt.MapFrom(src => Guid.NewGuid().ToString())) // Bỏ qua ResponseId vì sẽ được tạo tự động
                .ForMember(dest => dest.Score, opt => opt.Ignore()); // Bỏ qua Score nếu không có trong request
            CreateMap<PatientResponse, PatientResResponse>()
                .ForMember(dest => dest.ResponseId, opt => opt.MapFrom(src => src.PatientResponseId))
                .ForMember(dest => dest.SurveyId, opt => opt.MapFrom(src => src.PatientSurveyId))
                .ForMember(dest => dest.QuestionId, opt => opt.MapFrom(src => src.QuestionId))
                .ForMember(dest => dest.AnswerId, opt => opt.MapFrom(src => src.AnswerId))
                .ForMember(dest => dest.CustomerAnswer, opt => opt.MapFrom(src => src.CustomerAnswer))
                .ForMember(dest => dest.Score, opt => opt.MapFrom(src => src.Score));
            CreateMap<Question, ResponseQuestion>();
            CreateMap<QuestionRequest, Question>()
                .ForMember(dest => dest.QuestionId, opt => opt.MapFrom(src => Guid.NewGuid().ToString()));

            CreateMap<Category, ResponseCategory>();
            CreateMap<CategoryRequest, Category>()
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => Guid.NewGuid().ToString()));
          
            //PatientSurvey
            CreateMap<PatientSurveyRequest, PatientSurvey>()
                .ForMember(dest => dest.PatientSurveyId, opt => opt.MapFrom(src => Guid.NewGuid().ToString())) // Bỏ qua vì sẽ được tạo tự động
                .ForMember(dest => dest.PatientResponses, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt == default ? DateTime.UtcNow : src.CreatedAt));

            // Ánh xạ từ PatientSurvey sang PatientSurveyResponse
            CreateMap<PatientSurvey, PatientSurveyResponse>()
                .ForMember(dest => dest.PatientResponses, opt => opt.MapFrom(src => src.PatientResponses));
        }
    }
}
