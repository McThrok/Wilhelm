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
        public void SaveTask(WTask wTask)
        {
            using (WContext db = new WContext())
            {
                db.WTasks.Add(wTask);
                db.SaveChanges();
            }
        }
    }
}
