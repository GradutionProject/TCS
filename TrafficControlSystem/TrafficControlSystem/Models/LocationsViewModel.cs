using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TCS.Model;

namespace TrafficControlSystem.Models
{
    public class LocationsViewModel
    {

        public IEnumerable<Location> Locations { get; set; }

        public IEnumerable<Sensor> Sensors { get; set; }
    }
}