using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmuladorGBA.Business.Register
{
    public struct CpuRegisters
    {
        internal ushort PC { get; set; }

        internal ushort SP { get; set; }

        internal byte A, F; // AF

        internal byte B, C; // BC

        internal byte D, E; // DE

        internal byte H, L; // HL

        internal void SetRegisterA(byte value)
        {
            this.A = value;
        }

        internal void SetRegisterPC(ushort value)
        {
            this.PC = value;
        }

        internal void IncrementPC()
        {
            this.PC += 1;
        }
    }
}
