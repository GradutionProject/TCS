using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using TCS.Model;

namespace TrafficControlSystem.Models
{
    [DataContract]
    public class GeoSensor
    {
        public GeoSensor(Sensor sensor)
        {
            Geometry = new SensorGeometry()
            {
                Type = "Point",
                Coordinates = new List<decimal> { sensor.Longitude, sensor.Latitude }
            };
            Properties = new SensorProperties()
            {
                Name = sensor.Name,
                Id = sensor.SensorId
            };
        }
        [DataMember(Name="type")]
        public string Type { get { return "Feature"; } set { } }

        [DataMember(Name = "geometry")]
        public SensorGeometry Geometry { get; set; }
        [DataMember(Name = "properties")]
        public SensorProperties Properties { get; set; }

    }
    [DataContract]
    public class SensorGeometry
    {
        [DataMember(Name = "type")]
        public string Type { get; set; }
        [DataMember(Name = "coordinates")]
        public List<decimal> Coordinates { get; set; }
    }
    [DataContract]
    public class SensorProperties
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "id")]
        public string Id { get; set; }
    }
}