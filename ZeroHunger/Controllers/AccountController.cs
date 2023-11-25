using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ZeroHunger.Models;
using ZeroHunger.Models.DTOs;
using ZeroHunger.Data;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;

namespace ZeroHunger.Controllers
{
    
    public class AccountController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly IConfiguration _configuration;

        public AccountController(AppDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }
        public IActionResult Login(string type)
        {
            ViewData["UserType"] = type;
            return View();
        }

        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SignIn(SignInDTO signInDto)
        {
            if (ModelState.IsValid)
            {
                // Retrieve the user from the database based on the provided email
                var user = _dbContext.Users.SingleOrDefault(u => u.Email == signInDto.Email);

                if (user != null && BCrypt.Net.BCrypt.Verify(signInDto.Password, user.Password))
                {
                    var token = GenerateJwtToken(user);
                    Console.WriteLine($"Bearer {token}");
                    // Set the token as a cookie
                    Response.Cookies.Append("JWTToken", token);
                    // Set a session variable to indicate successful sign-in
                    HttpContext.Session.SetString("IsAuthenticated", "true");

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid email or password");
                }
            }
            return View(signInDto);
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SignUp(SignUpDTO signUpDto)
        {
            if (ModelState.IsValid)
            {
                // Map DTO to User entity
                var newUser = new User
                {
                    Name = signUpDto.Name,
                    Email = signUpDto.Email,
                    Phone = signUpDto.Phone,
                    Password = HashPassword(signUpDto.Password),
                    Address = signUpDto.Address
                };

                // Save the new user to the database or perform other actions
                _dbContext.Users.Add(newUser);
                _dbContext.SaveChanges();

                var token = GenerateJwtToken(newUser);
                Console.WriteLine($"Bearer {token}");
                // Set the token as a cookie
                Response.Cookies.Append("JWTToken", token);
                // Set a session variable to indicate successful sign-in
                HttpContext.Session.SetString("IsAuthenticated", "true");

                return RedirectToAction("Index", "Home");
            }
            return View(signUpDto);
        }

        private string HashPassword(string password)
        {
            // Hash the password using BCrypt
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        // Generate token
        private string GenerateJwtToken(User user)
        { 
            Console.WriteLine($"id: {user.Id}");
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
            };

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpiresInMinutes"])),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

