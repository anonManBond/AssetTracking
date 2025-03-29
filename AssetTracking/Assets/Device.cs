using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetTracking.Assets
{
    public abstract class Device
    {
        public string Type { get; }
        public string Brand { get; }
        public string Model { get; }
        public string Office { get; }
        public DateTime PurchaseDate { get; }
        public double PriceInUSD { get; }
        public string Currency { get; }
        public double LocalPriceToday { get; set; }

        public DateTime EndOfLife => PurchaseDate.AddYears(3);

        public Device(string type, string brand, string model, string office, string purchaseDate, double priceInUSD, string currency)
        {
            Type = type;
            Brand = brand;
            Model = model;
            Office = office;
            PurchaseDate = DateTime.Parse(purchaseDate);
            PriceInUSD = priceInUSD;
            Currency = currency;
        }

        // Abstract method for price calculation
        public abstract void CalculateLocalPrice();

        // Method to check if the device is close to end-of-life
        public string GetLifeStatus()
        {
            var timeToEndOfLife = EndOfLife - DateTime.Now;

            if (timeToEndOfLife.TotalDays <= 90)
                return "RED";
            else if (timeToEndOfLife.TotalDays <= 180)
                return "YELLOW";
            return "WHITE";
        }
    }
}
