using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ZeroHunger.Data;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ZeroHunger.Controllers
{
    public class RestaurantController : Controller
    {
        private readonly AppDbContext _dbContext;

        public RestaurantController(AppDbContext context)
        {
            _dbContext = context;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            ViewBag.Name = "RESTAURANT";
            int resId = int.Parse(String.IsNullOrEmpty(HttpContext.Request.Cookies["ResId"]) ? "0" : HttpContext.Request.Cookies["ResId"]);
            var restaurant = _dbContext.Restaurants.SingleOrDefault(u => u.Id == resId);
            if (restaurant != null)
            {
                ViewBag.name = restaurant.RestaurantName;
            }
            return View();
        }
    }
}

