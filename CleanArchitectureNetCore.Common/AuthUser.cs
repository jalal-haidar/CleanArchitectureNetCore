using CleanArchitectureNetCore.Common.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;


namespace CleanArchitectureNetCore.Common
{
    public class AuthUser
    {
        /// <summary>
        /// userId
        /// </summary>
        public int UserId { get; private set; }


        /// <summary>
        /// username
        /// </summary>
        public string Username { get; private set; }

        /// <summary>
        /// Role of user
        /// </summary>
        public long RoleId { get; private set; }

        public eUserType UserType { get; private set; }



        public AuthUser(IHttpContextAccessor httpContextAccessor)
        {
            HttpContext? httpContext = httpContextAccessor.HttpContext;
            if (httpContext == null)
            {
                throw new System.Exception("Unable to access HttpContext");
            }
            try
            {
                if (httpContext.User.Identity != null && httpContext.User.Identity.IsAuthenticated)
                {
                    int userId = 0;
                    int.TryParse(_ReadToken(httpContext, eTokenName.UserId), out userId);
                    UserId = userId;
                    Username = _ReadToken(httpContext, eTokenName.Username);
                    var roleId = int.Parse(_ReadToken(httpContext, eTokenName.RoleId));
                    RoleId = roleId;// eRole.Developer.Get(roleId);
                    var userType = int.Parse(_ReadToken(httpContext, eTokenName.UserType));
                    UserType = eUserType.Staff.Get(userType);
                }
            }
            catch { }


        }
        /// <summary>
        /// Read value from JWT token
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public string _ReadToken(HttpContext httpContext, eTokenName name)
        {
            return httpContext.User.Claims.First(x => x.Type == name.Get()).Value;
        }

        public string GetRequestIP(IHttpContextAccessor httpContextAccessor, bool tryUseXForwardHeader = true)
        {
            string ip = null;

            // todo support new "Forwarded" header (2014) https://en.wikipedia.org/wiki/X-Forwarded-For

            // X-Forwarded-For (csv list):  Using the First entry in the list seems to work
            // for 99% of cases however it has been suggested that a better (although tedious)
            // approach might be to read each IP from right to left and use the first public IP.
            // http://stackoverflow.com/a/43554000/538763
            //
            if (tryUseXForwardHeader)
                ip = GetHeaderValueAs<string>(httpContextAccessor, "X-Forwarded-For").SplitCsv().FirstOrDefault();

            // RemoteIpAddress is always null in DNX RC1 Update1 (bug).
            if (ip.IsNullOrWhitespace() && httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress != null)
                ip = httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();

            if (ip.IsNullOrWhitespace())
                ip = GetHeaderValueAs<string>(httpContextAccessor, "REMOTE_ADDR");

            // _httpContextAccessor.HttpContext?.Request?.Host this is the local host.

            if (ip.IsNullOrWhitespace())
                throw new Exception("Unable to determine caller's IP.");

            return ip;
        }
        public T GetHeaderValueAs<T>(IHttpContextAccessor httpContextAccessor, string headerName)
        {
            StringValues values;

            if (httpContextAccessor.HttpContext?.Request?.Headers?.TryGetValue(headerName, out values) ?? false)
            {
                string rawValues = values.ToString();   // writes out as Csv when there are multiple.

                if (!rawValues.IsNullOrWhitespace())
                    return (T)Convert.ChangeType(values.ToString(), typeof(T));
            }
            return default(T);
        }




    }

    public static class StringExtension
    {
        public static bool IsNullOrWhitespace(this string s)
        {
            return String.IsNullOrWhiteSpace(s);
        }

        public static List<string> SplitCsv(this string csvList, bool nullOrWhitespaceInputReturnsNull = false)
        {
            if (string.IsNullOrWhiteSpace(csvList))
                return nullOrWhitespaceInputReturnsNull ? null : new List<string>();

            return csvList
                .TrimEnd(',')
                .Split(',')
                .AsEnumerable<string>()
                .Select(s => s.Trim())
                .ToList();
        }
    }
}
