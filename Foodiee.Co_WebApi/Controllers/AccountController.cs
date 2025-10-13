using AuthenticationAPI.Models.DTOs;
using Foodiee.Co_WebApi.DTO;
using Foodiee.Co_WebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;

namespace Foodiee.Co_WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly FoodieeDbContext _db;

        public AccountController(IConfiguration config, FoodieeDbContext context)
        {
            _config = config;
            _db = context;
        }

        [HttpGet("users")]
        [Authorize]
        public IActionResult GetUsers()
        {
            var users = _db.Users.Select(u => new
            {
                u.UserId,
                u.FirstName,
                u.LastName,
                u.PhoneNumber,
                u.Password,
                u.Email
            }).ToList();

            return Ok(users);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public IActionResult Login(LoginDTO loginRequest)
        {
            if (loginRequest == null)
            {
                return BadRequest("Login data was null");
            }

            var user = _db.Users
                .Where(u => u.Email == loginRequest.Email && u.Password == loginRequest.Password)
                .FirstOrDefault();

            if (user == null)
            {
                return BadRequest("Invalid Credentials");
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var expiry = Convert.ToInt32(_config["Jwt:ExpireMinutes"]);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Audience"],
              null,
              expires: DateTime.Now.AddMinutes(expiry),
              signingCredentials: credentials);

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new { Token = tokenString, UserName = user.FirstName, UserId = user.UserId });
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public IActionResult Register(RegisterDTO registerRequest)
        {
            if (registerRequest == null)
            {
                return BadRequest("Registration data was null");
            }

            var existingUser = _db.Users.Where(u => u.Email == registerRequest.Email).FirstOrDefault();
            if (existingUser != null)
            {
                return BadRequest("User with this email already exists");
            }

            var newUser = new User
            {
                Name = $"{registerRequest.FirstName} {registerRequest.LastName}",
                FirstName = registerRequest.FirstName,
                LastName = registerRequest.LastName,
                PhoneNumber = registerRequest.PhoneNumber,
                //DateOfBirth = DateTime.Now,
               
                Email = registerRequest.Email,
                Password = registerRequest.Password
            };

            _db.Users.Add(newUser);
            _db.SaveChanges();

            return Ok("User registered successfully");
        }
    }
}
//
