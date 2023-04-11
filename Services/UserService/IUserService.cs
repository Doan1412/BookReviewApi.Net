using BookReview.DTO;
using BookReview.Model;

namespace BookReview.Services.UserService
{
    public interface IUserService
    {
        string CreateToken(User user);
        Task<RefreshToken> GenerateRefreshToken(User user);
        void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
        bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);
        public bool verifyExpiration(RefreshToken token);
        Task<AuthenticationResponse> registerUser(UserDto request);
        Task<AuthenticationResponse> registerAdmin(UserDto request);
        Task<AuthenticationResponse> authenticate(UserDto request);
        Task<AuthenticationResponse> refreshToken(string refreshtoken);
    }
}
