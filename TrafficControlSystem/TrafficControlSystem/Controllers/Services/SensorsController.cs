using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using TCS.Model;
using TrafficControlSystem.Models;

namespace TrafficControlSystem.Controllers.Services
{
    public class SensorsController : ApiController
    {
        private DataModel db = new DataModel();

        [HttpGet]
        public IEnumerable<Sensor> All()
        {
            return db.Sensors.ToList();
        }

        [HttpGet]
        public GeoSensorCollection Geo()
        {
            var sensors = db.Sensors.ToList();
            GeoSensorCollection collection = new GeoSensorCollection();
            collection.Features =sensors.Select(s => new GeoSensor(s)).ToList();
            return collection;
        }
        // GET: api/Sensors/5
        [ResponseType(typeof(Sensor))]
        public IHttpActionResult GetSensor(string id)
        {
            Sensor sensor = db.Sensors.Find(id);
            if (sensor == null)
            {
                return NotFound();
            }

            return Ok(sensor);
        }

        // PUT: api/Sensors/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutSensor(string id, Sensor sensor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != sensor.SensorId)
            {
                return BadRequest();
            }

            db.Entry(sensor).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SensorExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Sensors
        [ResponseType(typeof(Sensor))]
        public IHttpActionResult Add(Sensor sensor)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if(db.Sensors.Any(s => s.Name.Equals(sensor.Name,StringComparison.OrdinalIgnoreCase)))
                {
                    throw new Exception("The sensor name already exist.");
                }
                db.Sensors.Add(sensor);

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateException)
                {
                    if (SensorExists(sensor.SensorId))
                    {
                        return Conflict();
                    }
                    else
                    {
                        throw;
                    }
                }

                return CreatedAtRoute("DefaultApi", new { id = sensor.SensorId }, sensor);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        public IHttpActionResult Delete(string id)
        {
            try
            {
                Sensor sensor = db.Sensors.Include(s => s.SensorDatas).FirstOrDefault(s => s.SensorId.Equals(id, StringComparison.OrdinalIgnoreCase));
                if (sensor == null)
                {
                    return NotFound();
                }
                if (db.LocationSensors.Any(ls => ls.SensorId.Equals(sensor.SensorId, StringComparison.OrdinalIgnoreCase)))
                {
                    throw new Exception("Can't delete sensor assigned to location.");
                }
                List<SensorData> removedSensorData = new List<SensorData>();
                foreach (var sensorData in sensor.SensorDatas)
                {
                    removedSensorData.Add(sensorData);
                }
                foreach (var sensorData in removedSensorData)
                {
                    db.SensorDatas.Remove(sensorData);
                }
                db.Sensors.Remove(sensor);
                db.SaveChanges();

                return Ok(sensor);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SensorExists(string id)
        {
            return db.Sensors.Count(e => e.SensorId == id) > 0;
        }
    }
}