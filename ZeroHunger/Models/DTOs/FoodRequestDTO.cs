using System;
using System.ComponentModel.DataAnnotations;

namespace ZeroHunger.Models.DTOs
{
	public class FoodRequestDTO
	{
        [Required(ErrorMessage = "Food Name is required")]
        public string FoodName { get; set; }

        [Required(ErrorMessage = "Food Quantity is required")]
        public int FoodQuantity { get; set; }

        [Required(ErrorMessage = "Preserve Date is required")]
        public string PreserveDate { get; set; }
    }
}

