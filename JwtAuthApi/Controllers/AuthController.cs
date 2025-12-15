
using JwtAuthApi.Data;
using JwtAuthApi.Entities.Concrete;
using JwtAuthApi.Helpers;
using JwtAuthApi.Models.DTOs;
using JwtAuthApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
                PasswordHash = PasswordHasher.Hash(model.Password),
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
            if(user==null || !PasswordHasher.Verify(model.password, user.PasswordHash))
            {
                return Unauthorized("Invalid email or password");
            }
            
            var token = _jwtTokenService.GenerateToken(user);
            return Ok(new { token });
        }
    } 
}
