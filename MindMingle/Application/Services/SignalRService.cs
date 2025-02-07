using System;
using Application.Interface;
using Application.Response;
using AutoMapper;

namespace Application.Services
{
	public class SignalRService : ISignalRService
	{
        private readonly IMapper mapper;
        private readonly IUnitOfWorks unitOfWork;

        public SignalRService(IMapper mapper, IUnitOfWorks unitOfWork)
        {
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse> JoinCallRoom()
        {
            ApiResponse apiResponse = new ApiResponse();

            return apiResponse.SetBadRequest("JoinCallRoom Not Implimented");
        }

        public async Task<ApiResponse> ReceiveTextMessage()
        {
            ApiResponse apiResponse = new ApiResponse();

            return apiResponse.SetBadRequest("ReceiveTextMessage not Implimented");
        }

        public async Task<ApiResponse> SendTextMessage()
        {
            ApiResponse apiResponse = new ApiResponse();

            return apiResponse.SetBadRequest("SendTextMessage Not Implimented");
        }
    }
}

