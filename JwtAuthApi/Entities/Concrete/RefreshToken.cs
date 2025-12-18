using JwtAuthApi.Entities.Abstract;

namespace JwtAuthApi.Entities.Concrete
{
    public class RefreshToken:BaseEntity
    {
        public string Token { get; set; }
        public DateTime Expires { get; set; }
        public bool IsRevoked { get; set; }

        // Navigation Property
        public int UserId { get; set; }
        public AppUser User { get; set; }


    }
}
