using System;
using Application.IRepository;
using Application.Library;
using Microsoft.Extensions.Options;
using Twilio.Jwt.AccessToken;

namespace Infrastructure.Repository
{
	public class TwilioRepository : ITwilioRepository
	{
        private readonly string AccountSid;
        private readonly string ApiKeySid;
        private readonly string ApiKeySecret;

        public TwilioRepository(IOptions<TwilioOptions> options)
        {
            AccountSid = options.Value.AccountSid;
            ApiKeySecret = options.Value.ApiKeySecret;
            ApiKeySid = options.Value.ApiKeySid;
        }

        public Token GetCallRoomToken(string identity, string room)
        {

            Console.WriteLine("Fetching AccountSid... " + AccountSid);
            Console.WriteLine("Fetching ApiKeySecret... " + ApiKeySecret);
            Console.WriteLine("Fetching ApiKeySid... " + ApiKeySid);
            var grant = new VideoGrant { Room = room };
            var token = new Token(AccountSid, ApiKeySid, ApiKeySecret, identity, grants: new HashSet<IGrant> { grant });
            Console.WriteLine("Creating Token... " + token.ToString());
            
            return token;
        }
    }
}

