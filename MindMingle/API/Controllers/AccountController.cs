using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Application.Response;
using Application.Interface;
using Application.Request.Account;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/[Controller]")]
    public class AccountController : Controller
    {
        private readonly IAccountService accountService;
  
        public AccountController(IAccountService accountService)
        {
            this.accountService = accountService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAccountById([FromRoute] string id)
        {
            var response = await accountService.GetAccountById(id);
			return response.IsSuccess ? Ok(response) : BadRequest(response);
		}    
        
        [HttpGet("getall")]
        public async Task<IActionResult> GetAllAccount()
        {
            var response = await accountService.GetAllAccounts();
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }

        [HttpPut("updateavatar")]
        public async Task<IActionResult> UpdateAvatar([FromBody] AvatarRequest newAvatar)
        {
            if (newAvatar == null)
            {
                return BadRequest(new ApiResponse().SetBadRequest("Request body is null"));
            }
            Console.WriteLine($"Received Request - AccountId: {newAvatar.AccountId}, NewAvatar: {newAvatar.Avatar}");
            var response = await accountService.UpdateAvatarAsync(newAvatar);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }
    }
}

