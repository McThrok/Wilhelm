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

        [SetUp]
        protected void SetUp()
        {
            Database.SetInitializer(new TaskPageTestContexInitializer());
        }

        [Test]
        public void HomePageSaveActivityTest()
        {
            HomePageViewModel hpvm = new HomePageViewModel(_hs, _ps);
            int ownerId = -1;
            using (WContext db = new WContext())
            {
                ownerId = db.Users.Where(x => x.Login == "user1").Single().Id;
            }
            hpvm.Activate(ownerId);
            hpvm.CurrentList[0].IsDone = true;
            hpvm.Save();
            bool result = false;
            using (WContext db = new WContext())
            {
                if (db.WActivities.First().IsDone)
                    result = true;
            }
            Assert.IsTrue(result);
        }

        [Test]
        public void HomePageActivateArchivizedActivitiesTest()
        {
            HomePageViewModel hpvm = new HomePageViewModel(_hs, _ps);
            int ownerId = -1;
            using (WContext db = new WContext())
            {
                ownerId = db.Users.Where(x => x.Login == "user1").Single().Id;
            }
            hpvm.Activate(ownerId);
            foreach (var el in hpvm.CurrentList)
            {
                if (el.Task.Archivized)
                    Assert.IsTrue(false);
            }
            Assert.IsTrue(true);
        }

        [TearDown]
        public void TearDown()
        {
            using (var db = new WContext())
            {
                db.Database.Delete();
            }
        }
    }

    public class WilhelmHomePageTestContexInitializer : DropCreateDatabaseAlways<WContext>
    {
        protected override void Seed(WContext db)
        {
            WUser User1 = new WUser() { Login = "user1", Password = "Ἱꏁ\u2438ꥅ䫥쪋邳躮Ᏺ껫ꪉ⏺꼿ᆴ넿BD106B80630350E9B080DFB569CD0C337814169FA9350774ECB50AEB0164BD38" };
            db.Users.Add(User1);
            db.SaveChanges();

            WTask t1 = new WTask() { Name = "t1", OwnerId = User1.Id, Frequency = 1, StartDate = DateTime.Today };
            WActivity a1 = new WActivity() { WTask = t1, Date = DateTime.Today, IsDone = false };

            WTask t2 = new WTask() { Name = "t2", OwnerId = User1.Id, Frequency = 1, StartDate = DateTime.Today, Archivized = true };
            WActivity a2 = new WActivity() { WTask = t1, Date = DateTime.Today, IsDone = false };

            db.WActivities.Add(a1);
            db.WTasks.Add(t1);

            db.SaveChanges();
        }
    }
}
