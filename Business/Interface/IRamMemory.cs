using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmuladorGBA.Business.Interface
{
    public interface IRamMemory
    {
        byte ReadMemoryWRam(ushort address);
        void WriteMemoryWRam(ushort adress, byte value);

        byte ReadMemoryHRam(ushort address);
        void WriteMemoryHRam(ushort address, byte value);
    }
}
