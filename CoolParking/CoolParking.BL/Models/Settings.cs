// TODO: implement class Settings.
//       Implementation details are up to you, they just have to meet the requirements of the home task.
using System.Collections.Generic;

namespace CoolParking.BL.Models
{
    public static class Settings
    {
        public static  Dictionary<VehicleType, decimal> tarifs = new Dictionary<VehicleType, decimal>
        {
            { VehicleType.PassengerCar, 2},
            { VehicleType.Truck, 5},
            {VehicleType.Bus, 3.5M },
            {VehicleType.Motorcycle, 1 }

        };
 
        public const decimal StartBalance = 0;
        public const int Capacity = 10;
        public const int WithdrawTime = 5;
        public const int LogTime = 60;
        public const decimal PenaltyCoefficient = 2.5M;
    }
}
