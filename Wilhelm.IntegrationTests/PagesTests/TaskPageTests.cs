using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wilhelm.Backend.Services.Interfaces;
using Wilhelm.Backend.Services;
using Wilhelm.DataAccess;
using Wilhelm.Frontend.Services;
using Wilhelm.Frontend.Services.Interfaces;
using Wilhelm.Frontend.ViewModels.Pages;
using Wilhelm.Frontend.Model;

namespace Wilhelm.IntegrationTests.PagesTests
{
    [TestFixture]
    class TaskPageTests
    {
        IHoldersConversionService _hcs;
        IHoldersService _hs;
        IConfigurationService _cs;
        IWContextFactory _cf;
        IEntitiesService _es;
        IConversionService _convs;
        public TaskPageTests()
        {
            _hcs = new HoldersConversionService();
            _hs = new HoldersService(_hcs);

            _cf = new WContextFactory();
            _convs = new ConversionService();
            _es = new EntitiesService(_convs);
            _cs = new ConfigurationService(_cf, _es);
        }

        [SetUp]
        protected void SetUp()
        {
            Database.SetInitializer(new WilhelmTestContex2Initializer());
        }

        [Test]
        public void TaskPageActivateFunctionTest()
        {
            TaskPageViewModel tpvm = new TaskPageViewModel(_hs, _cs);
            int ownerId = -1;
            List<WTask> wtasks = new List<WTask>();
            using (WContext db = new WContext())
            {
                ownerId = db.Users.Where(x => x.Login == "user1").Single().Id;
                var cos = db.WTasks.ToList();
                wtasks = db.WTasks.Where(x => x.OwnerId == ownerId).OrderBy(x => x.Id).ToList();
            }
            tpvm.Activate(ownerId);
            var tasks = tpvm.Tasks.OrderBy(x => x.Id).ToList();
            if (tasks.Count != wtasks.Count)
                Assert.IsTrue(false);
            for (int i = 0; i < tasks.Count; i++)
            {
                if (!CompareTasks(tasks[i], wtasks[i]))
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
        private bool CompareTasks(TaskHolder th, WTask wt)
        {
            if (th.Archivized != wt.Archivized)
                return false;
            if (th.Description != wt.Description)
                return false;
            if (th.Frequency != wt.Frequency)
                return false;
            if (th.Id != wt.Id)
                return false;
            if (th.Name != wt.Name)
                return false;
            if (th.OwnerId != wt.OwnerId)
                return false;
            if (th.StartDate != wt.StartDate)
                return false;
            return true;
        }
    }

    public class WilhelmTestContex2Initializer : DropCreateDatabaseAlways<WContext>
    {
        protected override void Seed(WContext db)
        {
            WUser User1 = new WUser() { Login = "user1", Password = "Ἱꏁ\u2438ꥅ䫥쪋邳躮Ᏺ껫ꪉ⏺꼿ᆴ넿BD106B80630350E9B080DFB569CD0C337814169FA9350774ECB50AEB0164BD38" };
            db.Users.Add(User1);
            db.SaveChanges();

            WTask t1 = new WTask() { Name = "t1", OwnerId = User1.Id, Frequency = 1, StartDate = DateTime.Today };
            WTask t2 = new WTask() { Name = "t2", OwnerId = User1.Id, Frequency = 1, StartDate = DateTime.Today };
            WTask t3 = new WTask() { Name = "t3", OwnerId = User1.Id, Frequency = 2, StartDate = DateTime.Today };

            db.WTasks.Add(t1);
            db.WTasks.Add(t2);
            db.WTasks.Add(t3);

            db.SaveChanges();
        }
    }
}
