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


        public string this[string textBox]
        {
            get
            {
                return TextBoxes[textBox].Text;

            }
            set
            {
                TextBoxes[textBox].Text = value;
            }
        }

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

            int maxLabelLength = TextBoxes.Max(t => t.Value.Label.Length);

            foreach (var textbox in TextBoxes.Values)
            {
                textbox.Label = textbox.Label.PadLeft(maxLabelLength);
                textbox.Show();
            }
            ColoredConsole.Write(new string('\x2500', Console.WindowWidth), 0, LastTextBoxY, ConsoleColor.White);
            ColoredConsole.Write("[Esc] => Back", 1, LastTextBoxY + 1, ConsoleColor.DarkGray);
        }
        protected int LastTextBoxY => TextBoxes.Max(tb => tb.Value.Y) + 4;

        protected void FillForm()
        {
            ShowForm();

            foreach (var textbox in GetTextBoxesToFill())
            {
                textbox.Focus();
                if (textbox.EscapePressed)
                {
                    OnFormExit();
                    return;
                }
            }

            Console.CursorVisible = false;

            OnFormFilled();

            return;
        }

        private IEnumerable<TextBox> GetTextBoxesToFill()
        {
            return TextBoxes
              .Where(tb => !tb.Value.Locked)
              .OrderBy(tb => tb.Value.Order)
              .Select(tb => tb.Value);
        }

        public bool EscapePressed => TextBoxes.Where(t => t.Value.EscapePressed).Count() != 0;

        public virtual void Open() { }

        public Action OnFormFilled = () => { };
        public Action OnFormSaved = () => { };
        public Action OnFormExit = () => { };

        protected void AddTextBoxes(List<Field> fields)
        {
            int pos = 3;

            TextBoxes = new Dictionary<string, TextBox>();

            foreach (var field in fields)
            {
                TextBoxes.Add(field.Name, new TextBox(field.Name, 3, pos, field.Size, field.PasswordChar));
                pos += 2;
            }
        }
    }

}
