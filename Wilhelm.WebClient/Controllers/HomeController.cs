using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Wilhelm.WebClient.Controllers
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


        public ActionResult Home()
        {
            return View();
        }
        public ActionResult Tasks()
        {
            return View();
        }
        public ActionResult Groups()
        {
            return View();
        }
        public ActionResult Archive()
        {
            return View();
        }
        public ActionResult Reports()
        {
            return View();
        }
    }
}