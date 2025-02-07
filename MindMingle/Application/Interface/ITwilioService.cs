using System;
using Application.Response;
using Twilio.Jwt.AccessToken;

namespace Application.Interface
{
	public interface ITwilioService
	{
		public Task<ApiResponse> GetCallRoomToken(string identity, string room);
	}
}

