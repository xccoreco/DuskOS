using System;
using System.Collections.Generic;
using System.Text;

namespace DuskOS.System.Log
{
    class LogManager
    {
        /*
        *  Log Commit [Log save]?
        *      - Commits a log to 0:\DuskOS\Logs\<timestamp>.log
        *  Log Fetch
        *      - Fetches a log for reading from 0:\DuskOS\Logs\<timestamp>.log
        *      - Also puts the log in a non system file directory [??]
         *     - maybe fetch for upload [??]
        *  Log Display
        *      - Opens editor and show's the log or cat's it to the user.
        *
        * --after networking--
        * Log Upload
        *      - Uploads a log to duskOS this is internal function private from user
        *        used for uploading a crash report.
         *
         * Log status parser returns bools instead of enum
         * File Manager in logs with dusk fs
         * Log dates
        */
        public LogManager()
        {

        }
    }
}
