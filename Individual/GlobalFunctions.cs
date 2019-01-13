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
        public static bool GetPasswordIfNeeded(out string returnPassword, int userId, int loggedUserId, string passwordForAction)
        {
            string password = "";

            if (loggedUserId != userId)
            {
                PasswordForm passwordForm = new PasswordForm(passwordForAction);
                passwordForm.OnFormFilled = () => password = passwordForm["Password"];
                passwordForm.Open();
            }

            returnPassword = password;

            return loggedUserId == userId
                || password.Length != 0;
        }

    }
}
