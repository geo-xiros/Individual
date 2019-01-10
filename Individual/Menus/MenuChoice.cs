using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Individual
{
    public enum ActionsAfterRun { None, Exit, GoBack, LoadMenu };

    class MenuChoice
    {
        public ActionsAfterRun ActionAfterRun { get; set; }
        public Action<MenuChoice> Run { get; set; }
        public Func< bool> HasPermission { get; set; }
        public ConsoleKey Key { get; set; }

        public string Title { get; private set; }

        protected string _loadMenu;
        public string LoadMenu
        {
            get
            {
                ActionAfterRun = ActionsAfterRun.None;
                return _loadMenu;
            }
            set
            {
                _loadMenu = value;
                ActionAfterRun = ActionsAfterRun.LoadMenu;
            }
        }
        public MenuChoice(string title)
        {
            Title = title;
        }
        public MenuChoice(string title, Action<MenuChoice> runOnChoice) : this(title)
        {
            Run += runOnChoice;
        }

    }
}
