using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wilhelm.DataAccess;

namespace Wilhelm.MainDataSet
{
    public class DataSetInitializer
    {
        static void Main(string[] args)
        {
            var initializer = new DataSetInitializer();
            initializer.Clean();
            initializer.Init();
            Console.WriteLine("Done");
        }
        public void Clean()
        {
            using (var db = new WContext())
            {
                foreach (var item in db.WTasks.ToList())
                    db.WTasks.Remove(item);

                foreach (var item in db.WGroups.ToList())
                    db.WGroups.Remove(item);

                foreach (var item in db.WActivities.ToList())
                    db.WActivities.Remove(item);
                db.SaveChanges();
            }
        }
        public void Init()
        {
            using (var db = new WContext())
            {

                WGroup g1 = new WGroup() { Name = "Group1", Description = "Animals" };
                WGroup g2 = new WGroup() { Name = "Group2", Description = "Plants" };

                WTask t1 = new WTask() { Name = "Feed the cat", Description = "Royal Canin", Frequency = 1, StartDate = DateTime.Today };
                WTask t2 = new WTask() { Name = "Feed the dog", Frequency = 1, StartDate = new DateTime(2017, 12, 01) };
                WTask t3 = new WTask() { Name = "Water plant1", Frequency = 3, StartDate = new DateTime(2017, 12, 02) };
                WTask t4 = new WTask() { Name = "water Maciek", Frequency = 1, StartDate = new DateTime(2017, 12, 03) };
                WTask t5 = new WTask() { Name = "give insect to Maciek", Frequency = 20, StartDate = new DateTime(2017, 12, 01) };

                WActivity a1 = new WActivity() { WTask = t1, Date = DateTime.Today, IsDone = false };
                WActivity a2 = new WActivity() { WTask = t1, Date = new DateTime(2017, 12, 03), IsDone = false };
                WActivity a3 = new WActivity() { WTask = t1, Date = new DateTime(2017, 12, 03), IsDone = true };
                //t1.Activities = new List<WActivity>() { a1, a2, a3 };

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
}
