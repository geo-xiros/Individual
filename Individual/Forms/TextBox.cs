using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Individual
{
  class TextBox
  {
    public readonly string Label;

    public Func<TextBox, bool> Validate;
    public bool EscapePressed { get; private set; }
    public int Y { get { return _y; } }

    private int _labelX;
    private int _y;
    private int _maxLength;
    private bool _pressedEnter;
    private int _textX;
    private string _textOldvalue;
    private char _passwordChar;
    public bool Locked;
    public int Order { get { return _y; } }
    public TextBox(string label, int x, int y, int maxLength)
    {
      Label = label;
      Validate = (tb) => true;

      _labelX = x;
      _y = y;
      _maxLength = maxLength;
      _textX = _labelX + Label.Length + 2;
    }
    public TextBox(string label, int x, int y, int maxLength, char passwordChar) : this(label, x, y, maxLength)
    {
      _passwordChar = passwordChar;
    }
    public void Show(bool focused = false)
    {
      ConsoleColor backColor = focused ? ConsoleColor.Blue : ConsoleColor.Black;

      ColoredConsole.Write($"{Label}: ", _labelX, _y, ConsoleColor.White);
      ColoredConsole.Write(new string(' ', _maxLength), _textX, _y, consoleBackColor: backColor);

      if (_passwordChar == 0 || _text == null)
        ColoredConsole.Write(_text, _textX, _y, ConsoleColor.DarkGray, backColor);
      else
        ColoredConsole.Write(new string(_passwordChar, _text.Length), _textX, _y, ConsoleColor.DarkGray, backColor);
    }
    public string Text
    {
      get
      {
        return _text;
      }
      set
      {
        _text = value;
        _textOldvalue = value;
      }
    }
    private string _text;

    public void Focus()
    {
      EscapePressed = false;

      do
      {

        // print label and value focues (with back color blue)
        Show(true);

        _text = ReadLine();

        // print again to clear focus line
        Show();

        if (EscapePressed) return;

        // exit with no validation check
        // if pressed enter and Text is not changed and oldvalue is not null
        if (EnterPressedHavingOldValueAndNotChanged()) return;

      } while (!Validate(this) && !EscapePressed);

    }
    private bool EnterPressedHavingOldValueAndNotChanged()
    {
      return (_pressedEnter) && (_textOldvalue != null) && (_text == _textOldvalue);
    }
    private string ReadLine()
    {
      StringBuilder buffer = new StringBuilder();
      bool _clearOldValue = _text?.Length != 0;
      ConsoleKeyInfo info;
      Console.BackgroundColor = ConsoleColor.Blue;
      Console.ForegroundColor = ConsoleColor.White;
      do
      {
        info = Console.ReadKey(true);

        switch (info.Key)
        {
          case ConsoleKey.Enter:
            if ((buffer.Length == 0) && (_clearOldValue)) buffer.Append(_text);
            break;
          case ConsoleKey.LeftArrow:
          case ConsoleKey.RightArrow:
          case ConsoleKey.DownArrow:
          case ConsoleKey.UpArrow:
          case ConsoleKey.Tab:
          case ConsoleKey.PageDown:
          case ConsoleKey.PageUp:
          case ConsoleKey.Escape:
            break;
          case ConsoleKey.Backspace:
            if (buffer.Length > 0)
            {
              Console.Write("\b");
              Console.Write(" ");
              Console.Write("\b");
              buffer.Remove(buffer.Length - 1, 1);
            }
            break;
          default:
            if (_clearOldValue)
            {
              ColoredConsole.Write(new string(' ', _maxLength), _textX, _y);
              _clearOldValue = false;
            }
            if (_ValidationError != null)
            {
              ClearValidationError();
            }
            if (buffer.Length < _maxLength)
            {
              if (_passwordChar == 0)
                Console.Write(info.KeyChar);
              else
                Console.Write(_passwordChar);
              buffer.Append(info.KeyChar);
            }
            break;
        }
      } while (info.Key != ConsoleKey.Enter && info.Key != ConsoleKey.Escape);

      _pressedEnter = info.Key == ConsoleKey.Enter;
      EscapePressed = info.Key == ConsoleKey.Escape;

      return buffer.ToString();

    }
    private string _ValidationError;
    public string ValidationError
    {
      set
      {
        _ValidationError = value;
        ColoredConsole.Write(value, _textX, _y + 1, ConsoleColor.DarkRed, ConsoleColor.Black);
      }
      get
      {
        return _ValidationError;
      }
    }
    private void ClearValidationError()
    {
      int left = Console.CursorLeft;
      int top = Console.CursorTop;
      ColoredConsole.Write(new string(' ', _ValidationError.Length), _textX, _y + 1, ConsoleColor.White, ConsoleColor.Black);
      _ValidationError = null;
      Console.SetCursorPosition(left, top);

    }
  }
}
