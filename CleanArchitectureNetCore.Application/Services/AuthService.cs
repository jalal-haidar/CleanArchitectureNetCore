using CleanArchitectureNetCore.Application.Contracts;
using CleanArchitectureNetCore.Application.Contracts.Repositories;
using CleanArchitectureNetCore.Application.ResponseModels;
using CleanArchitectureNetCore.Common;
using CleanArchitectureNetCore.Common.Enums;
using CleanArchitectureNetCore.Domain.DTOs;
using CleanArchitectureNetCore.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SoftoException.Exceptions.Http;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace CleanArchitectureNetCore.Application.Services
{

    public class AuthService 
    {
        private readonly IUnitOfWork _UnitOfWork;
        private readonly IConfiguration _Configuration;
        private readonly IHttpContextAccessor _HttpContextAccessor;

        private IUserRepository users => _UnitOfWork.Users;
        public AuthService(IUnitOfWork unitOfWork, IConfiguration configuration,
          IHttpContextAccessor httpContextAccessor)
        {
            this._UnitOfWork = unitOfWork;
            _Configuration = configuration;
            _HttpContextAccessor = httpContextAccessor;
        }
        public LoginResponse Authenticate(string username, string password)
        {
            // call the GetByUsername method from the repository
            var user = users.Authenticate(username, password);

            // check if user is null, return null
            if (user == null)
                return null;


            var dto = user.ToDto();


            // authentication successful so generate refresh token 
            var refreshToken = _GenerateRefreshToken(dto);
            _UnitOfWork.RefreshTokens.Add(refreshToken);
            _UnitOfWork.SaveChanges();
            // set refresh token cookie
            _SetRefreshTokenCookie(refreshToken.Token);
            // refresh token is saved successfully, generate jwt token
            return new LoginResponse { Token = GenerateJsonWebToken(user), RefreshToken = refreshToken.Token };
        }


        public User GetByUsernameOrEmail(string identifier)
        {
            // Retrieve the user by email or username without filtering by active status
            return _UnitOfWork.Users.Get()
                .FirstOrDefault(x => x.Email == identifier || x.Username == identifier);
        }
        public User _Get(long id)
        {
            return users.Get()
                .Include(x => x.Role)
                .FirstOrDefault(x => x.Id == id);
        }
        public UserDto Get(long id)
        {
            return _Get(id)?.ToDto();
        }

        public bool HasDefaultPassword(long userId)
        {
            return _Get(userId).Password == "Assesment@path";
        }


        public User GeneratePassword(User user)
        {
            user.Salt = new Guid().ToString();
            user.Password = HashPassword(user.Password, user.Salt);
            return user;
        }



        private string GenerateJsonWebToken(User user)
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
                    expires:  DateTime.UtcNow.AddMinutes(Convert.ToInt32(_Configuration["jwt:timeout"])),
                    signingCredentials: credentials);
            var encodeToken = new JwtSecurityTokenHandler().WriteToken(token);
            return encodeToken;
        }




        protected string HashPassword(string password, string salt)
        {
            return Utilities.HashPassword(password, salt);
        }

        private string _GetBody(string link)
        {
            return $"{link}";
        }



        private string _Encode(string str)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(str));
        }
        private string _Decode(string str)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(str));
        }
        private RefreshToken _GenerateRefreshToken(UserDto user)
        {
            //_ExpireAllTokens(user.Id);
            return new RefreshToken
            {
                Token = Guid.NewGuid().ToString(),
                ExpireTime = DateTime.UtcNow.AddDays(7),
                IssuedTime = DateTime.UtcNow,
                IsActive = true,
                UserId = user.Id
            };
        }

        /// <summary>
        /// Expires user refresh token in database
        /// </summary>
        /// <param name="userId">user ID</param>
        /// <exception cref="Exception"></exception>
        public void Logout(long userId)
        {
            // expire current refresh token
            var token = _GetRefreshTokenCookie();
            if (token == null)
                return;
            var refreshToken = _UnitOfWork.RefreshTokens.Get().Where(x => x.Token == token).FirstOrDefault();
            if (refreshToken == null)
                return;
            refreshToken.IsActive = false;
            _UnitOfWork.RefreshTokens.Update(refreshToken);
            _UnitOfWork.SaveChanges();
        }
        private void _ExpireAllTokens(long userId)
        {
            var user = _UnitOfWork.Users.Get(userId);
            if (user == null)
                throw new NotFoundException("User not found");
            // get current request refresh token
            var currentRefreshToken = _GetRefreshTokenCookie();
            // get user refresh token
            var refreshToken = _UnitOfWork.RefreshTokens.GetByToken(currentRefreshToken);
            if (refreshToken == null || !refreshToken.IsActive || refreshToken.ExpireTime < DateTime.UtcNow)
                return;
            // set refresh token to inactive
            refreshToken.IsActive = false;
            _UnitOfWork.RefreshTokens.Update(refreshToken);
        }
        public LoginResponse RefreshToken(string refreshToken, string deviceId)
        {

            var token = _UnitOfWork.RefreshTokens.GetByToken(refreshToken);
            if (token == null)
                throw new NotFoundException("Refresh token not found");
            if (token.ExpireTime < DateTime.UtcNow)
                throw new ConflictException("Refresh token expired");
            if (!token.IsActive)
                throw new ConflictException("Refresh token is not active");
            var user = _Get(token.UserId);
            if (user == null)
                throw new NotFoundException("User not found");
            var dto = user.ToDto();
            // expire current token
            token.IsActive = false;
            // authentication successful so generate refresh token 
            var newRefreshToken = _GenerateRefreshToken(dto);
            _UnitOfWork.RefreshTokens.Add(newRefreshToken);
            _UnitOfWork.RefreshTokens.Update(token);
            _UnitOfWork.SaveChanges();
            // refresh token is saved successfully, generate jwt token
            return new LoginResponse { Token = GenerateJsonWebToken(user), RefreshToken = newRefreshToken.Token };
        }

        public string GetAuthToken(long userId)
        {
            // get from database based on username
            var user = users.Get()
                .Include(x => x.Role)
                .FirstOrDefault(x => x.Id == userId);
            // check if user is null, return null
            if (user == null)
                return null;


            var dto = user.ToDto();
            // authentication successful so generate refresh token 
            var authToken = new AuthToken

            {
                Token = Guid.NewGuid().ToString(),
                ExpireTime = DateTime.UtcNow.AddDays(7),
                IssuedTime = DateTime.UtcNow,
                IsActive = true,
                UserId = user.Id
            };
            _UnitOfWork.AuthTokens.Add(authToken);
            _UnitOfWork.SaveChanges();
            return authToken.Token;

        }

        public LoginResponse Authenticate(string token, string device, string deviceId)
        {
            var authToken = _UnitOfWork.AuthTokens.Get().FirstOrDefault(x => x.Token == token);
            if (authToken == null)
                throw new NotFoundException("Auth token not found");
            if (authToken.ExpireTime < DateTime.UtcNow)
                throw new ConflictException("Auth token expired");
            if (!authToken.IsActive)
                throw new ConflictException("Auth token is not active");
            var user = _Get(authToken.UserId);
            if (user == null)
                throw new NotFoundException("User not found");
            var dto = user.ToDto();
            // expire current token
            authToken.IsActive = false;
            // authentication successful so generate refresh token 
            var newRefreshToken = _GenerateRefreshToken(dto);
            _UnitOfWork.RefreshTokens.Add(newRefreshToken);
            _UnitOfWork.AuthTokens.Update(authToken);
            _UnitOfWork.SaveChanges();
            // refresh token is saved successfully, generate jwt token
            return new LoginResponse { Token = GenerateJsonWebToken(user), RefreshToken = newRefreshToken.Token };
        }

        #region COOKIES
        private IResponseCookies _ResCookies => _HttpContextAccessor.HttpContext.Response.Cookies;
        private IRequestCookieCollection _ReqCookies => _HttpContextAccessor.HttpContext.Request.Cookies;
        private void _SetRefreshTokenCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.UtcNow.AddDays(7)
            };
            _ResCookies.Append("refreshToken", token, cookieOptions);
        }

        private string _GetRefreshTokenCookie()
        {
            string value = null;
            _ReqCookies.TryGetValue("refreshToken", out value);
            return value;
        }

        #endregion
    }
}
