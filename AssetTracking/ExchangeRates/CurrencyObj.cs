using System;

namespace AssetTracking.ExchangeRates
{
    public class CurrencyObj
    {
        public string CurrencyCode { get; set; }
        public double ExchangeRateFromEUR { get; set; }

        // Constructor to initialize the currency and its exchange rate
        public CurrencyObj(string currencyCode, double exchangeRateFromEUR)
        {
            CurrencyCode = currencyCode;
            ExchangeRateFromEUR = exchangeRateFromEUR;
        }
    }
}