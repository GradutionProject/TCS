namespace TCS.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Sensor
    {
        public Sensor()
        {
            LocationSensors = new HashSet<LocationSensor>();
            SensorDatas = new HashSet<SensorData>();
            SensorId = Guid.NewGuid().ToString();
        }

        public string SensorId { get; set; }

        [Required]
        [StringLength(512)]
        public string Name { get; set; }

        [Required]
        [StringLength(128)]
        public string TypeId { get; set; }

        public decimal Longitude { get; set; }

        public decimal Latitude { get; set; }

        public int SpatialReference { get; set; }

        public virtual ICollection<LocationSensor> LocationSensors { get; set; }

        public virtual ICollection<SensorData> SensorDatas { get; set; }

        public virtual SensorType SensorType { get; set; }
    }
}
