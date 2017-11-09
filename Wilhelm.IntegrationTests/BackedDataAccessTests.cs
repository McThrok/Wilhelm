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
            Database.SetInitializer(new WilhelmContexInitializer());
        }

        [Test]
        public void AddTaskToDB_ChangeId()
        {
            DataAccessIntegration c = new DataAccessIntegration();
            WTask t1 = new WTask() { Name = "T1", Description = "T1", Frequency = 1, StartDate = DateTime.Today };
            c.SaveTask(t1);

            WTask dbTask;
            using (WContext db = new WContext())
            {
                dbTask = db.WTasks.Where((t) => (t.Name == t1.Name) && (t.Description == t1.Description) && (t.Frequency == t1.Frequency)).Single();
            }
            Assert.AreEqual(t1.Id, dbTask.Id);
        }
    }
}