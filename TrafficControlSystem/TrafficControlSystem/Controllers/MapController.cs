using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using TCS.Model;
using TrafficControlSystem.Models;

namespace TrafficControlSystem.Controllers
{
    [Authorize(Roles= "IT")]
    public class MapController : Controller
    {
        public MapController() {
            var user = GetDbUser();
            if (user != null)
            {
                ViewBag.User = GetDbUser().Name;
            }
        }

        public User GetDbUser()
        {
            if (User != null)
            {
                if (User.Identity != null)
                {
                    DataModel db = new DataModel();
                    string userName = User.Identity.Name;
                    var dbUser = db.Users.FirstOrDefault(u => u.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase));
                    db.Dispose();
                    return dbUser;
                }
            }
            return null;
        }
        // GET: Map
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Sensors()
        {
            DataModel db = new DataModel();
            ViewBag.SensorTypes = db.SensorTypes.ToList();
            return View();
        }

        public ActionResult AllSensors()
        {
            DataModel db = new DataModel();

            var model = db.Sensors.OrderBy(s => s.Name).ToList();
            return View(model);
        }

        public ActionResult Locations()
        {
            return View(new Location());
        }

        public ActionResult AllLocations()
        {
            DataModel db = new DataModel();
            LocationsViewModel model = new LocationsViewModel();
            model.Locations = db.Locations
                .Include(l => l.LocationSensors)
                .Include(l => l.LocationSensors.Select(ls => ls.Sensor))
                .OrderBy(l => l.Name).ToList();
            model.Sensors = db.Sensors.OrderBy(l => l.Name).ToList();
            return View(model);
        }
    }
}