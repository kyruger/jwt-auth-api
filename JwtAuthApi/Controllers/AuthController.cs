
using JwtAuthApi.Data;
using JwtAuthApi.Entities.Concrete;
using JwtAuthApi.Helpers;
using JwtAuthApi.Models.DTOs;
using JwtAuthApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JwtAuthApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly JwtTokenService _jwtTokenService;

        public AuthController(AppDbContext context, JwtTokenService jwtTokenService)
        {
            _jwtTokenService = jwtTokenService;
            _context = context;
        }

        [HttpPost("register")]
        public IActionResult Register(RegisterDTO model)
        {
            var isExisted = _context.Users.Any(u => u.Email == model.Email);
            if (isExisted)
            {
                return BadRequest("User with same email already exists");
            }

            var user = new AppUser
            {
                Email = model.Email,
                PasswordHash = PasswordHasherHelper.Hash(model.Password),
                //Role = "Admin" first time creating admin

            };
            _context.Users.Add(user);
            _context.SaveChanges();
            return Ok("User registered");

            
           
        }
        [HttpPost("login")]
        public IActionResult Login(LoginDTO model)
        {
            var user = _context.Users.SingleOrDefault(u => u.Email == model.Email);
            if(user==null || !PasswordHasherHelper.Verify(model.password, user.PasswordHash))
            {
                return Unauthorized("Invalid email or password");
            }
            
            var accessToken = _jwtTokenService.GenerateToken(user);

            var refreshToken = new RefreshToken
            {
                Token = RefreshTokenHelper.GenerateToken(),
                Expires=DateTime.Now.AddDays(7),
                UserId = user.Id
            };


            _context.RefreshTokens.Add(refreshToken);
            _context.SaveChanges();

            Response.Cookies.Append("refreshToken", refreshToken.Token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = refreshToken.Expires
            });

            return Ok(new 
            {
                AccessToken = accessToken,
            });
        }
        [HttpPost("refresh-token")]
        public IActionResult RefreshToken()
        {
            if(!Request.Cookies.TryGetValue("refreshToken", out var refreshTokenFromCookie))
            {
                return Unauthorized("Refresh token is missing");
            }
            var storedRefreshToken = _context.RefreshTokens.Include(rt=>rt.User)
                .SingleOrDefault(rt => 
                rt.Token == refreshTokenFromCookie);

            if(storedRefreshToken != null && storedRefreshToken.IsRevoked)
            {
                var userTokens= _context.RefreshTokens.Where(rt=> rt.UserId == storedRefreshToken.User.Id && !rt.IsRevoked).ToList();

                foreach(var token in userTokens)
                    token.IsRevoked = true; 


                _context.SaveChanges();

                Response.Cookies.Delete("refreshToken");

                return Unauthorized("Possible token abuse detected. All tokens revoked.");
            }


            if(storedRefreshToken==null || storedRefreshToken.Expires < DateTime.Now || storedRefreshToken.IsRevoked)
            {
                return Unauthorized("Invalid or expired refresh token");
            }


            storedRefreshToken.IsRevoked = true;

            var newRefreshToken = new RefreshToken
            {
                Token = RefreshTokenHelper.GenerateToken(),
                Expires = DateTime.Now.AddDays(7),
                UserId = storedRefreshToken.UserId
            };

            _context.RefreshTokens.Add(newRefreshToken);
            _context.SaveChanges();

            Response.Cookies.Append("refreshToken", newRefreshToken.Token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = newRefreshToken.Expires
            });

            var newAccessToken = _jwtTokenService.GenerateToken(storedRefreshToken.User);

            return Ok( new
            {
                AccessToken = newAccessToken,
            });
        }
        [HttpPost("logout")]
        public IActionResult Logout() 
        {
            if (!Request.Cookies.TryGetValue("refreshToken", out var refreshTokenFromCookie)) 
            {
                return BadRequest("Refresh token is missing");
            }

            var storedRefreshToken = _context.RefreshTokens.
                SingleOrDefault(rt=>
                rt.Token == refreshTokenFromCookie);

            if(storedRefreshToken == null)
            {
                Response.Cookies.Delete("refreshToken");
                return Ok("Logged out successfully");
            }

            if(storedRefreshToken.IsRevoked)
            {
                var userTokens = _context.RefreshTokens
                    .Where(rt=> rt.UserId == storedRefreshToken.UserId && !rt.IsRevoked).ToList();

                foreach(var token in userTokens)
                    token.IsRevoked = true;


                _context.SaveChanges();

                Response.Cookies.Delete("refreshToken");
            }

            storedRefreshToken.IsRevoked = true;
            _context.SaveChanges();


            Response.Cookies.Delete("refreshToken");

            return Ok("Logged out successfully");



        }
    } 
}
