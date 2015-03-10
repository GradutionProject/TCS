namespace TCS.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Location")]
    public partial class Location
    {
        public Location()
        {
            LocationSensors = new HashSet<LocationSensor>();
            LocationStatus = new HashSet<LocationStatu>();
            LocationStatusHistories = new HashSet<LocationStatusHistory>();
        }

        public string LocationId { get; set; }

        [Required]
        [StringLength(512)]
        public string Name { get; set; }

        public int SpatialReference { get; set; }

        public virtual ICollection<LocationSensor> LocationSensors { get; set; }

        public virtual ICollection<LocationStatu> LocationStatus { get; set; }

        public virtual ICollection<LocationStatusHistory> LocationStatusHistories { get; set; }
    }
}
