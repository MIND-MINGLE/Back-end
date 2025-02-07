using System;
using Application.Interface;
using Application.Library;
using Application.Response;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using Twilio;
using Twilio.Jwt.AccessToken;


namespace Application.Services
{
	public class TwilioService : ITwilioService
	{
        private readonly IUnitOfWorks unitOfWorks;

        public TwilioService(IUnitOfWorks unitOfWorks)
        {
            this.unitOfWorks = unitOfWorks;
        }

        public async Task<ApiResponse> GetCallRoomToken(string identity, string room)
        {
            ApiResponse apiResponse = new ApiResponse();
            Console.WriteLine("Fetching Token...");
            var token = unitOfWorks.TwilioRepo.GetCallRoomToken(identity, room);
            return apiResponse.SetOk(token.ToJwt());
        }
    }
}

