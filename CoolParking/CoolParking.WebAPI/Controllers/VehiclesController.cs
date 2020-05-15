using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoolParking.BL.Interfaces;
using CoolParking.BL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoolParking.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehiclesController : ControllerBase
    {
        private readonly IParkingService _parking;
        public VehiclesController(IParkingService parking)
        {
            _parking = parking;
        }

        [HttpGet]
        public IEnumerable<Vehicle> GetVehicles()
        {
            return _parking.GetVehicles();
        }

        [HttpGet("{id}")]
        public ActionResult<Vehicle> GetVehicleById(string id)
        {
            Vehicle vehicle;

            try 
            {
                vehicle = _parking.GetVehicleById(id);
            }
            catch(ArgumentException e)
            {
                return BadRequest(e.Message);
            }
            if (vehicle == null)
            {
                return NotFound("Cannot find vehicle with this Id");
            }

            return vehicle;
        }

        [HttpPost]
        public IActionResult AddVehicle(Vehicle vehicle)
        {
            try
            {
                _parking.AddVehicle(vehicle);
            }
            catch(Exception e)
            {
                 return BadRequest(e.Message);
            }
            return CreatedAtAction(nameof(GetVehicleById), new {id = vehicle.Id}, vehicle);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteVehicle(string id)
        {
            if (!Vehicle.ValidateId(id)) return BadRequest($"{id} - is invalid type of Id");

            try
            {
                _parking.RemoveVehicle(id);
            }
            catch(ArgumentException e)
            {
                return NotFound(e.Message);
            }

            return NoContent();
        }

    }
}