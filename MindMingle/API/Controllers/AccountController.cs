using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Application.Response;
using Application.Interface;

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

     
        [HttpGet("GetAllAccount")]
        public async Task<IActionResult> GetAllAccount()
        {
            var response = await accountService.GetAllAccounts();
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }
    }
}

