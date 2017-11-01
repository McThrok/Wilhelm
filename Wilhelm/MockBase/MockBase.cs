using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wilhelm.Frontend.Model;

namespace Wilhelm.Frontend.MockBase
{
    public static class MockBase
    {
        private static List<GroupHolder> _groups;
        private static List<TaskHolder> _tasks;
        private static List<ActivityHolder> _activities;
        private static List<ReportData> _reports;

        static MockBase()
        {
            _groups = new List<GroupHolder>();
            for (int i = 0; i < 10; i++)
                _groups.Add(new GroupHolder(i, "Group" + i.ToString()) { Description = "Description.. description.. description.", Archivized = i % 5 == 0 });

            _tasks = new List<TaskHolder>();
            for (int i = 0; i < 20; i++)
                _tasks.Add(new TaskHolder(i, "task" + i.ToString()) { StartDate = DateTime.Now, Frequency = i, Description = "Description.. description.. description.", Archivized = i % 4 == 0 });

            foreach (var group in _groups)
                foreach (var task in _tasks)
                    if (task.Id % 5 == group.Id % 5)
                    {
                        task.Groups.Add(group);
                        group.Tasks.Add(task);
                    }


            _activities = new List<ActivityHolder>();
            for (int i = 0; i < 100; i++)
                _activities.Add(new ActivityHolder(i) { IsDone = i % 10 != 0, Date = DateTime.Now, Task = _tasks[i % _tasks.Count] });

            _reports = new List<ReportData>();
            _reports.Add(new ReportData() { Category = "Najczęściej pomijane zadanie", Value = "zadanie 2" });
            _reports.Add(new ReportData() { Category = "Najrzadziej pomijane zadanie", Value = "zadanie 2" });
            _reports.Add(new ReportData() { Category = "Ilość wykonanych zadań", Value = "231" });
            _reports.Add(new ReportData() { Category = "Ilość niewykonanych zadań", Value = "21" });
            _reports.Add(new ReportData() { Category = "Procent niewykonanych zadań", Value = "91%" });

        }
        public static List<GroupHolder> GetGroups()
        {
            return _groups;
        }
        public static List<TaskHolder> GetTasks()
        {
            return _tasks;
        }
        public static List<ActivityHolder> GetActivities()
        {
            return _activities;
        }
        public static List<ReportData> GetReports()
        {
            return _reports;
        }
    }
}
