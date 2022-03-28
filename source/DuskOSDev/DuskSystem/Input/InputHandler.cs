using DuskOSDev.DuskSystem.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuskOSDev.DuskSystem.Input
{
    public class InputHandler
    {
        public static string HandleInput(string label, bool hideInput = false)
        {
            if (hideInput)
            {
                StringBuilder builder = new StringBuilder();
                Console.Write(label);
                while (true)
                {
                    var k = Console.ReadKey(true);
                    var c = k.KeyChar;
                    if (char.IsLetterOrDigit(c) || char.IsWhiteSpace(c) || char.IsSymbol(c) || char.IsPunctuation(c))
                        builder.Append(c);
                    else if (k.Key == ConsoleKey.Backspace)
                    {
                        if (builder.Length > 0)
                            builder.Remove(builder.Length - 1, 1);
                    }
                    else if (k.Key == ConsoleKey.Enter)
                        break;
                }
                return builder.ToString();
            }
            else
                return Console.ReadLine();
        }

        public static bool CheckInput(string label, string value, bool hideInput, out string result)
        {
            result = HandleInput(label);
            return !result.IsNullWhiteSpaceOrEmpty();
        }
    }
}
