using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using AssetTracking.Assets;

namespace AssetTracking.Services
{
    public class AssetTrackingService
    {
        private List<Device> devices;
        private const string CsvFilePath = "devices.csv";

        public AssetTrackingService()
        {
            devices = new List<Device>();

            if (File.Exists(CsvFilePath))
            {
                LoadDevicesFromCsv();
            }
            else
            {
                AddSampleDevices();
            }
        }

        // Method to load devices from a CSV file with error handling
        private void LoadDevicesFromCsv()
        {
            try
            {
                var lines = File.ReadAllLines(CsvFilePath, Encoding.UTF8);
                foreach (var line in lines.Skip(1))
                {
                    var parts = line.Split(',');
                    if (parts.Length == 7)
                    {
                        string type = parts[0];
                        string brand = parts[1];
                        string model = parts[2];
                        string office = parts[3];
                        string purchaseDate = parts[4];
                        double price = Convert.ToDouble(parts[5]);
                        string currency = parts[6];

                        Device newDevice = type.ToLower() == "phone"
                            ? new Phone(brand, model, office, purchaseDate, price, currency)
                            : new Computer(brand, model, office, purchaseDate, price, currency);

                        devices.Add(newDevice);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading devices from CSV: {ex.Message}");
            }
        }

        // Method to save devices to a CSV file with error handling
        public void SaveDevicesToCsv()
        {
            try
            {
                var csvLines = new StringBuilder();
                csvLines.AppendLine("Type,Brand,Model,Office,Purchase Date,Price in USD,Currency");
                foreach (var device in devices)
                {
                    csvLines.AppendLine($"{device.Type},{device.Brand},{device.Model},{device.Office},{device.PurchaseDate.ToShortDateString()},{device.PriceInUSD},{device.Currency}");
                }
                File.WriteAllText(CsvFilePath, csvLines.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving devices to CSV: {ex.Message}");
            }
        }

        // Method to add sample devices
        private void AddSampleDevices()
        {
            try
            {
                devices.Add(new Phone("iPhone", "8", "Spain", "12/29/2018", 970, "EUR"));
                devices.Add(new Computer("HP", "Elitebook", "Spain", "6/1/2019", 1423, "EUR"));
                devices.Add(new Phone("iPhone", "11", "Spain", "9/25/2020", 909, "EUR"));
                devices.Add(new Phone("iPhone", "X", "Sweden", "9/21/2020", 855, "SEK"));
                devices.Add(new Phone("Samsung", "S22", "Sweden", "9/05/2024", 678, "SEK"));
                devices.Add(new Computer("HP", "Elitebook", "Sweden", "9/20/2023", 900, "SEK"));
                devices.Add(new Computer("MacBook", "Pro", "USA", "9/07/2024", 1100, "USD"));
                devices.Add(new Computer("HP", "W234", "USA", "1/25/2021", 655, "USD"));
                devices.Add(new Computer("Lenovo", "Yoga 530", "USA", "4/11/2022", 845, "USD"));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding sample devices: {ex.Message}");
            }
        }

        // Method to get all devices
        public List<Device> GetAllDevices()
        {
            return devices;
        }

        // Method to display all devices
        public void DisplayDevices()
        {
            try
            {
                Console.WriteLine("{0,-10} {1,-10} {2,-10} {3,-10} {4,-14} {5,-14} {6,-10} {7,-15}",
                    "Type", "Brand", "Model", "Office", "Purchase Date", "Price in USD", "Currency", "Local price today");

                var sortedDevices = devices
                    .OrderBy(d => d.Office)
                    .ThenByDescending(d => d.PurchaseDate)
                    .ToList();

                foreach (var device in sortedDevices)
                {
                    device.CalculateLocalPrice();
                    string lifeStatus = device.GetLifeStatus();

                    ConsoleColor color = lifeStatus switch
                    {
                        "RED" => ConsoleColor.Red,
                        "YELLOW" => ConsoleColor.Yellow,
                        _ => ConsoleColor.White
                    };

                    Console.ForegroundColor = color;

                    Console.WriteLine($"{device.Type,-10} {device.Brand,-10} {device.Model,-10} {device.Office,-10} {device.PurchaseDate.ToShortDateString(),-14} {device.PriceInUSD,-14} {device.Currency,-10} {device.LocalPriceToday,-15}");
                    Console.ResetColor();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error displaying devices: {ex.Message}");
            }
        }


        // Method to add a new device
        public void AddNewDevice(Device device)
        {
            try
            {
                devices.Add(device);
                SaveDevicesToCsv();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding new device: {ex.Message}");
            }
        }

        // Method to delete a device
        public void DeleteDevice(int index)
        {
            try
            {
                if (index >= 0 && index < devices.Count)
                {
                    devices.RemoveAt(index);
                    SaveDevicesToCsv();
                }
                else
                {
                    Console.WriteLine("Invalid device index.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting device: {ex.Message}");
            }
        }

        // Method to search for a device by brand
        public List<Device> SearchDeviceByBrand(string brand)
        {
            try
            {
                return devices.Where(d => d.Brand.Contains(brand, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error searching devices: {ex.Message}");
                return new List<Device>();
            }
        }
    }
}