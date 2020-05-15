using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoolParking.BL.Interfaces;
using CoolParking.BL.Models;
using CoolParking.WebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoolParking.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        
        private readonly IParkingService _parking;
        public TransactionsController(IParkingService parking)
        {
            _parking = parking;
        }

        [HttpGet("last")]
        public TransactionInfo[] GetLastTransaction()
        {
            return _parking.GetLastParkingTransactions();
        }
        [HttpGet("all")]
        public ActionResult<string> GetLog()
        {
            string log;
            try
            {
               log = _parking.ReadFromLog();
            }
            catch(InvalidOperationException)
            {
                return NotFound("Cannot find log file");
            }
            return log;
        }
        [HttpPut("topUpVehicle")]
        public ActionResult<Vehicle> TopUpVehicle(TopUpVehicle topUpVehicle)
        {
            if (!Vehicle.ValidateId(topUpVehicle.Id) || topUpVehicle.Sum < 0) return BadRequest("Incorrect id or sum < 0");
            try
            {
                _parking.TopUpVehicle(topUpVehicle.Id, topUpVehicle.Sum);
            }
            catch(ArgumentException e)
            {
                return NotFound(e.Message);
            }
            var vehicle = _parking.GetVehicleById(topUpVehicle.Id);
            return vehicle;
        }
    }
}