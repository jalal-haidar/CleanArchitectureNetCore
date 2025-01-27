using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitectureNetCore.Application.ResponseModels
{
    public class LoginResponse
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
