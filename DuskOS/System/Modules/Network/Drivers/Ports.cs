/*
 * PROJECT:         Dusk Operating System Development
 * CONTENT          System/Modules/Network/Drivers/Ports.cs
 * PROGRAMMERS:     
 *                  ProfessorDJ/John Welsh <djlw78@gmail.com>
 * NOTES:
 *                  This person didn't have a copyright notice but it was Misha
 */

using Cosmos.Core;

namespace DuskOS.System.Modules.Network.Drivers
{
    public static class Ports
    {
        /// <summary>
        /// Reads a byte
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        public static byte InB(ushort port)
        {
            var io = new IOPort(port);
            return io.Byte;
        }

        /// <summary>
        /// Reads a 32 bit word
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        public static uint InD(ushort port)
        {
            var io = new IOPort(port);
            return io.DWord;
        }

        /// <summary>
        /// Reads a 16 bit word
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        public static ushort InW(ushort port)
        {
            var io = new IOPort(port);
            return io.Word;
        }

        /// <summary>
        /// Writes a byte
        /// </summary>
        /// <param name="port"></param>
        /// <param name="data"></param>
        public static void OutB(ushort port, byte data)
        {
            var io = new IOPort(port);
            io.Byte = data;
        }

        /// <summary>
        /// Writes a 32 bit word
        /// </summary>
        /// <param name="port"></param>
        /// <param name="data"></param>
        public static void OutD(ushort port, byte data)
        {
            var io = new IOPort(port);
            io.DWord = data;
        }

        /// <summary>
        /// Writes a 32 bit word
        /// </summary>
        /// <param name="port"></param>
        /// <param name="data"></param>
        public static void OutD(ushort port, uint data)
        {
            var io = new IOPort(port);
            io.DWord = data;
        }

        /// <summary>
        /// Writes a 16 bit word
        /// </summary>
        /// <param name="port"></param>
        /// <param name="data"></param>
        public static void OutW(ushort port, ushort data)
        {
            var io = new IOPort(port);
            io.Word = data;
        }
    }
}
