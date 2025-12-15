using JwtAuthApi.Entities.Abstract;

namespace JwtAuthApi.Entities.Concrete
{
    public class AppUser:BaseEntity
    {
        public string Email { get; set; }
        public string PasswordHash { get; set; }

        public string Role { get; set; } = "User";
    }
}
