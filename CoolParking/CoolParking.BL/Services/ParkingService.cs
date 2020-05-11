// TODO: implement the ParkingService class from the IParkingService interface.
//       For try to add a vehicle on full parking InvalidOperationException should be thrown.
//       For try to remove vehicle with a negative balance (debt) InvalidOperationException should be thrown.
//       Other validation rules and constructor format went from tests.
//       Other implementation details are up to you, they just have to match the interface requirements
//       and tests, for example, in ParkingServiceTests you can find the necessary constructor format and validation rules.
using CoolParking.BL.Interfaces;
using CoolParking.BL.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace CoolParking.BL.Services
{
    public class ParkingService : IParkingService
    {
        readonly ITimerService _withdrawTimer;
        readonly ITimerService _logTimer;
        readonly ILogService _logService;
        private readonly Parking parking;
        private const int SecToMilisec = 1000;
        public ParkingService(ITimerService withdrawTimer, ITimerService logTimer, ILogService logService)
        {
            parking = Parking.GetInstance();

            _withdrawTimer = withdrawTimer;
            _logTimer = logTimer;
            _logService = logService;

            _withdrawTimer.Interval = Settings.WithdrawTime*SecToMilisec;
            _withdrawTimer.Elapsed += Withdraw;
            _logTimer.Interval = Settings.LogTime*SecToMilisec;
            _logTimer.Elapsed += Log;

            _withdrawTimer.Start();
            _logTimer.Start();
        }

        private List<TransactionInfo> transactionInfos = new List<TransactionInfo>();
        private void Withdraw(Object source, System.Timers.ElapsedEventArgs e)
        {
            foreach (var vehicle in parking.Vehicles)
            {
                var tarif = Settings.tarifs[vehicle.VehicleType];
                decimal sum = 0.0m;
                if (vehicle.Balance <= 0)
                {
                    sum = tarif * Settings.PenaltyCoefficient;
                }
                else if (vehicle.Balance < tarif && vehicle.Balance > 0)
                {
                    decimal rest = tarif - vehicle.Balance;
                    sum = rest * Settings.PenaltyCoefficient + vehicle.Balance;
                }
                else
                {
                    sum = tarif;
                }

                vehicle.Balance -= sum;
                parking.Balance += sum;
                TransactionInfo transactionInfo = new TransactionInfo
                {
                    Sum = sum,
                    TransactionTime = DateTime.Now,
                    VehicleId = vehicle.Id
                };
                transactionInfos.Add(transactionInfo);
            }
            
        }
        private void Log(Object source, System.Timers.ElapsedEventArgs e)
        {
            string transactions = String.Join(", ", transactionInfos.Select(x => $"vehicleId: {x.VehicleId}, sum: {x.Sum}, transactionTime: {x.TransactionTime}"));
            _logService.Write(transactions);
            transactionInfos = new List<TransactionInfo>();
        }

        public void AddVehicle(Vehicle vehicle)
        {
            if (parking.Vehicles.Count == Settings.Capacity)
            {
                throw new InvalidOperationException();
            }

            if (parking.Vehicles.SingleOrDefault(x => x.Id == vehicle.Id) != null)
            {
                throw new ArgumentException();
            }

            parking.Vehicles.Add(vehicle);
        }

        public void Dispose()
        {
            _withdrawTimer.Stop();
            _withdrawTimer.Dispose();

            _logTimer.Stop();
            _logTimer.Dispose();

            parking.Vehicles = new List<Vehicle>();
            parking.Balance = 0;
        }

        public decimal GetBalance()
        {
            return parking.Balance;
        }

        public int GetCapacity()
        {
            return Settings.Capacity;
        }

        public int GetFreePlaces()
        {
            return Settings.Capacity - parking.Vehicles.Count();
        }

        public TransactionInfo[] GetLastParkingTransactions()
        {
            return transactionInfos.ToArray();
        }

        public ReadOnlyCollection<Vehicle> GetVehicles()
        {
            return parking.Vehicles.AsReadOnly();
        }

        public string ReadFromLog()
        {
            return _logService.Read();
        }

        public void RemoveVehicle(string vehicleId)
        {
            Vehicle vehicle = parking.Vehicles.SingleOrDefault(x => x.Id == vehicleId);
            if (vehicle == null) throw new ArgumentException();
            if (vehicle.Balance < 0) throw new InvalidOperationException();
            parking.Vehicles.Remove(vehicle);
        }

        public void TopUpVehicle(string vehicleId, decimal sum)
        {
            Vehicle vehicle = parking.Vehicles.SingleOrDefault(x => x.Id == vehicleId);
            if (vehicle == null || sum < 0) throw new ArgumentException();
            vehicle.Balance += sum;
        }
    }
}