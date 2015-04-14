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
    public class LocationsController : ApiController
    {
        private DataModel db = new DataModel();

        [HttpGet]
        [ResponseType(typeof(Location))]
        public IHttpActionResult All()
        {
            return Ok(db.Locations.ToList());
        }

        [HttpGet]
        [ResponseType(typeof(Location))]
        public GeoLocationCollection Geo()
        {
            var locations = db.Locations
                .Include(l => l.LocationStatus)
                .Include(l => l.LocationSensors)
                .Include(l => l.LocationSensors.Select(ls => ls.Sensor))
                .ToList();
            GeoLocationCollection collection = new GeoLocationCollection();
            collection.Features = locations.Where(l => l.LocationSensors.Count > 0).Select(s => new GeoLocation(s)).ToList();
            return collection;
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
            Location location = db.Locations
                .Include(l => l.LocationSensors)
                .Include(l => l.LocationStatus)
                .Include(l => l.LocationStatusHistories)
                .FirstOrDefault(l => l.LocationId.Equals(id, StringComparison.OrdinalIgnoreCase));
            if (location == null)
            {
                return NotFound();
            }
            List<LocationSensor> removedLocSenors = new List<LocationSensor>();
            List<LocationStatusHistory> removeHistory = new List<LocationStatusHistory>();
            foreach (var locSensor in location.LocationSensors)
            {
                removedLocSenors.Add(locSensor);
            }
            foreach (var history in location.LocationStatusHistories)
            {
                removeHistory.Add(history);
            }
            foreach (var locSensor in removedLocSenors)
            {
                db.LocationSensors.Remove(locSensor);
            }
            foreach (var history in removeHistory)
            {
                db.LocationStatusHistories.Remove(history);
            }
            var status = location.LocationStatus.FirstOrDefault();
            if (status != null)
            {
                db.LocationStatus.Remove(location.LocationStatus.FirstOrDefault());
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
                int maxOrder = 1;
                if (location.LocationSensors.Any())
                {
                    maxOrder = location.LocationSensors.Max(ls => ls.Order) + 1;
                }
                locationSensor.Order = maxOrder;
                db.LocationSensors.Add(locationSensor);
                location.LocationSensors = location.LocationSensors ?? new List<LocationSensor>();
                location.LocationSensors.Add(locationSensor);
                db.SaveChanges();
            }


            return Ok(location);
        }

        [HttpGet]
        [ResponseType(typeof(Location))]
        public IHttpActionResult UnassignSensor(string locationSensorId)
        {
            LocationSensor locSensor = db.LocationSensors.FirstOrDefault(ls => ls.LocationSensorsId.Equals(locationSensorId, StringComparison.OrdinalIgnoreCase));
            if (locSensor == null)
            {
                return NotFound();
            }
            Location location = db.Locations
                .Include(l => l.LocationSensors)
                .FirstOrDefault(l => l.LocationId.Equals(locSensor.LocationId, StringComparison.OrdinalIgnoreCase));

            if (location != null)
            {
                var assignedLocSensors = location.LocationSensors.FirstOrDefault(ls => ls.LocationSensorsId.Equals(locSensor.LocationSensorsId, StringComparison.OrdinalIgnoreCase));
                location.LocationSensors.Remove(assignedLocSensors);
            }
            db.LocationSensors.Remove(locSensor);
            db.SaveChanges();
            return Ok(location);
        }
        [HttpGet]
        [ResponseType(typeof(Location))]
        public IHttpActionResult MoveUpSensor(string locationSensorId)
        {
            LocationSensor locSensor = db.LocationSensors
                .Include(ls => ls.Location)
                .Include(ls => ls.Location.LocationSensors)
                .FirstOrDefault(ls => ls.LocationSensorsId.Equals(locationSensorId, StringComparison.OrdinalIgnoreCase));
            if (locSensor == null)
            {
                return NotFound();
            }
          
            if (locSensor.Location != null)
            {
                var currentOrder = locSensor.Order;
                var prevLocSensor = locSensor.Location.LocationSensors
                    .Where(ls => ls.Order < currentOrder)
                    .OrderByDescending(ls => ls.Order).FirstOrDefault();
                if (prevLocSensor != null)
                {
                    locSensor.Order = prevLocSensor.Order;
                    prevLocSensor.Order = currentOrder;
                    db.SaveChanges();
                }
            }

            locSensor.Location.LocationSensors = locSensor.Location.LocationSensors.OrderBy(ls => ls.Order).ToList();
            return Ok(locSensor.Location);
        }

        [HttpGet]
        [ResponseType(typeof(Location))]
        public IHttpActionResult MoveDownSensor(string locationSensorId)
        {
            LocationSensor locSensor = db.LocationSensors
                .Include(ls => ls.Location)
                .Include(ls => ls.Location.LocationSensors)
                .FirstOrDefault(ls => ls.LocationSensorsId.Equals(locationSensorId, StringComparison.OrdinalIgnoreCase));
            if (locSensor == null)
            {
                return NotFound();
            }

            if (locSensor.Location != null)
            {
                var currentOrder = locSensor.Order;
                var nextLocSensor = locSensor.Location.LocationSensors
                    .Where(ls => ls.Order > currentOrder)
                    .OrderBy(ls => ls.Order).FirstOrDefault();
                if (nextLocSensor != null)
                {
                    locSensor.Order = nextLocSensor.Order;
                    nextLocSensor.Order = currentOrder;
                    db.SaveChanges();
                }
            }
            locSensor.Location.LocationSensors = locSensor.Location.LocationSensors.OrderBy(ls => ls.Order).ToList();
            return Ok(locSensor.Location);
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