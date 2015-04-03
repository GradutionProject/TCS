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

namespace TrafficControlSystem.Controllers.Services
{
    public class LocationsController : ApiController
    {
        private DataModel db = new DataModel();

        [HttpGet]
        [ResponseType(typeof(Location))]
        public IHttpActionResult All()
        {
            return Ok(db.Locations.Include(l => l.LocationSensors).Include(l => l.LocationSensors.Select(ls => ls.Sensor)).ToList());
        }

        // GET: api/Locations/5
        [ResponseType(typeof(Location))]
        public IHttpActionResult GetLocation(string id)
        {
            Location location = db.Locations.Find(id);
            if (location == null)
            {
                return NotFound();
            }

            return Ok(location);
        }

        // PUT: api/Locations/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutLocation(string id, Location location)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != location.LocationId)
            {
                return BadRequest();
            }

            db.Entry(location).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LocationExists(id))
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

        [HttpPost]
        [ResponseType(typeof(Location))]
        public IHttpActionResult Add(Location location)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Locations.Add(location);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (LocationExists(location.LocationId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = location.LocationId }, location);
        }

        [HttpGet]
        [ResponseType(typeof(Location))]
        public IHttpActionResult Delete(string id)
        {
            Location location = db.Locations.Include(l => l.LocationSensors).FirstOrDefault(l => l.LocationId.Equals(id, StringComparison.OrdinalIgnoreCase));
            if (location == null)
            {
                return NotFound();
            }
            List<LocationSensor> removedLocSenors = new List<LocationSensor>();
            foreach (var locSensor in location.LocationSensors)
            {
                removedLocSenors.Add(locSensor);
            }
            foreach (var locSensor in removedLocSenors)
            {
                db.LocationSensors.Remove(locSensor);
            }
            db.Locations.Remove(location);
            db.SaveChanges();

            return Ok(location);
        }

        [HttpGet]
        [ResponseType(typeof(Location))]
        public IHttpActionResult AssignSensor(string locationId, string sensorId, bool input)
        {
            Location location = db.Locations
                .Include(l => l.LocationSensors)
                .Include(l => l.LocationSensors.Select(ls => ls.Sensor))
                .FirstOrDefault(l => l.LocationId.Equals(locationId, StringComparison.OrdinalIgnoreCase));

            if (location == null)
            {
                return NotFound();
            }

            if (location.LocationSensors.Any(ls => ls.SensorId.Equals(sensorId, StringComparison.OrdinalIgnoreCase)))
            {
                var locationSensor = location.LocationSensors.FirstOrDefault(ls => ls.SensorId.Equals(sensorId, StringComparison.OrdinalIgnoreCase));
                locationSensor.InputOrOutput = input;
                db.Entry(locationSensor).State = EntityState.Modified;
                db.SaveChanges();
            }
            else
            {
                var locationSensor = new LocationSensor();
                locationSensor.LocationId = locationId;
                locationSensor.SensorId = sensorId;
                locationSensor.InputOrOutput = input;
                locationSensor.Sensor = db.Sensors.FirstOrDefault(s => s.SensorId.Equals(sensorId, StringComparison.OrdinalIgnoreCase));
                db.LocationSensors.Add(locationSensor);
                location.LocationSensors = location.LocationSensors ?? new List<LocationSensor>();
                location.LocationSensors.Add(locationSensor);
                db.SaveChanges();
            }


            return Ok(location);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool LocationExists(string id)
        {
            return db.Locations.Count(e => e.LocationId == id) > 0;
        }
    }
}