using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Individual
{
  public enum ActionsAfterRun { None, Exit, GoBack, LoadMenu };

  class MenuChoice
  {

    //public Func<string> TitleSuffixes;

    public readonly string _title;
    public ActionsAfterRun ActionAfterRun;
    public Action<MenuChoice> Run;
    public Func<User.Roles, bool> HasPermission;
    public ConsoleKey Key;

    public string Title
    {
      get
      {
        //if (TitleSuffixes == null)
        //  return _title;
        return _title; //+ TitleSuffixes();
      }
    }

    protected string _loadMenu;
    public string LoadMenu
    {
      get
      {
        ActionAfterRun = ActionsAfterRun.None;
        return _loadMenu;
      }
      set
      {
        _loadMenu = value;
        ActionAfterRun = ActionsAfterRun.LoadMenu;
      }
    }
    public MenuChoice(string title)
    {
      _title = title;
    }
    public MenuChoice(string title, Action<MenuChoice> runOnChoice) : this(title)
    {
      Run += runOnChoice;
    }

  }
}
