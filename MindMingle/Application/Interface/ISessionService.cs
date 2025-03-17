using System;
using Application.Request.Session;
using Application.Response;

namespace Application.Interface
{
	public interface ISessionService
	{
		Task<ApiResponse> GetSession();
        Task<ApiResponse> GetSessionBySessionId(string sessionId);
        Task<ApiResponse> DeleteSession(string sessionId);
        Task<ApiResponse> GetSessionByTherapistId(string therapistId);
        Task<ApiResponse> CreateSession(CreateSessionRequest createSessionRequest);
    }
}

