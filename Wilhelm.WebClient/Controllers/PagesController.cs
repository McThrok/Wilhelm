﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Wilhelm.Client.Services;

namespace Wilhelm.WebClient.Controllers
{
    public class PagesController : Controller
    {
        private ProxyService _proxy = new ProxyService();

        public async Task<ActionResult> HomePage()
        {
            ViewBag.Activities = await _proxy.GetTodaysTasks(1);
            return View();
        }
        public ActionResult TasksPage()
        {
            return View();
        }
        public ActionResult GroupsPage()
        {
            return View();
        }
        public ActionResult ArchivePage()
        {
            return View();
        }
        public ActionResult ReportPage()
        {
            return View();
        }

    }
}