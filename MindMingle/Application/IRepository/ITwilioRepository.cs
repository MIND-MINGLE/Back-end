using System;
using Twilio.Jwt.AccessToken;

namespace Application.IRepository
{
	public interface ITwilioRepository
	{
		public Token GetCallRoomToken(string identity, string room);
    }
}

