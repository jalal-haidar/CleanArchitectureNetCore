namespace CleanArchitectureNetCore.Common.Enums
{
    public enum eUserType
    {
        Staff = 1,
        Patient = 2
    }

    public static class UserTypeExtension
    {
        public static string ToFriendlyString(this eUserType userType)
        {
            switch (userType)
            {
                case eUserType.Staff:
                    return "Staff";
                case eUserType.Patient:
                    return "Patient";
                default:
                    return "Unknown";
            }
        }
        public static eUserType Get(this eUserType userType, int value)
        {
            switch (value)
            {
                case 1:
                    return eUserType.Staff;
                case 2:
                    return eUserType.Patient;
                default:
                    return eUserType.Staff;
            }
        }
    }
}
