using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class HistoryOrder
    {

        public HistoryOrder(int id, DateTime startDate, DateTime endDate, Decimal totalCost, string manufacturer, string modelName)
        {
            Id = id;
            StartDate = startDate;
            EndDate = endDate;
            TotalCost = totalCost;
            Manufacturer = manufacturer;
            ModelName = modelName;
        }

        public HistoryOrder(int id, DateTime startDate, DateTime endDate, Decimal totalCost, string manufacturer, string modelName, DateTime dateReturned)
        {
            Id = id;
            StartDate = startDate;
            EndDate = endDate;
            TotalCost = totalCost;
            Manufacturer = manufacturer;
            ModelName = modelName;
            DateReturned = dateReturned;
        }

        public int Id{ get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime? DateReturned { get; set; }
        public Decimal TotalCost { get; set; }
        public string Manufacturer { get; set; }
        public string ModelName { get; set; }
    }
}
