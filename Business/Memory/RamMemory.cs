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

        private MemoryMap MemoryMap;

        public RamMemory(MemoryMap memoryMap)
        {
            this.WRAM = new byte[memoryMap.WRAM_Length];
            this.HRAM = new byte[memoryMap.HRAM_Length];
            this.MemoryMap = memoryMap;
        }

        #region W-RAM

        public void WriteMemoryWRam(ushort address, byte value)
        {
            address -= 0xC000;

            if ((int)address > this.MemoryMap.WRAM_Length)
            {
                Console.WriteLine($"Registro WRITE WRAM Fora do registro: {address:X2}");
                return;
            }

            this.WRAM[address] = value;
        }

        public byte ReadMemoryWRam(ushort address)
        {
            address -= 0xC000;

            if ((int)address > this.MemoryMap.WRAM_Length)
            {
                Console.WriteLine($"Registro WRITE WRAM Fora do registro: {address:X2}");
                throw new ArgumentException($"READ RAM IN {address:X2} ERRO");
            }

            return this.WRAM[address];
        }

        #endregion

        #region H-RAM
    
        public void WriteMemoryHRam(ushort address, byte value)
        {
            address -= 0xFF80;

            if ((int)address > this.MemoryMap.HRAM_Length)
            {
                Console.WriteLine($"Registro WRITE HRAM Fora do registro: {address:X2}");
                return;
            }

            this.HRAM[address] = value;
        }
        public byte ReadMemoryHRam(ushort address)
        {
            address -= 0xFF80;

            if ((int)address > this.MemoryMap.HRAM_Length)
            {
                Console.WriteLine($"Registro WRITE HRAM Fora do registro: {address:X2}");
                return 0;
            }


            return this.HRAM[address];
        }

        #endregion
    }
}
