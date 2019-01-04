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
        public int Id;
        private string _titles;
        private int _menuSkip;
        private int menuSkip
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

        public ListMenu(string menu, string titles)
        {
            _menu = menu;
            _titles = titles;
            _allMenuChoices = new List<KeyValuePair<int, string>>() { new KeyValuePair<int, string>(0, "empty") };
        }

        public ListMenu(string menu, List<KeyValuePair<int, string>> listItems, string titles) : this(menu, titles) => _allMenuChoices = listItems;

        public void SetListItems(List<KeyValuePair<int, string>> listItems) => _allMenuChoices = listItems;

        public bool ChooseListItem()
        {

            ReadKey<Action> readKey = new ReadKey<Action>(new Dictionary<ConsoleKey, Action>()
                {
                    {ConsoleKey.Escape, ()=> Id = 0 },
                    {ConsoleKey.PageUp, ()=> menuSkip -= _MaxItemsPerPage },
                    {ConsoleKey.PageDown, ()=> menuSkip += _MaxItemsPerPage },
                    {ConsoleKey.D1, ()=> Id= _menuChoices[0].Key },
                    {ConsoleKey.D2, ()=> Id= _menuChoices[1].Key },
                    {ConsoleKey.D3, ()=> Id= _menuChoices[2].Key},
                    {ConsoleKey.D4, ()=> Id= _menuChoices[3].Key },
                    {ConsoleKey.D5, ()=> Id= _menuChoices[4].Key },
                    {ConsoleKey.D6, ()=> Id= _menuChoices[5].Key },
                    {ConsoleKey.D7, ()=> Id= _menuChoices[6].Key },
                    {ConsoleKey.D8, ()=> Id= _menuChoices[7].Key },
                    {ConsoleKey.D9, ()=> Id= _menuChoices[8].Key }

                });

            Id = -1;

            while (Id == -1)
            {
                Show();
                readKey.GetKey()();
            }

            return Id != 0;
        }


        private void Show()
        {
            Console.CursorVisible = false;
            Console.ResetColor();
            Console.Clear();
            ColoredConsole.WriteLine($"{_menu} {Application.Username}\n", ConsoleColor.Yellow);
            ColoredConsole.WriteLine(new string('\x2500', _titles.Length), ConsoleColor.White);
            ColoredConsole.WriteLine(_titles, ConsoleColor.White);
            ColoredConsole.WriteLine(new string('\x2500', _titles.Length), ConsoleColor.White);

            _menuChoices = _allMenuChoices
                .Skip(_menuSkip)
                .Take(_MaxItemsPerPage)
                .ToList();

            for (byte i = 0; i < _menuChoices.Count; i++)
            {
                string aa = (i + 1).ToString() + ' ';

                Console.WriteLine($"\x2502{aa,3}{_menuChoices[i].Value}");
            }
            ColoredConsole.WriteLine(new string('\x2500', _titles.Length), ConsoleColor.White);
            ColoredConsole.WriteLine($"\n      [Esc] => Back\n  [Page Up] => Previous Set\n[Page Down] => Next Set", ConsoleColor.DarkGray);

        }
    }
}
