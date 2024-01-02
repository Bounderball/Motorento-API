using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace DAL.Models
{
    public partial class User
    {
        public User()
        {
            Orders = new HashSet<Order>();
        }

        [StringLength(50, MinimumLength = 6, ErrorMessage = "User name must be between 6 and 50 characters long")]
        public string UserName { get; set; }
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 50 characters long")]
        public string Password { get; set; }
        [RegularExpression("user|employee|admin")]
        public string Role { get; set; }
        [StringLength(50, MinimumLength = 2, ErrorMessage = "First name must be between 2 and 50 characters long")]
        public string FirstName { get; set; }
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Last name must be between 2 and 50 characters long")]
        public string LastName { get; set; }
        public DateTime? BirthDate { get; set; }
        public bool Sex { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Pic { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
