namespace TCS.Model
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class DataModel : DbContext
    {
        public DataModel()
            : base("name=DatabaseConnection")
        {
        }

        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<LocationSensor> LocationSensors { get; set; }
        public virtual DbSet<LocationStatu> LocationStatus { get; set; }
        public virtual DbSet<LocationStatusHistory> LocationStatusHistories { get; set; }
        public virtual DbSet<SensorData> SensorDatas { get; set; }
        public virtual DbSet<Sensor> Sensors { get; set; }
        public virtual DbSet<SensorType> SensorTypes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Location>()
                .HasMany(e => e.LocationSensors)
                .WithRequired(e => e.Location)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Location>()
                .HasMany(e => e.LocationStatus)
                .WithRequired(e => e.Location)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Location>()
                .HasMany(e => e.LocationStatusHistories)
                .WithRequired(e => e.Location)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SensorData>()
                .HasMany(e => e.LocationStatus)
                .WithRequired(e => e.SensorData)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SensorData>()
                .HasMany(e => e.LocationStatusHistories)
                .WithRequired(e => e.SensorData)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Sensor>()
                .Property(e => e.Longitude)
                .HasPrecision(38, 10);

            modelBuilder.Entity<Sensor>()
                .Property(e => e.Latitude)
                .HasPrecision(38, 10);

            modelBuilder.Entity<Sensor>()
                .HasMany(e => e.LocationSensors)
                .WithRequired(e => e.Sensor)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Sensor>()
                .HasMany(e => e.SensorDatas)
                .WithRequired(e => e.Sensor)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SensorType>()
                .HasMany(e => e.Sensors)
                .WithRequired(e => e.SensorType)
                .WillCascadeOnDelete(false);
        }
    }
}
