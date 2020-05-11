using CoolParking.BL.Models;
using CoolParking.BL.Services;
using System;
using System.Linq;

namespace CoolParking
{
    class Program
    {
        const string path = "Transactions.log";
        
        static void Main(string[] args)
        {
            TimerService withdrawTimer = new TimerService();
            TimerService logTimer = new TimerService();
            LogService logService = new LogService(path);
            ParkingService parking = new ParkingService(withdrawTimer, logTimer, logService );
            bool showMenu = true;
            while (showMenu)
            {
                showMenu = MainMenu(parking);
            }

            Console.ReadLine();
        }

        private static bool MainMenu(ParkingService parking)
        {
            Console.WriteLine("Choose an option:");
            Console.WriteLine("1) Show parking balance");
            Console.WriteLine("2) Show received money for current period");
            Console.WriteLine("3) Show free places");
            Console.WriteLine("4) Show transactions for current period");
            Console.WriteLine("5) Show history of transaction");
            Console.WriteLine("6) Show list of vehicles");
            Console.WriteLine("7) Add a car to the parking");
            Console.WriteLine("8) Remove car with id");                                      
            Console.WriteLine("9) Add money to the car balance");

            Console.WriteLine("10) Exit");
            Console.Write("\r\nSelect an option: ");

            switch (Console.ReadLine())
            {
                case "1":
                    Console.WriteLine($"Current parking balance: {parking.GetBalance()}");
                    return true;
                case "2":
                    var currentBalance = parking.GetLastParkingTransactions().Sum(x => x.Sum);
                    Console.WriteLine($"Received money for current period: {currentBalance}");
                    return true;
                case "3":
                    Console.WriteLine($"Number of free places: {parking.GetFreePlaces()}");
                    return true;
                case "4":
                    Console.WriteLine($"History of transactions:");
                    var transactions = parking.GetLastParkingTransactions();
                    foreach(var transaction in transactions)
                    {
                        Console.WriteLine($"vehicleId: {transaction.VehicleId}, sum: {transaction.Sum}, transactionTime: {transaction.TransactionTime}");
                    }
                    return true;
                case "5":
                    Console.WriteLine("Transaction log:");
                    Console.WriteLine(parking.ReadFromLog());
                    return true;
                case "6":
                    Console.WriteLine("List of vehicles:");
                    var vehicles = parking.GetVehicles();
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
                        parking.AddVehicle(new Vehicle(id, type, balance));
                    }
                    catch (ArgumentException)
                    {
                        Console.WriteLine($"Error, vehicle with id:{id} and alance:{balance} can't be added");
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
                        parking.RemoveVehicle(id);
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
                        parking.TopUpVehicle(id, sum);
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
