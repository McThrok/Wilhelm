using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wilhelm.Backend.Model;
using Wilhelm.DataAccess;
using Wilhelm.Shared.Dto;

namespace Wilhelm.Backend.MockBase
{
    public static class MockBase
    {
        private static List<WGroup> _groups;
        private static List<WTask> _tasks;
        private static List<WActivity> _activities;
        private static List<ReportDto> _reports;

        static MockBase()
        {
            _groups = new List<WGroup>();
            for (int i = 0; i < 10; i++)
                _groups.Add(new WGroup() { Id = i, Name = "Group" + i.ToString(), Description = "Description.. description.. description.", Archivized = i % 5 == 0 });

            _tasks = new List<WTask>();
            for (int i = 0; i < 20; i++)
                _tasks.Add(new WTask() { Id = i, Name = "Task" + i.ToString(), StartDate = DateTime.Now, Frequency = i, Description = "Description.. description.. description.", Archivized = i % 4 == 0 });

            foreach (var group in _groups)
                foreach (var task in _tasks)
                    if (task.Id % 5 == group.Id % 5)
                    {
                        task.WGroups.Add(group);
                        group.WTasks.Add(task);
                    }


            _activities = new List<WActivity>();
            for (int i = 0; i < 100; i++)
                _activities.Add(new WActivity() { Id = i, IsDone = i % 5 != 0, Date = DateTime.Now, WTask = _tasks[i % _tasks.Count] });

            _reports = new List<ReportDto>
            {
                new ReportDto() { Category = "Najczęściej pomijane zadanie", Value = "zadanie 2" },
                new ReportDto() { Category = "Najrzadziej pomijane zadanie", Value = "zadanie 2" },
                new ReportDto() { Category = "Ilość wykonanych zadań", Value = "231" },
                new ReportDto() { Category = "Ilość niewykonanych zadań", Value = "21" },
                new ReportDto() { Category = "Procent niewykonanych zadań", Value = "91%" }
            };

        }
        public static List<WGroup> GetGroups()
        {
            return _groups;
        }
        public static List<WTask> GetTasks()
        {
            return _tasks;
        }
        public static List<WActivity> GetActivities()
        {
            return _activities;
        }
        public static List<WActivity> GetTodaysActivities()
        {
            return _activities.Where((x, i) => i < 7).ToList();
        }
        public static List<ReportDto> GetReports()
        {
            return _reports;
        }
    }
}
