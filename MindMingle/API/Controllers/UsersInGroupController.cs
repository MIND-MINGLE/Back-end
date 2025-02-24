using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Interface;
using Application.Request.UsersInGroup;
using Application.Response;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/[controller]")]
    public class UsersInGroupController : ControllerBase
    {
        private readonly IUsersInGroupService usersInGroupService;

        public UsersInGroupController(IUsersInGroupService usersInGroupService)
        {
            this.usersInGroupService = usersInGroupService;
        }
        [HttpGet("getall")]
        public async Task<IActionResult> GetAllData()
        {
            var response = await usersInGroupService.GetAllGroupChatWithUser();
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }
        [HttpGet("getallclient")]
        public async Task<IActionResult> GetAllUserInGroup([FromRoute]string groupId)
        {
            var response = await usersInGroupService.GetAllUserInGroup(groupId);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }
        [HttpPost("addclient")]
        public async Task<IActionResult> AddUsersIntoGroup(UsersInGroupRequest usersInGroupRequest)
        {
            var response = await usersInGroupService.AddUsersIntoGroup(usersInGroupRequest);
            return response.IsSuccess ? Ok(response) : BadRequest(response);
        }
    }
}

