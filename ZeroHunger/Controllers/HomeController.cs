using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZeroHunger.Data;
using ZeroHunger.Models;
using ZeroHunger.Models.DTOs;

namespace ZeroHunger.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly AppDbContext _dbContext;

    public HomeController(ILogger<HomeController> logger, AppDbContext context)
    {
        _logger = logger;
        _dbContext = context;
    }

    public IActionResult Index()
    {
        var restaurant = _dbContext.Restaurants.SingleOrDefault(u => u.UserId == int.Parse(HttpContext.Request.Cookies["UserId"]));
        if (restaurant != null)
        {
            Response.Cookies.Append("ResId", restaurant.Id.ToString());
            return RedirectToAction("Index", "Restaurant");
        }
        ViewData["Title"] = "Welcome to the Home Page!";
        return View();
    }

    [HttpGet]
    public IActionResult CreateRestaurant()
    {
        return View();
    }

    [HttpPost]
    public IActionResult CreateRestaurant(RestaurantDTO restaurantDTO)
    {
       if (ModelState.IsValid)
        {
            var newRestaurant = new Restaurant
            {
                UserId = int.Parse(String.IsNullOrEmpty(HttpContext.Request.Cookies["UserId"]) ? "NULL" : HttpContext.Request.Cookies["UserId"]),
                RestaurantName = restaurantDTO.RestaurantName,
                Location = restaurantDTO.Location
            };
            _dbContext.Restaurants.Add(newRestaurant);
            _dbContext.SaveChanges();

            // cookies to save resID
            Response.Cookies.Append("ResId", newRestaurant.Id.ToString());

            
            return RedirectToAction("Index", "Restaurant");
        }
        return View(restaurantDTO);
    }

    public IActionResult JoinAsEmployee()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

