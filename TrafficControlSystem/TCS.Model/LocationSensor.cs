namespace TCS.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class LocationSensor
    {
        [Key]
        public string LocationSensorsId { get; set; }

        [Required]
        [StringLength(128)]
        public string LocationId { get; set; }

        [Required]
        [StringLength(128)]
        public string SensorId { get; set; }

        public bool InputOrOutput { get; set; }

        public virtual Location Location { get; set; }

        public virtual Sensor Sensor { get; set; }
    }
}
