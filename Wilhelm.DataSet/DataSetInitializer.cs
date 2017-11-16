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
            Console.WriteLine("Cleaning data..");
            initializer.Clean();
            Console.WriteLine("Loading data..");
            initializer.Init();
            Console.WriteLine("Database is ready.");
        }
        public void Clean()
        {
            using (var db = new WContext())
            {
                db.Database.Delete();
            }
        }
        public void Init()
        {
            using (var db = new WContext())
            {
                Random random = new Random(1);
                WGroup g1 = new WGroup() { Name = "Zwierzęta", Description = "wszystkie moje domowe zwierzaki" };
                WGroup g2 = new WGroup() { Name = "Rośliny", Description = "wszystkie rośliny doniczkowe" };
                WGroup g3 = new WGroup() { Name = "Finanse" };
                WGroup g4 = new WGroup() { Name = "Ważne", Description = "Lepiej o tym nie zapominać" };


                WTask t1 = new WTask() { Name = "Nakarmić kota", Description = "Tom jest wybredny i je tylko Royal Canin", Frequency = 1, StartDate = DateTime.Today };
                WTask t2 = new WTask() { Name = "Nakarmić psa", Description = "Burek zje wszystko", Frequency = 1, StartDate = DateTime.Today };
                WTask t3 = new WTask() { Name = "Nakarmić rybki", Description = "Jeśli pływają brzuszkiem do góry to można przestać karmić", Frequency = 2, StartDate = DateTime.Today };
                WTask t4 = new WTask() { Name = "Wymienić wodę rybkom", Frequency = 14, StartDate = DateTime.Today };

                WTask t5 = new WTask() { Name = "Podlać kwiaty", Description = "Wszytkie oprócz rosiczki", Frequency = 3, StartDate = DateTime.Today };
                WTask t6 = new WTask() { Name = "Podlać rosiczkę", Description = "Rosiczka jest mięsożerna, ale można i trzeba ją regularnie podlewać", Frequency = 1, StartDate = DateTime.Today };

                WTask t7 = new WTask() { Name = "Zapłacić czynsz", Description = "1000zl plus ewentualne dopłaty", Frequency = 30, StartDate = DateTime.Today };
                WTask t8 = new WTask() { Name = "Zapłacić za internet", Description = "60 plus ewentualne dopłaty", Frequency = 30, StartDate = DateTime.Today };

                db.WTasks.Add(t1);
                db.WTasks.Add(t2);
                db.WTasks.Add(t3);
                db.WTasks.Add(t5);
                db.WTasks.Add(t6);
                db.WTasks.Add(t7);
                db.WTasks.Add(t8);

                db.WGroups.Add(g1);
                db.WGroups.Add(g2);
                db.WGroups.Add(g3);
                db.WGroups.Add(g4);

                db.SaveChanges();

                foreach (var task in db.WTasks)
                    foreach (var activity in GenerateActivities(random, task))
                        db.WActivities.Add(activity);

                Link(g1, t1);
                Link(g1, t2);
                Link(g1, t3);
                Link(g1, t4);

                Link(g2, t5);
                Link(g2, t6);

                Link(g3, t7);
                Link(g3, t8);

                Link(g4, t2);
                Link(g4, t7);

                db.SaveChanges();
                var a = db.WActivities.ToList();
            }
        }
        private void Link(WGroup group, WTask task)
        {
            task.WGroups.Add(group);
            group.WTasks.Add(task);
        }
        private List<WActivity> GenerateActivities(Random rd, WTask task)
        {
            var activities = new List<WActivity>();
            int n = rd.Next() % 10 + 7;

            for (int i = 0; i < n; i++)
                activities.Add(new WActivity() { WTask = task, IsDone = rd.Next() % 5 != 0, Date = DateTime.Today.Date.AddDays(- i * task.Frequency) });
            return activities;
        }
    }
}
