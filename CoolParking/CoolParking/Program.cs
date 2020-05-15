using CoolParking.BL.Models;
using CoolParking.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace CoolParking
{
    class Program
    {        
        static void Main(string[] args)
        {

            bool showMenu = true;
            while (showMenu)
            {
                showMenu = MainMenu();
            }

            Console.ReadLine();
        }

        static HttpClient client = new HttpClient();

        private static bool MainMenu()
        {
            Console.WriteLine("Choose an option:");
            Console.WriteLine("1) Show parking balance");
            Console.WriteLine("2) Show received money for current period");
            Console.WriteLine("3) Show free places");
            Console.WriteLine("4) Show transactions for current period");
            Console.WriteLine("5) Show history of transactions");
            Console.WriteLine("6) Show list of vehicles");
            Console.WriteLine("7) Add a car to the parking");
            Console.WriteLine("8) Remove car with id");                                      
            Console.WriteLine("9) Add money to the car balance");

            Console.WriteLine("10) Exit");
            Console.Write("\r\nSelect an option: ");

            HttpResponseMessage response;

            switch (Console.ReadLine())
            {
                case "1":
                    response = client.GetAsync("https://localhost:44341/api/parking/balance").Result;
                    Console.WriteLine($"Current parking balance: {response.Content.ReadAsStringAsync().Result}");
                    return true;
                case "2": 
                    response = client.GetAsync("https://localhost:44341/api/transactions/last").Result;
                    var currentBalance = response.Content.ReadAsAsync<TransactionInfo[]>().Result.Sum(x=>x.Sum);
                    Console.WriteLine($"Received money for current period: {currentBalance}");
                    return true;
                case "3":
                    response = client.GetAsync("https://localhost:44341/api/parking/freePlaces").Result;
                    Console.WriteLine($"Number of free places: {response.Content.ReadAsStringAsync().Result}");
                    return true;
                case "4":
                    Console.WriteLine($"History of transactions:");
                    response = client.GetAsync("https://localhost:44341/api/transactions/last").Result;
                    var transactions = response.Content.ReadAsAsync<TransactionInfo[]>().Result;
                    foreach (var transaction in transactions)
                    {
                        Console.WriteLine($"vehicleId: {transaction.VehicleId}, sum: {transaction.Sum}, transactionTime: {transaction.TransactionTime}");
                    }
                    return true;
                case "5":
                    Console.WriteLine("Transaction log:");
                    response = client.GetAsync("https://localhost:44341/api/transactions/all").Result;
                    Console.WriteLine(response.Content.ReadAsStringAsync().Result);
                    return true;
                case "6":
                    Console.WriteLine("List of vehicles:");
                    response = client.GetAsync("https://localhost:44341/api/vehicles").Result;
                    var vehicles = response.Content.ReadAsAsync<IEnumerable<Vehicle>>().Result;
                    foreach (var vehicle in vehicles)
                    {
                        Console.WriteLine($"vehicleId: {vehicle.Id}, balance: {vehicle.Balance}, type: {vehicle.VehicleType}");
                    }
                    return true;
                case "7":
                    Console.WriteLine("Add new vehicle:");
                    Console.WriteLine("Input vehicle id:");
                    string id = Console.ReadLine();
                    Console.WriteLine("Input balance:");
                    decimal balance;
                    if (!Decimal.TryParse(Console.ReadLine(), out balance))
                    {
                        Console.WriteLine("Incorrect input");
                        return true;
                    }
                    VehicleType type;
                    Console.WriteLine("Input vehicle type: 1-Passenger car, 2- Truck, 3-Bus, 4-Motorcycle");
                    switch (Console.ReadLine())
                    {
                        case "1":
                            type = VehicleType.PassengerCar;
                            break;
                        case "2":
                            type = VehicleType.Truck;
                            break;
                        case "3":
                            type = VehicleType.Bus;
                            break;
                        case "4":
                            type = VehicleType.Motorcycle;
                            break;
                        default:
                            Console.WriteLine("Incorrect input");
                            return true;
                    }

                    try
                    {
                        response = client.PostAsJsonAsync("https://localhost:44341/api/vehicles", new Vehicle(id, type, balance)).Result;
                    }
                    catch (ArgumentException)
                    {
                        Console.WriteLine($"Error, vehicle with id:{id} and balance:{balance} can't be added");
                        return true;
                    }
                    Console.WriteLine($"Vehicle with id: {id} was added to the parking");
                    return true;
                case "8":
                    Console.WriteLine("Remove vehicle:");
                    Console.WriteLine("Input vehicle id:");
                    id = Console.ReadLine();
                    try
                    {
                        response = client.DeleteAsync($"https://localhost:44341/api/vehicles/{id}").Result;
                    }
                    catch (ArgumentException)
                    {
                        Console.WriteLine($"Error, vehicle with id:{id} not exists");
                        return true;
                    }
                    Console.WriteLine("Vehicle was removed from parking");
                    return true;
                case "9":
                    Console.WriteLine("Add money to the balance:");
                    Console.WriteLine("Input vehicle id:");
                    id = Console.ReadLine();
                    Console.WriteLine("Input sum:");
                    decimal sum;
                    if(!Decimal.TryParse(Console.ReadLine(), out sum))
                    {
                        Console.WriteLine("Incorrect input");
                        return true;
                    }
                    try
                    {
                        var topUpVehicle = new TopUpVehicle()
                        {
                            Id = id,
                            Sum = sum
                        };
                        response = client.PutAsJsonAsync("https://localhost:44341/api/transactions/topUpVehicle", topUpVehicle).Result;
                    }
                    catch (ArgumentException)
                    {
                        Console.WriteLine($"Error, vehicle with id:{id} not exists");
                        return true;
                    }
                    Console.WriteLine("Balance was succesfully added");
                    return true;
                case "10":
                    return false;
                default:
                    return true;
            }
        }
    }
}
