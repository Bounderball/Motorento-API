using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class WorkingCar
    {

        public WorkingCar(int id,
            //int modelId,
            decimal mileage, string pic,
            //bool inWorkingOrder, bool isAvailable,
            string licensePlateNumber,
            //int branch,
            string manufacturer, string modelName, decimal dailyPrice,
            decimal delayPricePerDay, DateTime manufactureYear, bool gear,
            string address,
            //decimal latitude, decimal longitude,
            string branchName)
        {
            Id = id;
            Mileage = mileage;
            Pic = pic;
            LicensePlateNumber = licensePlateNumber;
            Manufacturer = manufacturer;
            ModelName = modelName;
            DailyPrice = dailyPrice;
            DelayPricePerDay = delayPricePerDay;
            ManufactureYear = manufactureYear;
            Gear = gear;
            Address = address;
            BranchName = branchName;
        }

        //From Cars
        public int Id { get; set; }
        public int ModelId { get; set; } //FK
        public decimal Mileage { get; set; }
        public string Pic { get; set; }
        //public bool InWorkingOrder { get; set; } //uncommented
        public string LicensePlateNumber { get; set; }
        public int Branch { get; set; } //FK

        // From Models
        public string Manufacturer { get; set; }
        public string ModelName { get; set; }
        public decimal DailyPrice { get; set; }
        public decimal DelayPricePerDay { get; set; }
        public DateTime ManufactureYear { get; set; }
        public bool Gear { get; set; }

        //From Branches
        public string Address { get; set; }
        //public decimal Latitude { get; set; } //uncommented
        //public decimal Longitude { get; set; } //uncommented
        public string BranchName { get; set; }

    }
}
