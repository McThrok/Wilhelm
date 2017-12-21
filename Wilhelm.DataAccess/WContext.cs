using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wilhelm.DataAccess
{
    public class WContext : DbContext, IWContext
    {
        public WContext() : base("Wilhelm")
        {
            
            Database.SetInitializer<WContext>(new DropCreateDatabaseIfModelChanges<WContext>());
        }

        public IDbSet<WTask> WTasks { get; set; }
        public IDbSet<WGroup> WGroups { get; set; }
        public IDbSet<WActivity> WActivities { get; set; }
        public IDbSet<WUser> Users { get; set; }
    }

    //public class WilhelmContexInitializer : DropCreateDatabaseIfModelChanges<WilhelmContext>
    public class WilhelmContexInitializer : DropCreateDatabaseAlways<WContext>
    {
        protected override void Seed(WContext db)
        {
            WGroup g1 = new WGroup() { Name = "Group1", Description = "Animals" };
            WGroup g2 = new WGroup() { Name = "Group2", Description = "Plants" };

            WTask t1 = new WTask() { Name = "Feed the cat", Description = "Royal Canin", Frequency = 1, StartDate = DateTime.Today };
            WTask t2 = new WTask() { Name = "Feed the dog", Frequency = 1, StartDate = DateTime.Today };
            WTask t3 = new WTask() { Name = "Water plant1", Frequency = 3, StartDate = DateTime.Today };
            WTask t4 = new WTask() { Name = "water Maciek", Frequency = 1, StartDate = DateTime.Today };
            WTask t5 = new WTask() { Name = "give insect to Maciek", Frequency = 20, StartDate = DateTime.Today };

            WActivity a1 = new WActivity() { WTask = t1, Date = DateTime.Today, IsDone = true };
            WActivity a2 = new WActivity() { WTask = t1, Date = DateTime.Now, IsDone = false };
            WActivity a3 = new WActivity() { WTask = t1, Date = DateTime.UtcNow, IsDone = true };

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
