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
                Random random = new Random(1);
                WGroup g1 = new WGroup() { Name = "Group1", Description = "Animals" };
                WGroup g2 = new WGroup() { Name = "Group2", Description = "Plants" };

                WTask t1 = new WTask() { Name = "Feed the cat", Description = "Royal Canin", Frequency = 1, StartDate = DateTime.Today };
                WTask t2 = new WTask() { Name = "Feed the dog", Frequency = 1, StartDate = DateTime.Today };
                WTask t3 = new WTask() { Name = "Water plant1", Frequency = 3, StartDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day + 1) };
                WTask t4 = new WTask() { Name = "water Maciek", Frequency = 1, StartDate = DateTime.Today };
                WTask t5 = new WTask() { Name = "give insect to Maciek", Frequency = 30, StartDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day + 5) };

                List<WActivity> activities = new List<WActivity>();
                for (int i = 0; i <7; i++)
                    activities.Add(new WActivity() { WTask = t1, Date = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day - i), IsDone = random.Next() % 2 == 0 });
                for (int i = 0; i < 14; i++)
                    activities.Add(new WActivity() { WTask = t2, Date = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day - i), IsDone = random.Next() % 2 == 0 });
                for (int i = 0; i < 3; i++)
                    activities.Add(new WActivity() { WTask = t3, Date = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day - i * t3.Frequency), IsDone = random.Next() % 2 == 0 });
                for (int i = 0; i < 5; i++)
                    activities.Add(new WActivity() { WTask = t4, Date = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day - i * t4.Frequency), IsDone = random.Next() % 2 == 0 });
                for (int i = 0; i < 8; i++)
                    activities.Add(new WActivity() { WTask = t5, Date = new DateTime(DateTime.Today.Year, DateTime.Today.Month-i, DateTime.Today.Day), IsDone = random.Next() % 2 == 0 });

                activities.Sort((x, y) => DateTime.Compare(x.Date, y.Date));
                foreach (WActivity a in activities)
                    db.WActivities.Add(a);

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

                db.SaveChanges();
            }

        }
    }
}
