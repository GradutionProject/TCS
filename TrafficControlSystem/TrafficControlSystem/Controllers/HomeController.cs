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
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Map()
        {
            return View();
        }

        public ActionResult Sensors()
        {
            DataModel db = new DataModel();
            ViewBag.SensorTypes =  db.SensorTypes.ToList();
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