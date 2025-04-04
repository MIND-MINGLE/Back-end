using Application.Interface;
using Application.Request.PatientSurvey;
using Application.Response.PatientSurvey;
using Application.Response;
using AutoMapper;
using Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class PatientSurveyService : IPatientSurveyService
    {
        private readonly IUnitOfWorks unitOfWorks;
        private readonly IMapper _mapper;

        public PatientSurveyService(IUnitOfWorks unitOfWorks, IMapper mapper)
        {
            this.unitOfWorks = unitOfWorks;
            _mapper = mapper;
        }

        public async Task<ApiResponse> AddSurveyAsync(PatientSurveyRequest request)
        {
            if (request == null)
                return new ApiResponse().SetBadRequest(message: "Request data is null");
            try
            {
                var survey = _mapper.Map<PatientSurvey>(request);
                survey.PatientSurveyId = Guid.NewGuid().ToString();
                await unitOfWorks.PatientSurveyRepo.AddAsync(survey);
                await unitOfWorks.SaveChangeAsync();
                var surveyResponse = _mapper.Map<PatientSurveyResponse>(survey);
                return new ApiResponse().SetOk(surveyResponse);
            }
            catch (Exception ex)
            {
                return new ApiResponse().SetBadRequest(message: $"Error adding survey: {ex.Message}");
            }
        }
        public async Task<ApiResponse> GetSurveysByPatientIdAsync(string patientId, int pageIndex = 1, int pageSize = 10)
        {
            if (string.IsNullOrEmpty(patientId))
                return new ApiResponse().SetBadRequest(message: "PatientId is required");

            try
            {
                var surveys = await unitOfWorks.PatientSurveyRepo.GetSurveysByPatientIdAsync(patientId, pageIndex, pageSize);
                var surveyResponses = _mapper.Map<List<PatientSurveyResponse>>(surveys);
                return new ApiResponse().SetOk(surveyResponses);
            }
            catch (Exception ex)
            {
                return new ApiResponse().SetBadRequest(message: $"Error fetching surveys: {ex.Message}");
            }
        }
        public async Task<ApiResponse> GetLatestSurveysByPatientIdAsync(string patientId)
        {
            if (string.IsNullOrEmpty(patientId))
                return new ApiResponse().SetBadRequest(message: "PatientId is required");

            try
            {
                // Fetch all surveys for the patient
                var surveys = await unitOfWorks.PatientSurveyRepo
                    .GetAllAsync(
                    ps => ps.PatientId == patientId,
                    ps=>ps.Include(p=>p.PatientResponses)
                    );
              

                if (surveys == null || surveys.Count == 0)
                {
                    return new ApiResponse().SetNotFound(message: "No surveys found for this patient");
                }

                // Sort by CreatedAt descending and take the latest one
                var latestSurvey = surveys
                    .OrderByDescending(ps => ps.CreatedAt)
                    .FirstOrDefault();

                if (latestSurvey == null)
                {
                    return new ApiResponse().SetNotFound(message: "No surveys found for this patient");
                }

                // Map to PatientSurveyResponse
                var surveyResponse = _mapper.Map<PatientSurveyResponse>(latestSurvey);

              
                return new ApiResponse().SetOk(surveyResponse);
            }
            catch (Exception ex)
            {
                return new ApiResponse().SetBadRequest(message: $"Error fetching survey: {ex.Message}");
            }
        }
    }
}
