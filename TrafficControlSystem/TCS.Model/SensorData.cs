namespace TCS.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SensorData")]
    public partial class SensorData
    {
        public SensorData()
        {
            LocationStatus = new HashSet<LocationStatu>();
            LocationStatusHistories = new HashSet<LocationStatusHistory>();
            SensorDataId = Guid.NewGuid().ToString();
        }

        public string SensorDataId { get; set; }

        public DateTime Time { get; set; }

        [Required]
        [StringLength(128)]
        public string SensorId { get; set; }

        public virtual ICollection<LocationStatu> LocationStatus { get; set; }

        public virtual ICollection<LocationStatusHistory> LocationStatusHistories { get; set; }

        public virtual Sensor Sensor { get; set; }
    }
}
