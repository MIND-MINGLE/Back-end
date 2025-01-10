using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Interface;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/[Controller]")]
    public class RoleController : Controller
    {
        private readonly IRoleService roleService;

        public RoleController(IRoleService roleService)
        {
            this.roleService = roleService;
        }


        [HttpGet("GetAllAccount")]
        public async Task<IActionResult> GetAllRole()
        {
            var response = await roleService.GetAllRoles();
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }
    }
}

