using System;
namespace ZeroHunger.Models
{
	public class User
	{
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }

        public virtual Restaurant Restaurant { get; set; }
        public virtual Employee Employee { get; set; }
    }
}

