using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TCS.Model;

namespace TCS.SensorSimulator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start Sensor Simulator....");
            while (true)
            {
                DataModel db = new DataModel();
                var sensors = db.Sensors.Include(s => s.LocationSensors)
                    .Include(s => s.LocationSensors.Select(sl => sl.Location))
                    .Include(s => s.LocationSensors.Select(sl => sl.Location.LocationStatus))
                    .Include(s => s.LocationSensors.Select(sl => sl.Location.LocationStatusHistories))
                    .ToList();
                var random = new Random();
                var sensor = sensors[random.Next(0, sensors.Count - 1)];
                SensorData data = new SensorData() { 
                    SensorId = sensor.SensorId,
                    Time = DateTime.Now
                };
                Console.WriteLine(string.Format("Add new snsor data for sensor: {0}", sensor.Name));
                db.SensorDatas.Add(data);
                foreach (var location in sensor.LocationSensors)
                {
                    location.Location.LocationStatus = location.Location.LocationStatus ?? new List<LocationStatu>();
                    location.Location.LocationStatusHistories = location.Location.LocationStatusHistories ?? new List<LocationStatusHistory>();
                    var status = location.Location.LocationStatus.FirstOrDefault();

                    Console.WriteLine(string.Format("It will affect location: {0} as {1}", location.Location.Name, location.InputOrOutput ? "Input" : "Output"));
                    if (status == null)
                    {
                        status = new LocationStatu();
                        status.LocationId = location.LocationId;
                        status.SensorDataId = data.SensorDataId;
                        status.Count = location.InputOrOutput ? 1 : 0;
                        location.Location.LocationStatus.Add(status);
                    }
                    else
                    {
                        status.SensorDataId = data.SensorDataId;
                        status.SensorData = data;
                        status.Count = location.InputOrOutput  ? status.Count + 1 : (status.Count > 0 ? status.Count -1 : 0); 
                    }
                    var history = new LocationStatusHistory() { 
                        Count = status.Count,
                        Location = location.Location,
                        LocationId = location.LocationId,
                        SensorData = data,
                        SensorDataId= data.SensorDataId,
                        Time=data.Time
                    };
                    location.Location.LocationStatusHistories.Add(history);
                }
                db.SaveChanges();
                Thread.Sleep(1000);
            }
        }
    }
}
