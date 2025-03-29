using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace AssetTracking.ExchangeRates
{
    // Root class representing the envelope structure
    [XmlRoot("Envelope")]
    public class Envelope
    {
        [XmlElement("Cube")]
        public Cube Cube { get; set; }
    }

    // Class representing the Cube structure
    public class Cube
    {
        [XmlElement("Cube")]
        public List<CurrencyCube> Cube1 { get; set; }
    }

    // Class representing a single currency and its rate
    public class CurrencyCube
    {
        [XmlAttribute("currency")]
        public string Currency { get; set; }

        [XmlAttribute("rate")]
        public decimal Rate { get; set; }
    }
}