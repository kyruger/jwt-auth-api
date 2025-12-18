using JwtAuthApi.Entities.Abstract;

namespace JwtAuthApi.Entities.Concrete
{
    public class AppUser:BaseEntity
    {
        public AppUser()
        {
             RefreshTokens = new HashSet<RefreshToken>();
        }
        public string Email { get; set; }
        public string PasswordHash { get; set; }

        public string Role { get; set; } = "User";

        // Navigation Property
        public IEnumerable<RefreshToken> RefreshTokens { get; set; }
    }
}
