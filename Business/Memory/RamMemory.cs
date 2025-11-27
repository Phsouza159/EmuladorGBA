using EmuladorGBA.Business.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmuladorGBA.Business.Memory
{
    public class RamMemory 
    {
        private byte[] Data { get; set; }

        public RamMemory(MemoryMap memoryMap)
        {
            this.Config(memoryMap);
        }

        private void Config(MemoryMap memoryMap)
        {
            this.Data = new byte[memoryMap.Length];
        }

        public void WriteMemory(ushort adress, byte value)
        {
            if (Data.Length > (int)adress)
                throw new ArgumentException("Erro de gravação de memoria");

            this.Data[adress] = value;
        }

        public void WriteMemory(ushort adress, byte[] values)
        {
            for (int i = 0; i < values.Length; i++)
            {
                if (Data.Length > ((int)adress + i))
                    throw new ArgumentException("Erro de gravação de memoria");

                this.Data[adress + i] = values[i];
            }
        }
    }
}
