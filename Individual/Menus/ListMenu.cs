using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Individual
{
    class ListMenu
    {
        private static readonly int _MaxItemsPerPage = 9;
        private List<KeyValuePair<int, string>> _menuChoices;
        private List<KeyValuePair<int, string>> _allMenuChoices;
        private readonly string _menu;
        public int Id { get; private set; }
        private readonly string _titles;
        private int _menuSkip;
        private Func<string> _menuTitle;

        private int MenuSkip
        {
            get
            {
                return _menuSkip;
            }
            set
            {
                if ((value >= 0) && (value < _allMenuChoices.Count()))
                {
                    _menuSkip = value;
                }
            }
        }

        public ListMenu(string menu, string titles, Func<string> menuTitle)
        {
            _menu = menu;
            _titles = titles;
            _allMenuChoices = new List<KeyValuePair<int, string>>() { new KeyValuePair<int, string>(0, string.Empty) };
            _menuTitle = menuTitle;
        }

        public ListMenu(string menu, List<KeyValuePair<int, string>> listItems, string titles, Func<string> menuTitle) : this(menu, titles, menuTitle)
        {
            _allMenuChoices = listItems;
        }

        public void SetListItems(List<KeyValuePair<int, string>> listItems) => _allMenuChoices = listItems;

        public bool ChooseListItem()
        {
            Id = -1;

            while (Id == -1)
            {
                GeneratePageMenuChoices();
                Show();
                ReadKey<Action> readKey = new ReadKey<Action>(GetKeyChoices());
                readKey.GetKey()();
            }

            return Id != 0;
        }


        private void Show()
        {
            Console.CursorVisible = false;
            Console.ResetColor();
            Console.Clear();
            ColoredConsole.Write(string.Format("{0," + (Console.WindowWidth - 1) + "}\r", $"Total List Rows : {_allMenuChoices.Count()} "), ConsoleColor.Green);
            ColoredConsole.WriteLine($"{_menu} {_menuTitle()}", ConsoleColor.Yellow);
            ColoredConsole.Write(new string('\x2500', Console.WindowWidth), ConsoleColor.White);
            ColoredConsole.WriteLine(_titles, ConsoleColor.White);
            ColoredConsole.Write(new string('\x2500', Console.WindowWidth), ConsoleColor.White);

            for (int i = 0; i < _menuChoices.Count; i++)
            {
                string aa = (i + 1).ToString() + ' ';
                Console.WriteLine($"{aa,3}{_menuChoices[i].Value}");
            }

            ColoredConsole.Write(new string('\x2500', Console.WindowWidth), ConsoleColor.White);
            ColoredConsole.WriteLine($"\n      [Esc] => Back\n  [Page Up] => Previous Set\n[Page Down] => Next Set", ConsoleColor.DarkGray);

        }
        private void GeneratePageMenuChoices()
        {
            _menuChoices = _allMenuChoices
               .Skip(_menuSkip)
               .Take(_MaxItemsPerPage)
               .ToList();
        }
        private Dictionary<ConsoleKey, Action> GetKeyChoices()
        {
            Dictionary<ConsoleKey, Action> keyChoices = new Dictionary<ConsoleKey, Action>()
                {
                    {ConsoleKey.Escape, ()=> Id = 0 },
                    {ConsoleKey.PageUp, ()=> MenuSkip -= _MaxItemsPerPage },
                    {ConsoleKey.PageDown, ()=> MenuSkip += _MaxItemsPerPage }
                };

            for (int i = 0; i < _menuChoices.Count; i++)
            {
                int choiceIndex = i;
                keyChoices.Add(Keys1To9[i + 1], () => Id = _menuChoices[choiceIndex].Key);
            }

            return keyChoices;
        }

        public static Dictionary<int, ConsoleKey> Keys1To9 { get; } = new Dictionary<int, ConsoleKey>()
        {
              { 1, ConsoleKey.D1 }
            , { 2, ConsoleKey.D2 }
            , { 3, ConsoleKey.D3 }
            , { 4, ConsoleKey.D4 }
            , { 5, ConsoleKey.D5 }
            , { 6, ConsoleKey.D6 }
            , { 7, ConsoleKey.D7 }
            , { 8, ConsoleKey.D8 }
            , { 9, ConsoleKey.D9 }
        };
    }
}
