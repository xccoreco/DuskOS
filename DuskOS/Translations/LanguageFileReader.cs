/*
 * PROJECT:         Dusk Operating System Development
 * CONTENT          Translations/LanguageFileReader.cs
 * PROGRAMMERS:     
 *                  ProfessorDJ/John Welsh <djlw78@gmail.com>
 *
 */

using System;
using System.Text;

namespace DuskOS.Translations
{
    public static class LanguageFiles
    {
        /*
         * BASE64 OF ALL LANGUAGE FILES FOR BACKUP
         * 
         * 
         */
        static string en_US = "ZXhhbXBsZQ==";

        public static string test()
        {
            byte[] data = Convert.FromBase64String(en_US);
            return Encoding.UTF8.GetString(data);
        }
    }
    class LanguageFileReader
    {
    }
}
