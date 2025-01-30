using CleanArchitectureNetCore.Application.Contracts.Repositories;
using CleanArchitectureNetCore.Application.Contracts;
using CleanArchitectureNetCore.Application.RequestModels;
using CleanArchitectureNetCore.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitectureNetCore.Application.Services
{
    public class RoleService 
    {
        private readonly IUnitOfWork _UnitOfWork;
        private IRoleRepository _Roles => _UnitOfWork.RoleRepository;

        public RoleService(IUnitOfWork unitOfWork)
        {
            this._UnitOfWork = unitOfWork;
        }

        public RoleDto Create(RoleRequest request)
        {
            var role = _Roles.Add(request.ToRole())?.ToDto();
            _UnitOfWork.SaveChanges();
            return role;
        }

        public RoleDto Delete(long id)
        {
            var role = _Roles.Delete(id)?.ToDto();
            _UnitOfWork.SaveChanges();
            return role;
        }

        public IEnumerable<RoleDto> Get()
        {
            return _Roles.Get().Select(x => x.ToDto()).ToList();
        }

        public RoleDto Get(long id)
        {
            return _Roles.Get(id)?.ToDto();
        }

        public RoleDto Update(long id, RoleRequest request)
        {
            var role = request.ToRole();
            role.Id = id;
            var roleUpdated = _Roles.Update(role)?.ToDto();
            _UnitOfWork.SaveChanges();
            return roleUpdated;
        }
    }
}
