using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Wiser.API.BL.Config;

namespace Wiser.API.BL.Helpers
{
    public class Global
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public Global(IHttpContextAccessor _httpContextAccessor)
        {
            httpContextAccessor = _httpContextAccessor;
        }

        public Guid GetCurrentUserId()
        {
            var value = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(value))
                return new Guid(value);
            return Constants.DEFAULT_GUID;
        }

        public List<string> GetCurrentUserRoles()
        {
            var roles = httpContextAccessor.HttpContext.User?.Claims?.Where(c => c.Type == ClaimTypes.Role).Select(x => x.Value).ToList();
            return roles;
        }
    }
}
