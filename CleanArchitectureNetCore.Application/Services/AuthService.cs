using CleanArchitectureNetCore.Application.Contracts;
using CleanArchitectureNetCore.Application.Contracts.Repositories;
using CleanArchitectureNetCore.Application.RequestModels;
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
    internal enum eEmailType
    {
        ResetPassword = 1,
        ConfirmAddress = 2
    }

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

        protected string GenerateResetLink(string email, DateTime dateTime)
        {
            string url = _Configuration["ClientRedirectUrl"] as string;
            string type = _Configuration["ResetPassword"] as string;
            if (url == null || url == "")
                throw new InternalServerError("Link for reset password is missing");
            // create reset password link with token, email and expire time
            var expireTime = _Encode(dateTime.ToString("G"));
            string token = _Encode("emptyToken");
            string encodedEmail = _Encode(email);
            url += $"?type={type}&e={encodedEmail}&et={expireTime}&t={token}";
            return url;
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
            _ExpireAllTokens(userId);
            _UnitOfWork.SaveChanges();
        }
        private void _ExpireAllTokens(long userId)
        {
            var user = _UnitOfWork.Users.Get(userId);
            if (user == null)
                throw new NotFoundException("User not found");

            // get user refresh token
            var refreshTokens = _UnitOfWork.RefreshTokens.GetByUserId(userId);
            foreach (var token in refreshTokens)
            {
                if (token == null || !token.IsActive || token.ExpireTime < DateTime.UtcNow)
                    continue;
                token.IsActive = false;
                _UnitOfWork.RefreshTokens.Update(token);
            }
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

        public UserDto ResetPassword(ResetPasswordRequestModel request)
        {
            var email = _Decode(request.E);
            var expireTime = _Decode(request.ET);
            var token = _Decode(request.T);

            // if expire time is less than current time, link is expired
            if (Convert.ToDateTime(expireTime) < DateTime.UtcNow)
            {
                throw new ConflictException("Reset password link has expired");
            }
            // if token is valid
            if (!true)
                throw new BadRequestException("Invalid token");
            // if user exists against email
            var user = GetByUsernameOrEmail(email);
            if (user == null)
            {
                throw new NotFoundException("Username or email is not registered.");
            }
            // Create new salt
            var salt = Guid.NewGuid().ToString();
            // hash password
            var hashedPassword = HashPassword(request.Password, salt);
            user.Password = hashedPassword;
            user.Salt = salt;
            _UnitOfWork.Users.Update(user);
            _UnitOfWork.SaveChanges();
            return user.ToDto();
        }



    }
}
