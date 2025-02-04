using CleanArchitectureNetCore.Application.RequestModels;
using CleanArchitectureNetCore.Application.RequestModels.Auth;
using CleanArchitectureNetCore.Application.Services;
using CleanArchitectureNetCore.Common.Enums;
using CleanArchitectureNetCore.Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


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



        //Login Endpoint
        [HttpPost, Route("Login"), AllowAnonymous]
        public IActionResult Login(LoginRequest request)
        {
            IActionResult response = Unauthorized();//set our reponse to unauthorize
            var tokens = _AuthService.Authenticate(request.Email, request.Password);
            if (tokens != null)
            {
                response = Ok(tokens);
            }
            return response;
        }
        [HttpPost, Route("Login/Patient"), AllowAnonymous]
        public IActionResult LoginPatient(LoginRequest request)
        {
            IActionResult response = Unauthorized();//set our reponse to unauthorize
            var tokens = _AuthService.AuthenticatePatient(request.Email, request.Password);
            if (tokens != null)
            {
                response = Ok(tokens);
            }
            return response;
        }


        //Logout Endpoint
        [HttpPost, Route("Logout")]
        public IActionResult Logout(LoginRequest request)
        {
            _AuthService.Logout(base.GetUserId());
            return Ok();
        }


        [HttpPost("ForgotPassword")]
        [AllowAnonymous]
        public IActionResult ForgotPassword(ForgotPasswordRequestModel requestModel)
        {
            if (requestModel.IsPatient) 
                return  Ok(_AuthService.ForgotPasswordPatient(requestModel.Email));
            else return Ok(_AuthService.ForgotPassword(requestModel.Email));
        }

        //ResetPassword Endpoint
        [HttpPost("resetPassword")]
        [AllowAnonymous]
        public IActionResult ResetPassword(ResetPasswordRequestModel request)
        {
            return Ok(_AuthService.ResetPassword(request));
        }


        //RefreshToken Endpoint
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
