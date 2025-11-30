using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmuladorGBA.Business.Interface
{
    internal interface ICart
    {
        byte Read(ushort adress);

        void Write(ushort adress, byte value);
    }
}
