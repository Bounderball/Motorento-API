using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace DAL.Models
{
    public partial class Model
    {
        public Model()
        {
            Cars = new HashSet<Car>();
        }

        public int Id { get; set; }
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Manufacturer name must be between 2 and 50 characters long")]
        public string Manufacturer { get; set; }
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Model name must be between 1 and 50 characters long")]
        public string ModelName { get; set; }
        [Range(0, 999_999_999_999_999, ErrorMessage = "Daily price must be a number between 0 and 999,999,999,999,999")]
        public decimal DailyPrice { get; set; }
        [Range(0, 999_999_999_999_999, ErrorMessage = "Daily delay price must be a number between 0 and 999,999,999,999,999")]
        public decimal DelayPricePerDay { get; set; }
        public DateTime ManufactureYear { get; set; }
        public bool Gear { get; set; }

        public virtual ICollection<Car> Cars { get; set; }
    }
}
