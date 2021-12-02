/*
 * PROJECT:         Dusk Operating System Development
 * CONTENT          Kernel
 * PROGRAMMERS:     
 *                  ProfessorDJ/John Welsh <djlw78@gmail.com>
 *                  WinMister332/Chris Emberley <cemberley@nerdhub.net>
 */

//using DuskResearchKenrel.System.Users;

namespace DuskOS
{
    public enum OSEdition
    {
        /// <summary>
        /// A bare-bones server build of DuskOS with no graphical support except where provided by the console.
        /// </summary>
        BASIC = 0,
        /// <summary>
        /// A server build of DuskOS with limited graphics support. This server build supports up to 16bpp graphics. And unlike desktop builds, has limited theming and personalization features. This build of DuskOS is perfect for anyone who wants a server build, but likes a GUI to help with server configuration.
        /// </summary>
        DESKTOP_BASIC = 1,
        DESKTOP = 2,
        DESKTOP_PROFESSIONAL = 3,
        DESKTOP_ULTIMATE = 4,
        /// <summary>
        /// A specialized version of DuskOS basic for organizations. It combines server features of DuskOS Desktop Basic with DuskOS Desktop Professional and is optimized for speed and security for organizations.
        /// </summary>
        DATACENTER = 5
    }
}


