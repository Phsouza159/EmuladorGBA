using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmuladorGBA.Business.Register
{
    public struct CpuRegisters
    {
        public ushort PC { get; set; }

        public ushort SP { get; set; }

        public byte A, F; // AF
        
        public byte B, C; // BC
        
        public byte D, E; // DE

        public byte H, L; // HL

    }
}
