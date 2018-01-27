using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Wilhelm.Client.Services;
using Wilhelm.Shared.Dto;

namespace Wilhelm.WebClient.Controllers
{
    public class PagesController : Controller
    {
        private ProxyService _proxy = new ProxyService();

        public async Task<ActionResult> HomePage(int userId)
        {
            ViewBag.UserId = userId;
            ViewBag.Name = (await _proxy.GetUser(userId))?.Login;
            ViewBag.Activities = await _proxy.GetTodaysTasks(userId);
            ViewBag.JsonActivities = JsonConvert.SerializeObject(ViewBag.Activities);
            return View();
        }

        public async Task<ActionResult> TasksPage(int userId)
        {
            ViewBag.UserId = userId;
            ViewBag.Name = (await _proxy.GetUser(userId))?.Login;
            return View();
        }
        public async Task<ActionResult> GroupsPage(int userId)
        {
            ViewBag.UserId = userId;
            ViewBag.Name = (await _proxy.GetUser(userId))?.Login;
            return View();
        }
        public async Task<ActionResult> ArchivePage(int userId)
        {
            ViewBag.UserId = userId;
            ViewBag.Name = (await _proxy.GetUser(userId))?.Login;
            return View();
        }
        public async Task<ActionResult> ReportPage(int userId)
        {
            ViewBag.UserId = userId;
            ViewBag.Name = (await _proxy.GetUser(userId))?.Login;
            ViewBag.Reports = await _proxy.GetReports(userId);
            return View();
        }

    }
}