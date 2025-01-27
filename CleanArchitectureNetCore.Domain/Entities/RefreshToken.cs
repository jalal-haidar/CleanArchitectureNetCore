using CleanArchitectureNetCore.Common.Entities;
using System;

namespace CleanArchitectureNetCore.Domain.Entities
{
    public class RefreshToken : AuditableBaseEntity
    {
        public long UserId { get; set; }
        public string Token { get; set; }
        public DateTime IssuedTime { get; set; }
        public DateTime ExpireTime { get; set; }
    }
}
