using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Individual
{
    class Application
    {
        public static User LoggedUser { get; private set; }

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
