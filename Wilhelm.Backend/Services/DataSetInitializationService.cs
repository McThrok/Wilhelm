using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wilhelm.DataAccess;

namespace Wilhelm.Backend.Services
{
    public class DataSetInitializationService
    {
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

                //pass: user1
                WUser User1 = new WUser() { Login = "user1", Password = "Ἱꏁ\u2438ꥅ䫥쪋邳躮Ᏺ껫ꪉ⏺꼿ᆴ넿BD106B80630350E9B080DFB569CD0C337814169FA9350774ECB50AEB0164BD38" };
                //pass: user2
                WUser User2 = new WUser() { Login = "user2", Password = "碼爪�廡驴僁䬠䄑ꨡ㌸쵂줙䌎벟6D10221691D36BD398A5177D2404545D911661DF8E5A0975044DBBD50F8B2A97" };

                db.Users.Add(User1);
                db.Users.Add(User2);
                db.SaveChanges();

                WGroup g1 = new WGroup() { Name = "Zwierzęta", OwnerId = User1.Id, Description = "wszystkie moje domowe zwierzaki" };
                WGroup g2 = new WGroup() { Name = "Rośliny", OwnerId = User1.Id, Description = "wszystkie rośliny doniczkowe" };
                WGroup g3 = new WGroup() { Name = "Finanse", OwnerId = User1.Id, };
                WGroup g4 = new WGroup() { Name = "Ważne", OwnerId = User1.Id, Description = "Lepiej o tym nie zapominać" };


                WTask t1 = new WTask() { Name = "Nakarmić kota", OwnerId = User1.Id, Description = "Tom jest wybredny i je tylko Royal Canin", Frequency = 1, StartDate = DateTime.Today };
                WTask t2 = new WTask() { Name = "Nakarmić psa", OwnerId = User1.Id, Description = "Burek zje wszystko", Frequency = 1, StartDate = DateTime.Today };
                WTask t3 = new WTask() { Name = "Nakarmić rybki", OwnerId = User1.Id, Description = "Jeśli pływają brzuszkiem do góry to można przestać karmić", Frequency = 2, StartDate = DateTime.Today };
                WTask t4 = new WTask() { Name = "Wymienić wodę rybkom", OwnerId = User1.Id, Frequency = 14, StartDate = DateTime.Today };
                WTask t5 = new WTask() { Name = "Podlać kwiaty", OwnerId = User1.Id, Description = "Wszytkie oprócz rosiczki", Frequency = 3, StartDate = DateTime.Today };
                WTask t6 = new WTask() { Name = "Podlać rosiczkę", OwnerId = User1.Id, Description = "Rosiczka jest mięsożerna, ale można i trzeba ją regularnie podlewać", Frequency = 1, StartDate = DateTime.Today };
                WTask t7 = new WTask() { Name = "Zapłacić czynsz", OwnerId = User1.Id, Description = "1000zl plus ewentualne dopłaty", Frequency = 30, StartDate = DateTime.Today };
                WTask t8 = new WTask() { Name = "Zapłacić za internet", OwnerId = User1.Id, Description = "60 plus ewentualne dopłaty", Frequency = 30, StartDate = DateTime.Today };


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

                Link(g4, t1);
                Link(g4, t7);

                db.SaveChanges();
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
                activities.Add(new WActivity() { WTask = task, IsDone = rd.Next() % 5 != 0, Date = DateTime.Today.Date.AddDays(-i * task.Frequency) });
            return activities;
        }
    }
}
