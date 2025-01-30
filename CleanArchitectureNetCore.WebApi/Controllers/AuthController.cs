using CleanArchitectureNetCore.Common.Enums;
using CleanArchitectureNetCore.Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using CleanArchitectureNetCore.Application.Contracts;
using CleanArchitectureNetCore.Application.RequestModels;
using CleanArchitectureNetCore.Application.Services;


namespace CleanArchitectureNetCore.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : BaseController
    {
        private readonly IConfiguration _Configuration;
        private readonly AuthService _AuthService;
        private readonly UserService userService;

        public AuthController(IConfiguration configuration, AuthService authService, UserService userService)
        {
            _Configuration = configuration;
            _AuthService = authService;
            this.userService = userService;
        }

        #region AUTHENTICATION




        [HttpPost, Route("Login"), AllowAnonymous]
        public IActionResult Login(LoginRequest request)
        {
            IActionResult response = Unauthorized();//set our reponse to unauthorize
            var tokens = _AuthService.Authenticate(request.Username, request.Password);
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
            var tokens = _AuthService.Authenticate(request.Token, request.device);
            if (tokens != null)
            {
                response = Ok(tokens);
            }
            return response;
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
