using System;
using System.Net;
using System.Xml;
using System.Globalization;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.Collections.Generic;
using AssetTracking.ExchangeRates;

namespace AssetTracking.ExchangeRates
{
    public class LiveCurrency
    {
        private static List<CurrencyObj> currencyList = new List<CurrencyObj>();

        public static void FetchRates()
        {
            string url = "https://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml";

            XmlTextReader reader = new XmlTextReader(url);
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    while (reader.MoveToNextAttribute())
                    {
                        if (reader.Name == "currency") // Identifies each currency attribute and saves the currency code and rate as an object
                        {
                            string currencyCode = reader.Value;

                            reader.MoveToNextAttribute();
                            string rateString = reader.Value;

                            // Try-catch block for error handling during parsing (very useful when interacting with web)
                            try
                            {
                                double rate = double.Parse(rateString, CultureInfo.InvariantCulture);
                                currencyList.Add(new CurrencyObj(currencyCode, rate));
                            }
                            catch (FormatException ex)
                            {
                                Console.WriteLine($"Error parsing rate for currency {currencyCode}: {rateString}. Exception: {ex.Message}");
                            }
                        }
                    }
                }
            }
        }

        public static double Convert(double input, string fromCurrency, string toCurrency)
        {
            // If the source and target currencies are the same, return the input amount 
            if (fromCurrency == toCurrency)
            {
                return input;
            }


            double value = 0;

            try
            {
                // Convert from EUR to another currency
                if (fromCurrency == "EUR")
                {
                    var targetCurrency = currencyList.Find(c => c.CurrencyCode == toCurrency);
                    if (targetCurrency != null)
                    {
                        value = input * targetCurrency.ExchangeRateFromEUR;
                    }
                    else
                    {
                        Console.WriteLine($"Error: Currency code '{toCurrency}' not found.");
                        return value; // Returning the default value (0 in this case)
                    }
                }
                // Convert from another currency to EUR
                else if (toCurrency == "EUR")
                {
                    var sourceCurrency = currencyList.Find(c => c.CurrencyCode == fromCurrency);
                    if (sourceCurrency != null)
                    {
                        value = input / sourceCurrency.ExchangeRateFromEUR;
                    }
                    else
                    {
                        Console.WriteLine($"Error: Currency code '{fromCurrency}' not found.");
                        return value; // Returning the default value (0 in this case)
                    }
                }
                else
                {
                    // Convert from one currency to another (neither is EUR)
                    var sourceCurrency = currencyList.Find(c => c.CurrencyCode == fromCurrency);
                    var targetCurrency = currencyList.Find(c => c.CurrencyCode == toCurrency);

                    if (sourceCurrency != null && targetCurrency != null)
                    {
                        value = input / sourceCurrency.ExchangeRateFromEUR;
                        value *= targetCurrency.ExchangeRateFromEUR;
                    }
                    else
                    {
                        if (sourceCurrency == null)
                        {
                            Console.WriteLine($"Error: Currency code '{fromCurrency}' not found.");
                        }
                        if (targetCurrency == null)
                        {
                            Console.WriteLine($"Error: Currency code '{toCurrency}' not found.");
                        }
                        return value; // Returning the default value (0 in this case)
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle unexpected exceptions
                Console.WriteLine($"An error occurred during currency conversion: {ex.Message}");
            }

            return value;
        }

    }
}