using EmuladorGBA.Business.Enum;
using EmuladorGBA.Business.Interface;
using EmuladorGBA.Business.Intruction;
using EmuladorGBA.Business.Process.Load;
using EmuladorGBA.Business.Register;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmuladorGBA.Business.Process
{
    internal abstract class CpuProcesses : ICpu
    {
        #region DATA VALUES

        internal IBus Bus { get; set; }

        internal CpuRegisters CpuRegisters;

        internal CpuInstruction Instruction;

        internal ushort FetchedData;

        internal bool IntMasterEnabled;

        internal const short ResolutionWeidth = 160;

        internal const short ResolutionHeight = 144;

        internal CpuInstruction[] CpuInstructions { get; set; }

        internal byte CpuOpeCode { get; set; }

        internal ushort MemoryAdressDest { get; set; }

        internal bool DestIsMemory { get; set; }

        internal bool Halted;

        internal bool Stepping;

        public int Tickets = 0;

        #endregion

        internal void Cycles(int cpu_cycles)
        {
            // TODO...
        }

        protected void Execute()
        {
            IN_PROC proc = this.GetProcessor(this.Instruction.Type);

            if (proc is null)
            {
                Console.WriteLine($" - PROCESSO não implementado em: {this.Instruction.Type}");
                return;
            }

            //Console.Write($" - PROCESSO: {this.Instruction.Type}");

            proc.Invoke();
        }

        #region CPU PROCESSOR

        internal delegate void IN_PROC();

        internal IN_PROC[] Processors;

        internal IN_PROC GetProcessor(InType type)
        {
            return this.Processors[(short)type];
        }

        #endregion PROCESS


        internal bool CPU_FLAG_Z => BitHelper.Bit(this.CpuRegisters.F, 7) == 1;
        internal bool CPU_FLAG_N => BitHelper.Bit(this.CpuRegisters.F, 6) == 1;
        internal bool CPU_FLAG_H => BitHelper.Bit(this.CpuRegisters.F, 5) == 1;
        internal bool CPU_FLAG_C => BitHelper.Bit(this.CpuRegisters.F, 4) == 1;

        internal void PROC_NONE()
        {
            throw new ArgumentException($"Instrução Inválida");
        }

        internal void PROC_JP()
        {
            if (this.CheckCond())
            {
                this.CpuRegisters.SetRegisterPC(this.FetchedData);
                this.Cycles(1);
            }
        }

        internal void PROC_NOP()
        {
            // TODO...
        }

        #region LD

        internal void PROC_LD()
        {
            if(this is Cpu cpu)
            {
                if(this.DestIsMemory)
                {
                    //if 16 bit register...
                    if (this.Instruction.Reg2 >= RegType.RT_AF)
                    {
                        this.Cycles(1);
                        this.Bus.WriteB16(this.MemoryAdressDest, this.FetchedData);
                    }
                    else
                        this.Bus.Write(this.MemoryAdressDest, (byte)(this.FetchedData));

                    return;
                }

                if (this.Instruction.Mode == AddrMode.AM_HL_SPR)
                {
                    int hflag = ((cpu.CpuReadRegister(this.Instruction.Reg2) & 0xF)
                            + (this.FetchedData & 0xF)) >= 0x10 ? 1 : 0;

                    int cflag = ((cpu.CpuReadRegister(this.Instruction.Reg2) & 0xF)
                                + (this.FetchedData & 0xF)) >= 0x100 ? 1 : 0;

                    cpu.CpuSetFlags(0, 0, hflag, cflag);
                    cpu.CpuWriteRegister(
                        this.Instruction.Reg1
                        , (ushort)(cpu.CpuReadRegister(this.Instruction.Reg2) + ((byte)this.FetchedData))
                    );

                    return;
                }

                cpu.CpuWriteRegister(this.Instruction.Reg1 , this.FetchedData);
            }
        }

        #endregion

        internal void PROC_XOR()
        {
            byte a = this.CpuRegisters.A;
            a ^= (byte)(this.FetchedData & 0xFF);
            this.CpuRegisters.SetRegisterA(a);

            // TODO: VALIDAR 
            int z = this.CpuRegisters.A == 0 ? 0 : 1;
            this.CpuSetFlags(z, 0, 0, 0);
        }

        internal void PROC_DI()
        {
            this.IntMasterEnabled = false;
        }

        #region AUX PROCESS

        internal bool CheckCond()
        {
            bool z = this.CPU_FLAG_Z;
            bool c = this.CPU_FLAG_C;

            switch (this.Instruction.Cond)
            {
                case CondType.CT_NONE: return true;
                case CondType.CT_C: return c;
                case CondType.CT_NC: return !c;
                case CondType.CT_Z: return z;
                case CondType.CT_NZ: return !z;
            }

            return false;
        }

        // TODO: VALIDAR 
        internal void CpuSetFlags(int z, int n, int h, int c)
        {
            if (z != -1)
            {
                BitHelper.BitSet(ref this.CpuRegisters.F, 7, z == 1);
            }

            if (n != -1)
            {
                BitHelper.BitSet(ref this.CpuRegisters.F, 6, n == 1);
            }

            if (h != -1)
            {
                BitHelper.BitSet(ref this.CpuRegisters.F, 5, h == 1);
            }

            if (c != -1)
            {
                BitHelper.BitSet(ref this.CpuRegisters.F, 4, c == 1);
            }
        }

        #endregion
    }
}
