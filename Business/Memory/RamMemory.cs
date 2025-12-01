using EmuladorGBA.Business.Config;
using EmuladorGBA.Business.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmuladorGBA.Business.Memory
{
    public class RamMemory : IRamMemory
    {
        private byte[] HRAM { get; set; }

        private byte[] WRAM { get; set; }


        public RamMemory(MemoryMap memoryMap)
        {
            this.WRAM = new byte[memoryMap.WRAM_Length];
            this.HRAM = new byte[memoryMap.HRAM_Length];
        }

        #region READ

        public byte ReadMemoryWRam(ushort address)
        {
            address -= 0xC000;

            if ((int)address > WRAM.Length)
            {
                Console.WriteLine($"Registro WRITE WRAM Fora do registro: {address:X2}");
                return 0;
            }

            return this.WRAM[address];
        }

        public byte ReadMemoryHRam(ushort address)
        {
            address -= 0xC000;

            if ((int)address > HRAM.Length)
            {
                Console.WriteLine($"Registro WRITE HRAM Fora do registro: {address:X2}");
                return 0;
            }


            return this.HRAM[address];
        }

        #endregion

        #region WRITE

        public void WriteMemoryWRam(ushort address, byte value)
        {
            address -= 0xC000;

            if ((int)address > WRAM.Length)
            {
                Console.WriteLine($"Registro WRITE WRAM Fora do registro: {address:X2}");
                return;
            }

            this.WRAM[address] = value;
        }

        public void WriteMemoryHRam(ushort address, byte value)
        {
            if ((int)address > HRAM.Length)
            {
                Console.WriteLine($"Registro WRITE WRAM Fora do registro: {address:X2}");
                return;
            }

            this.HRAM[address] = value;
        }

        #endregion
    }
}
