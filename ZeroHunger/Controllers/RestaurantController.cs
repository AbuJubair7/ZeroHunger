using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ZeroHunger.Data;
using ZeroHunger.Models.DTOs;
using ZeroHunger.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ZeroHunger.Controllers
{
    public class RestaurantController : Controller
    {
        private readonly AppDbContext _dbContext;
        private string name = "RESTAURANT";

        public RestaurantController(AppDbContext context)
        {
            _dbContext = context;

        }

        private List<FoodRequest> GetFoodRequestsByRestaurantId(int restaurantId)
        {
            return _dbContext.FoodRequests
                .Where(request => request.RestaurantId == restaurantId)
                .ToList();
        }

        [HttpGet]
        public IActionResult Dashboard()
        {

            ViewBag.selected = "dash";
            int resId = int.Parse(String.IsNullOrEmpty(HttpContext.Request.Cookies["ResId"]) ? "0" : HttpContext.Request.Cookies["ResId"]);
            var restaurant = _dbContext.Restaurants.SingleOrDefault(u => u.Id == resId);

            List<FoodRequest> foodRequests = restaurant != null ? GetFoodRequestsByRestaurantId(restaurant.Id) : new List<FoodRequest>();
            ViewBag.name = restaurant != null ? restaurant.RestaurantName : name;

            Response.Cookies.Append("ResName", name);

            return View(foodRequests);
        }

        [HttpGet]
        public IActionResult AllRequest()
        {
            ViewBag.name = HttpContext.Request.Cookies["ResName"];
            ViewBag.selected = "all";
            return View();
        }

        [HttpGet]
        public IActionResult EditRequest()
        {
            ViewBag.name = HttpContext.Request.Cookies["ResName"];
            ViewBag.selected = "edit";
            return View();
        }

        [HttpGet]
        public IActionResult AddRequest()
        {
            ViewBag.name = HttpContext.Request.Cookies["ResName"];
            ViewBag.selected = "add";
            return View();
        }

        [HttpPost]
        public IActionResult AddRequest(FoodRequestDTO foodRequest)
        {
            ViewBag.name = HttpContext.Request.Cookies["ResName"];
            ViewBag.selected = "add";
            var newRequest = new FoodRequest
            {
                RestaurantId = int.Parse(String.IsNullOrEmpty(HttpContext.Request.Cookies["ResId"]) ? "0" : HttpContext.Request.Cookies["ResId"]),
                FoodName = foodRequest.FoodName,
                FoodQuantity = foodRequest.FoodQuantity,
                Status = "Not Assigned",
                PreserveDate = foodRequest.PreserveDate
            };

            _dbContext.FoodRequests.Add(newRequest);
            _dbContext.SaveChanges();
            return RedirectToAction("Dashboard");
        }
    }
}

