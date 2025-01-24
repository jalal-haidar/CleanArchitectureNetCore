using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System;
using CleanArchitectureNetCore.Common.Enums;

namespace CleanArchitectureNetCore.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BaseController : ControllerBase
    {
        protected long GetUserId() => long.Parse(_GetValueFromToken(eTokenName.UserId.Get()));
        protected long GetUserRoleId() => long.Parse(_GetValueFromToken(eTokenName.RoleId.Get()));
        private string _GetValueFromToken(string name)
        {
            if (!User.Identity.IsAuthenticated)
                throw new Exception("Unauthorized access");
            return User.Claims.FirstOrDefault(x => x.Type == name)?.Value ?? "";
        }
    }
}
