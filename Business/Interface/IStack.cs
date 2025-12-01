using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmuladorGBA.Business.Interface
{
    internal interface IStack
    {
        byte Pop();
        ushort Pop16Bits();

        void Push(ushort data);
        void Push(byte data);
    }
}
