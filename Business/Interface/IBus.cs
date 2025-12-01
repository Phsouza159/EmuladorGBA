using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmuladorGBA.Business.Interface
{
    public interface IBus
    {
        byte Read(ushort address);

        ushort ReadB16(ushort address);

        void Write(ushort address, byte value);

        void WriteB16(ushort address, ushort value);
    }
}
