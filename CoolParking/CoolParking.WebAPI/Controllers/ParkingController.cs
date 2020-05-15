using CoolParking.BL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CoolParking.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParkingController : ControllerBase
    {
        private readonly IParkingService _parking;
        public ParkingController(IParkingService parking)
        {
            _parking = parking;
        }

        [HttpGet("balance")]
        public decimal GetBalance()
        {
            return _parking.GetBalance();
        }

        [HttpGet("capacity")]
        public int GetCapacity()
        {
            return _parking.GetCapacity();
        }

        [HttpGet("freePlaces")]
        public int GetFreePlaces()
        {
            return _parking.GetFreePlaces();
        }
    }
}