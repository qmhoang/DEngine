using System;
using System.Text;
using DEngine.Core;
using libtcod;

namespace DEngine.Extensions {
    public static class ConsoleExtensions {
        public static void DrawColorChar(this TCODConsole console, Point p, char c, TCODColor color) {
            console.putCharEx(p.X, p.Y, c, color, console.getBackgroundColor());
        }

        public static void DrawColorChar(this TCODConsole console, int x, int y, char c, TCODColor color) {
            console.putChar(x, y, c);
            console.setCharForeground(x, y, color);
        }

        public static void PrintColorString(this TCODConsole console, string str, int x, int y, TCODColor fg) {
            PrintColorString(console, str, x, y, fg, console.getCharBackground(x, y), TCODAlignment.LeftAlignment);
        }

        public static void PrintColorString(this TCODConsole console, string str, int x, int y, TCODColor fg, TCODAlignment alignment) {
            PrintColorString(console, str, x, y, fg, console.getCharBackground(x, y), alignment);
        }

        public static void PrintColorString(this TCODConsole console, string str, int x, int y, TCODColor fg, TCODColor bg,
                                            TCODAlignment alignment) {
            TCODColor ofg = console.getForegroundColor();
            TCODColor obg = console.getBackgroundColor();
            console.setForegroundColor(fg);
            console.setBackgroundColor(bg);
            console.printEx(x, y, TCODBackgroundFlag.None, alignment, str);
            console.setForegroundColor(ofg);
            console.setBackgroundColor(obg);
        }

        [Obsolete]
        public static void PrintColorChar(TCODConsole console, char c, int x, int y, TCODColor fg, TCODColor bg) {
            console.putChar(x, y, c);
            console.setCharBackground(x, y, bg);
            console.setCharForeground(x, y, fg);
        }

        [Obsolete]
        public static void PrintColorCharFg(TCODConsole console, char c, int x, int y, TCODColor fg) {
            console.putChar(x, y, c);
            console.setCharForeground(x, y, fg);
        }

        [Obsolete]
        public static void PrintColorCharBg(TCODConsole console, char c, int x, int y, TCODColor bg) {
            console.putChar(x, y, c);
            console.setCharBackground(x, y, bg);
        }

        // Examples: PrintTCODColorFormattedString("This string has a {red} color", ..., TCODTCODColor.Red, TCODTCODColor.white);
        //TODO add escape chars
        public static void PrintColorFormattedString(this TCODConsole console, string str, int x, int y, TCODAlignment alignment,
                                                     params object[] args) {
            var sb = new StringBuilder(str);
            sb.Replace("{", "");
            sb.Replace("}", "");
            console.printEx(x, y, TCODBackgroundFlag.None, alignment, sb.ToString());

            // reset x accounting for the missing { and } so that it starts from the leftmost position
            if (alignment == TCODAlignment.RightAlignment)
                x = x - sb.Length + 1;
            else if (alignment == TCODAlignment.CenterAlignment)
                x = x - sb.Length / 2;

            int start = 0, counter = 0;
            while (true) {
                int ptr = str.IndexOf('{', start) + 1;
                start = ptr;

                if (ptr < 1)
                    break;

                while (str[ptr++] != '}') {
                    int xpos = x + ptr - counter * 2 - 2;
                    if (args[counter] is Pair<TCODColor, TCODColor>) {
                        console.setCharForeground(xpos, y,
                                                  ((Pair<TCODColor, TCODColor>) args[counter]).First);
                        console.setCharBackground(xpos, y, ((Pair<TCODColor, TCODColor>) args[counter]).Second);
                    } else if (args[counter] is TCODColor)
                        console.setCharForeground(xpos, y, (TCODColor) args[counter]);
                }
                counter++;
            }
        }

        public static void PrintColorFormattedString(this TCODConsole console, string str, int x, int y, TCODAlignment alignment,
                                                     params Tuple<TCODColor, TCODColor>[] args) {
            var sb = new StringBuilder(str);
            sb.Replace("{", "");
            sb.Replace("}", "");
            console.printEx(x, y, TCODBackgroundFlag.None, alignment, sb.ToString());

            // reset x accounting for the missing { and } so that it starts from the leftmost position
            if (alignment == TCODAlignment.RightAlignment)
                x = x - sb.Length + 1;
            else if (alignment == TCODAlignment.CenterAlignment)
                x = x - sb.Length / 2;

            int start = 0, counter = 0;
            while (true) {
                int ptr = str.IndexOf('{', start) + 1;
                start = ptr;

                if (ptr < 1)
                    break;

                while (str[ptr++] != '}') {
                    int xpos = x + ptr - counter * 2 - 2;

                    if (args[counter].Item1 == null)
                        console.setCharBackground(xpos, y, args[counter].Item2);
                    else if (args[counter].Item2 == null)
                        console.setCharForeground(xpos, y, args[counter].Item1);
                    else {
                        console.setCharBackground(xpos, y, args[counter].Item2);
                        console.setCharForeground(xpos, y, args[counter].Item1);
                    }
                }
                counter++;
            }
        }

        public static void PrintColorFormattedStringRect(TCODConsole console, string str, int x, int y, TCODAlignment alignment,
                                                         params object[] args) {}
    }
}