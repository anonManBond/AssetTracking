using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AssetTracking.ExchangeRates;


namespace AssetTracking.Assets
{
    public class Phone : Device
    {
        public Phone(string brand, string model, string office, string purchaseDate, double priceInUSD, string currency)
            : base("Phone", brand, model, office, purchaseDate, priceInUSD, currency)
        { }

        // Implement the CalculateLocalPrice method for Phone
        public override void CalculateLocalPrice()
        {
            // Convert the price to the local currency using LiveCurrency
            LocalPriceToday = LiveCurrency.Convert(PriceInUSD, "USD", Currency);
        }
    }
}
