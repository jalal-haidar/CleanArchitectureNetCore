using CleanArchitectureNetCore.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitectureNetCore.Domain.Entities
{
    public class AuthToken : AuditableBaseEntity
    {
        public long UserId { get; set; }
        public string Token { get; set; }
        public DateTime IssuedTime { get; set; }
        public DateTime ExpireTime { get; set; }
        public string DeviceId { get; set; }
    }
}
