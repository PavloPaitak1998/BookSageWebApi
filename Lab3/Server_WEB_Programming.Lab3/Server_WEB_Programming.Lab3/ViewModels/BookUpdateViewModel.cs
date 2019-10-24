using System.Collections.Generic;
using Server_WEB_Programming.Lab2.Dal.Entities;

namespace Server_WEB_Programming.Lab3.ViewModels
{
    public class BookUpdateViewModel
    {
        public Book Book { get; set; }

        public IList<int> SelectedSages { get; set; }
    }
}