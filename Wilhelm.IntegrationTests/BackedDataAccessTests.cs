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
    [TestFixture]
    class BackendDataAccessTests
    {
        [SetUp]
        protected void SetUp()
        {
            Database.SetInitializer(new WilhelmTestContexInitializer());
        }

        [TestCase]
        public void AddTaskToDB_ReturnsCorrectId()
        {
            DataAccessIntegration c = new DataAccessIntegration(new WContextFactory());
            WTask t1 = new WTask() { Name = "T1", Description = "T1", Frequency = 1, StartDate = DateTime.Today };
            c.SaveTask(t1);

            WTask dbTask;
            using (WContext db = new WContext())
            {
                dbTask = db.WTasks.Where((t) => (t.Name == t1.Name) && (t.Description == t1.Description) && (t.Frequency == t1.Frequency)).Single();
            }
            Assert.AreEqual(t1.Id, dbTask.Id);
        }

        [Test]
        public void GetTodayActiovities_ReturnsTodayActivities()
        {
            DataAccessIntegration c = new DataAccessIntegration(new WContextFactory());
            var activities = c.GetTodayActivities();

            List<WActivity> allActivities = new List<WActivity>();
            using (WContext db = new WContext())
            {
                allActivities = db.WActivities.ToList();
            }

           // var bbb = activities.Intersect(allActivities, (a)=>a.);
            //var aaa = activities.Intersect(allActivities).Count();
            var hasAll = activities.Intersect(allActivities).Count() == activities.Count();
            Assert.IsTrue(hasAll);
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
            WTask t2 = new WTask() { Name = "Feed the dog", Frequency = 1, StartDate = new DateTime(2017,12,01) };
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