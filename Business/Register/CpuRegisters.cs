using EmuladorGBA.Business.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmuladorGBA.Business.Register
{
    public struct CpuRegisters
    {
        internal ushort PC { get; private set; }

        internal ushort SP { get; private set; }

        internal byte A { get; private set; } // AF
        internal byte F { get; private set; } // AF

        internal byte B { get; private set; } // BC
        internal byte C { get; private set; } // BC

        internal byte D { get; private set; } // DE
        internal byte E { get; private set; } // DE

        internal byte H { get; private set; } // HL
        internal byte L { get; private set; } // HL

        internal void SetRegisterA(byte value) => this.A = value;
        internal void SetRegisterF(byte value) => this.F = value;

        internal void SetRegisterB(byte value) => this.B = value;
        internal void SetRegisterC(byte value) => this.C = value;

        internal void SetRegisterD(byte value) => this.D = value;
        internal void SetRegisterE(byte value) => this.E = value;

        internal void SetRegisterH(byte value) => this.H = value;
        internal void SetRegisterL(byte value) => this.L = value;

        internal void SetRegisterPC(ushort value) => this.PC = value;
        internal void SetRegisterSP(ushort value) => this.SP = value;

        internal void IncrementPC() => this.PC += 1;
        internal void DescrementPC() => this.PC -= 1;

        internal void IncrementSP() => this.SP += 1;
        internal void DescrementSP() => this.SP -= 1;

        internal void SetRegisterAF(ushort af)
        {
            this.A = af.HighBit();   // High byte
            this.F = af.LowBit();    // Low byte
        }

        internal void SetRegisterBC(ushort bc)
        {
            this.B = bc.HighBit();   // High byte
            this.C = bc.LowBit();    // Low byte
        }

        internal void SetRegisterDE(ushort de)
        {
            this.D = de.HighBit();   // High byte
            this.E = de.LowBit();    // Low byte
        }

        internal void SetRegisterHL(ushort hl)
        {
            this.H = hl.HighBit();  // High byte
            this.L = hl.LowBit();    // Low byte
        }
    }
}
