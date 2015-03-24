using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TCS.Model;

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
            DataModel db = new DataModel();
            return View(new Location());
        }

        public ActionResult AllLocations()
        {
            DataModel db = new DataModel();
            var model = db.Locations.OrderBy(l => l.Name).ToList();
            return View(model);
        }
    }
}