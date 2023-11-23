using System;
namespace ZeroHunger.Models
{
	public class Restaurant
	{
        public int Id { get; set; }
        public int UserId { get; set; }
        public string RestaurantName { get; set; }
        public string Location { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<FoodRequest> FoodRequests { get; set; }
    }
}

