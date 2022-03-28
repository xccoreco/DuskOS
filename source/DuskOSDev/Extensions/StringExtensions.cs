using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DuskOSDev.DuskSystem.Security;
namespace DuskOSDev.Extensions
{
    public static class StringExtensions
    {
        public static string Caesar(this string source, short shift)
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

        public static string ToMD5(this string hash)
        {
            return MD5.hash(hash);
        }

        public static string ToSHA256(this string hash)
        {
            return SHA256.hash(hash);
        }
    }
}
