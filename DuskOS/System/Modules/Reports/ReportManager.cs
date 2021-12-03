/*
 * PROJECT:         Dusk Operating System Development
 * CONTENT          System/Modules/Reports/ReportManager.cs
 * PROGRAMMERS:     
 *                  ProfessorDJ/John Welsh <djlw78@gmail.com>
 *
 */

using System;
using System.Collections.Generic;
using System.Text;
using Cosmos.HAL.BlockDevice;

namespace DuskOS.System.Modules.Reports
{
    public class ReportManager
    {
        /*
         * ReportManager ---
         *  - Save Report
         *  - Open Report
         *  - Delete Report
         *  - Update Report
         *  After networking ---
         *   - Upload
         *   - Download [??]
         *
         * Report Manager User ---
         *  - add user functions for these
         *  - add system functions
         */
        private static string GetTimeStamp()
        {
            return DateTime.Now.ToString();
        }
    }
}
