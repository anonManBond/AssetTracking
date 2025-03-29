using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AssetTracking.Assets;
using AssetTracking.ExchangeRates;
using AssetTracking.Services;


namespace AssetTracking
{
    class Program
    {
        static void Main()
        {
            var assetTrackingService = new AssetTrackingService();

            // Fetch live exchange rates from the ECB API
            LiveCurrency.FetchRates();

            /*
            // Add sample data before user input
            AddSampleDevices(assetTrackingService);
            */

            // Main application loop
            while (true)
            {
                Console.Clear();
                // Display the devices with their converted prices
                assetTrackingService.DisplayDevices();

                // Menu options
                Console.WriteLine("\nOptions:");
                Console.WriteLine("1. Add new product");
                Console.WriteLine("2. Delete a product");
                Console.WriteLine("3. Search a product");
                Console.WriteLine("4. Exit");

                // Get user's choice using switch-case
                Console.Write("Enter your choice: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddNewDevice(assetTrackingService);
                        break;
                    case "2":
                        DeleteDevice(assetTrackingService);
                        break;
                    case "3":
                        SearchDeviceByBrand(assetTrackingService);
                        break;
                    case "4":
                        Console.WriteLine("Exiting program...");
                        return;
                    default:
                        Console.WriteLine("Invalid option, please try again.");
                        Console.ReadKey();
                        break;
                }
            }
        }

        // Method to add a new device
        static void AddNewDevice(AssetTrackingService assetTrackingService)
        {
            try
            {
                Console.WriteLine("\nEnter the details of the new device:");

                Console.Write("Type (e.g., Phone, Computer): ");
                string type = Console.ReadLine();

                Console.Write("Brand (e.g., iPhone, HP, Lenovo): ");
                string brand = Console.ReadLine();

                Console.Write("Model (e.g., 8, Elitebook, Yoga 730): ");
                string model = Console.ReadLine();

                Console.Write("Office (e.g., Spain, USA, Sweden): ");
                string office = Console.ReadLine();

                Console.Write("Purchase Date (e.g., 12/29/2018): ");
                string purchaseDate = Console.ReadLine();

                Console.Write("Price in USD: ");
                double priceInUSD = Convert.ToDouble(Console.ReadLine());

                Console.Write("Currency (e.g., EUR, SEK, USD): ");
                string currency = Console.ReadLine();

                // Create the appropriate device (Phone or Computer)
                Device newDevice = type.ToLower() switch
                {
                    "phone" => new Phone(brand, model, office, purchaseDate, priceInUSD, currency),
                    "computer" => new Computer(brand, model, office, purchaseDate, priceInUSD, currency),
                    _ => throw new ArgumentException("Invalid device type.")
                };

                assetTrackingService.AddNewDevice(newDevice);

                Console.WriteLine("Device added successfully.");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding new device: {ex.Message}");
                Console.ReadKey();
            }
        }

        // Method to delete a device
        static void DeleteDevice(AssetTrackingService assetTrackingService)
        {
            try
            {
                Console.WriteLine("\nEnter the index of the device to delete:");

                var devices = assetTrackingService.GetAllDevices();
                for (int i = 0; i < devices.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {devices[i].Brand} {devices[i].Model} - {devices[i].Currency}");
                }

                Console.Write("Enter the number of the device to delete: ");
                int deviceIndex = Convert.ToInt32(Console.ReadLine()) - 1;

                assetTrackingService.DeleteDevice(deviceIndex);

                Console.WriteLine("Device deleted successfully.");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting device: {ex.Message}");
                Console.ReadKey();
            }
        }

        // Method to search for a device by brand
        static void SearchDeviceByBrand(AssetTrackingService assetTrackingService)
        {
            try
            {
                Console.Write("\nEnter the brand of the product you want to search: ");
                string searchBrand = Console.ReadLine();

                // Call the search method from AssetTrackingService
                var foundDevices = assetTrackingService.SearchDeviceByBrand(searchBrand);

                if (foundDevices.Any())
                {
                    Console.WriteLine($"\nDevices found for brand '{searchBrand}':");
                    foreach (var device in foundDevices)
                    {
                        Console.WriteLine($"{device.Type} {device.Brand} {device.Model} - {device.Currency} {device.LocalPriceToday}");
                    }
                }
                else
                {
                    Console.WriteLine($"No devices found for brand '{searchBrand}'.");
                }
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error searching devices: {ex.Message}");
                Console.ReadKey();
            }
        }

        // Add sample data before user input (1 new device gets added each time running the program)
        /*
        static void AddSampleDevices(AssetTrackingService assetTrackingService)
        {
            try
            {
                assetTrackingService.AddNewDevice(new Phone("iPhone", "8", "Spain", "12/29/2018", 970, "EUR"));

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding sample devices: {ex.Message}");
            }
        }
        */
    }
}

