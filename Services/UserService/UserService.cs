using Azure;
using BookReview.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using BookReview.Controllers;
using BookReview.Data;
using Microsoft.EntityFrameworkCore;
using BookReview.DTO;
using Azure.Core;

namespace BookReview.Services.UserService
{
    
    public class UserService : IUserService
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DataContext _dataContext;
        public UserService(IHttpContextAccessor httpContextAccessor, IConfiguration configuration,DataContext context)
        {
            _dataContext = context;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }
        public string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
        public async Task<RefreshToken> GenerateRefreshToken(User user)
        {
            var refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expires = DateTime.Now.AddDays(7),
                Created = DateTime.Now,
                User = user
            };
            _dataContext.Add(refreshToken);
            await _dataContext.SaveChangesAsync();
            return refreshToken;
        }

        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
        public async Task<AuthenticationResponse> registerUser(UserDto request)
        {
            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
            User user = new User();
            user.Username = request.Username;
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.Role = Role.USER.ToString();
            await _dataContext.users.AddAsync(user);
            await _dataContext.SaveChangesAsync();
            AuthenticationResponse response = new AuthenticationResponse();
            response.Role = Role.USER.ToString();
            response.token = CreateToken(user);
            response.refreshToken = GenerateRefreshToken(user).Result;
            return response;
        }
        public async Task<AuthenticationResponse> registerAdmin(UserDto request)
        {
            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
            User user = new User();
            user.Username = request.Username;
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.Role = Role.ADMIN.ToString();
            _dataContext.users.AddAsync(user);
            await _dataContext.SaveChangesAsync();
            AuthenticationResponse response = new AuthenticationResponse();
            response.Role = Role.USER.ToString();
            response.token = CreateToken(user);
            response.refreshToken = GenerateRefreshToken(user).Result;
            return response;
        }
        public bool verifyExpiration(RefreshToken token)
        {
            if (token.Expires.CompareTo(DateTime.Now) < 0)
            {
                _dataContext.refreshTokens.Remove(token);
                return false;
            }
            return true;
        }
        public async Task<AuthenticationResponse> authenticate(UserDto request)
        {
            var user = await _dataContext.users.FirstOrDefaultAsync(u => u.Username == request.Username);
            if (user is null || !VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt)) return null ;
            if (VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
            {
                return new AuthenticationResponse()
                {
                    token = CreateToken(user),
                    refreshToken = GenerateRefreshToken(user).Result,
                    Role = user.Role,
                };
            }
            return null;
        }
        public async Task<string> refreshToken(string rt)
        {
            Console.WriteLine(rt);
            var token = _dataContext.refreshTokens
                .Include(rt => rt.User)
                .FirstOrDefault(token => token.Token == rt);
            if (token != null && verifyExpiration(token))
            {
                token.Token=CreateToken(token.User);
                await _dataContext.SaveChangesAsync();
                return token.Token;
            }
            return null;
        }
    }
}
