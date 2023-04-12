using BookReview.Model;
using BookReview.DTO;
using BookReview.Services.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace BookReview.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthenticationResponse>> Register(UserDto request)
        {
            var r = await _userService.registerUser(request);
            if (r.token == null) return BadRequest("Username is already taken");
            return Ok(r);
        }
        [HttpPost("registerAdmin")]
        public async Task<ActionResult<AuthenticationResponse>> RegisterAdmin(UserDto request)
        {
            var r = await _userService.registerAdmin(request);
            if (r.token == null) return BadRequest("Username is already taken");
            return Ok(r);
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthenticationResponse>> Login(UserDto request)
        {
            var r = await _userService.authenticate(request);
            if (r is null) return BadRequest("Login fail");
            return Ok(r);
        }
        [HttpPost("refresh-token")]
        public async Task<ActionResult<AuthenticationResponse>> refreshToken(string refreshToken)
        {
            if (refreshToken is null) return BadRequest("Expired RefreshToken");
            var r = await _userService.refreshToken(refreshToken);
            if (r is null) return BadRequest("Invalid refreshToken");
            return Ok(r);
        }
    }
}
