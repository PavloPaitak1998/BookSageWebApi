using System.Collections.Generic;
using Server_WEB_Programming.Lab2.Dal.Entities;

namespace Server_WEB_Programming.Lab3.ViewModels
{
    public class SageUpdateViewModel
    {
        public Sage Sage { get; set; }

        public IList<int> SelectedBooks { get; set; }
    }
}