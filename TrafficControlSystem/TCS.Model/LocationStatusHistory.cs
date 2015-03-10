namespace TCS.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("LocationStatusHistory")]
    public partial class LocationStatusHistory
    {
        public string LocationStatusHistoryId { get; set; }

        [Required]
        [StringLength(128)]
        public string LocationId { get; set; }

        public int Count { get; set; }

        [Required]
        [StringLength(128)]
        public string SensorDataId { get; set; }

        public DateTime Time { get; set; }

        public virtual Location Location { get; set; }

        public virtual SensorData SensorData { get; set; }
    }
}
