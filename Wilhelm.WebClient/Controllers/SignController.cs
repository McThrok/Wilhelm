using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Wilhelm.WebClient.Controllers
{
    public class SignController : Controller
    {
        public ActionResult SignInPage()
        {
            return View();
        }
        public ActionResult SignUpPage()
        {
            return View();
        }
    }
}