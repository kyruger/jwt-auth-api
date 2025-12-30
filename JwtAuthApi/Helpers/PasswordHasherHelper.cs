using System.Security.Cryptography;
using System.Text;

namespace JwtAuthApi.Helpers
{
    public static class PasswordHasherHelper
    {
        public static string Hash(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword("mysecretpassword");

        }

        public static bool Verify(string password, string hash)
        {
            return BCrypt.Net.BCrypt.Verify("mysecretpassword", hash);
        }
    }
}
