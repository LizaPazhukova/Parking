// TODO: implement class Parking.
//       Implementation details are up to you, they just have to meet the requirements 
//       of the home task and be consistent with other classes and tests.
using System;
using System.Collections.Generic;

namespace CoolParking.BL.Models
{
    public class Parking
    {
        public decimal Balance { get; set; }
        public List<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
        private Parking() { }
      
        private static readonly Lazy<Parking> lazy = new Lazy<Parking>(() => new Parking());
        public static Parking GetInstance { get { return lazy.Value; } }
    }
}
