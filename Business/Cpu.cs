using EmuladorGBA.Business.Enum;
using EmuladorGBA.Business.Interface;
using EmuladorGBA.Business.Intruction;
using EmuladorGBA.Business.Register;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmuladorGBA.Business
{
    public class Cpu : ICpu
    {
        public Cpu()
        {
            this.CpuRegisters = new CpuRegisters();

            this.LoadInstruction();
            this.LoadLoockUp();
        }

        public const short ResolutionWeidth = 160;

        public const short ResolutionHeight = 144;

        public CpuRegisters CpuRegisters { get; set; }

        internal CpuInstruction[] CpuInstruction { get; private set; }

        private void LoadInstruction()
        {
            #region LOAD INSTRUCTIONS 

            this.CpuInstruction = new CpuInstruction[0x100];

            this.CpuInstruction[0x00] = new CpuInstruction { Type = InType.IN_NOP   , Mode = AddrMode.AM_IMP };
            this.CpuInstruction[0x05] = new CpuInstruction { Type = InType.IN_DEC   , Mode = AddrMode.AM_R      , Reg1 = RegType.RT_B };
            this.CpuInstruction[0x0E] = new CpuInstruction { Type = InType.IN_LD    , Mode = AddrMode.AM_R_D8   , Reg1 = RegType.RT_C };
            this.CpuInstruction[0xAF] = new CpuInstruction { Type = InType.IN_XOR   , Mode = AddrMode.AM_R      , Reg1 = RegType.RT_A };
            this.CpuInstruction[0xC3] = new CpuInstruction { Type = InType.IN_JP    , Mode = AddrMode.AM_D16 };
            this.CpuInstruction[0xF3] = new CpuInstruction { Type = InType.IN_DI };

            #endregion
        }

        private void LoadLoockUp()
        {
            //TODO..
        }

        internal CpuInstruction InstructionByCode(byte code)
        {
            if (this.CpuInstruction.Length > (short)code)
                throw new ArgumentException("Instrução não implementada");
         
            return this.CpuInstruction[(int)code];
        }

        internal string InstName(Enum.InType inType)
        {
            //TODO...
            return string.Empty;
        }


        internal ushort Reverse(ushort n)
        {
            return (ushort)(((n & 0xFF00) >> 8) | ((n & 0x00FF) << 8));

        }
        
        internal ushort CpuReadRegister(RegType rt)
        {
            switch (rt)
            {
                case RegType.RT_A: return this.CpuRegisters.A;
                case RegType.RT_F: return this.CpuRegisters.F;
                case RegType.RT_B: return this.CpuRegisters.B;
                case RegType.RT_C: return this.CpuRegisters.C;
                case RegType.RT_D: return this.CpuRegisters.D;
                case RegType.RT_E: return this.CpuRegisters.E;
                case RegType.RT_H: return this.CpuRegisters.H;
                case RegType.RT_L: return this.CpuRegisters.L;

                case RegType.RT_AF: return this.Reverse((ushort)((this.CpuRegisters.A << 8) | this.CpuRegisters.F));
                case RegType.RT_BC: return this.Reverse((ushort)((this.CpuRegisters.B << 8) | this.CpuRegisters.C));
                case RegType.RT_DE: return this.Reverse((ushort)((this.CpuRegisters.D << 8) | this.CpuRegisters.E));
                case RegType.RT_HL: return this.Reverse((ushort)((this.CpuRegisters.H << 8) | this.CpuRegisters.L));

                case RegType.RT_PC: return this.CpuRegisters.PC;
                case RegType.RT_SP: return this.CpuRegisters.SP;

                default: return 0;
            }
        }

        internal bool Step()
        {
            ///TODO...
            return true;
        }
    }
}
