namespace CleanArchitectureNetCore.Common.Enums
{
    public enum eTokenName
    {
        Username = 2,
        UserId = 3,
        RoleId = 4,
        RoleName = 5,
        UserType = 6,

    }

    public static class TokenNameExtension
    {
        public static string Get(this eTokenName tokenName)
        {
            switch (tokenName)
            {
                case eTokenName.Username:
                    return "Username";
                case eTokenName.UserId:
                    return "UserId";
                case eTokenName.RoleId:
                    return "RoleId";
                case eTokenName.RoleName:
                    return "RoleName";
                case eTokenName.UserType:
                    return "UserType";
                default:
                    return "";
            }
        }
    }
}
