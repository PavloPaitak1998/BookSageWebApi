using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Server_WEB_Programming.Lab2.Dal.Entities;

namespace Server_WEB_Programming.Lab2.ViewModels
{
    public class SageUpdateViewModel
    {
        public Sage Sage { get; set; }

        public IList<int> SelectedBooks { get; set; }
    }
}