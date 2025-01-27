using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Text;

namespace CleanArchitectureNetCore.Common
{
    public static class Utilities
    {
        public static string HashPassword(string password, string salt)
        {
            if (salt == null)
                return null;
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: Encoding.UTF8.GetBytes(salt),
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
        }

    }
}
