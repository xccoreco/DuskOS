/*
 * PROJECT:         Dusk Operating System Development
 * CONTENT          System/Utilities/Encryption.cs
 * PROGRAMMERS:     
 *                  ProfessorDJ/John Welsh <djlw78@gmail.com>
 *
 */

using System;

namespace DuskOS.System.Utilities
{
    public static class Encryption
    {
        
        /*
         * For reference this DOES work, but it is weird characters when converting.
         * But to encrypt a string or something it's a pretty decent method.
         * 
         */
        internal static string Caesar(this string source, Int16 shift)
        {
            var maxChar = Convert.ToInt32(char.MaxValue);
            var minChar = Convert.ToInt32(char.MinValue);

            var buffer = source.ToCharArray();

            for (var i = 0; i < buffer.Length; i++)
            {
                var shifted = Convert.ToInt32(buffer[i]) + shift;

                if (shifted > maxChar)
                {
                    shifted -= maxChar;
                }
                else if (shifted < minChar)
                {
                    shifted += maxChar;
                }

                buffer[i] = Convert.ToChar(shifted);
            }

            return new string(buffer);
        }
    }
}
