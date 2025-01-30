using CleanArchitectureNetCore.Application.Contracts;
using CleanArchitectureNetCore.Application.Contracts.Repositories;
using CleanArchitectureNetCore.Common;
using CleanArchitectureNetCore.Common.Entities;
using CleanArchitectureNetCore.Infrastructure.Persistence.EfMariaDb.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

namespace CleanArchitectureNetCore.Infrastructure.Persistence.EfMariaDb
{
    public class UnitOfWork : IUnitOfWork
    {
        #region FIELDS
        private readonly AppDbContext _Context;
        private readonly AuthUser _AuthUser;
        private readonly IConfiguration _Configuration;
        private UserRespository _UserRepository;
        private IRoleRepository _RoleRepository;
        private IPatientRepository _PatientRepository;
        private IRefreshTokenRepository _RefreshTokenRepository;
        private IAuthTokenRepository _AuthTokenRepository;
        private IRecommendationRepository _RecommendationRepository;
        #endregion

        #region PROPERTIES
        public IUserRepository Users => _UserRepository ??= new UserRespository(_Context, _AuthUser, _Configuration);

        public IPatientRepository PatientRepository => _PatientRepository ??= new PatientRepository(_Context, _AuthUser, _Configuration);
        public IRoleRepository RoleRepository => _RoleRepository ??= new RoleRepository(_Context);
        public IRefreshTokenRepository RefreshTokens => _RefreshTokenRepository ?? new RefreshTokenRepository(_Context, _AuthUser, _Configuration);

        public IAuthTokenRepository AuthTokens => _AuthTokenRepository ?? new AuthTokenRepository(_Context, _AuthUser, _Configuration);

        public IRecommendationRepository RecommendationRepository => _RecommendationRepository ?? new RecommendationRepository(_Context, _AuthUser, _Configuration);
        #endregion

        #region CONSTRUCTOR
        public UnitOfWork(AppDbContext context, AuthUser authUser, IConfiguration configuration)
        {
            _Context = context ?? throw new ArgumentNullException(nameof(context));
            _AuthUser = authUser ?? throw new ArgumentNullException(nameof(authUser));
            _Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        #endregion
        public void SaveChanges()
        {
            // get all entities that are changed
            var entries = _Context.ChangeTracker.Entries();
            // filter entities of type AuditableEntity
            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Unchanged) continue;
                if (entry.Entity is AuditableBaseEntity entity)
                {
                    // set the updated date of the entity
                    entity.LastModifiedOn = DateTime.UtcNow;
                    entity.LastModifiedBy = _AuthUser.UserId;
                    // set the created date of the entity
                    if (entry.State == EntityState.Added)
                    {
                        entity.CreatedBy = _AuthUser.UserId;
                        entity.CreatedOn = DateTime.UtcNow;
                    }
                }
            }
            this._Context.SaveChanges();
        }

        public void RevertChanges()
        {

            this._Context.ChangeTracker.Clear();
        }

        #region METHODS

        #endregion
    }
}

