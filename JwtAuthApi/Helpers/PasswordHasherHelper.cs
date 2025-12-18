using System.Security.Cryptography;
using System.Text;

namespace JwtAuthApi.Helpers
{
    public static class PasswordHasherHelper
    {
        public static  string Hash(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes =Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);

        }

        public static bool Verify(string password, string hash) 
        {
            return Hash(password) == hash;
        }
    }
}
