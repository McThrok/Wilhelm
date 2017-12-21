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
using Wilhelm.Client.Services;
using Wilhelm.Client.Services.Interfaces;
using Wilhelm.Client.ViewModels.Pages;
using Wilhelm.Client.Model;

namespace Wilhelm.IntegrationTests.PagesTests
{
    [TestFixture]
    class TaskPageTests
    {
        IHoldersService _hs;
        IProxyService _ps;
        public TaskPageTests()
        {
            _hs = new HoldersService(new HoldersConversionService());
            _ps = new ProxyService();
        }

        [Test]
        public async Task TaskPageActivateFunctionTest()
        {
            TaskPageViewModel tpvm = new TaskPageViewModel(_hs, _ps);
            int ownerId = -1;
            List<WTask> wtasks = new List<WTask>();
            using (WContext db = new WContext())
            {
                ownerId = db.Users.Where(x => x.Login == "user1").Single().Id;
                var cos = db.WTasks.ToList();
                wtasks = db.WTasks.Where(x => x.OwnerId == ownerId).OrderBy(x => x.Id).ToList();
            }
           await tpvm.Activate(ownerId);
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
}
