using System.Security.Cryptography;

namespace JwtAuthApi.Helpers
{
    public static class RefreshTokenHelper
    {
        public static string GenerateToken()
        {
            var randomBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);

        }
    }
}
