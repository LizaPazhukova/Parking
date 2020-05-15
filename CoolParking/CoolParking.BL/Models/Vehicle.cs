// TODO: implement class Vehicle.
//       Properties: Id (string), VehicleType (VehicleType), Balance (decimal).
//       The format of the identifier is explained in the description of the home task.
//       Id and VehicleType should not be able for changing.
//       The Balance should be able to change only in the CoolParking.BL project.
//       The type of constructor is shown in the tests and the constructor should have a validation, which also is clear from the tests.
//       Static method GenerateRandomRegistrationPlateNumber should return a randomly generated unique identifier.
using Fare;
using System;
using System.Text.RegularExpressions;

namespace CoolParking.BL.Models
{
    public class Vehicle
    {
        public string Id { get; set; }
        public VehicleType VehicleType { get; set; }
        public decimal Balance { get; set; }

        static Regex regex = new Regex(@"[A-Z]{2}\-[0-9]{4}\-[A-Z]{2}");
        public Vehicle() { }

        public Vehicle(string id, VehicleType vehicleType, decimal balance)
        {
            if (balance < 0 || !regex.IsMatch(id)) throw new ArgumentException();
            Id = id;
            VehicleType = vehicleType;
            Balance = balance;
        }
        public static string GenerateRandomRegistrationPlateNumber()
        {
            string regexString = "[A-Z]{2}-[0-9]{4}-[A-Z]{2}";
            Xeger generator = new Xeger(regexString);
            string result = generator.Generate();
            return result;
        }
        public static bool ValidateId(string id)
        {
            return regex.IsMatch(id);
        }
    }
}
