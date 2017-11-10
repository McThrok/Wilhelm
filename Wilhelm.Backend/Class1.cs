using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wilhelm.DataAccess;

namespace Wilhelm.Backend
{
    public class Class1
    {
        public int Add(int a, int b)
        {
            return a + b;
        }
    }
    public class DataAccessIntegration
    {
        IWContextFactory _contextFactory;
        public DataAccessIntegration(IWContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public void SaveTask(WTask wTask)
        {
            using (IWContext db = _contextFactory.Create())
            {
                db.WTasks.Add(wTask);
                db.SaveChanges();
            }
        }

        public List<WActivity> GetTodayActivities()
        {
            List<WActivity> activities = new List<WActivity>();
            using (WContext db = new WContext())
            {
                activities = db.WActivities.Where((a) => a.Date == DateTime.Today).ToList();
            }
            return activities;
        }
    }
}
