using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AssetTracking.Assets;
using AssetTracking.ExchangeRates;

namespace AssetTracking.Assets
{
    public class Computer : Device
    {
        public Computer(string brand, string model, string office, string purchaseDate, double priceInUSD, string currency)
            : base("Computer", brand, model, office, purchaseDate, priceInUSD, currency)
        { }

        // Implement the CalculateLocalPrice method for Computer
        public override void CalculateLocalPrice()
        {
            // Convert the price to the local currency using LiveCurrency
            LocalPriceToday = LiveCurrency.Convert(PriceInUSD, "USD", Currency);
        }
    }
}

