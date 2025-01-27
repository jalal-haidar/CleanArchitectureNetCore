using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitectureNetCore.Application.RequestModels
{
    public class TokenLoginRequest
    {
        public string Token { get; set; }
        public string device { get; set; } = "web";
    }
}
