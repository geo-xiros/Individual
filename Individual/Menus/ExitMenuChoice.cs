using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Individual
{
    class ExitMenuChoice : MenuChoice
    {
        public ExitMenuChoice(string title) : base(title)
        {
            Run = (menuChoice) => menuChoice.ActionAfterRun = ActionsAfterRun.Exit;
            Key = ConsoleKey.Escape;
        }
    }
}
