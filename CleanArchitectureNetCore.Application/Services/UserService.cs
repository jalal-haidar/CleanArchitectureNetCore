using CleanArchitectureNetCore.Application.Contracts;
using CleanArchitectureNetCore.Application.Contracts.Repositories;
using CleanArchitectureNetCore.Application.RequestModels;
using CleanArchitectureNetCore.Domain.DTOs;
using CleanArchitectureNetCore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using SoftoException.Exceptions.Http;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CleanArchitectureNetCore.Application.Services
{
    public class UserService 
    {
        private readonly IUnitOfWork _UnitOfWork;
        private readonly AuthService _AuthService;
        private IUserRepository _Users => _UnitOfWork.Users;


        public IQueryable<User> users => _UnitOfWork.Users.Get()
                .Include(x => x.Role);

        public UserService(IUnitOfWork unitOfWork, AuthService authService)
        {
            this._UnitOfWork = unitOfWork;
            _AuthService = authService;
        }
        public UserDto Create(UserRequest request)
        {
            var newUser = request.ToUser();
            if (_UnitOfWork.Users.Get().Any(x => x.Email.ToLower() == request.Email.ToLower()))
                throw new ConflictException("Email/Username already used");
            newUser.Password = "SoftoSol@isb";
            newUser = _AuthService.GeneratePassword(newUser);
            // newUser.Role = _UnitOfWork.RoleRepository.Get(2);
            // TODO: check new user role based on use role
            // Verify RoleId exists
            if (!_UnitOfWork.RoleRepository.Get().Any(r => r.Id == request.RoleId))
                throw new ArgumentException("Invalid RoleId");

            newUser.RoleId = request.RoleId != 0 ? request.RoleId : 2;
            newUser = _UnitOfWork.Users.Add(newUser);
            _UnitOfWork.SaveChanges();
            return newUser.ToDto();
        }

        public UserDto Update(long id, UserRequest request)
        {
            var user = request.ToUser();
            user.Id = id;
            // TODO: check new user role based on use role
            user = _UnitOfWork.Users.Update(user);
            _UnitOfWork.SaveChanges();
            return user.ToDto();
        }

        public IEnumerable<UserDto> GetAll()
        {
            return users.Select(x => x.ToDto())?.ToList();
        }
        public IEnumerable<UserDto> Get(int pageNumber, int pageSize, string filter)
        {
            var query = _Users.Get();

            if (!string.IsNullOrEmpty(filter))
            {
                query = query.Where(p => p.Name.Contains(filter));
            }

            return query.Skip((pageNumber - 1) * pageSize).Take(pageSize).Select(p => p.ToDto()).ToList();
        }
        public IEnumerable<UserDto> Search(string searchTerm)
        {
            var query = _Users.Get();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(p => p.Name.Contains(searchTerm) || p.Id.ToString().Contains(searchTerm) );
            }

            return query.Select(p => p.ToDto()).ToList();
        }
        public UserDto Get(long id)
        {
            return _Get(id).ToDto();
        }
        private User _Get(long id)
        {
            return users.FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<UserDto> Get()
        {
            return GetAll();
        }

        public UserDto Delete(long id)
        {
            var user = _UnitOfWork.Users.Delete(id)?.ToDto();
            _UnitOfWork.SaveChanges();
            return user;
        }
      

    }
}
