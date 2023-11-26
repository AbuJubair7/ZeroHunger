using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ZeroHunger.Data;
using ZeroHunger.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ZeroHunger.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _dbContext;

        public AdminController(AppDbContext context)
        {
            _dbContext = context;
        }

        private List<FoodRequest> GetFoodRequests()
        {
            return _dbContext.FoodRequests
                .Where(request => request.Status == "Not Assigned")
                .ToList();
        }

        [HttpGet]
        public IActionResult Dashboard()
        {

            ViewBag.selected = "dash";
            List<FoodRequest> allFood = GetFoodRequests();
            List<Employee> allEmployees = _dbContext.Employees.ToList();

            var viewModel = new DashboardViewModel
            {
                FoodRequests = allFood,
                Employees = allEmployees
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Assign(int foodRequestId, int employeeId)
        {
            if (employeeId > 0)
            {
                var employee = _dbContext.Employees.SingleOrDefault(u => u.Id == employeeId);
                var newAssign = new FoodAssign
                {
                    FoodRequestId = foodRequestId,
                    UserId = employee.UserId
                };
                var foodRequest = _dbContext.FoodRequests.Find(foodRequestId);
                foodRequest.Status = "Assigned";
                _dbContext.SaveChanges();

                _dbContext.FoodAssigns.Add(newAssign);
                _dbContext.SaveChanges();
            }

            return RedirectToAction("Dashboard"); // Redirect to your Dashboard action after processing.
        }


        public IActionResult AllEmployee()
        {
            ViewBag.selected = "emp";
            List<Employee> employees = _dbContext.Employees.ToList();
            return View(employees);
        }

        public IActionResult AllRestaurant()
        {
            ViewBag.selected = "res";
            List<Restaurant> restaurants = _dbContext.Restaurants.ToList();
            return View(restaurants);
        }

        public IActionResult SignOut()
        {
            foreach (var cookie in Request.Cookies.Keys)
            {
                Response.Cookies.Delete(cookie);
            }
            // Clear session
            HttpContext.Session.Clear();

            return RedirectToAction("Login", "Account");
        }
    }
}

