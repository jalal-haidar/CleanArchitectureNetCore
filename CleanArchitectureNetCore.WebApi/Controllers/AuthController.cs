using CleanArchitectureNetCore.Common.Enums;
using CleanArchitectureNetCore.Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace CleanArchitectureNetCore.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : BaseController
    {
        private readonly IConfiguration _Configuration;
        private readonly IAuthService _AuthService;
        private readonly IUserService userService;
        private readonly IAttendanceService attendanceService;

        public AuthController(IConfiguration configuration, IAuthService authService, IUserService userService, IAttendanceService attendanceService)
        {
            _Configuration = configuration;
            _AuthService = authService;
            this.userService = userService;
            this.attendanceService = attendanceService;
        }

        #region AUTHENTICATION




        [HttpPost, Route("Login"), AllowAnonymous]
        public IActionResult Login(LoginRequest request, string deviceId)
        {
            IActionResult response = Unauthorized();//set our reponse to unauthorize
            var tokens = _AuthService.Authenticate(request.Username, request.Password, request.device, deviceId);
            if (tokens != null)
            {
                response = Ok(tokens);
            }
            return response;
        }


        [HttpPost, Route("Logout")]
        public IActionResult Logout(LoginRequest request)
        {
            _AuthService.Logout(base.GetUserId());
            return Ok();
        }

        [HttpPost, Route("RefreshToken"), AllowAnonymous]
        public IActionResult RefreshToken(RefreshTokenRequest request, string deviceId)
        {
            IActionResult response = Unauthorized();//set our reponse to unauthorize
            var tokens = _AuthService.RefreshToken(request.Token, deviceId);
            if (tokens != null)
            {
                response = Ok(tokens);
            }
            return response;
        }


        [HttpGet, Route("AuthToken")]
        public IActionResult AuthToken(string deviceId)
        {
            IActionResult response = Unauthorized();//set our reponse to unauthorize
            var tokens = _AuthService.GetAuthToken(base.GetUserId());
            if (tokens != null)
            {
                response = Ok(tokens);
            }
            return response;
        }
        [HttpPost, Route("AuthToken"), AllowAnonymous]
        public IActionResult AuthToken(TokenLoginRequest request, string deviceId)
        {
            IActionResult response = Unauthorized();//set our reponse to unauthorize
            var tokens = _AuthService.Authenticate(request.Token, request.device, deviceId);
            if (tokens != null)
            {
                response = Ok(tokens);
            }
            return response;
        }

        [HttpGet, Authorize, Route("Info")]
        public IActionResult GetLoginDetails()
        {
            var user = new InfoResponseModel(_AuthService.Get(GetUserId()));
            if (user != null)
            {
                user.HasDefaultPassword = _AuthService.HasDefaultPassword(GetUserId());
                return Ok(user);
            }
            return NotFound();
        }

        [HttpPost("forgotPassword")]
        [AllowAnonymous]
        public IActionResult ForgetPassword(ForgotPasswordRequestModel request)
        {
            return Ok(new { Sent = _AuthService.ForgetPassword(request) });
        }

        [HttpPost("resetPassword")]
        [AllowAnonymous]
        public IActionResult ResetPassword(ResetPasswordRequestModel request)
        {
            return Ok(_AuthService.ResetPassword(request));
        }
        [HttpPost, Route("ChangePassword")]
        public IActionResult ChangePassword(ChangePasswordRequest request)
        {
            return Ok(_AuthService.ChangePassword(GetUserId(), request));
        }
        //method to generate web token
        private string GenerateJsonWebToken(UserDto user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_Configuration["jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]{
                new Claim(eTokenName.UserId.Get(), user.Id.ToString()),
                new Claim(eTokenName.RoleId.Get(), user.RoleId.ToString()),
                new Claim(eTokenName.Username.Get(), user.Username.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            var token = new JwtSecurityToken(
                issuer: _Configuration["jwt:Issuer"],
                audience: _Configuration["jwt:Issuer"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(_Configuration["jwt:timeout"])),
                signingCredentials: credentials);
            var encodeToken = new JwtSecurityTokenHandler().WriteToken(token);
            return encodeToken;
        }
        #endregion

    }
}
