using System;
using System.Collections.Generic;

#nullable disable

namespace DAL.Models
{
    public partial class Order
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime? DateReturned { get; set; }
        public string UserName { get; set; }
        public int CarId { get; set; }

        public virtual Car Car { get; set; }
        public virtual User UserNameNavigation { get; set; }
    }
}
