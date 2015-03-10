namespace TCS.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SensorType")]
    public partial class SensorType
    {
        public SensorType()
        {
            Sensors = new HashSet<Sensor>();
        }

        [Key]
        public string TypeId { get; set; }

        [Required]
        [StringLength(512)]
        [Display(Name="Sensor Type")]
        public string Name { get; set; }

        public virtual ICollection<Sensor> Sensors { get; set; }
    }
}
