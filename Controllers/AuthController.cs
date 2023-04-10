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
            return Ok(await _userService.registerUser(request));
        }
        [HttpPost("registerAdmin")]
        public async Task<ActionResult<AuthenticationResponse>> RegisterAdmin(UserDto request)
        {
            return Ok(_userService);
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthenticationResponse>> Login(UserDto request)
        {
            var r = await _userService.authenticate(request);
            if (r is null) return BadRequest("Login fail");
            return Ok(r);
        }

        /*[HttpPost("refresh-token")]
        public Task<AuthenticationResponse RefreshToken([FromBody] string refreshToken)
        {
            if (refreshToken == null) return BadRequest();
            return Ok(_userService.refreshToken(refreshToken));
            *//*var refreshToken = Request.Cookies["refreshToken"];

            if (!user.RefreshToken.Equals(refreshToken))
            {
                return Unauthorized("Invalid Refresh Token.");
            }
            else if (user.TokenExpires < DateTime.Now)
            {
                return Unauthorized("Token expired.");
            }

            string token = _userService.CreateToken(user);
            var newRefreshToken = _userService.GenerateRefreshToken();
            SetRefreshToken(newRefreshToken);

            return Ok(token);*//*
        }*/
    }
}
