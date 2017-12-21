using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wilhelm.Client.Pages
{
    public interface IMenuPage
    {
        Task Activate(int userId);
        Task Save();
    }
}
