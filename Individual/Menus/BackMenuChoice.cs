using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Individual
{
    class BackMenuChoice : MenuChoice
    {
        public BackMenuChoice(string title) : base(title)
        {
            Run += (menuChoice) => menuChoice.ActionAfterRun = ActionsAfterRun.GoBack;
            Key = ConsoleKey.Escape;
        }
        public BackMenuChoice(string title, Action<MenuChoice> runOnChoice) : this(title)
        {
            Run += runOnChoice;
            Key = ConsoleKey.Escape;
        }
    }
}
