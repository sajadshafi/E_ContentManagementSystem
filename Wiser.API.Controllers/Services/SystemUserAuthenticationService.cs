using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Wiser.API.BL.Config;
using Wiser.API.BL.Helpers;
using Wiser.API.BL.I_Services;
using Wiser.API.Entities;
using Wiser.API.Entities.BusinessModels;
//using Wiser.API.Entities.BusinessModels;
using Wiser.API.Entities.Models;


namespace Wiser.API.BL.Services
{
    public class SystemUserAuthenticationService : ISystemUserAuthenticationService
    {
        private readonly ApplicationSetting applicationSetting;
        private readonly UserManager<SystemUser> _userManager;
        private readonly SignInManager<SystemUser> signInManager;
        private readonly RoleManager<SystemRole> roleManager;
        private readonly WiserContext wiserContext;
        private IHttpContextAccessor _httpContextAccessor;

        private const string UserRegisteredSuccessfully = "System User Registered Successfully";
        private const string UserRegistrationFailed = "System User Registration Failed";
        private const string InvalidCredentials = "Invalid Credentials entered";
        private const string LoginSuccess = "Login Successfull";
        private const string RoleCreatedSucess = "Role created successfully";
        private const string RoleCreateFailed = "Role creation failed";
        private const string RoleExists = "Role already exists";
        private const string UserFound = "User found";
        private const string UserNotFound = "User not found";

        public SystemUserAuthenticationService(WiserContext wiserContext, IHttpContextAccessor httpContextAccessor, IOptions<ApplicationSetting> applicationSetting, UserManager<SystemUser> userManager, SignInManager<SystemUser> signInManager, RoleManager<SystemRole> roleManager)
        {
            this.applicationSetting = applicationSetting.Value;
            _userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            this._httpContextAccessor = httpContextAccessor;
            this.wiserContext = wiserContext;
        }

        public async Task<Response<string>> CreateRole(string RoleName)
        {
            string message = string.Empty;
            var roleInstance = await roleManager.RoleExistsAsync(RoleName);
            if (!roleInstance)
            {
                var role = new SystemRole();
                role.Name = RoleName;

                var roleResult = await roleManager.CreateAsync(role);
                if (roleResult.Succeeded)
                    message = RoleCreatedSucess;
                else
                    message = RoleCreateFailed;
            }
            else
                message = RoleExists;

            return new Response<string>
            {
                Message = message,
                Success = true,
            };
        }

        public async Task<Response<LoginResponse>> LoginSystemUser(LoginModel model)
        {
            var result = await this.signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberMe, true);
            if (result.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(model.Username);
                var rolesOfUser = await _userManager.GetRolesAsync(user);
                var token = GenerateJWTtoken(user, rolesOfUser);
                LoginResponse loginResponse = new LoginResponse
                {
                    Name = user.Name,
                    Username = user.UserName,
                    token = token,
                    Role = string.Join(",", rolesOfUser)
                };
                return new Response<LoginResponse>
                {
                    Success = true,
                    Data = loginResponse,
                    Message = LoginSuccess
                };
            }
            return new Response<LoginResponse>
            {
                Success = true,
                Data = null,
                Message = InvalidCredentials
            };
        }

        public async Task<Response<IdentityResult>> RegisterSystemUser(UserProfile user)
        {
            SystemUser _user = new SystemUser()
            {
                Email = user.Email,
                UserName = user.Username,
                Name = user.Name,
                DepartmentId = user.DepartmentId
            };
            var result = await _userManager.CreateAsync(_user, user.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(_user, SystemRoles.Teacher);
            }
            return new Response<IdentityResult>()
            {
                Success = result.Succeeded,
                Data = result,
                Message = result.Succeeded ? UserRegisteredSuccessfully : UserRegistrationFailed,
                Errors = result.Errors?.Select(x => x.Description).ToList()
            };
        }

        public async Task<Response<LoginResponse>> IsUserLoggedIn()
        {
            var id = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                var rolesOfUser = await _userManager.GetRolesAsync(user);
                LoginResponse loginResponse = new LoginResponse
                {
                    Name = user.Name,
                    Username = user.UserName,
                    Role = string.Join(",", rolesOfUser)
                };
                return new Response<LoginResponse>
                {
                    Success = true,
                    Data = loginResponse,
                    Message = UserFound
                };
            }
            return new Response<LoginResponse>
            {
                Success = true,
                Data = null,
                Message = UserNotFound
            }; ;
        }

        public async Task<Response<List<UpdateUserVM>>> GetAllTeachersAsync(Guid? DepartmentId)
        {
            var users = await _userManager.GetUsersInRoleAsync("Teacher");
            if (DepartmentId.HasValue)
            {
                users = users.Where(x => x.DepartmentId == DepartmentId).ToList();
            }
            if (users.Any())
            {
                var departments = this.wiserContext.Departments.ToList();
                var dataToSent = users.Where(x=>x.IsDeleted==false).Select(x => new UpdateUserVM()
                {
                    Id=x.Id,
                    Name = x.Name,
                    Username = x.UserName,
                    Email = x.Email,
                    DepartmentId = x.DepartmentId,
                    DepartmentName = departments?.FirstOrDefault(v => v.Id == x.DepartmentId)?.DepartmentName
                }).ToList();

                return new Response<List<UpdateUserVM>>()
                {
                    Success = true,
                    Count = dataToSent.Count,
                    Data = dataToSent,
                    Message = "Users with teacher role found"
                };
            }
            return new Response<List<UpdateUserVM>>()
            {
                Success = true,
                Count = 0,
                Data = null,
                Message = "Users with teacher role not found"
            };
        }

        public async Task<Response<IdentityResult>> UpdateUserAsync(UpdateUserVM systemUser)
        {
            var user = await _userManager.FindByIdAsync(systemUser.Id);
            if (user != null)
            {
                user.Name = systemUser.Name;
                user.UserName = systemUser.Username;
                user.Email = systemUser.Email;
                user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, systemUser.Password);
                var result = await _userManager.UpdateAsync(user);

                return new Response<IdentityResult>()
                {
                    Success = result.Succeeded,
                    Data = result,
                    Message = result.Succeeded ? "User updated successfully" : "User updation failed",
                    Errors = result.Errors?.Select(x => x.Description).ToList()
                };
            }
            return new Response<IdentityResult>()
            {
                Success = false,
                Data = null,
                Message = "User updation failed",
                Errors = new List<string>() { "User not found" }
            };
        }

        #region JWT Token Generation
        private string GenerateJWTtoken(SystemUser userInfo, IList<string> rolesOfUser)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,userInfo.Id.ToString()),
                new Claim(ClaimTypes.Name, userInfo.Name),
            };
            AddRolesToClaims(claims, rolesOfUser);
            var descriptor = new SecurityTokenDescriptor();
            descriptor.Subject = new ClaimsIdentity(claims);
            descriptor.Expires = DateTime.UtcNow.AddDays(10);
            descriptor.SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(applicationSetting.JWT_Secret)),
                SecurityAlgorithms.HmacSha256Signature);

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(descriptor);
            var token = tokenHandler.WriteToken(securityToken);
            return token;
        }

        private void AddRolesToClaims(List<Claim> claims, IEnumerable<string> roles)
        {
            foreach (var role in roles)
            {
                var roleClaim = new Claim(ClaimTypes.Role, role);
                claims.Add(roleClaim);
            }
        }

        public async Task<Response<bool>> DeleteUserAsync(string Id)
        {
            var user=wiserContext.Users.FirstOrDefault(x => x.Id == Id);
            if (user != null)
            {
                wiserContext.Users.Remove(user);
                await wiserContext.SaveChangesAsync();
                return new Response<bool>()
                {
                    Message = "User deleted successfully",
                    Success = true,
                    Data = true
                };
            }
            return new Response<bool>()
            {
                Data = false,
                Success = true,
                Message = "User not found"
            };
        }
        #endregion
    }
}
