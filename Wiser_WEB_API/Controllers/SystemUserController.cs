using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wiser.API.BL.Config;
using Wiser.API.BL.I_Services;
using Wiser.API.Entities.BusinessModels;
//using Wiser.API.BL.I_Services;
//using Wiser.API.Entities.BusinessModels;
using Wiser.API.Entities.Models;

namespace Wiser_WEB_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SystemUserController : ControllerBase
    {
        public readonly ISystemUserAuthenticationService service;

        public SystemUserController(ISystemUserAuthenticationService service)
        {
            this.service = service;
        }

        [Authorize(Roles = SystemRoles.Admin)]
        [HttpPost, Route("/api/v1/system-register")]
        public async Task<IActionResult> RegisterSystemUser(UserProfile user)
        {
            var response = await this.service.RegisterSystemUser(user);
            return Ok(response);
        }

        [Authorize(Roles = SystemRoles.Admin)]
        [HttpPost, Route("/api/v1/update-user")]
        public async Task<IActionResult> UpdateUserAsync(UpdateUserVM user)
        {
            var response = await this.service.UpdateUserAsync(user);
            return Ok(response);
        }

        [Authorize(Roles = SystemRoles.Admin)]
        [HttpPost, Route("/api/v1/delete-user")]
        public async Task<IActionResult> DeleteUserAsync(string Id)
        {
            var response = await this.service.DeleteUserAsync(Id);
            return Ok(response);
        }

        [HttpPost, Route("/api/v1/system-login")]
        public async Task<IActionResult> LoginSystemUser(LoginModel model)
        {
            var response = await this.service.LoginSystemUser(model);
            return Ok(response);
        }

        [Authorize(Roles = SystemRoles.SuperAdmin)]
        [HttpPost, Route("/api/v1/create-role")]
        public async Task<IActionResult> CreateRole(string RoleName)
        {
            var response = await this.service.CreateRole(RoleName);
            return Ok(response);
        }

        [Authorize]
        [HttpGet, Route("/api/v1/is-user-authenticated")]
        public async Task<IActionResult> IsUserLoggedIn()
        {
            var response = await this.service.IsUserLoggedIn();
            return Ok(response);
        }

        [Authorize(Roles = SystemRoles.Admin)]
        [HttpGet, Route("/api/v1/get-teachers")]
        public async Task<IActionResult> GetAllTeachersAsync(Guid? DepartmentId = null)
        {
            var response = await this.service.GetAllTeachersAsync(DepartmentId);
            return Ok(response);
        }
    }
}
