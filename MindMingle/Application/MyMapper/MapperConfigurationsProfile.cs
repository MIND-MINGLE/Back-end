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
using Application.Request.Answer;
using Application.Request.PatientSurvey;
using Application.Response.PatientSurvey;
using Application.Request.Session;
using Application.Request.Appointment;
using Application.Response.Appointment;
using Application.Request.Credential;
using Application.Request.EmergencyEnd;

namespace Application.MyMapper
{
    public class MapperConfigurationsProfile : Profile
    {
        public MapperConfigurationsProfile()
        {
            //Account
           
            CreateMap<Account, ResponseAccount>();
            CreateMap<Account, RequestAccount>();
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
                .ForMember(dest => dest.ChatGroupId, opt => opt.MapFrom(cg => cg.Id));
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
            // Ánh xạ từ PatientResponse sang PatientResResponse
            CreateMap<PatientResponse, PatientResResponse>();

            CreateMap<Answer, ResponseAnswer>();
            CreateMap<NewAnswerRequest, Answer>()
                .ForMember(dest => dest.AnswerId, opt => opt.MapFrom(src => Guid.NewGuid().ToString()));
          
            //PatientSurvey
            CreateMap<PatientSurveyRequest, PatientSurvey>()
                .ForMember(dest => dest.PatientSurveyId, opt => opt.MapFrom(src => Guid.NewGuid().ToString())) // Bỏ qua vì sẽ được tạo tự động
                .ForMember(dest => dest.PatientResponses, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt == default ? DateTime.UtcNow : src.CreatedAt));

            // Ánh xạ từ PatientSurvey sang PatientSurveyResponse
            CreateMap<PatientSurvey, PatientSurveyResponse>()
                .ForMember(dest => dest.PatientResponses, opt => opt.MapFrom(src => src.PatientResponses));
            // Appointment

            //Session
            CreateMap<CreateSessionRequest, Session>()
             .ForMember(dest => dest.SessionId, opt => opt.MapFrom(src => Guid.NewGuid().ToString()))
              .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true));
            CreateMap<Session, ResponseSession>();

            CreateMap<UpdateSessionRequest, Session>()
                 .ForMember(dest => dest.SessionId, opt => opt.Ignore())
                .ForMember(dest=>dest.TherapistId, opt => opt.Ignore())
                .ForMember(dest => dest.DayOfWeek, opt => opt.Ignore());

            CreateMap<UpdatePersonRequest, Patient>()
                .ForMember(dest => dest.Dob, opt => opt.MapFrom(src => src.DateOfBirth));

            CreateMap<UpdatePersonRequest, Therapist>()
                .ForMember(dest => dest.Dob, opt => opt.MapFrom(src => src.DateOfBirth));
            // Ánh xạ từ AppointmentRequest sang Appointment (dùng cho Create)
            CreateMap<AppointmentRequest, Appointment>()
                .ForMember(dest => dest.AppointmentId, opt => opt.Ignore()) // Tự sinh trong service
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());    // Tự đặt trong service
                //.ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null)); // Chỉ ánh xạ nếu có giá trị

            // Ánh xạ từ AppointmentUpdateRequest sang Appointment (dùng cho Update)
            CreateMap<AppointmentUpdateRequest, Appointment>()
                .ForMember(dest => dest.AppointmentId, opt => opt.Ignore())
                .ForMember(dest => dest.PatientId, opt => opt.Ignore())
                .ForMember(dest => dest.TherapistId, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null)); // Chỉ cập nhật các field có giá trị

            // Ánh xạ từ Appointment sang AppointmentResponse (dùng cho Get)
            CreateMap<Appointment, AppointmentResponse>()
                .ForMember(dest => dest.AppointmentId, opt => opt.MapFrom(src => src.AppointmentId))
                .ForMember(dest => dest.PatientId, opt => opt.MapFrom(src => src.PatientId))
                .ForMember(dest => dest.TherapistId, opt => opt.MapFrom(src => src.TherapistId))
                .ForMember(dest => dest.CoWorkingSpaceId, opt => opt.MapFrom(src => src.CoWorkingSpaceId))
                .ForMember(dest => dest.SessionId, opt => opt.MapFrom(src => src.SessionId))
                .ForMember(dest => dest.EmergencyEndId, opt => opt.MapFrom(src => src.EmergencyEndId))
                .ForMember(dest => dest.AppointmentType, opt => opt.MapFrom(src => src.AppointmentType))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.TotalFee, opt => opt.MapFrom(src => src.TotalFee))
                .ForMember(dest => dest.PlatformFee, opt => opt.MapFrom(src => src.PlatformFee))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt));

            CreateMap < CredentialRequest, Credentials>()
                .ForMember(dest => dest.CredentialsId, opt => opt.MapFrom(src => Guid.NewGuid().ToString()));

            CreateMap<EmergencyEndRequest, EmergencyEnd>()
                .ForMember(dest => dest.EmergencyEndId, opt => opt.MapFrom(src => Guid.NewGuid().ToString()));

            CreateMap<Credentials, RepsonseCredential>()
                .ForMember(dest => dest.credentialId, opt => opt.MapFrom(src => src.CredentialsId))
                .ForMember(dest => dest.therapistId, opt => opt.MapFrom(src => src.TherapistId))
                .ForMember(dest => dest.imageUrl, opt => opt.MapFrom(src => src.ImageURL))
                .ForMember(dest => dest.isDisabled, opt => opt.MapFrom(src => src.IsDisabled))
                .ForMember(dest => dest.createdAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.updatedAt, opt => opt.MapFrom(src => src.UpdatedAt));
        }
    }
}
