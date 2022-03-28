using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuskOSDev.DuskSystem.Utilities
{
    public class AlphaNumericGenerator
    {
        public enum CharFormat
        {
            UPPERCASE_LOWERCASE,
            UPPERCASE_ONLY,
            LOWERCASE_ONLY
        }

        private readonly char[] charmap;

        public AlphaNumericGenerator(int maxLength) : this(maxLength, CharFormat.UPPERCASE_LOWERCASE) { }

        public AlphaNumericGenerator(int maxLength, CharFormat frmt = CharFormat.UPPERCASE_LOWERCASE)
        {

        }

        public AlphaNumericGenerator(char[] charmap)
        {
            if (charmap == null)
                throw new ArgumentNullException("charmap", "The character map cannot be null.");
            if (charmap.Length <= 0)
                throw new ArgumentOutOfRangeException("charmap", "The charmap cannot be empty, it must contain at least on value.");
            this.charmap = charmap;
        }
    }
}
