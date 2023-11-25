using System;
namespace ZeroHunger.Models
{
	public class FoodRequest
	{
        public int Id { get; set; }
        public int RestaurantId { get; set; }
        public string FoodName { get; set; }
        public int FoodQuantity { get; set; }
        public string Status { get; set; }
        public string PreserveDate { get; set; }

        public virtual Restaurant Restaurant { get; set; }
    }
}

