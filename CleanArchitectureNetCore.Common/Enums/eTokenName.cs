using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitectureNetCore.Common.Enums
{
    public enum eTokenName
    {
        TenantId = 1,
        Username = 2,
        UserId = 3,
        RoleId = 4,
        RoleName = 5,

    }

    public static class TokenNameExtension
    {
        public static string Get(this eTokenName tokenName)
        {
            switch (tokenName)
            {
                case eTokenName.TenantId:
                    return "TenantId";
                case eTokenName.Username:
                    return "Username";
                case eTokenName.UserId:
                    return "UserId";
                case eTokenName.RoleId:
                    return "RoleId";
                case eTokenName.RoleName:
                    return "RoleName";
                default:
                    return "";
            }
        }
    }
}
