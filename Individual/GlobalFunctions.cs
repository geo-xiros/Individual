using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Individual
{
    static class GlobalFunctions
    {
        public static void SelectFromList(Func<List<KeyValuePair<int, string>>> listOfItems, Action<int> RunOnSelection, string listMenuTitle, string headers)
        {
            ListMenu lm = new ListMenu(listMenuTitle, headers);
            do
            {
                lm.SetListItems(listOfItems());
                lm.ChooseListItem();

                if (lm.Id != 0)
                {
                    RunOnSelection(lm.Id);
                }

            } while (lm.Id != 0);

        }
        public static bool TryToRunAction<T>(T onObject, Func<T, bool> action, string questionMessage, string successMessage, string failMessage)
        {
            bool tryAgain = false;
            do
            {
                try
                {
                    if (action(onObject))
                    {
                        Alerts.Success(successMessage);
                        return true;
                    }
                    else
                    {
                        Alerts.Warning(failMessage);
                        return false;
                    }

                }
                catch (DatabaseException e)
                {
                    Alerts.Error(e.Message);
                    Console.Clear();
                    tryAgain = MessageBox.Show(questionMessage) == MessageBox.MessageBoxResult.Yes;
                }

            } while (tryAgain);

            return false;
        }

    }
}
