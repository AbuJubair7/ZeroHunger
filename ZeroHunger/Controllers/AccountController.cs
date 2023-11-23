using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ZeroHunger.Models;
using ZeroHunger.Models.DTOs;
using BCrypt.Net; // Adjust the namespace

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ZeroHunger.Controllers
{
    public class AccountController : Controller
    {

        // GET: /<controller>/
        public IActionResult Login(string type)
        {
            ViewData["UserType"] = type;
            return View();
        }

        public IActionResult SignIn()
        {
            return View();
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
                // Perform additional validation if needed
                // Example: Check if the email is unique before creating a new user

                // Map DTO to User entity
                var newUser = new User
                {
                    Name = signUpDto.Name,
                    Email = signUpDto.Email,
                    Phone = signUpDto.Phone,
                    Password = HashPassword(signUpDto.Password), // Manually hash the password
                    Address = signUpDto.Address
                };

                // Save the new user to the database or perform other actions
                // Example: userRepository.Create(newUser);

                // Redirect to a signIn page or another action
                return RedirectToAction("SignIn");
            }

            // If ModelState is not valid, return to the sign-up page with validation errors
            return View(signUpDto);
        }

        private string HashPassword(string password)
        {
            // Hash the password using BCrypt
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
    }
}

