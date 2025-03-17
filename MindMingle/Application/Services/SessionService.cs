using System;
using Application.Interface;
using Application.Request.Session;
using Application.Response;
using AutoMapper;
using Domain.Entity;

namespace Application.Services
{
	public class SessionService:ISessionService
	{
        private readonly IMapper mapper;
        private readonly IUnitOfWorks unitOfWorks;
        public SessionService(IMapper mapper, IUnitOfWorks unitOfWorks)
		{
            this.mapper = mapper;
            this.unitOfWorks = unitOfWorks;
        }

        async public Task<ApiResponse> CreateSession(CreateSessionRequest createSessionRequest)
        {
            // Create a new API Response everytime the api route is called
            ApiResponse apiResponse = new ApiResponse();
            try
            {
                var therapistModel = await unitOfWorks.TherapistRepo.GetAsync(x => x.TherapistId.Equals(createSessionRequest.TherapistId));
                if (therapistModel==null)
                {
                    return apiResponse.SetNotFound("No Therpist Found");
                }
                var sessionModel = mapper.Map<Session>(createSessionRequest);
                await unitOfWorks.SessionRepo.AddAsync(sessionModel);
                await unitOfWorks.SaveChangeAsync();
                //Console.WriteLine($"Fetch data complete: {resSession.Count}");
                return apiResponse.SetOk(createSessionRequest);
            }
            catch (Exception ex)
            {
                return apiResponse.SetBadRequest(ex);
            }
        }

        async public Task<ApiResponse> GetSession()
        {
            // Create a new API Response everytime the api route is called
            ApiResponse apiResponse = new ApiResponse();
            try
            {
                var sessionModel = await unitOfWorks.SessionRepo.GetAllAsync(null);
                var resSession = mapper.Map<List<ResponseSession>>(sessionModel);
                //Console.WriteLine($"Fetch data complete: {resSession.Count}");
                return apiResponse.SetOk(resSession);
            }
            catch (Exception ex)
            {
                return apiResponse.SetBadRequest(ex);
            }
        }

        async public Task<ApiResponse> GetSessionBySessionId(string sessionId)
        {
            // Create a new API Response everytime the api route is called
            ApiResponse apiResponse = new ApiResponse();
            try
            {
                var sessionModel = await unitOfWorks.SessionRepo.GetAsync(x=>x.SessionId.Equals(sessionId));
                if (sessionModel == null)
                {
                    return apiResponse.SetNotFound("No Session with that id");
                }
                var resSession = mapper.Map<ResponseSession>(sessionModel);
                //Console.WriteLine($"Fetch data complete: {resSession.Count}");
                return apiResponse.SetOk(resSession);
            }
            catch (Exception ex)
            {
                return apiResponse.SetBadRequest(ex);
            }
        }

        async public Task<ApiResponse> GetSessionByTherapistId(string therapistId)
        {
            // Create a new API Response everytime the api route is called
            ApiResponse apiResponse = new ApiResponse();
            try
            {
                var sessionModel = await unitOfWorks.SessionRepo.GetAllAsync(x => x.TherapistId.Equals(therapistId));
                if (sessionModel.Count<=0)
                {
                    return apiResponse.SetNotFound("No Session with that therapistId");
                }
                var resSession = mapper.Map<List<ResponseSession>>(sessionModel);
                //Console.WriteLine($"Fetch data complete: {resSession.Count}");
                return apiResponse.SetOk(resSession);
            }
            catch (Exception ex)
            {
                return apiResponse.SetBadRequest(ex);
            }
        }

        async public Task<ApiResponse> DeleteSession(string sessionId)
        {
            // Create a new API Response everytime the api route is called
            ApiResponse apiResponse = new ApiResponse();
            try
            {
                var sessionModel = await unitOfWorks.SessionRepo.GetAsync(x => x.SessionId.Equals(sessionId));
                if (sessionModel==null)
                {
                    return apiResponse.SetNotFound("No Session with that id");
                }
                await unitOfWorks.SessionRepo.RemoveByIdAsync(sessionId);
                await unitOfWorks.SaveChangeAsync();
                return apiResponse.SetOk(sessionId);
            }
            catch (Exception ex)
            {
                return apiResponse.SetBadRequest(ex);
            }
        }

        public async Task<ApiResponse> UpdateSession(UpdateSessionRequest updateSessionRequest)
        {
            ApiResponse apiResponse = new ApiResponse();
            try
            {
                var sessionModel = await unitOfWorks.SessionRepo.GetAsync(x => x.SessionId == updateSessionRequest.SessionId);
                if (sessionModel == null)
                {
                    return apiResponse.SetNotFound("No Session Found");
                }

                mapper.Map(updateSessionRequest, sessionModel); // Maps request onto existing entity
                await unitOfWorks.SaveChangeAsync();

                return apiResponse.SetOk(sessionModel);
            }
            catch (Exception ex)
            {
                return apiResponse.SetBadRequest(ex);
            }
        }
    }
}

