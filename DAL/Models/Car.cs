using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace DAL.Models
{
    public partial class Car
    {
        public Car()
        {
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }
        public int ModelId { get; set; }
        [Range(0.0, 99999999, ErrorMessage = "Mileage must be between 0 and 99,999,999")]
        public decimal Mileage { get; set; }
        public string Pic { get; set; }
        public bool InWorkingOrder { get; set; }
        [RegularExpression("^[0-9]{3,9}$", ErrorMessage = "License plate number must consist of only between 3 and 9 digits")]
        public string LicensePlateNumber { get; set; }
        public int Branch { get; set; }

        public virtual Branch BranchNavigation { get; set; }
        public virtual Model Model { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
