namespace TCS.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class LocationStatu
    {
        [Key]
        public string LocationStatusId { get; set; }

        public int Count { get; set; }
        
        [Required]
        [StringLength(128)]
        public string LocationId { get; set; }

        [Required]
        [StringLength(128)]
        public string SensorDataId { get; set; }

        public virtual Location Location { get; set; }

        public virtual SensorData SensorData { get; set; }
    }
}
