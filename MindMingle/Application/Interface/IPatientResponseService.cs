using Application.Request.PatientResponse;
using Application.Response;
using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interface
{
    public interface IPatientResponseService
    {
        Task<ApiResponse> AddResponseAsync(PatientResRequest request);
        Task<ApiResponse> ComposeResponse(PatientResRequest[] request);
        Task<ApiResponse> GetResponsesBySurveyIdAsync(string surveyId, int pageIndex = 1, int pageSize = 10);
    }
}
