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

        // GET: api/Locations
        public IQueryable<Location> GetLocations()
        {
            return db.Locations;
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
            Location location = db.Locations.Find(id);
            if (location == null)
            {
                return NotFound();
            }

            db.Locations.Remove(location);
            db.SaveChanges();

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