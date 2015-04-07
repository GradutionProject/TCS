using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using TCS.Model;

namespace TrafficControlSystem.Models
{
    [DataContract]
    public class GeoLocation
    {
        public GeoLocation(Location location)
        {
            Type = "Feature";
            Geometry = new LocationGeometry()
            {
                Coordinates = new List<List<List<decimal>>>(),
                Type = "Polygon"
            };

            var paths = new List<List<decimal>>();
            foreach (var locSensor in location.LocationSensors.OrderBy(ls => ls.Order))
            {
                paths.Add(new List<decimal> { locSensor.Sensor.Longitude, locSensor.Sensor.Latitude });
            }
            if (location.LocationSensors.Count > 0)
            {
                var locSensor = location.LocationSensors.OrderBy(ls => ls.Order).First();
                paths.Add(new List<decimal> { locSensor.Sensor.Longitude, locSensor.Sensor.Latitude });
            }
            Geometry.Coordinates.Add(paths);
            Properties = new LocationProperties()
            {
                Count =  location.LocationStatus.Count > 0 ? location.LocationStatus.FirstOrDefault().Count : 0,
                Id = location.LocationId,
                Name = location.Name
            };
        }
        [DataMember(Name = "type")]
        public string Type { get; set; }
        [DataMember(Name = "geometry")]
        public LocationGeometry Geometry { get; set; }
        [DataMember(Name = "properties")]
        public LocationProperties Properties { get; set; }
    }
    [DataContract]
    public class LocationGeometry
    {
        [DataMember(Name = "type")]
        public string Type { get; set; }
        [DataMember(Name = "coordinates")]
        public List<List<List<decimal>>> Coordinates { get; set; }
    }
    [DataContract]
    public class LocationProperties
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "count")]
        public int Count { get; set; }
        [DataMember(Name = "id")]
        public string Id { get; set; }
    }
}