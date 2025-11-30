using EmuladorGBA.Business.Enum;
using EmuladorGBA.Business.Extensions;
using EmuladorGBA.Business.Intruction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmuladorGBA.Business.Process
{
    internal static class CpuHelper
    {

        internal static void FetchInstruction(this Cpu cpu)
        {
            cpu.CpuOpeCode = cpu.Bus.Read(cpu.CpuRegisters.PC);
            // NEXT 
            cpu.CpuRegisters.IncrementPC();

            cpu.Instruction = cpu.InstructionByCode(cpu.CpuOpeCode);
        }

        internal static CpuInstruction InstructionByCode(this Cpu cpu, byte code)
        {
            if (code > cpu.CpuInstructions.Length)
                throw new ArgumentException("Instrução não implementada");

            return cpu.CpuInstructions[(int)code];
        }

        internal static string InstName(this Cpu cpu, InType inType)
        {
            return inType.GetDescription();
        }


        internal static ushort Reverse(this Cpu cpu, ushort n)
        {
            return (ushort)(((n & 0xFF00) >> 8) | ((n & 0x00FF) << 8));
        }

        internal static ushort CpuReadRegister(this Cpu cpu, RegType rt)
        {
            switch (rt)
            {
                case RegType.RT_A: return cpu.CpuRegisters.A;
                case RegType.RT_F: return cpu.CpuRegisters.F;
                case RegType.RT_B: return cpu.CpuRegisters.B;
                case RegType.RT_C: return cpu.CpuRegisters.C;
                case RegType.RT_D: return cpu.CpuRegisters.D;
                case RegType.RT_E: return cpu.CpuRegisters.E;
                case RegType.RT_H: return cpu.CpuRegisters.H;
                case RegType.RT_L: return cpu.CpuRegisters.L;

                case RegType.RT_AF: return cpu.Reverse((ushort)((cpu.CpuRegisters.A << 8) | cpu.CpuRegisters.F));
                case RegType.RT_BC: return cpu.Reverse((ushort)((cpu.CpuRegisters.B << 8) | cpu.CpuRegisters.C));
                case RegType.RT_DE: return cpu.Reverse((ushort)((cpu.CpuRegisters.D << 8) | cpu.CpuRegisters.E));
                case RegType.RT_HL: return cpu.Reverse((ushort)((cpu.CpuRegisters.H << 8) | cpu.CpuRegisters.L));

                case RegType.RT_PC: return cpu.CpuRegisters.PC;
                case RegType.RT_SP: return cpu.CpuRegisters.SP;
                case RegType.RT_NONE:

                default: return 0;
            }
        }
    }
}
