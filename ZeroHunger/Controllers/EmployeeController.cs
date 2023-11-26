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
    public class EmployeeController : Controller
    {
        private readonly AppDbContext _dbContext;
        public EmployeeController(AppDbContext context)
        {
            _dbContext = context;
        }
        // GET: /<controller>/

        private List<FoodAssign> GetAssignById(int id)
        {
            return _dbContext.FoodAssigns
                .Where(request => request.UserId == id)
                .ToList();
        }

        public IActionResult Assigned()
        {
            int userId = int.Parse(HttpContext.Request.Cookies["UserId"]);
            var user = _dbContext.Users.SingleOrDefault(u => u.Id == userId);
            ViewBag.name = user.Name;
            ViewBag.selected = "assign";

            List<FoodAssign> foodAssigns = GetAssignById(userId);

            List<FoodRequest> foodRequests = new List<FoodRequest>();

            foreach (var foodAssign in foodAssigns)
            {
                var foodRequest = _dbContext.FoodRequests.SingleOrDefault(req => req.Id == foodAssign.FoodRequestId);

                if (foodRequest != null)
                {
                    if (foodRequest.Status == "Assigned")
                    {
                        foodRequests.Add(foodRequest);
                    }   
                }
            }

            return View(foodRequests);
        }

        public IActionResult Accept(int foodRequestId)
        {
            var foodRequest = _dbContext.FoodRequests.SingleOrDefault(u => u.Id == foodRequestId);
            foodRequest.Status = "Collected";
            _dbContext.SaveChanges();

            int userId = int.Parse(HttpContext.Request.Cookies["UserId"]);
            var employee = _dbContext.Employees.SingleOrDefault(u => u.UserId == userId);
            employee.NoOfOrderCompleted += 1;

            _dbContext.SaveChanges();

            return RedirectToAction("Assigned");
        }

        public IActionResult Completed()
        {
            int userId = int.Parse(HttpContext.Request.Cookies["UserId"]);
            var user = _dbContext.Users.SingleOrDefault(u => u.Id == userId);
            ViewBag.name = user.Name;
            ViewBag.selected = "complete";

            List<FoodAssign> foodAssigns = GetAssignById(userId);

            List<FoodRequest> foodRequests = new List<FoodRequest>();

            foreach (var foodAssign in foodAssigns)
            {
                var foodRequest = _dbContext.FoodRequests.SingleOrDefault(req => req.Id == foodAssign.FoodRequestId);

                if (foodRequest != null)
                {
                    if (foodRequest.Status == "Collected")
                    {
                        foodRequests.Add(foodRequest);
                    }
                }
            }
            return View(foodRequests);
        }

        public IActionResult SignOut()
        {
            foreach (var cookie in Request.Cookies.Keys)
            {
                Response.Cookies.Delete(cookie);
            }
            // Clear session
            HttpContext.Session.Clear();

            return RedirectToAction("SignIn", "Account");
        }
    }
}

