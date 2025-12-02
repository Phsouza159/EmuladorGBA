using EmuladorGBA.Business.Enum;
using EmuladorGBA.Business.Extensions;
using EmuladorGBA.Business.Interface;
using EmuladorGBA.Business.Intruction;
using EmuladorGBA.Business.Process.Load;
using EmuladorGBA.Business.Register;
using EmuladorGBA.Business.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EmuladorGBA.Business.Process
{
    internal abstract class CpuProcesses : ICpu
    {
        #region DATA VALUES

        internal IBus Bus { get; set; }

        internal IStack Stack { get; set; }

        public CpuRegisters CpuRegisters;

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

        protected byte RegisterIE;

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

        #region REGISTER IE

        public byte CpuGetRegisterIE() => this.RegisterIE;

        public void CpuSetRegisterIE(byte value) => this.RegisterIE = value;

        #endregion

        #region CPU PROCESSOR

        internal delegate void IN_PROC();

        internal IN_PROC[] Processors;

        internal IN_PROC GetProcessor(InType type)
        {
            return this.Processors[(short)type];
        }

        internal bool CPU_FLAG_Z => BitHelper.Bit(this.CpuRegisters.F, 7) == 1;
        internal bool CPU_FLAG_N => BitHelper.Bit(this.CpuRegisters.F, 6) == 1;
        internal bool CPU_FLAG_H => BitHelper.Bit(this.CpuRegisters.F, 5) == 1;
        internal bool CPU_FLAG_C => BitHelper.Bit(this.CpuRegisters.F, 4) == 1;

        #region PROC NONE

        internal void PROC_NONE()
        {
            throw new ArgumentException($"Instrução Inválida");
        }

        #endregion

        #region AUX - GO TO ADDRESS

        internal void GOTO_ADDR (ushort address, bool isPushPC)
        {
            if (this.CheckCond())
            { 
                if(isPushPC)
                {
                    this.Cycles(2);
                    this.Stack.Push(this.CpuRegisters.PC);
                }

                this.CpuRegisters.SetRegisterPC(address);
                this.Cycles(1);
            }
        }

        #endregion

        #region PROC JP

        internal void PROC_JP()
        {
            // AUX JUMP
            this.GOTO_ADDR(this.FetchedData, isPushPC: false);
        }

        #endregion

        #region PROC JR

        internal void PROC_JR()
        {
            byte rel = (byte)(this.FetchedData & 0xFF);
            ushort address = (ushort)(this.CpuRegisters.PC + rel);

            // AUX JUMP R
            this.GOTO_ADDR(address, isPushPC: false);
        }

        #endregion

        #region PROC CALL

        internal void PROC_CALL()
        {
            // AUX JUMP
            this.GOTO_ADDR(this.FetchedData, isPushPC: true);
        }

        #endregion

        #region PROC RST

        internal void PROC_RST()
        {
            // AUX JUMP
            this.GOTO_ADDR(this.Instruction.Param, isPushPC: true);
        }

        #endregion

        #region PROC RET

        internal void PROC_RET()
        {
            if(this.Instruction.Cond != CondType.CT_NONE)
            {
                this.Cycles(1); 
            }

            if (this.CheckCond())
            {
                byte lo = this.Stack.Pop();
                this.Cycles(1);

                byte hi = this.Stack.Pop();
                this.Cycles(1);

                ushort value = (ushort)((hi << 8) | lo);
                this.CpuRegisters.SetRegisterPC(value);

                this.Cycles(1);
            }
        }

        #endregion

        #region PROC RETI

        internal void PROC_RETI()
        {
            if (this is Cpu cpu)
            {
                cpu.IntMasterEnabled = true;
                this.PROC_RET();
                return;
            }

            throw ExceptionsUtil.NotSupportedCpu();
        }

        #endregion

        #region PROC NOP

        internal void PROC_NOP()
        {
            // TODO...
        }

        #endregion

        #region PROC LD

        internal void PROC_LD()
        {
            if(this is Cpu cpu)
            {
                if(this.DestIsMemory)
                {
                    //if 16 bit register...
                    if (this.Instruction.Is16Bits())
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

        #region PROC XOR

        internal void PROC_XOR()
        {
            byte a = this.CpuRegisters.A;
            a ^= (byte)(this.FetchedData & 0xFF);
            this.CpuRegisters.SetRegisterA(a);

            // TODO: VALIDAR 
            int z = this.CpuRegisters.A == 0 ? 0 : 1;
            this.CpuSetFlags(z, 0, 0, 0);
        }

        #endregion

        #region PROC DI

        internal void PROC_DI()
        {
            this.IntMasterEnabled = false;
        }

        #endregion

        #region PROC LDH

        internal void PROC_LDH()
        {
            if (this.Instruction.Reg1 == RegType.RT_A && this is Cpu cpu)
            {
                cpu.CpuWriteRegister(this.Instruction.Reg1, this.Bus.Read((ushort)(0xFF00 | this.FetchedData)));
            }
            else
            {
                // this.Bus.Write((ushort)(0xFF00 | this.FetchedData), this.CpuRegisters.A);
                this.Bus.Write(this.MemoryAdressDest, this.CpuRegisters.A);
            }

            this.Cycles(1);
        }

        #endregion

        #region PROC POP

        public void PROC_POP()
        {
            if(this is Cpu cpu)
            {
                ushort lo = this.Stack.Pop();
                this.Cycles(1);
                ushort hi = this.Stack.Pop();
                this.Cycles(1);

                ushort value = (ushort)((hi << 8) | lo);
                cpu.CpuWriteRegister(cpu.Instruction.Reg1, value);

                // CORRECT TO REG AF
                if(cpu.Instruction.Reg1 == RegType.RT_AF)
                    cpu.CpuWriteRegister(cpu.Instruction.Reg1, (ushort)(value & 0xFFF0));

                return;
            }

            throw ExceptionsUtil.NotSupportedCpu();
        }

        #endregion

        #region PROC PUSH

        public void PROC_PUSH()
        {
            if (this is Cpu cpu)
            {
                byte hi = (byte)((cpu.CpuReadRegister(cpu.Instruction.Reg1) >> 8) & 0xFF);
                this.Cycles(1);

                this.Stack.Push(hi);

                byte lo = (byte)(cpu.CpuReadRegister(cpu.Instruction.Reg1) & 0xFF);
                this.Cycles(1);

                this.Stack.Push(lo);
                this.Cycles(1);

                return;
            }

            throw ExceptionsUtil.NotSupportedCpu();
        }

        #endregion

        #region PROC INC

        internal void PROC_INC()
        {
            if(this is Cpu cpu)
            {
                ushort value = cpu.CpuReadRegister(cpu.Instruction.Reg1);
                // INCREMENT
                value += 1;

                if(cpu.Instruction.Is16Bits())
                {
                    cpu.Cycles(1);
                }

                if(cpu.Instruction.Reg1 == RegType.RT_HL && cpu.Instruction.Mode == AddrMode.AM_MR)
                {
                    value = cpu.Bus.Read((ushort)(cpu.CpuReadRegister(RegType.RT_HL) + 1));
                    value &= 0xFF;
                    // TODO: is 16 or 8 bits /
                    cpu.Bus.Write(cpu.CpuReadRegister(RegType.RT_HL), (byte)value);
                }
                else
                {
                    cpu.CpuWriteRegister(cpu.Instruction.Reg1, value);
                    value = cpu.CpuReadRegister(cpu.Instruction.Reg1);
                }

                // BREAK //0x03 NOT FLAGS
                if ((cpu.CpuOpeCode & 0x03) == 0x03) return;

                // TODO VALIDADE 
                cpu.CpuSetFlags(value == 0 ? 1 : 0, 0, (value & 0x0F), - 1);
            }
        }

        #endregion

        #region PROC DEC

        internal void PROC_DEC()
        {
            if (this is Cpu cpu)
            {
                ushort value = cpu.CpuReadRegister(cpu.Instruction.Reg1);
                // INCREMENT
                value += 1;

                if (cpu.Instruction.Is16Bits())
                {
                    cpu.Cycles(1);
                }

                if (cpu.Instruction.Reg1 == RegType.RT_HL && cpu.Instruction.Mode == AddrMode.AM_MR)
                {
                    value = cpu.Bus.Read((ushort)(cpu.CpuReadRegister(RegType.RT_HL) - 1));
                    // TODO: is 16 or 8 bits /
                    cpu.Bus.Write(cpu.CpuReadRegister(RegType.RT_HL), (byte)value);
                }
                else
                {
                    cpu.CpuWriteRegister(cpu.Instruction.Reg1, value);
                    value = cpu.CpuReadRegister(cpu.Instruction.Reg1);
                }

                // BREAK //0x0B NOT FLAGS
                if ((cpu.CpuOpeCode & 0x0B) == 0x0B) return;

                // TODO VALIDADE 
                cpu.CpuSetFlags((value == 0 ? 1 : 0), 1, ((value & 0x0F) == 0X0F) ? 1 : 0, -1);
            }
        }

        #endregion

        #region PROC ADD

        internal void PROC_ADD()
        {
            if(this is Cpu cpu)
            {
                // TODO...
                return;
            }

            throw ExceptionsUtil.NotSupportedCpu();
        }

        #endregion

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

        #endregion PROCESS
    }
}
