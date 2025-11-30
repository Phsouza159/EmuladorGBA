using EmuladorGBA.Business.Enum;
using EmuladorGBA.Business.Intruction;

namespace EmuladorGBA.Business.Process.Load
{
    internal static class InstructionsLoad
    {
        internal static void Load(Cpu cpu)
        {
            cpu.CpuInstructions = new CpuInstruction[0x100];

            cpu.CpuInstructions[0x00] = new CpuInstruction { Type = InType.IN_NOP,  Mode = AddrMode.AM_IMP };
            cpu.CpuInstructions[0x05] = new CpuInstruction { Type = InType.IN_DEC,  Mode = AddrMode.AM_R,       Reg1 = RegType.RT_B };
            cpu.CpuInstructions[0x0E] = new CpuInstruction { Type = InType.IN_LD,   Mode = AddrMode.AM_R_D8,    Reg1 = RegType.RT_C };
            cpu.CpuInstructions[0xAF] = new CpuInstruction { Type = InType.IN_XOR,  Mode = AddrMode.AM_R,       Reg1 = RegType.RT_A };
            cpu.CpuInstructions[0xC3] = new CpuInstruction { Type = InType.IN_JP,   Mode = AddrMode.AM_D16 };
            cpu.CpuInstructions[0xF3] = new CpuInstruction { Type = InType.IN_DI };
        }
    }
}
