﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wilhelm.Frontend.Pages
{
    public interface IMenuPage
    {
        void Activate(int userId);
        void Save();
    }
}
