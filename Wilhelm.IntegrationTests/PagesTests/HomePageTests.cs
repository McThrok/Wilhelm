using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wilhelm.Backend.Services;
using Wilhelm.Backend.Services.Interfaces;
using Wilhelm.DataAccess;
using Wilhelm.Client.Services;
using Wilhelm.Client.Services.Interfaces;
using Wilhelm.Client.ViewModels.Pages;

namespace Wilhelm.IntegrationTests.PagesTests
{
    class HomePageTests
    {
        IHoldersService _hs;
        IProxyService _ps;
        public HomePageTests()
        {
            _hs = new HoldersService(new HoldersConversionService());
            _ps = new ProxyService();
        }

        [Test]
        public async Task HomePageSaveActivityTest()
        {
            HomePageViewModel hpvm = new HomePageViewModel(_hs, _ps);
            int ownerId = -1;
            using (WContext db = new WContext())
            {
                var a = db.Users.ToList();
                ownerId = db.Users.Where(x => x.Login == "user1").Single().Id;
            }
            await hpvm.Activate(ownerId);
            hpvm.CurrentList[0].IsDone = true;
            await hpvm.Save();
            bool result = false;
            using (WContext db = new WContext())
            {
                var a = db.WActivities.First();
                if (db.WActivities.First().IsDone)
                    result = true;
            }
            Assert.IsTrue(result);
        }

        [Test]
        public async Task HomePageActivateArchivizedActivitiesTest()
        {
            HomePageViewModel hpvm = new HomePageViewModel(_hs, _ps);
            int ownerId = -1;
            using (WContext db = new WContext())
            {
                ownerId = db.Users.Where(x => x.Login == "user1").Single().Id;
            }
            await hpvm.Activate(ownerId);
            foreach (var el in hpvm.CurrentList)
            {
                if (el.Task.Archivized)
                    Assert.IsTrue(false);
            }
            Assert.IsTrue(true);
        }
    }



}
