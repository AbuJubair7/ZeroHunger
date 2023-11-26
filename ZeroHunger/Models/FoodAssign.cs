using System;
namespace ZeroHunger.Models
{
	public class FoodAssign
	{
        public int Id { get; set; }
        public int FoodRequestId { get; set; }
        public int UserId { get; set; }

        public virtual User User { get; set; }
        public virtual FoodRequest FoodRequest { get; set; }
    }
}

