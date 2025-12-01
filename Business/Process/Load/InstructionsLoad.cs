using EmuladorGBA.Business.Enum;
using EmuladorGBA.Business.Intruction;

namespace EmuladorGBA.Business.Process.Load
{
    internal static class InstructionsLoad
    {
        internal static void Load(Cpu cpu)
        {
            cpu.CpuInstructions = new CpuInstruction[0x100];

            //-0X
            cpu.CpuInstructions[0x00] = new CpuInstruction { Type = InType.IN_NOP,  Mode = AddrMode.AM_IMP };
            cpu.CpuInstructions[0x01] = new CpuInstruction { Type = InType.IN_LD,   Mode = AddrMode.AM_R_D16,   Reg1 = RegType.RT_BC };
            cpu.CpuInstructions[0x02] = new CpuInstruction { Type = InType.IN_LD,   Mode = AddrMode.AM_MR_R,    Reg1 = RegType.RT_A };

            cpu.CpuInstructions[0x05] = new CpuInstruction { Type = InType.IN_DEC,  Mode = AddrMode.AM_R,       Reg1 = RegType.RT_B };
            cpu.CpuInstructions[0x06] = new CpuInstruction { Type = InType.IN_LD,   Mode = AddrMode.AM_R_D8,    Reg1 = RegType.RT_B };

            cpu.CpuInstructions[0x08] = new CpuInstruction { Type = InType.IN_LD,   Mode = AddrMode.AM_A16_R,   Reg1 = RegType.RT_NONE, Reg2 = RegType.RT_SP };

            cpu.CpuInstructions[0x0A] = new CpuInstruction { Type = InType.IN_LD,   Mode = AddrMode.AM_R_MR,    Reg1 = RegType.RT_A,    Reg2 = RegType.RT_BC };

            cpu.CpuInstructions[0x0E] = new CpuInstruction { Type = InType.IN_LD,   Mode = AddrMode.AM_R_D8,    Reg1 = RegType.RT_C };

            //-1X
            cpu.CpuInstructions[0x11] = new CpuInstruction { Type = InType.IN_LD,   Mode = AddrMode.AM_R_D16,   Reg1 = RegType.RT_DE };
            cpu.CpuInstructions[0x12] = new CpuInstruction { Type = InType.IN_LD,   Mode = AddrMode.AM_MR_R,    Reg1 = RegType.RT_DE,   Reg2 = RegType.RT_A };
            cpu.CpuInstructions[0x15] = new CpuInstruction { Type = InType.IN_DEC,  Mode = AddrMode.AM_R,       Reg1 = RegType.RT_D };
            cpu.CpuInstructions[0x16] = new CpuInstruction { Type = InType.IN_LD,   Mode = AddrMode.AM_R_D8,    Reg1 = RegType.RT_D };
            cpu.CpuInstructions[0x1A] = new CpuInstruction { Type = InType.IN_LD,   Mode = AddrMode.AM_MR_R,    Reg1 = RegType.RT_A,    Reg2 = RegType.RT_DE };
            cpu.CpuInstructions[0x1E] = new CpuInstruction { Type = InType.IN_LD,   Mode = AddrMode.AM_R_D8,    Reg1 = RegType.RT_E };

            //-2X
            cpu.CpuInstructions[0x21] = new CpuInstruction { Type = InType.IN_LD,   Mode = AddrMode.AM_R_D16,   Reg1 = RegType.RT_HL };
            cpu.CpuInstructions[0x22] = new CpuInstruction { Type = InType.IN_LD,   Mode = AddrMode.AM_HLI_R,   Reg1 = RegType.RT_HL,   Reg2 = RegType.RT_A };
            cpu.CpuInstructions[0x25] = new CpuInstruction { Type = InType.IN_DEC,  Mode = AddrMode.AM_R,       Reg1 = RegType.RT_H };
            cpu.CpuInstructions[0x26] = new CpuInstruction { Type = InType.IN_LD,   Mode = AddrMode.AM_R_D8,    Reg1 = RegType.RT_H };
            cpu.CpuInstructions[0x2A] = new CpuInstruction { Type = InType.IN_LD,   Mode = AddrMode.AM_R_HLI,   Reg1 = RegType.RT_A,    Reg2 = RegType.RT_HL };
            cpu.CpuInstructions[0x2E] = new CpuInstruction { Type = InType.IN_LD,   Mode = AddrMode.AM_R_D8,    Reg1 = RegType.RT_L };

            //-3X
            cpu.CpuInstructions[0x31] = new CpuInstruction { Type = InType.IN_LD,   Mode = AddrMode.AM_R_D16,   Reg1 = RegType.RT_SP };
            cpu.CpuInstructions[0x32] = new CpuInstruction { Type = InType.IN_LD,   Mode = AddrMode.AM_HLD_R,   Reg1 = RegType.RT_HL,   Reg2 = RegType.RT_A };
            cpu.CpuInstructions[0x35] = new CpuInstruction { Type = InType.IN_DEC,  Mode = AddrMode.AM_R,       Reg1 = RegType.RT_HL };
            cpu.CpuInstructions[0x36] = new CpuInstruction { Type = InType.IN_LD,   Mode = AddrMode.AM_MR_D8,   Reg1 = RegType.RT_HL };
            cpu.CpuInstructions[0x3A] = new CpuInstruction { Type = InType.IN_LD,   Mode = AddrMode.AM_R_HLD,   Reg1 = RegType.RT_A,    Reg2 = RegType.RT_HL };
            cpu.CpuInstructions[0x3E] = new CpuInstruction { Type = InType.IN_LD,   Mode = AddrMode.AM_R_D8,    Reg1 = RegType.RT_A };

            //-4X
            cpu.CpuInstructions[0x40] = new CpuInstruction { Type = InType.IN_LD,   Mode = AddrMode.AM_R_R,     Reg1 = RegType.RT_B,    Reg2 = RegType.RT_B };
            cpu.CpuInstructions[0x41] = new CpuInstruction { Type = InType.IN_LD,   Mode = AddrMode.AM_R_R,     Reg1 = RegType.RT_B,    Reg2 = RegType.RT_C };
            cpu.CpuInstructions[0x42] = new CpuInstruction { Type = InType.IN_LD,   Mode = AddrMode.AM_R_R,     Reg1 = RegType.RT_B,    Reg2 = RegType.RT_D };
            cpu.CpuInstructions[0x43] = new CpuInstruction { Type = InType.IN_LD,   Mode = AddrMode.AM_R_R,     Reg1 = RegType.RT_B,    Reg2 = RegType.RT_E };
            cpu.CpuInstructions[0x44] = new CpuInstruction { Type = InType.IN_LD,   Mode = AddrMode.AM_R_R,     Reg1 = RegType.RT_B,    Reg2 = RegType.RT_H };
            cpu.CpuInstructions[0x45] = new CpuInstruction { Type = InType.IN_LD,   Mode = AddrMode.AM_R_R,     Reg1 = RegType.RT_B,    Reg2 = RegType.RT_L };
            cpu.CpuInstructions[0x46] = new CpuInstruction { Type = InType.IN_LD,   Mode = AddrMode.AM_R_MR,    Reg1 = RegType.RT_B,    Reg2 = RegType.RT_HL };
            cpu.CpuInstructions[0x47] = new CpuInstruction { Type = InType.IN_LD,   Mode = AddrMode.AM_R_R,     Reg1 = RegType.RT_B,    Reg2 = RegType.RT_A };

            cpu.CpuInstructions[0x48] = new CpuInstruction { Type = InType.IN_LD,   Mode = AddrMode.AM_R_R,     Reg1 = RegType.RT_C,    Reg2 = RegType.RT_B };
            cpu.CpuInstructions[0x49] = new CpuInstruction { Type = InType.IN_LD,   Mode = AddrMode.AM_R_R,     Reg1 = RegType.RT_C,    Reg2 = RegType.RT_C };
            cpu.CpuInstructions[0x4A] = new CpuInstruction { Type = InType.IN_LD,   Mode = AddrMode.AM_R_R,     Reg1 = RegType.RT_C,    Reg2 = RegType.RT_D };
            cpu.CpuInstructions[0x4B] = new CpuInstruction { Type = InType.IN_LD,   Mode = AddrMode.AM_R_R,     Reg1 = RegType.RT_C,    Reg2 = RegType.RT_E };
            cpu.CpuInstructions[0x4C] = new CpuInstruction { Type = InType.IN_LD,   Mode = AddrMode.AM_R_R,     Reg1 = RegType.RT_C,    Reg2 = RegType.RT_H };
            cpu.CpuInstructions[0x4D] = new CpuInstruction { Type = InType.IN_LD,   Mode = AddrMode.AM_R_R,     Reg1 = RegType.RT_C,    Reg2 = RegType.RT_L };
            cpu.CpuInstructions[0x4E] = new CpuInstruction { Type = InType.IN_LD,   Mode = AddrMode.AM_R_MR,    Reg1 = RegType.RT_C,    Reg2 = RegType.RT_HL };
            cpu.CpuInstructions[0x4F] = new CpuInstruction { Type = InType.IN_LD,   Mode = AddrMode.AM_R_R,     Reg1 = RegType.RT_C,    Reg2 = RegType.RT_A };

            //-5X
            cpu.CpuInstructions[0x50] = new CpuInstruction { Type = InType.IN_LD,   Mode = AddrMode.AM_R_R,     Reg1 = RegType.RT_D,    Reg2 = RegType.RT_B };
            cpu.CpuInstructions[0x51] = new CpuInstruction { Type = InType.IN_LD,   Mode = AddrMode.AM_R_R,     Reg1 = RegType.RT_D,    Reg2 = RegType.RT_C };
            cpu.CpuInstructions[0x52] = new CpuInstruction { Type = InType.IN_LD,   Mode = AddrMode.AM_R_R,     Reg1 = RegType.RT_D,    Reg2 = RegType.RT_D };
            cpu.CpuInstructions[0x53] = new CpuInstruction { Type = InType.IN_LD,   Mode = AddrMode.AM_R_R,     Reg1 = RegType.RT_D,    Reg2 = RegType.RT_E };
            cpu.CpuInstructions[0x54] = new CpuInstruction { Type = InType.IN_LD,   Mode = AddrMode.AM_R_R,     Reg1 = RegType.RT_D,    Reg2 = RegType.RT_H };
            cpu.CpuInstructions[0x55] = new CpuInstruction { Type = InType.IN_LD,   Mode = AddrMode.AM_R_R,     Reg1 = RegType.RT_D,    Reg2 = RegType.RT_L };
            cpu.CpuInstructions[0x56] = new CpuInstruction { Type = InType.IN_LD,   Mode = AddrMode.AM_R_MR,    Reg1 = RegType.RT_D,    Reg2 = RegType.RT_HL };
            cpu.CpuInstructions[0x57] = new CpuInstruction { Type = InType.IN_LD,   Mode = AddrMode.AM_R_R,     Reg1 = RegType.RT_D,    Reg2 = RegType.RT_A };

            cpu.CpuInstructions[0x58] = new CpuInstruction { Type = InType.IN_LD,   Mode = AddrMode.AM_R_R,     Reg1 = RegType.RT_E,    Reg2 = RegType.RT_B };
            cpu.CpuInstructions[0x59] = new CpuInstruction { Type = InType.IN_LD,   Mode = AddrMode.AM_R_R,     Reg1 = RegType.RT_E,    Reg2 = RegType.RT_C };
            cpu.CpuInstructions[0x5A] = new CpuInstruction { Type = InType.IN_LD,   Mode = AddrMode.AM_R_R,     Reg1 = RegType.RT_E,    Reg2 = RegType.RT_D };
            cpu.CpuInstructions[0x5B] = new CpuInstruction { Type = InType.IN_LD,   Mode = AddrMode.AM_R_R,     Reg1 = RegType.RT_E,    Reg2 = RegType.RT_E };
            cpu.CpuInstructions[0x5C] = new CpuInstruction { Type = InType.IN_LD,   Mode = AddrMode.AM_R_R,     Reg1 = RegType.RT_E,    Reg2 = RegType.RT_H };
            cpu.CpuInstructions[0x5D] = new CpuInstruction { Type = InType.IN_LD,   Mode = AddrMode.AM_R_R,     Reg1 = RegType.RT_E,    Reg2 = RegType.RT_L };
            cpu.CpuInstructions[0x5E] = new CpuInstruction { Type = InType.IN_LD,   Mode = AddrMode.AM_R_MR,    Reg1 = RegType.RT_E,    Reg2 = RegType.RT_HL };
            cpu.CpuInstructions[0x5F] = new CpuInstruction { Type = InType.IN_LD,   Mode = AddrMode.AM_R_R,     Reg1 = RegType.RT_E,    Reg2 = RegType.RT_A };

            //-6X
            cpu.CpuInstructions[0x60] = new CpuInstruction { Type = InType.IN_LD,   Mode = AddrMode.AM_R_R,     Reg1 = RegType.RT_H,    Reg2 = RegType.RT_B };
            cpu.CpuInstructions[0x61] = new CpuInstruction { Type = InType.IN_LD,   Mode = AddrMode.AM_R_R,     Reg1 = RegType.RT_H,    Reg2 = RegType.RT_C };
            cpu.CpuInstructions[0x62] = new CpuInstruction { Type = InType.IN_LD,   Mode = AddrMode.AM_R_R,     Reg1 = RegType.RT_H,    Reg2 = RegType.RT_D };
            cpu.CpuInstructions[0x63] = new CpuInstruction { Type = InType.IN_LD,   Mode = AddrMode.AM_R_R,     Reg1 = RegType.RT_H,    Reg2 = RegType.RT_E };
            cpu.CpuInstructions[0x64] = new CpuInstruction { Type = InType.IN_LD,   Mode = AddrMode.AM_R_R,     Reg1 = RegType.RT_H,    Reg2 = RegType.RT_H };
            cpu.CpuInstructions[0x65] = new CpuInstruction { Type = InType.IN_LD,   Mode = AddrMode.AM_R_R,     Reg1 = RegType.RT_H,    Reg2 = RegType.RT_L };
            cpu.CpuInstructions[0x66] = new CpuInstruction { Type = InType.IN_LD,   Mode = AddrMode.AM_R_MR,    Reg1 = RegType.RT_H,    Reg2 = RegType.RT_HL };
            cpu.CpuInstructions[0x67] = new CpuInstruction { Type = InType.IN_LD,   Mode = AddrMode.AM_R_R,     Reg1 = RegType.RT_H,    Reg2 = RegType.RT_A };

            cpu.CpuInstructions[0x68] = new CpuInstruction { Type = InType.IN_LD,   Mode = AddrMode.AM_R_R,     Reg1 = RegType.RT_L,    Reg2 = RegType.RT_B };
            cpu.CpuInstructions[0x69] = new CpuInstruction { Type = InType.IN_LD,   Mode = AddrMode.AM_R_R,     Reg1 = RegType.RT_L,    Reg2 = RegType.RT_C };
            cpu.CpuInstructions[0x6A] = new CpuInstruction { Type = InType.IN_LD,   Mode = AddrMode.AM_R_R,     Reg1 = RegType.RT_L,    Reg2 = RegType.RT_D };
            cpu.CpuInstructions[0x6B] = new CpuInstruction { Type = InType.IN_LD,   Mode = AddrMode.AM_R_R,     Reg1 = RegType.RT_L,    Reg2 = RegType.RT_E };
            cpu.CpuInstructions[0x6C] = new CpuInstruction { Type = InType.IN_LD,   Mode = AddrMode.AM_R_R,     Reg1 = RegType.RT_L,    Reg2 = RegType.RT_H };
            cpu.CpuInstructions[0x6D] = new CpuInstruction { Type = InType.IN_LD,   Mode = AddrMode.AM_R_R,     Reg1 = RegType.RT_L,    Reg2 = RegType.RT_L };
            cpu.CpuInstructions[0x6E] = new CpuInstruction { Type = InType.IN_LD,   Mode = AddrMode.AM_R_MR,    Reg1 = RegType.RT_L,    Reg2 = RegType.RT_HL };
            cpu.CpuInstructions[0x6F] = new CpuInstruction { Type = InType.IN_LD,   Mode = AddrMode.AM_R_R,     Reg1 = RegType.RT_L,    Reg2 = RegType.RT_A };

            //-7X
            cpu.CpuInstructions[0x70] = new CpuInstruction { Type = InType.IN_LD,   Mode = AddrMode.AM_MR_R,    Reg1 = RegType.RT_HL,   Reg2 = RegType.RT_B };
            cpu.CpuInstructions[0x71] = new CpuInstruction { Type = InType.IN_LD,   Mode = AddrMode.AM_MR_R,    Reg1 = RegType.RT_HL,   Reg2 = RegType.RT_C };
            cpu.CpuInstructions[0x72] = new CpuInstruction { Type = InType.IN_LD,   Mode = AddrMode.AM_MR_R,    Reg1 = RegType.RT_HL,   Reg2 = RegType.RT_D };
            cpu.CpuInstructions[0x73] = new CpuInstruction { Type = InType.IN_LD,   Mode = AddrMode.AM_MR_R,    Reg1 = RegType.RT_HL,   Reg2 = RegType.RT_E };
            cpu.CpuInstructions[0x74] = new CpuInstruction { Type = InType.IN_LD,   Mode = AddrMode.AM_MR_R,    Reg1 = RegType.RT_HL,   Reg2 = RegType.RT_H };
            cpu.CpuInstructions[0x75] = new CpuInstruction { Type = InType.IN_LD,   Mode = AddrMode.AM_MR_R,    Reg1 = RegType.RT_HL,   Reg2 = RegType.RT_L };
            cpu.CpuInstructions[0x76] = new CpuInstruction { Type = InType.IN_HALT };
            cpu.CpuInstructions[0x77] = new CpuInstruction { Type = InType.IN_LD,   Mode = AddrMode.AM_MR_R,    Reg1 = RegType.RT_HL,   Reg2 = RegType.RT_A };

            cpu.CpuInstructions[0x78] = new CpuInstruction { Type = InType.IN_LD,   Mode = AddrMode.AM_R_R,     Reg1 = RegType.RT_A,    Reg2 = RegType.RT_B };
            cpu.CpuInstructions[0x79] = new CpuInstruction { Type = InType.IN_LD,   Mode = AddrMode.AM_R_R,     Reg1 = RegType.RT_A,    Reg2 = RegType.RT_C };
            cpu.CpuInstructions[0x7A] = new CpuInstruction { Type = InType.IN_LD,   Mode = AddrMode.AM_R_R,     Reg1 = RegType.RT_A,    Reg2 = RegType.RT_D };
            cpu.CpuInstructions[0x7B] = new CpuInstruction { Type = InType.IN_LD,   Mode = AddrMode.AM_R_R,     Reg1 = RegType.RT_A,    Reg2 = RegType.RT_E };
            cpu.CpuInstructions[0x7C] = new CpuInstruction { Type = InType.IN_LD,   Mode = AddrMode.AM_R_R,     Reg1 = RegType.RT_A,    Reg2 = RegType.RT_H };
            cpu.CpuInstructions[0x7D] = new CpuInstruction { Type = InType.IN_LD,   Mode = AddrMode.AM_R_R,     Reg1 = RegType.RT_A,    Reg2 = RegType.RT_L };
            cpu.CpuInstructions[0x7E] = new CpuInstruction { Type = InType.IN_LD,   Mode = AddrMode.AM_R_MR,    Reg1 = RegType.RT_A,    Reg2 = RegType.RT_HL };
            cpu.CpuInstructions[0x7F] = new CpuInstruction { Type = InType.IN_LD,   Mode = AddrMode.AM_R_R,     Reg1 = RegType.RT_A,    Reg2 = RegType.RT_A };


            cpu.CpuInstructions[0xAF] = new CpuInstruction { Type = InType.IN_XOR,  Mode = AddrMode.AM_R,       Reg1 = RegType.RT_A };

            cpu.CpuInstructions[0xC3] = new CpuInstruction { Type = InType.IN_JP,   Mode = AddrMode.AM_D16 };

            //-EX
            cpu.CpuInstructions[0xE2] = new CpuInstruction { Type = InType.IN_LD,   Mode = AddrMode.AM_MR_R,    Reg1 = RegType.RT_C,    Reg2 = RegType.RT_A };
            cpu.CpuInstructions[0xEA] = new CpuInstruction { Type = InType.IN_LD,   Mode = AddrMode.AM_A16_R,   Reg1 = RegType.RT_NONE, Reg2 = RegType.RT_A };

            //-FX
            cpu.CpuInstructions[0xF2] = new CpuInstruction { Type = InType.IN_LD,   Mode = AddrMode.AM_R_MR,    Reg1 = RegType.RT_A,    Reg2 = RegType.RT_C };
            cpu.CpuInstructions[0xF3] = new CpuInstruction { Type = InType.IN_DI };
            cpu.CpuInstructions[0xFA] = new CpuInstruction { Type = InType.IN_LD,   Mode = AddrMode.AM_R_A16,   Reg1 = RegType.RT_A };


            cpu.CpuInstructions[0xF3] = new CpuInstruction { Type = InType.IN_DI };
        }
    }
}
