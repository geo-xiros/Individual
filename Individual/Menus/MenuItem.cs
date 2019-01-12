using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Individual.Menus
{
    class MenuItem
    {
        public string Title { get; set; }
        public Action Action { get; set; }

        public MenuItem(string title,Action action)
        {
            Title = title;
            Action = action;
        }
    }
}
