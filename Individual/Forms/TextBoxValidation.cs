using System.Linq;
using System.Text.RegularExpressions;
namespace Individual
{
    static class TextBoxValidation
    {

        public static bool ValidLength(TextBox textBox)
        {
            if (textBox.Text.Length == 0)
            {
                textBox.ValidationError = $"{textBox.Label} can not be empty !!!";
                return false;
            }
            return true;
        }

        public static bool ValidRole(TextBox textBox)
        {
            if (User.ParseRole(textBox.Text) == User.Roles.None)
            {
                textBox.ValidationError = $"Wrong Role Select from List (Simple, View, ViewEdit,ViewEditDelete, Super) !!!";
                return false;
            }

            return true;
        }

        public static bool ValidUserName(TextBox textBox)
        {
            if (textBox.Text.Length == 0)
            {
                textBox.ValidationError = $"{textBox.Label} can not be empty !!!";
                return false;
            }
            if (User.Exists(textBox.Text))
            {
                textBox.ValidationError = "Username Exists!!!";
                return false;
            }
            return true;
        }

        public static bool ValidPassword(TextBox textBox)
        {
            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasMinimum4Chars = new Regex(@".{4,}");

            var isValidated = hasNumber.IsMatch(textBox.Text)
                && hasUpperChar.IsMatch(textBox.Text)
                && hasMinimum4Chars.IsMatch(textBox.Text);

            if (!isValidated)
            {
                textBox.ValidationError = $"Password should be at least 4 letters containnig at least one digit and one capital letter.";
                return false;
            }
            return true;
        }
    }
}
