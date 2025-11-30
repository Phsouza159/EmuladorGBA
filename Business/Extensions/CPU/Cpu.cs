using EmuladorGBA.Business.Enum;
using EmuladorGBA.Business.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmuladorGBA.Business.Process
{
    // EXTENSIONS PARTIAL

    internal partial class Cpu  
    {
        #region FECTH DATA 

        internal void FecthData()
        {
            this.MemoryAdressDest = 0;
            this.DestIsMemory = false;

            if (this.Instruction.IsEmpty())
                return;

            switch (this.Instruction.Mode)
            {
                case AddrMode.AM_IMP:
                    return;

                case AddrMode.AM_R:
                    this.FetchedData = this.CpuReadRegister(this.Instruction.Reg1);
                    return;

                case AddrMode.AM_R_D8:
                    this.FetchedData = this.Bus.Read(this.CpuRegisters.PC);
                    this.Cycles(1);
                    this.CpuRegisters.IncrementPC();
                    return;


                case AddrMode.AM_D16:

                    ushort pc = this.CpuRegisters.PC;
                    byte lo = this.Bus.Read(pc);
                    this.Cycles(1);

                    byte hi = this.Bus.Read((ushort)(pc + 1));
                    this.Cycles(1);

                    this.FetchedData = (ushort)(lo | (hi << 8));
                    this.CpuRegisters.SetRegisterPC((ushort)(pc + 2));

                    return;

                default:
                    Console.Write($"Mode sem suporte '{this.Instruction.Mode}' em '{this.CpuOpeCode}'");
                    throw new ArgumentException("STOP CPU");
            }
        }

        #endregion
    }
}
