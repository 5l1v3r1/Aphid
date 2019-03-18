﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components.External.ConsolePlus
{
    public class AutocompleteConsole
    {
        private int
            _autocompleteLeft,
            _autocompleteTop,
            _autocompleteWidth,
            _autoCompleteHeight,
            _autocompleteIndex,
            _oldLeft,
            _oldTop,
            _cursorIndex;

        private string _consoleBuffer = "";

        private bool _autocompleteActive;

        private Autocomplete[] _matches = new Autocomplete[0];

        private string _searchBuffer;

        private readonly string _prompt;

        private bool _forceAutocomplete;

        private int _historyIndex;

        private readonly List<string> _history = new List<string>();

        public int MaxListSize { get; set; }

        public IScanner Scanner { get; }

        public ISyntaxHighlighter Highlighter { get; }

        public IAutocompletionSource Source { get; }

        public int MaxHistoryCount { get; set; }

        public AutocompleteConsole(
            string prompt,
            IScanner scanner,
            ISyntaxHighlighter highlighter,
            IAutocompletionSource sources)
        {
            MaxListSize = 10;
            MaxHistoryCount = 100;
            _prompt = prompt;
            Scanner = scanner;
            Highlighter = highlighter;
            Source = sources;
        }

        public string ReadLine()
        {
            DrawText(skipAutocomplete: true);

            while (true)
            {
                var k = Console.ReadKey(true);

                if (_autocompleteActive)
                {
                    _autocompleteActive = false;

                    Erase(
                        _autocompleteLeft,
                        _autocompleteTop,
                        _autocompleteWidth,
                        _autoCompleteHeight);
                }

                if (k.Modifiers == ConsoleModifiers.Control &&
                    k.Key == ConsoleKey.Spacebar)
                {
                    _forceAutocomplete = true;
                    UpdateAutocomplete();
                    continue;
                }
                else if (k.Key != ConsoleKey.UpArrow &&
                    k.Key != ConsoleKey.DownArrow)
                {
                    _historyIndex = -1;
                    _forceAutocomplete = false;
                }

                switch (k.Key)
                {
                    case ConsoleKey.LeftArrow:
                        InterpretHorizontalArrow(left: true);
                        break;

                    case ConsoleKey.RightArrow:
                        InterpretHorizontalArrow(left: false);
                        break;

                    case ConsoleKey.UpArrow:
                        InterpretVerticalArrow(up: true);
                        break;

                    case ConsoleKey.DownArrow:
                        InterpretVerticalArrow(up: false);
                        break;

                    case ConsoleKey.Tab:
                        InterpretTab();
                        break;

                    case ConsoleKey.Home:
                        InterpretHome();
                        break;

                    case ConsoleKey.End:
                        InterpretEnd();
                        break;

                    case ConsoleKey.Enter:
                        return InterpretEnter();

                    case ConsoleKey.Backspace:
                        InterpretBackspace();
                        break;

                    case ConsoleKey.Delete:
                        InterpretDelete();
                        break;

                    default:
                        InterpretOther(k);
                        break;
                }
            }
        }

        private void InterpretHorizontalArrow(bool left)
        {
            if ((left && _cursorIndex > 0) ||
                (!left && _cursorIndex < _consoleBuffer.Length))
            {
                _cursorIndex += left ? -1 : 1;
                SetCursor();
            }
        }

        private void InterpretVerticalArrow(bool up)
        {
            if (_matches.Length == 0)
            {
                SetHistoryValue(up);
            }
            else
            {
                if (up && _autocompleteIndex > 0)
                {
                    _autocompleteIndex--;
                }
                else if (!up && _autocompleteIndex < _matches.Length - 1)
                {
                    _autocompleteIndex++;
                }

                UpdateAutocomplete(clearMatches: false);
            }
        }

        private void InterpretTab()
        {
            string remaining = null;

            if (_matches.Length == 1)
            {
                remaining = _matches[0].Text;
            }
            else if (_matches.Length > 1)
            {
                remaining = _matches[_autocompleteIndex].Text;
            }
            else
            {
                _consoleBuffer += "    ";
                _cursorIndex += "    ".Length;
            }

            if (remaining != null)
            {
                if (!string.IsNullOrEmpty(_searchBuffer))
                {
                    remaining = remaining.Substring(_searchBuffer.Length);
                }

                _consoleBuffer += remaining;
                _cursorIndex += remaining.Length;
                _searchBuffer = "";
            }

            DrawText();
        }

        private void InterpretHome()
        {
            _cursorIndex = 0;
            SetCursor();
        }

        private void InterpretEnd()
        {
            _cursorIndex = _consoleBuffer.Length;
            SetCursor();
        }

        private string InterpretEnter()
        {
            Console.WriteLine();
            var value = _consoleBuffer;
            _history.Insert(0, value);

            if (_history.Count > MaxHistoryCount)
            {
                _history.Remove(_history.Last());
            }

            _consoleBuffer = "";
            _cursorIndex = 0;
            _searchBuffer = "";

            return value;
        }

        private void InterpretBackspace()
        {
            if (_consoleBuffer.Length == 0 || _cursorIndex == 0)
            {
                return;
            }

            EraseChar(--_cursorIndex);
        }

        private void InterpretDelete()
        {
            if (_consoleBuffer.Length == 0 || _cursorIndex == _consoleBuffer.Length)
            {
                return;
            }

            EraseChar(_cursorIndex);
        }

        private void InterpretOther(ConsoleKeyInfo k)
        {
            var keyStr = k.KeyChar.ToString();
            _consoleBuffer = _consoleBuffer.Insert(_cursorIndex, keyStr);
            _cursorIndex++;
            DrawText();
            SetCursor();
        }

        private void DrawText(bool skipAutocomplete = false)
        {
            var backup = Console.CursorVisible;
            Console.CursorVisible = false;
            var sb = new StringBuilder(string.Format("\r{0}", _prompt));

            foreach (var t in Highlighter.Highlight(_consoleBuffer))
            {
                VT100.Append(sb, t);
            }

            Console.Write(sb.ToString());

            if (!skipAutocomplete)
            {
                UpdateAutocomplete();
            }

            Console.CursorVisible = backup;
        }

        private void BackupCursor()
        {
            _oldLeft = Console.CursorLeft;
            _oldTop = Console.CursorTop;
        }

        private void RestoreCursor() => Console.SetCursorPosition(_oldLeft, _oldTop);

        private void Erase(int left, int top, int width, int height)
        {
            var backup = Console.CursorVisible;
            Console.CursorVisible = false;
            BackupCursor();
            var line = new string(' ', width);

            for (var i = 0; i < height; i++)
            {
                var t = top + i;

                if (t >= Console.BufferHeight)
                {
                    continue;
                }

                Console.SetCursorPosition(left, t);
                Console.Write(line);
            }

            RestoreCursor();
            Console.CursorVisible = backup;
        }

        private void UpdateAutocomplete() => UpdateAutocomplete(clearMatches: true);

        private void UpdateAutocomplete(bool clearMatches)
        {
            if (clearMatches)
            {
                var curText = _cursorIndex == _consoleBuffer.Length ?
                    _consoleBuffer :
                    _consoleBuffer.Remove(_cursorIndex);

                var tokens = Scanner.Tokenize(curText).ToArray();

                var matches = Source.GetWords(
                    _consoleBuffer,
                    _cursorIndex,
                    _forceAutocomplete,
                    out _searchBuffer);

                if ((!_forceAutocomplete && tokens.Length == 0) || matches == null)
                {
                    _matches = new Autocomplete[0];

                    return;
                }

                _matches = matches.ToArray();
            }

            if (_matches.Length != 0)
            {
                DrawAutocomplete();
            }
            else
            {
                _searchBuffer = "";
            }
        }

        private void DrawAutocomplete()
        {
            if (_autocompleteIndex >= _matches.Length)
            {
                _autocompleteIndex = 0;
            }

            var oldTop = Console.CursorTop;

            _autocompleteWidth = _matches.Length != 0 ?
                _matches.Max(x => Cli.EraseStyles(x.View).Length) : 0;

            var maxWidth =
                Console.WindowWidth -
                _prompt.Length -
                _cursorIndex +
                _searchBuffer.Length -
                1;

            if (maxWidth < 0)
            {
                maxWidth = 0;
            }

            if (_autocompleteWidth > maxWidth)
            {
                _autocompleteWidth = maxWidth;
            }

            _autocompleteActive = true;
            _autocompleteLeft = _cursorIndex + _prompt.Length + 0 - _searchBuffer.Length;
            _autocompleteTop = Console.CursorTop + 1;

            _autoCompleteHeight = _matches.Length < GetMaxResults() ?
                _matches.Length :
                GetMaxResults() + 1;

            if (maxWidth < _autocompleteWidth)
            {
                _autocompleteWidth = maxWidth;
            }

            int tmpLeft = Console.CursorLeft;

            var linesNeeded = _autoCompleteHeight + Console.CursorTop + 1 - Console.BufferHeight;
            var t = Console.CursorTop;

            if (linesNeeded > 0)
            {
                oldTop -= linesNeeded;
                t -= linesNeeded;
            }

            for (var i = 0; i < linesNeeded; i++)
            {
                Console.WriteLine();
            }

            Console.SetCursorPosition(tmpLeft, t);

            var entryNum = -1;
            var linesDrawn = 0;

            foreach (var n in _matches)
            {
                entryNum++;

                if (_autocompleteIndex - entryNum > GetMaxResults() ||
                    linesDrawn > GetMaxResults())
                {
                    continue;
                }

                var top = Console.CursorTop + 1;
                Console.SetCursorPosition(_autocompleteLeft, top);

                if (_autocompleteIndex != entryNum)
                {
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.White;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.Blue;
                }

                // Hack, until Cli tokenizer is rewritten and exposed
                // so string can be scanned and truncated precisely.
                var viewLen = Cli.EraseStyles(n.View).Length;
                var viewStr = viewLen <= maxWidth ? n.View : n.View.Remove(maxWidth);
                var space = _autocompleteWidth - (viewLen <= maxWidth ? viewLen : maxWidth);
                Console.Write(viewStr + (space > 0 ? new string(' ', space) : ""));
                linesDrawn++;
            }

            Console.ResetColor();
            Console.SetCursorPosition(_prompt.Length + _cursorIndex, oldTop);
        }

        private void SetCursor() =>
            Console.SetCursorPosition(_prompt.Length + _cursorIndex, Console.CursorTop);

        private void SetHistoryValue(bool up)
        {
            int index;

            if (up && _history.Count - 1 > _historyIndex)
            {
                index = ++_historyIndex;
            }
            else if (!up && _historyIndex > 0)
            {
                index = --_historyIndex;
            }
            else
            {
                return;
            }

            var historyVal = _history[index];

            if (_consoleBuffer.Length > historyVal.Length)
            {
                Erase(
                    _prompt.Length,
                    Console.CursorTop,
                    _consoleBuffer.Length,
                    1);
            }

            _consoleBuffer = historyVal;
            _cursorIndex = _consoleBuffer.Length;
            DrawText(skipAutocomplete: true);
            SetCursor();
        }

        private void EraseChar(int index)
        {
            _consoleBuffer = _consoleBuffer.Remove(index, 1);
            Console.CursorVisible = false;

            Console.SetCursorPosition(
                _prompt.Length + _consoleBuffer.Length,
                Console.CursorTop);

            Console.Write(" ");
            DrawText();

            Console.SetCursorPosition(
                _prompt.Length + _cursorIndex,
                Console.CursorTop);

            Console.CursorVisible = true;
        }

        private int GetMaxResults() => Math.Min(Console.WindowHeight - 6, MaxListSize);

        private int GetMaxWidth() => Console.WindowWidth - _autocompleteLeft - 1;
    }
}
