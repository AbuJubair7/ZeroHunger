using System;
namespace ZeroHunger.Models
{
	public class Employee
	{
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime JoiningDate { get; set; }
        public int NoOfOrderCompleted { get; set; }

        public virtual User User { get; set; }
    }
}

