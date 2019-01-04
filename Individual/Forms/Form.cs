using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Individual
{
    class Form
    {
        protected string Title;
        protected Dictionary<string, TextBox> TextBoxes;
        public string this[string value] => TextBoxes[value].Text;

        public Form(string title)
        {
            Title = title;
        }
        protected void ShowForm()
        {
            Console.ResetColor();
            Console.Clear();
            ColoredConsole.WriteLine($"{Title}\n", ConsoleColor.Yellow);
            ColoredConsole.Write(new string('\x2500', Console.WindowWidth), 0, 1, ConsoleColor.White);
            Console.CursorVisible = true;

            foreach (var textbox in TextBoxes.Values)
            {
                textbox.Show();
            }
            ColoredConsole.Write(new string('\x2500', Console.WindowWidth), 0, GetLastTextBoxY(), ConsoleColor.White);
            ColoredConsole.Write("[Esc] => Back", 1, GetLastTextBoxY() + 1, ConsoleColor.DarkGray);
        }
        protected int GetLastTextBoxY()
        {
            return TextBoxes.Max(tb => tb.Value.Y) + 4;
        }
        protected void FillForm()
        {
            ShowForm();

            foreach (var textbox in GetTextBoxesToFill())
            {
                textbox.Focus();
                if (textbox.EscapePressed) return ;
            }

            Console.CursorVisible = false;

            OnFormFilled();

            return ;
        }

        private IEnumerable<TextBox> GetTextBoxesToFill()
        {
            return TextBoxes
              .Where(tb => !tb.Value.Locked)
              .OrderBy(tb => tb.Value.Order)
              .Select(tb => tb.Value);
        }

        public bool FormFilled => TextBoxes.Where(t => t.Value.EscapePressed).Count() == 0;


        protected ConsoleKey GetKey(ConsoleKey[] acceptedKeys)
        {
            ConsoleKeyInfo key;
            do
            {
                key = Console.ReadKey(true);
            } while (!acceptedKeys.Contains(key.Key));

            return key.Key;
        }
        public virtual void Open()
        {
        }

        public Action OnFormFilled = () => { };
        public Action OnFormSaved = () => { };

    }

}
