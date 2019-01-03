using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Individual
{
  class ListMenu
  {
    private static readonly int MaxItemsPerPage = 9;
    private List<KeyValuePair<int, string>> _menuChoices;
    private List<KeyValuePair<int, string>> _allMenuChoices;
    private readonly string _menu;
    public int Id;
    private string _titles;
    public ListMenu(string menu, string titles)
    {
      _menu = menu;
      _titles = titles;
      _allMenuChoices = new List<KeyValuePair<int, string>>() { new KeyValuePair<int, string>(0, "empty") };
    }
    public ListMenu(string menu, List<KeyValuePair<int, string>> listItems, string titles) : this(menu, titles)
    {
      _allMenuChoices = listItems;
    }
    public void SetListItems(List<KeyValuePair<int, string>> listItems)
    {
      _allMenuChoices = listItems;
    }
    public bool ChooseListItem()
    {
      do
      {
        Show();
        ConsoleKeyInfo key = Console.ReadKey(true);
        switch (key.Key)
        {
          case ConsoleKey.Escape:
            Id = 0;
            return false;
          case ConsoleKey.PageUp:
            if (menuSkip > 0) { menuSkip -= MaxItemsPerPage; Show(); }
            break;
          case ConsoleKey.PageDown:
            if (menuSkip + MaxItemsPerPage < _allMenuChoices.Count()) { menuSkip += MaxItemsPerPage; Show(); }
            break;
          default:
            byte keyByte;
            if (byte.TryParse(key.KeyChar.ToString(), out keyByte))
            {
              if (keyByte >= 1 && keyByte <= _menuChoices.Count)
              {
                Id = _menuChoices[keyByte - 1].Key;
                return true;
              }
            }
            break;
        }

      } while (true);
    }

    private int menuSkip;
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
          .Skip(menuSkip)
          .Take(MaxItemsPerPage)
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
