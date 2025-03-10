using System;
using Domain.Entity;

namespace Application.Interface
{
	public interface ITokenService
	{
        string GetToken(string token);
        string CreateToken(Account account);
    }
}

