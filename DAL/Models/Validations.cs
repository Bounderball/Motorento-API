using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{

    [MetadataType(typeof(BranchValidation))]
    public partial class Branch { }
    public class BranchValidation
    {
        [StringLength(100, MinimumLength = 7, ErrorMessage = "Address must be between 7 and 100 characters long")]
        public string Address { get; set; }
        [Range(-90, 90, ErrorMessage = "Latitude must be between -90 and 90")]
        public decimal Latitude { get; set; }
        [Range(-180, 180, ErrorMessage = "Longitude must be between -180 and 180")]
        public decimal Longitude { get; set; }
        [StringLength(100, MinimumLength = 7, ErrorMessage = "Branch name must be between 2 and 50 characters long")]
        public string BranchName { get; set; }
    }

    [MetadataType(typeof(CarValidation))]
    public partial class Car { }
    public class CarValidation
    {
        [Range(0.0, 99999999, ErrorMessage = "Mileage must be between 0 and 99,999,999")]
        public decimal Mileage { get; set; }
        [RegularExpression("^[0-9]{3,9}$", ErrorMessage = "License plate number must consist of only between 3 and 9 digits")]
        public string LicensePlateNumber { get; set; }
    }

    [MetadataType(typeof(ModelValidation))]
    public partial class Model { }
    public class ModelValidation
    {
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Manufacturer name must be between 2 and 50 characters long")]
        public string Manufacturer { get; set; }
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Model name must be between 1 and 50 characters long")]
        public string ModelName { get; set; }
        [Range(0, 999_999_999_999_999, ErrorMessage = "Daily price must be a number between 0 and 999,999,999,999,999")]
        public decimal DailyPrice { get; set; }
        [Range(0, 999_999_999_999_999, ErrorMessage = "Daily delay price must be a number between 0 and 999,999,999,999,999")]
        public decimal DelayPricePerDay { get; set; }
    }

    [MetadataType(typeof(OrderValidation))]
    public partial class Order { }
    public class OrderValidation
    {

    }

    [MetadataType(typeof(UserValidation))]
    public partial class User { }
    public class UserValidation
    {
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
        [EmailAddress]
        public string Email { get; set; }
    }


}
