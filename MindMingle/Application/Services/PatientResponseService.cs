using Application.Interface;
using Application.IRepository;
using Application.Request.PatientResponse;
using Application.Response;
using Application.Response.PatientResponse;
using AutoMapper;
using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class PatientResponseService : IPatientResponseService
    {
        private readonly IPatientResponseRepository _patientResponseRepository;
        private readonly IMapper _mapper;

        public PatientResponseService(IPatientResponseRepository patientResponseRepository, IMapper mapper)
        {
            _patientResponseRepository = patientResponseRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponse> AddResponseAsync(PatientResRequest request)
        {
            if (request == null)
                return new ApiResponse().SetBadRequest(message: "Request data is null");

            try
            {
                // Ánh xạ từ PatientResRequest sang PatientResponse
                var response = _mapper.Map<PatientResponse>(request);
                await _patientResponseRepository.AddAsync(response);

                // Ánh xạ từ PatientResponse sang PatientResResponse để trả về
                var responseDto = _mapper.Map<PatientResResponse>(response);
                return new ApiResponse().SetOk(responseDto);
            }
            catch (Exception ex)
            {
                return new ApiResponse().SetBadRequest(message: $"Error adding response: {ex.Message}");
            }
        }

        public async Task<ApiResponse> GetResponsesBySurveyIdAsync(string surveyId, int pageIndex = 1, int pageSize = 10)
        {
            try
            {
                var responses = await _patientResponseRepository.GetResponsesBySurveyIdAsync(surveyId, pageIndex, pageSize);

                // Ánh xạ danh sách PatientResponse sang danh sách PatientResResponse
                var responseDtos = _mapper.Map<List<PatientResResponse>>(responses);
                return new ApiResponse().SetOk(responseDtos);
            }
            catch (Exception ex)
            {
                return new ApiResponse().SetBadRequest(message: $"Error fetching responses: {ex.Message}");
            }
        }
    }
}
