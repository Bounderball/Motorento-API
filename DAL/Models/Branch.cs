using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace DAL.Models
{
    public partial class Branch
    {
        public Branch()
        {
            Cars = new HashSet<Car>();
        }

        public int Id { get; set; }
        [StringLength(100, MinimumLength = 7, ErrorMessage = "Address must be between 7 and 100 characters long")]
        public string Address { get; set; }
        [Range(-90, 90, ErrorMessage = "Latitude must be between -90 and 90")]
        public decimal Latitude { get; set; }
        [Range(-180, 180, ErrorMessage = "Longitude must be between -180 and 180")]
        public decimal Longitude { get; set; }
        [StringLength(100, MinimumLength = 7, ErrorMessage = "Branch name must be between 2 and 50 characters long")]
        public string BranchName { get; set; }

        public virtual ICollection<Car> Cars { get; set; }
    }
}
