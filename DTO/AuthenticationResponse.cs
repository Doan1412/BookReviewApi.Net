using BookReview.Model;

namespace BookReview.DTO
{
    public class AuthenticationResponse
    {
        public string token { get; set; }
        public RefreshToken refreshToken { get; set; }
        public Role Role { get; set; }
    }
}
