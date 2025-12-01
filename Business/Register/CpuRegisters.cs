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
    }
}
