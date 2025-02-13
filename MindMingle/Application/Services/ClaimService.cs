using AutoMapper;
using Domain.Entity;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
	public class ClaimService
	{
		private readonly IHttpContextAccessor _httpContextAccessor;

		public ClaimService(IHttpContextAccessor httpContextAccessor)
		{
			_httpContextAccessor = httpContextAccessor;
		}

		public  ClaimDTO GetClaim()
		{
			var userId = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Id")?.Value;
			var claim = new ClaimDTO();
			var role = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Role")?.Value;
			if (userId == null)
			{
				throw new ArgumentNullException("UserId can not be found!");
			}
			claim.Id = int.Parse(userId);
			claim.Role = (Role)Enum.Parse(typeof(Role), role);
			claim.Name = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Name")?.Value;

			return claim;
		}

	}

	public class ClaimDTO
	{
		public int Id { get; set; }
		public Role Role { get; set; }

		public string Name { get; set; }
	}
}
