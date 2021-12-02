/*
 * PROJECT:         Dusk Operating System Development
 * CONTENT          System/Utilities/SystemInfomation.cs
 * PROGRAMMERS:     
 *                  ProfessorDJ/John Welsh <djlw78@gmail.com>
 *
 */

using Cosmos.Core;

namespace DuskOS.System.Utilities
{
    public class SystemInfomation
    {

        //computer name

        //host info (computer name)

        //set computer name

        //networking mac address (DO LAST)

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetAmountOfRAM(string type)
        {
            //check if amount is under "GB" display unhead of info for under "GB" such as
            // can't calculate because it doesn't support a float, or add passive float support.
            
            /*
             * Megabytes and Gigabytes technically aren't 1024, they're 1000. MiB and GiB are 1024.
             * You can verify this with a simple google search, or by using the google conversion calculator. In school we've always been taught converting MB to GB is 1024 and that's wrong and sometimes not even programmers remember MiB as Windows and Mac use GB and MB incorrectly, but Linux uses MiB which is the correct conversion.
             */
            
            uint KB = CPU.GetAmountOfRAM() * 1024; //Incorrect.
            uint MB = CPU.GetAmountOfRAM();
            uint GB = CPU.GetAmountOfRAM() / 1024; //Incorrect.
            switch (type)
            {
                case "KB":
                    return KB + "KB";
                case "MB":
                    return MB + "MB";
                case "GB":
                    return GB + "GB";
                default:
                    return MB + "MB";
            
            }
        }
    }

    public enum CapacityType
    {
        Gigabyte, //1000
        Megabyte, //1000
        Kilobyte, //1000
        Gibibyte, //1024
        Mebibyte, //1024
        Kibibyte //1024
    }
}
