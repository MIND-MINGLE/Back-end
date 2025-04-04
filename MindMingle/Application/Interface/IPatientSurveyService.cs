using Application.Request.PatientSurvey;
using Application.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface
{
    public interface IPatientSurveyService
    {
        Task<ApiResponse> AddSurveyAsync(PatientSurveyRequest request);
        Task<ApiResponse> GetSurveysByPatientIdAsync(string patientId, int pageIndex = 1, int pageSize = 10);
        Task<ApiResponse> GetLatestSurveysByPatientIdAsync(string patientId);
        
    }
}
