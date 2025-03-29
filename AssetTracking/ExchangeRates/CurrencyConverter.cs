using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Xml;
using AssetTracking.ExchangeRates;

namespace AssetTracking.ExchangeRates
{
    static class CurrencyConverter
    {
        static private string xmlUrl = "https://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml";
        static Envelope envelope = CurrencyConverter.Update();

        static public decimal ConvertTo(decimal value, string currency, out decimal newValue)
        {
            newValue = -1;

            foreach (var cube in envelope.Cube.Cube1)
            {
                if (cube.Currency == "USD")
                {
                    newValue = value / cube.Rate;
                    break;
                }
            }

            if (currency == "EUR")
            {
                return newValue;
            }

            foreach (var cube in envelope.Cube.Cube1)
            {
                if (cube.Currency == currency)
                {
                    newValue *= cube.Rate;
                    break;
                }
            }

            return newValue;
        }

        static public Envelope Update()
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Envelope));
                XmlReader xmlReader = XmlReader.Create(xmlUrl);

                using (xmlReader)
                {
                    envelope = (Envelope)(serializer.Deserialize(xmlReader));
                }

                return envelope;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new Envelope();
            }
        }
    }
}