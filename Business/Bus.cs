using EmuladorGBA.Business.Interface;
using EmuladorGBA.Business.Memory;
using EmuladorGBA.Business.Util;
using System.Net;

namespace EmuladorGBA.Business
{
    internal class Bus : IBus
    {
        // 0x0000 - 0x3FFF : ROM Bank 0
        // 0x4000 - 0x7FFF : ROM Bank 1 - Switchable
        // 0x8000 - 0x97FF : CHR RAM
        // 0x9800 - 0x9BFF : BG Map 1
        // 0x9C00 - 0x9FFF : BG Map 2
        // 0xA000 - 0xBFFF : Cartridge RAM
        // 0xC000 - 0xCFFF : RAM Bank 0
        // 0xD000 - 0xDFFF : RAM Bank 1-7 - switchable - Color only
        // 0xE000 - 0xFDFF : Reserved - Echo RAM
        // 0xFE00 - 0xFE9F : Object Attribute Memory
        // 0xFEA0 - 0xFEFF : Reserved - Unusable
        // 0xFF00 - 0xFF7F : I/O Registers
        // 0xFF80 - 0xFFFE : Zero Page

        public Bus(ICart Cart, IRamMemory ramMemory, ICpu cpu)
        {
            this.Cart = Cart;
            this.Memory = ramMemory;
            this.Cpu = cpu;
        }

        private ICart Cart { get; set; }

        private IRamMemory Memory { get; set; }

        private ICpu Cpu { get; set; }

        private bool IsLog = true;

        #region READ

        public byte Read(ushort address)
        {
            byte value = 0;

            if (address < 0x8000)
            {
                //ROM Data
                value = this.Cart.Read(address);
            } 
            else if (address < 0xA000)
            {
                // CHAR/MAP DATA 
                // TODO
                ConsoleUtil.ShowMensagemNotImplement();
                Console.WriteLine($"Sem suporte para leitura em {address:4X}");
            }
            else if (address < 0xC000)
            {
                // Cartridge RAM
                value = this.Cart.Read(address);
            }
            else if (address < 0xE000)
            {
                // WRAM
                value = this.Memory.ReadMemoryWRam(address);
            }
            else if (address < 0xFE00)
            {
                // RESERVED
                value = 0;
            }
            else if (address < 0xFEA0)
            {
                // Object Attribute Memory
                // TODO
                ConsoleUtil.ShowMensagemNotImplement();
                Console.WriteLine($"Sem suporte para leitura em {address:4X}");

            }
            else if (address < 0xFF00)
            {
                // RESERVED
                value = 0;
            }
            else if (address < 0xFF80)
            {
                // IO Registers
                // TODO
                ConsoleUtil.ShowMensagemNotImplement();
                Console.WriteLine($"Sem suporte para leitura em {address:4X}");
            }
            else if (address == 0xFFFF)
            {
                // CPU REGISTERS
                value = this.Cpu.CpuGetRegisterIE();
            }
            else
            {
                value = this.Memory.ReadMemoryHRam(address);
            }

            if(this.IsLog)
                Console.WriteLine($"* READ  in '{address.ToString("X2"), 5}' VALUE '{value.ToString("X2"), 5}'");
            
            return value;
        }

        public ushort ReadB16(ushort address)
        {
            byte lo = this.Read(address);
            byte hi = this.Read((ushort)(address + 1));

            return (ushort)(lo | (hi << 8));
        }

        #endregion

        #region WRITE

        public void Write(ushort address, byte value)
        {

            if (this.IsLog)
                Console.WriteLine($"* WRITE in '{address.ToString("X2"),5}' VALUE '{value.ToString("X2"),5}'");

            if (address < 0x8000)
            {
                //ROM Data
                this.Cart.Write(address, value);
            }
            else if (address < 0xA000)
            {
                // CHAR/MAP DATA 
                // TODO
                Console.WriteLine($"Sem suporte para WRITE em CHAR/MAP DATA  {address:X2}");
            }
            else if (address < 0xC000)
            {
                // Cartridge EXTRA RAM
                this.Cart.Write(address, value);
            }
            else if (address < 0xE000)
            {
                // WRAM
                this.Memory.WriteMemoryWRam(address, value);
            }
            else if (address < 0xFE00)
            {
                // RESERVED
            }
            else if (address < 0xFEA0)
            {
                // Object Attribute Memory
                // TODO
                Console.WriteLine($"Sem suporte para WRITE em Object Attribute Memory {address:X2}");
            }
            else if (address < 0xFF00)
            {
                // RESERVED
            }
            else if (address < 0xFF80)
            {
                // IO Registers
                // TODO
                Console.WriteLine($"Sem suporte para WRITE em IO Registers {address:X2}");
            }
            else if (address == 0xFFFF)
            {
                // CPU REGISTERS
                this.Cpu.CpuSetRegisterIE(value);
            }
            else
            {
                this.Memory.WriteMemoryHRam(address, value);
            }
        }

        public void WriteB16(ushort address, ushort value)
        {
            // high byte
            this.Cart.Write((ushort)(address + 1), (byte)((value >> 8) & 0xFF));
            // low byte
            this.Cart.Write(address, (byte)(value & 0xFF));
        }

        #endregion
    }
}
