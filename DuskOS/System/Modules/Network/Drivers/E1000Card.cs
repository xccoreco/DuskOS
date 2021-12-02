﻿using Cosmos.Core;
using Cosmos.HAL;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Cosmos.HAL.Network;

namespace DuskOS.System.Modules.Network.Drivers
{
    public unsafe class Intel8254X : NetworkDevice
    {
        private uint BAR0;
        private uint RXDescs;
        private uint TXDescs;
        public uint RXCurr = 0;
        public uint TXCurr = 0;
        private MACAddress address;
        private PCIDevice dev;
        public override CardType CardType => CardType.Ethernet;
        public override MACAddress MACAddress => address;
        public override string Name => "Intel Gigabit Ethernet";
        public override bool Ready => true;

        //Register defenitions
        private const ushort REG_CTRL = 0x0000;
        private const ushort REG_STATUS = 0x0008;
        private const ushort REG_EEPROM = 0x0014;
        private const ushort REG_IMASK = 0x00D0;

        //TX
        private const ushort REG_TCTRL = 0x0400;
        private const ushort REG_TXDESCLO = 0x3800;
        private const ushort REG_TXDESCHI = 0x3804;
        private const ushort REG_TXDESCLEN = 0x3808;
        private const ushort REG_TXDESCHEAD = 0x3810;
        private const ushort REG_TXDESCTAIL = 0x3818;

        //RX
        private const ushort REG_RCTRL = 0x0100;
        private const ushort REG_RXDESCLO = 0x2800;
        private const ushort REG_RXDESCHI = 0x2804;
        private const ushort REG_RXDESCLEN = 0x2808;
        private const ushort REG_RXDESCHEAD = 0x2810;
        private const ushort REG_RXDESCTAIL = 0x2818;

        private const ushort REG_TIPG = 0x0410;
        public Intel8254X(PCIDevice dev)
        {
            this.dev = dev;
            dev.EnableDevice();
            BAR0 = (uint)(dev.BAR0 & (~3));
            INTs.SetIrqHandler(dev.InterruptLine, HandleIRQ);

            var HasEEPROM = DetectEEPROM();

            //Read the mac address
            if (!HasEEPROM)
            {
                address = new MACAddress(new byte[] { Read1ByteRegister(BAR0 + 0x5400), Read1ByteRegister(BAR0 + 0x5401), Read1ByteRegister(BAR0 + 0x5402), Read1ByteRegister(BAR0 + 0x5403), Read1ByteRegister(BAR0 + 0x5404), Read1ByteRegister(BAR0 + 0x5405) });
            }
            else
            {
                var firstWord = ReadROM(0);
                var secondWord = ReadROM(1);
                var thirdWord = ReadROM(2);
                address = new MACAddress(new byte[] {
                    (byte)(firstWord & 0xFF),
                    (byte)(firstWord >> 8),
                    (byte)(secondWord & 0xFF) ,
                    (byte)(secondWord >> 8),
                    (byte)(thirdWord & 0xFF),
                    (byte)(thirdWord >> 8)});
            }

            LinkUp();

            //zero out the MTA (multicast tabel array)
            for (int i = 0; i < 0x80; i++)
                WriteRegister((ushort)(0x5200 + i * 4), 0);

            //Enable interupts
            WriteRegister(REG_IMASK, 0x1F6DC);
            WriteRegister(REG_IMASK, 0xFF & ~4);
            ReadRegister(0xC0);

            RXInitialize();
            TXInitialize();
        }

        private void LinkUp()
        {
            //Turn link up
            WriteRegister(REG_CTRL, ReadRegister(REG_CTRL) | 0x40);
        }

        private bool DetectEEPROM()
        {
            //Detect EPROM
            WriteRegister(REG_EEPROM, 0x1);

            bool HasEEPROM = false;
            for (int i = 0; i < 1024 && !HasEEPROM; i++)
            {
                if ((ReadRegister(REG_EEPROM) & 0x10) != 0)
                {
                    HasEEPROM = true;
                    break;
                }
            }
            return HasEEPROM;
        }

        private void HandleIRQ(ref INTs.IRQContext aContext)
        {
            uint Status = ReadRegister(0xC0);

            if ((Status & 0x04) != 0)
            {
                //Turn link up
                LinkUp();
            }
            if ((Status & 0x10) != 0)
            {
                //Good threshold
            }

            if ((Status & 0x80) != 0)
            {
                uint _RXCurr = RXCurr;
                RXDesc* desc = (RXDesc*)(RXDescs + (RXCurr * 16));
                while ((desc->status & 0x1) != 0)
                {
                    byte[] data = new byte[desc->length];
                    byte* ptr = (byte*)desc->addr;
                    for (int i = 0; i < desc->length; i++)
                    {
                        data[i] = ptr[i];
                    }
                    DataReceived(data);

                    desc->status = 0;
                    RXCurr = (RXCurr + 1) % 32;
                    WriteRegister(0x2818, _RXCurr);
                }
            }
        }
        private void RXInitialize()
        {
            var tmp = Cosmos.Core.Memory.Old.Heap.MemAlloc(32 * 16 + 16);
            RXDescs = (tmp % 16 != 0) ? (tmp + 16 - (tmp % 16)) : tmp;

            for (uint i = 0; i < 32; i++)
            {
                RXDesc* desc = (RXDesc*)(RXDescs + (i * 16));
                desc->addr = Cosmos.Core.Memory.Old.Heap.MemAlloc(2048 + 16);
                desc->status = 0;
            }


            WriteRegister(REG_RXDESCLO, RXDescs);
            WriteRegister(REG_RXDESCHI, 0);

            WriteRegister(REG_RXDESCLEN, 32 * 16);

            WriteRegister(REG_RXDESCHEAD, 0);
            WriteRegister(REG_RXDESCTAIL, 32 - 1);

            WriteRegister(REG_RCTRL,
                     (1 << 1) | // Receiver Enable
                     (1 << 2) | // Store Bad Packets
                     (1 << 3) | // Unicast Promiscuous Enabled
                     (1 << 4) | // Multicast Promiscuous Enabled
                     (0 << 6) | // No Loopback
                     (0 << 8) | // Free Buffer Threshold is 1/2 of RDLEN
                    (1 << 15) | // Broadcast Accept Mode
                    (1 << 26) | // Strip Ethernet CRC
                    (0 << 16) //Buffer size of 2048 bytes
                );
        }
        private void TXInitialize()
        {
            var tmp = (uint)Cosmos.Core.Memory.Old.Heap.MemAlloc(8 * 16 + 16);
            TXDescs = (tmp % 16 != 0) ? (tmp + 16 - (tmp % 16)) : tmp;

            for (int i = 0; i < 8; i++)
            {
                TXDesc* desc = (TXDesc*)(TXDescs + (i * 16));
                desc->addr = 0;
                desc->cmd = 0;
            }

            WriteRegister(REG_TXDESCLO, TXDescs);
            WriteRegister(REG_TXDESCHI, 0);

            WriteRegister(REG_TXDESCLEN, 8 * 16);
            WriteRegister(REG_TXDESCHEAD, 0);
            WriteRegister(REG_TXDESCTAIL, 0);

            //Set the transmit control register (padshortpackets)
            //E1000 only
            //WriteRegister(REG_TCTRL, (1 << 1) | (1 << 3));

            //For the e1000e
            WriteRegister(REG_TCTRL, 0b0110000000000111111000011111010);
            WriteRegister(REG_TIPG, 0x0060200A);

            WriteRegister(REG_TXDESCTAIL, 0);
        }
        private byte Read1ByteRegister(uint a)
        {
            if (dev.BaseAddressBar[0].IsIO)
            {
                return new IOPort((ushort)a).Byte;
            }
            else
            {
                return *(byte*)(a);
            }
        }
        public void WriteRegister(ushort reg, uint val)
        {
            if (dev.BaseAddressBar[0].IsIO)
            {
                new IOPort((ushort)(BAR0 + reg)).DWord = val;
            }
            else
            {
                *(uint*)(BAR0 + reg) = val;
            }
        }
        public uint ReadRegister(ushort reg)
        {
            if (dev.BaseAddressBar[0].IsIO)
            {
                return new IOPort((ushort)(BAR0 + reg)).DWord;
            }
            else
            {
                return *(uint*)(BAR0 + reg);
            }
        }
        public ushort ReadROM(uint Addr)
        {
            uint Temp;
            WriteRegister(REG_EEPROM, 1 | (Addr << 8));
            while (((Temp = ReadRegister(REG_EEPROM)) & 0x10) == 0) ;
            return ((ushort)((Temp >> 16) & 0xFFFF));
        }

        public override bool QueueBytes(byte[] buffer, int offset, int length)
        {
            //send bytes

            //Copy Buffer[offset] to ptr
            byte* ptr = (byte*)Cosmos.Core.Memory.Old.Heap.MemAlloc((uint)length);
            int a = 0;
            for (int i = offset; i < offset + length; i++)
            {
                ptr[a] = buffer[i];
                a++;
            }

            TXDesc* desc = (TXDesc*)(TXDescs + (TXCurr * 16));
            desc->addr = (ulong)ptr;
            desc->length = (ushort)length;
            desc->cmd = (1 << 0) | (1 << 1) | (1 << 3);
            desc->status = 0;

            byte _TXCurr = (byte)TXCurr;
            TXCurr = (TXCurr + 1) % 8;
            WriteRegister(REG_TXDESCTAIL, TXCurr);
            int timeout = 5000;
            while ((desc->status & 0xF) == 0)
            {
                if (timeout <= 0)
                {
                    //This is most likly a driver issue
                    return false;
                }
                timeout--;
            }
            return true;
        }
        public override bool ReceiveBytes(byte[] buffer, int offset, int max)
        {
            throw new NotImplementedException();
        }
        public override byte[] ReceivePacket()
        {
            throw new NotImplementedException();
        }
        public override int BytesAvailable()
        {
            throw new NotImplementedException();
        }
        public override bool Enable()
        {
            return true;
        }
        public override bool IsSendBufferFull()
        {
            return false;
        }
        public override bool IsReceiveBufferFull()
        {
            return false;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct RXDesc
        {
            public ulong addr;
            public ushort length;
            public ushort checksum;
            public byte status;
            public byte errors;
            public ushort special;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct TXDesc
        {
            public ulong addr;
            public ushort length;
            public byte cso;
            public byte cmd;
            public byte status;
            public byte css;
            public ushort special;
        }

        #region Utils
        public static List<Intel8254X> FindAll()
        {
            List<Intel8254X> l = new List<Intel8254X>();
            foreach (var dev in PCI.Devices)
            {
                if (dev.VendorID == 0x8086)
                {
                    if (
                       dev.DeviceID == (ushort)DeviceID.Intel82542 ||
                       dev.DeviceID == (ushort)DeviceID.Intel82543GC ||
                       dev.DeviceID == (ushort)DeviceID.Intel82543GC_1 ||
                       dev.DeviceID == (ushort)DeviceID.Intel82544EI ||
                       dev.DeviceID == (ushort)DeviceID.Intel82544EI_1 ||
                       dev.DeviceID == (ushort)DeviceID.Intel82543EI ||
                       dev.DeviceID == (ushort)DeviceID.Intel82544GC ||
                       dev.DeviceID == (ushort)DeviceID.Intel82540EM ||
                       dev.DeviceID == (ushort)DeviceID.Intel82545EM ||
                       dev.DeviceID == (ushort)DeviceID.Intel82546EB ||
                       dev.DeviceID == (ushort)DeviceID.Intel82545EM_1 ||
                       dev.DeviceID == (ushort)DeviceID.Intel82546EB_1 ||
                       dev.DeviceID == (ushort)DeviceID.Intel82541EI ||
                       dev.DeviceID == (ushort)DeviceID.Intel82541ER ||
                       dev.DeviceID == (ushort)DeviceID.Intel82540EM_1 ||
                       dev.DeviceID == (ushort)DeviceID.Intel82540EP ||
                       dev.DeviceID == (ushort)DeviceID.Intel82540EP_1 ||
                       dev.DeviceID == (ushort)DeviceID.Intel82541EI_1 ||
                       dev.DeviceID == (ushort)DeviceID.Intel82547EI ||
                       dev.DeviceID == (ushort)DeviceID.Intel82547EI_1 ||
                       dev.DeviceID == (ushort)DeviceID.Intel82546EB_2 ||
                       dev.DeviceID == (ushort)DeviceID.Intel82540EP_2 ||
                       dev.DeviceID == (ushort)DeviceID.Intel82545GM ||
                       dev.DeviceID == (ushort)DeviceID.Intel82545GM_1 ||
                       dev.DeviceID == (ushort)DeviceID.Intel82545GM_2 ||
                       dev.DeviceID == (ushort)DeviceID.Intel82566MM_ICH8 ||
                       dev.DeviceID == (ushort)DeviceID.Intel82566DM_ICH8 ||
                       dev.DeviceID == (ushort)DeviceID.Intel82566DC_ICH8 ||
                       dev.DeviceID == (ushort)DeviceID.Intel82562V_ICH8 ||
                       dev.DeviceID == (ushort)DeviceID.Intel82566MC_ICH8 ||
                       dev.DeviceID == (ushort)DeviceID.Intel82571EB ||
                       dev.DeviceID == (ushort)DeviceID.Intel82571EB_1 ||
                       dev.DeviceID == (ushort)DeviceID.Intel82571EB_2 ||
                       dev.DeviceID == (ushort)DeviceID.Intel82547EI_2 ||
                       dev.DeviceID == (ushort)DeviceID.Intel82541GI ||
                       dev.DeviceID == (ushort)DeviceID.Intel82547EI_3 ||
                       dev.DeviceID == (ushort)DeviceID.Intel82541ER_1 ||
                       dev.DeviceID == (ushort)DeviceID.Intel82546EB_3 ||
                       dev.DeviceID == (ushort)DeviceID.Intel82546EB_4 ||
                       dev.DeviceID == (ushort)DeviceID.Intel82546EB_5 ||
                       dev.DeviceID == (ushort)DeviceID.Intel82541PI ||
                       dev.DeviceID == (ushort)DeviceID.Intel82572EI ||
                       dev.DeviceID == (ushort)DeviceID.Intel82572EI_1 ||
                       dev.DeviceID == (ushort)DeviceID.Intel82572EI_2 ||
                       dev.DeviceID == (ushort)DeviceID.Intel82546GB ||
                       dev.DeviceID == (ushort)DeviceID.Intel82573E ||
                       dev.DeviceID == (ushort)DeviceID.Intel82573E_1 ||
                       dev.DeviceID == (ushort)DeviceID.Intel80003ES2LAN ||
                       dev.DeviceID == (ushort)DeviceID.Intel80003ES2LAN_1 ||
                       dev.DeviceID == (ushort)DeviceID.Intel82546GB_1 ||
                       dev.DeviceID == (ushort)DeviceID.Intel82573L ||
                       dev.DeviceID == (ushort)DeviceID.Intel82571EB_3 ||
                       dev.DeviceID == (ushort)DeviceID.Intel82575 ||
                       dev.DeviceID == (ushort)DeviceID.Intel82575_serdes ||
                       dev.DeviceID == (ushort)DeviceID.Intel82546GB_2 ||
                       dev.DeviceID == (ushort)DeviceID.Intel82572EI_3 ||
                       dev.DeviceID == (ushort)DeviceID.Intel80003ES2LAN_2 ||
                       dev.DeviceID == (ushort)DeviceID.Intel80003ES2LAN_3 ||
                       dev.DeviceID == (ushort)DeviceID.Intel82571EB_4 ||
                       dev.DeviceID == (ushort)DeviceID.Intel82566DM_ICH9 ||
                       dev.DeviceID == (ushort)DeviceID.Intel82562GT_ICH8 ||
                       dev.DeviceID == (ushort)DeviceID.Intel82562G_ICH8 ||
                       dev.DeviceID == (ushort)DeviceID.Intel82576 ||
                       dev.DeviceID == (ushort)DeviceID.Intel82574L ||
                       dev.DeviceID == (ushort)DeviceID.Intel82575_quadcopper ||
                       dev.DeviceID == (ushort)DeviceID.Intel82567V_ICH9 ||
                       dev.DeviceID == (ushort)DeviceID.Intel82567LM_4_ICH9 ||
                       dev.DeviceID == (ushort)DeviceID.Intel82577LM ||
                       dev.DeviceID == (ushort)DeviceID.Intel82577LC ||
                       dev.DeviceID == (ushort)DeviceID.Intel82578DM ||
                       dev.DeviceID == (ushort)DeviceID.Intel82578DC ||
                       dev.DeviceID == (ushort)DeviceID.Intel82567LM_ICH9_egDellE6400Notebook ||
                       dev.DeviceID == (ushort)DeviceID.Intel82579LM ||
                       dev.DeviceID == (ushort)DeviceID.Intel82579V ||
                       dev.DeviceID == (ushort)DeviceID.Intel82576NS ||
                       dev.DeviceID == (ushort)DeviceID.Intel82580 ||
                       dev.DeviceID == (ushort)DeviceID.IntelI350 ||
                       dev.DeviceID == (ushort)DeviceID.IntelI210 ||
                       dev.DeviceID == (ushort)DeviceID.IntelI210_1 ||
                       dev.DeviceID == (ushort)DeviceID.IntelI217LM ||
                       dev.DeviceID == (ushort)DeviceID.IntelI217VA ||
                       dev.DeviceID == (ushort)DeviceID.IntelI218V ||
                       dev.DeviceID == (ushort)DeviceID.IntelI218LM ||
                       dev.DeviceID == (ushort)DeviceID.IntelI218LM2 ||
                       dev.DeviceID == (ushort)DeviceID.IntelI218V_1 ||
                       dev.DeviceID == (ushort)DeviceID.IntelI218LM3 ||
                       dev.DeviceID == (ushort)DeviceID.IntelI218V3 ||
                       dev.DeviceID == (ushort)DeviceID.IntelI219LM ||
                       dev.DeviceID == (ushort)DeviceID.IntelI219V ||
                       dev.DeviceID == (ushort)DeviceID.IntelI219LM2 ||
                       dev.DeviceID == (ushort)DeviceID.IntelI219V2 ||
                       dev.DeviceID == (ushort)DeviceID.IntelI219LM3 ||
                       dev.DeviceID == (ushort)DeviceID.IntelI219LM_1 ||
                       dev.DeviceID == (ushort)DeviceID.IntelI219LM_2
                       )
                    {
                        l.Add(new Intel8254X(dev));
                    }
                }
            }
            return l;
        }
        private enum DeviceID
        {
            Intel82542 = 0x1000,
            Intel82543GC = 0x1001,
            Intel82543GC_1 = 0x1004,
            Intel82544EI = 0x1008,
            Intel82544EI_1 = 0x1009,
            Intel82543EI = 0x100C,
            Intel82544GC = 0x100D,
            Intel82540EM = 0x100E,
            Intel82545EM = 0x100F,
            Intel82546EB = 0x1010,
            Intel82545EM_1 = 0x1011,
            Intel82546EB_1 = 0x1012,
            Intel82541EI = 0x1013,
            Intel82541ER = 0x1014,
            Intel82540EM_1 = 0x1015,
            Intel82540EP = 0x1016,
            Intel82540EP_1 = 0x1017,
            Intel82541EI_1 = 0x1018,
            Intel82547EI = 0x1019,
            Intel82547EI_1 = 0x101A,
            Intel82546EB_2 = 0x101D,
            Intel82540EP_2 = 0x101E,
            Intel82545GM = 0x1026,
            Intel82545GM_1 = 0x1027,
            Intel82545GM_2 = 0x1028,
            Intel82566MM_ICH8 = 0x1049,
            Intel82566DM_ICH8 = 0x104A,
            Intel82566DC_ICH8 = 0x104B,
            Intel82562V_ICH8 = 0x104C,
            Intel82566MC_ICH8 = 0x104D,
            Intel82571EB = 0x105E,
            Intel82571EB_1 = 0x105F,
            Intel82571EB_2 = 0x1060,
            Intel82547EI_2 = 0x1075,
            Intel82541GI = 0x1076,
            Intel82547EI_3 = 0x1077,
            Intel82541ER_1 = 0x1078,
            Intel82546EB_3 = 0x1079,
            Intel82546EB_4 = 0x107A,
            Intel82546EB_5 = 0x107B,
            Intel82541PI = 0x107C,
            Intel82572EI = 0x107D,
            Intel82572EI_1 = 0x107E,
            Intel82572EI_2 = 0x107F,
            Intel82546GB = 0x108A,
            Intel82573E = 0x108B,
            Intel82573E_1 = 0x108C,
            Intel80003ES2LAN = 0x1096,
            Intel80003ES2LAN_1 = 0x1098,
            Intel82546GB_1 = 0x1099,
            Intel82573L = 0x109A,
            Intel82571EB_3 = 0x10A4,
            Intel82575 = 0x10A7,
            Intel82575_serdes = 0x10A9,
            Intel82546GB_2 = 0x10B5,
            Intel82572EI_3 = 0x10B9,
            Intel80003ES2LAN_2 = 0x10BA,
            Intel80003ES2LAN_3 = 0x10BB,
            Intel82571EB_4 = 0x10BC,
            Intel82566DM_ICH9 = 0x10BD,
            Intel82562GT_ICH8 = 0x10C4,
            Intel82562G_ICH8 = 0x10C5,
            Intel82576 = 0x10C9,
            Intel82574L = 0x10D3,
            Intel82575_quadcopper = 0x10A9,
            Intel82567V_ICH9 = 0x10CB,
            Intel82567LM_4_ICH9 = 0x10E5,
            Intel82577LM = 0x10EA,
            Intel82577LC = 0x10EB,
            Intel82578DM = 0x10EF,
            Intel82578DC = 0x10F0,
            Intel82567LM_ICH9_egDellE6400Notebook = 0x10F5,
            Intel82579LM = 0x1502,
            Intel82579V = 0x1503,
            Intel82576NS = 0x150A,
            Intel82580 = 0x150E,
            IntelI350 = 0x1521,
            IntelI210 = 0x1533,
            IntelI210_1 = 0x157B,
            IntelI217LM = 0x153A,
            IntelI217VA = 0x153B,
            IntelI218V = 0x1559,
            IntelI218LM = 0x155A,
            IntelI218LM2 = 0x15A0,
            IntelI218V_1 = 0x15A1,
            IntelI218LM3 = 0x15A2,
            IntelI218V3 = 0x15A3,
            IntelI219LM = 0x156F,
            IntelI219V = 0x1570,
            IntelI219LM2 = 0x15B7,
            IntelI219V2 = 0x15B8,
            IntelI219LM3 = 0x15BB,
            IntelI219LM_1 = 0x15D7,
            IntelI219LM_2 = 0x15E3
        }
        #endregion
    }
}
