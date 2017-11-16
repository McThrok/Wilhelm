using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wilhelm.Backend;
using Wilhelm.DataAccess;

namespace Wilhelm.IntegrationTests
{
    class DbConnectionTests
    {
        [Test]
        public void DbConnectionTest()
        {
            using (var db = new WContext())
            {
                db.WActivities.ToList();
                db.Database.Delete();
            }
        }
    }

    [TestFixture]
    class BackendDataAccessTests
    {
        [SetUp]
        protected void SetUp()
        {
            Database.SetInitializer(new WilhelmTestContexInitializer());
        }


        [Test]
        public void AutoAddIdTest()
        {
            WTask t1 = new WTask() { Id = 0, Name = nameof(AutoAddIdTest), Description = "T1", Frequency = 1, StartDate = DateTime.Today };
            using (WContext db = new WContext())
            {
                db.WTasks.Add(t1);
                db.SaveChanges();
            }

            WTask dbTask;
            using (WContext db = new WContext())
            {
                dbTask = db.WTasks.Where(x => x.Name == nameof(AutoAddIdTest)).SingleOrDefault();
            }
            Assert.AreNotEqual(0, dbTask.Id);
        }

        [Test]
        public void GetActivityWithNotNullTask()
        {
            WTask t1 = new WTask() { Id = 1234,Name="t1", Frequency = 1, StartDate = new DateTime(2017,10,10) };
            WActivity a1 = new WActivity() {Id = 5123, WTask = t1, Date = new DateTime(2017, 10, 10) };

            using (WContext db = new WContext())
            {
                db.WActivities.Add(a1);
                db.SaveChanges();
            }

            bool resut = false;
            using (WContext db = new WContext())
            {
                var dbActivity = db.WActivities.Include(a => a.WTask).Where(x => x.Id == a1.Id).SingleOrDefault();
                if (dbActivity != null && dbActivity.WTask != null)
                    resut = true;
            }
            Assert.IsTrue(resut);

        }
        [Test]
        public void GetGroupWithNotNullTask()
        {
            WTask t1 = new WTask() { Id = 12345, Name = "t1", Frequency = 1, StartDate = new DateTime(2017, 10, 10) };
            WGroup g1 = new WGroup() {Id = 45678, Name = "g1", WTasks = new List<WTask>() { t1 }};
            t1.WGroups = new List<WGroup>() { g1 };

            using (WContext db = new WContext())
            {
                db.WGroups.Add(g1);
                db.SaveChanges();
            }

            bool resut = false;
            using (WContext db = new WContext())
            {
                var dbActivity = db.WGroups.Include(a => a.WTasks).Where(x => x.Id == g1.Id).SingleOrDefault();
                if (dbActivity != null && dbActivity.WTasks != null && dbActivity.WTasks.Count == 1)
                    resut = true;
            }
            Assert.IsTrue(resut);
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

    public class WilhelmTestContexInitializer : DropCreateDatabaseAlways<WContext>
    {
        protected override void Seed(WContext db)
        {
            WGroup g1 = new WGroup() { Name = "Group1", Description = "Animals" };
            WGroup g2 = new WGroup() { Name = "Group2", Description = "Plants" };

            WTask t1 = new WTask() { Name = "Feed the cat", Description = "Royal Canin", Frequency = 1, StartDate = DateTime.Today };
            WTask t2 = new WTask() { Name = "Feed the dog", Frequency = 1, StartDate = new DateTime(2017, 12, 01) };
            WTask t3 = new WTask() { Name = "Water plant1", Frequency = 3, StartDate = new DateTime(2017, 12, 02) };
            WTask t4 = new WTask() { Name = "water Maciek", Frequency = 1, StartDate = new DateTime(2017, 12, 03) };
            WTask t5 = new WTask() { Name = "give insect to Maciek", Frequency = 20, StartDate = new DateTime(2017, 12, 01) };

            WActivity a1 = new WActivity() { WTask = t1, Date = DateTime.Today, IsDone = true };
            WActivity a2 = new WActivity() { WTask = t1, Date = new DateTime(2017, 12, 03), IsDone = false };
            WActivity a3 = new WActivity() { WTask = t1, Date = new DateTime(2017, 12, 03), IsDone = true };

            g1.WTasks.Add(t1);
            g1.WTasks.Add(t2);
            g1.WTasks.Add(t5);

            g2.WTasks.Add(t3);
            g2.WTasks.Add(t4);

            t1.WGroups.Add(g1);
            t2.WGroups.Add(g1);
            t3.WGroups.Add(g2);
            t4.WGroups.Add(g2);
            t5.WGroups.Add(g1);
            t5.WGroups.Add(g2);

            db.WGroups.Add(g1);
            db.WGroups.Add(g2);

            db.WTasks.Add(t1);
            db.WTasks.Add(t2);
            db.WTasks.Add(t3);
            db.WTasks.Add(t5);

            db.WActivities.Add(a1);
            db.WActivities.Add(a2);
            db.WActivities.Add(a3);

            db.SaveChanges();
        }
    }
}