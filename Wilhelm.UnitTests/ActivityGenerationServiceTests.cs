using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wilhelm.DataAccess;
using Wilhelm.Backend.Services;
using Wilhelm.Backend.Services.Interfaces;

namespace Wilhelm.UnitTests
{
    [TestFixture]
    public class ActivityGenerationServiceTests
    {
        public DateTime startDateTime;
        public List<WTask> tasks;
        public List<WActivity> activities;
        public IActivityGenerationService ActivityGenerationService;

        [SetUp]
        public void Init()
        {
            ActivityGenerationService = new ActivityGenerationService();
            startDateTime = new DateTime(2017, 5, 15);
            tasks = new List<WTask>();
            tasks.Add(new WTask() { Id = 1, Name = "Task1", Frequency = 2, StartDate = startDateTime });
            activities = new List<WActivity>();
            activities.Add(new WActivity() { WTask = tasks[0], Date = startDateTime });
        }

        [Test]
        public void EmptyActivitiesTodayStartDate()
        {
            var result = ActivityGenerationService.GenerateActivities(new List<WActivity>(), tasks, startDateTime);

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(startDateTime.Date, result[0].Date.Date);
        }

        [Test]
        public void EmptyActivitiesPastStartDate()
        {
            tasks[0].StartDate = tasks[0].StartDate.AddDays(-3);

            var result = ActivityGenerationService.GenerateActivities(new List<WActivity>(), tasks, startDateTime);

            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.Where(x => x.Date.Date == startDateTime.Date.AddDays(-3)).SingleOrDefault() != null);
            Assert.IsTrue(result.Where(x => x.Date.Date == startDateTime.Date.AddDays(-1)).SingleOrDefault() != null);
        }

        [Test]
        public void EmptyActivitiesPastStartDateToday()
        {
            tasks[0].StartDate = tasks[0].StartDate.AddDays(-2);

            var result = ActivityGenerationService.GenerateActivities(new List<WActivity>(), tasks, startDateTime);

            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.Where(x => x.Date.Date == startDateTime.Date.AddDays(-2)).SingleOrDefault() != null);
            Assert.IsTrue(result.Where(x => x.Date.Date == startDateTime.Date).SingleOrDefault() != null);
        }

        [Test]
        public void EmptyActivitiesFutureStartDate()
        {
            tasks[0].StartDate = tasks[0].StartDate.AddDays(1);

            var result = ActivityGenerationService.GenerateActivities(new List<WActivity>(), tasks, startDateTime);

            Assert.AreEqual(0, result.Count);
        }

        [Test]
        public void PastStartDateBeforeLastActivity()
        {
            tasks[0].StartDate = tasks[0].StartDate.AddDays(-4);
            activities[0].Date = activities[0].Date.AddDays(-3);

            var result = ActivityGenerationService.GenerateActivities(activities, tasks, startDateTime);

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(startDateTime.Date.AddDays(-1), result[0].Date.Date);
        }

        [Test]
        public void PastStartDateBeforeLastActivityToday()
        {
            tasks[0].StartDate = tasks[0].StartDate.AddDays(-6);
            activities[0].Date = activities[0].Date.AddDays(-4);

            var result = ActivityGenerationService.GenerateActivities(activities, tasks, startDateTime);

            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.Where(x => x.Date.Date == startDateTime.Date.AddDays(-2)).SingleOrDefault() != null);
            Assert.IsTrue(result.Where(x => x.Date.Date == startDateTime.Date).SingleOrDefault() != null);
        }

        [Test]
        public void PastStartDateAfterLastActivity()
        {
            tasks[0].StartDate = tasks[0].StartDate.AddDays(-3);
            activities[0].Date = activities[0].Date.AddDays(-7);

            var result = ActivityGenerationService.GenerateActivities(activities, tasks, startDateTime);

            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.Where(x => x.Date.Date == startDateTime.Date.AddDays(-3)).SingleOrDefault() != null);
            Assert.IsTrue(result.Where(x => x.Date.Date == startDateTime.Date.AddDays(-1)).SingleOrDefault() != null);

        }

        [Test]
        public void PastStartDateAfterLastActivity2()
        {
            tasks[0].StartDate = tasks[0].StartDate.AddDays(-4);
            activities[0].Date = activities[0].Date.AddDays(-5);

            var result = ActivityGenerationService.GenerateActivities(activities, tasks, startDateTime);

            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.Where(x => x.Date.Date == startDateTime.Date.AddDays(-3)).SingleOrDefault() != null);
            Assert.IsTrue(result.Where(x => x.Date.Date == startDateTime.Date.AddDays(-1)).SingleOrDefault() != null);

        }

        [Test]
        public void PastStartDateAfterLastActivityToday()
        {
            tasks[0].StartDate = tasks[0].StartDate.AddDays(-2);
            activities[0].Date = activities[0].Date.AddDays(-5);

            var result = ActivityGenerationService.GenerateActivities(activities, tasks, startDateTime);

            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.Where(x => x.Date.Date == startDateTime.Date.AddDays(-2)).SingleOrDefault() != null);
            Assert.IsTrue(result.Where(x => x.Date.Date == startDateTime.Date).SingleOrDefault() != null);
        }

        [Test]
        public void ActivitiesFutureStartDate()
        {
            tasks[0].StartDate = tasks[0].StartDate.AddDays(1);
            activities[0].Date = activities[0].Date.AddDays(-2);

            var result = ActivityGenerationService.GenerateActivities(activities, tasks, startDateTime);

            Assert.AreEqual(0, result.Count);
        }
    }
}
