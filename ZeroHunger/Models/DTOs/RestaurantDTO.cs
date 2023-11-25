using System;
using System.ComponentModel.DataAnnotations;

namespace ZeroHunger.Models.DTOs
{
    public class RestaurantDTO
    {
        [Required(ErrorMessage = "Restaurant name is required")]
        public string RestaurantName { get; set; }

        [Required(ErrorMessage = "Location is required")]
        public string Location { get; set; }
    }
}

