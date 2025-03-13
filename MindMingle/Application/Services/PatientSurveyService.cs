using Application.Interface;
using Application.IRepository;
using Application.Request.PatientSurvey;
using Application.Response.PatientSurvey;
using Application.Response;
using AutoMapper;
using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class PatientSurveyService : IPatientSurveyService
    {
        private readonly IPatientSurveyRepository _patientSurveyRepository;
        private readonly IMapper _mapper;

        public PatientSurveyService(IPatientSurveyRepository patientSurveyRepository, IMapper mapper)
        {
            _patientSurveyRepository = patientSurveyRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponse> AddSurveyAsync(PatientSurveyRequest request)
        {
            if (request == null)
                return new ApiResponse().SetBadRequest(message: "Request data is null");

            try
            {
                var survey = _mapper.Map<PatientSurvey>(request);
                await _patientSurveyRepository.AddAsync(survey);

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
                var surveys = await _patientSurveyRepository.GetSurveysByPatientIdAsync(patientId, pageIndex, pageSize);
                var surveyResponses = _mapper.Map<List<PatientSurveyResponse>>(surveys);
                return new ApiResponse().SetOk(surveyResponses);
            }
            catch (Exception ex)
            {
                return new ApiResponse().SetBadRequest(message: $"Error fetching surveys: {ex.Message}");
            }
        }
    }
}
