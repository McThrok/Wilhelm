using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wilhelm.DataAccess
{
    public class Class1
    {
        static void Main(string[] args)
        {
            Database.SetInitializer(new WilhelmContexInitializer());
            using (WilhelmContext db = new WilhelmContext())
            {
                Task t1 = new Task() { Name = "bb", Frequency = 1, StartDate = DateTime.Today };
                db.Tasks.Add(t1);
                db.SaveChanges();
            }
        }
    }

    [Table("Tasks")]
    public class Task
    {
        public Task()
        {
            Group = new List<Group>();
        }

        public int TaskId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public int Frequency { get; set; }

        public List<Group> Group { get; set; }
    }

    [Table("Groups")]
    public class Group
    {
        public Group()
        {
            Tasks = new List<Task>();
        }

        public int GroupId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }

        public List<Task> Tasks { get; set; }
    }

    [Table("Activities")]
    public class Activity
    {
        public int ActivityId { get; set; }
        [Required]
        public Task Task { get; set; }
        [Required]
        public DateTime Date { get; set; }
        public bool Status { get; set; }
    }
    public class WilhelmContext : DbContext
    {
        public WilhelmContext()
        {
        }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Activity> Activities { get; set; }
    }

    //public class WilhelmContexInitializer : DropCreateDatabaseIfModelChanges<WilhelmContext>
    public class WilhelmContexInitializer : DropCreateDatabaseAlways<WilhelmContext>
    {
        protected override void Seed(WilhelmContext db)
        {
            Group g1 = new Group() { Name = "Group1", Description = "Animals" };
            Group g2 = new Group() { Name = "Group2", Description = "Plants" };

            Task t1 = new Task() { Name = "Feed the cat", Description = "Royal Canin", Frequency = 1, StartDate = DateTime.Today };
            Task t2 = new Task() { Name = "Feed the dog", Frequency = 1, StartDate = DateTime.Today };
            Task t3 = new Task() { Name = "Water plant1", Frequency = 3, StartDate = DateTime.Today };
            Task t4 = new Task() { Name = "water Maciek", Frequency = 1, StartDate = DateTime.Today };
            Task t5 = new Task() { Name = "give insect to Maciek", Frequency = 20, StartDate = DateTime.Today };

            Activity a1 = new Activity() { Task = t1, Date = DateTime.Today, Status = true };
            Activity a2 = new Activity() { Task = t1, Date = DateTime.Now, Status = false };
            Activity a3 = new Activity() { Task = t1, Date = DateTime.UtcNow, Status = true };

            g1.Tasks.Add(t1);
            g1.Tasks.Add(t2);
            g1.Tasks.Add(t5);

            g2.Tasks.Add(t3);
            g2.Tasks.Add(t4);

            t1.Group.Add(g1);
            t2.Group.Add(g1);
            t3.Group.Add(g2);
            t4.Group.Add(g2);
            t5.Group.Add(g1);
            t5.Group.Add(g2);

            db.Groups.Add(g1);
            db.Groups.Add(g2);

            db.Tasks.Add(t1);
            db.Tasks.Add(t2);
            db.Tasks.Add(t3);
            db.Tasks.Add(t5);

            db.Activities.Add(a1);
            db.Activities.Add(a2);
            db.Activities.Add(a3);

            db.SaveChanges();
        }
    }
}
