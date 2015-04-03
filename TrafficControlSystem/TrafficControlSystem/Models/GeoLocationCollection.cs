using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace TrafficControlSystem.Models
{
    [DataContract]
    public class GeoLocationCollection
    {
        [DataMember(Name="type")]
        public string Type { get { return "FeatureCollection"; } set { } }

        [DataMember(Name="features")]
        public List<GeoLocation> Features { get; set; }
    }
}