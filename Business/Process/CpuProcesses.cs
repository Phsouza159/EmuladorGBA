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
using Windows.Devices.WiFi;

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

        internal ushort MemoryAdressDest { get; private set; }

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
                throw new ArgumentException($" - PROCESSO não implementado em: {this.Instruction.Type}");
                return;
            }

            //Console.Write($" - PROCESSO: {this.Instruction.Type}");

            proc.Invoke();
        }

        #region SET MEMOMRY DEST

        internal void SetMemoryAdressDest(ushort value)
        {
            this.MemoryAdressDest = value;
            //Console.WriteLine($"* SET MEMORY DESTI: '{value.ToString("X2"), 4}'");
        }

        #endregion

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
                ushort pc = this.CpuRegisters.PC;

                if (isPushPC)
                {
                    this.Cycles(2);
                    this.Stack.Push16Bits(this.CpuRegisters.PC);
                }

                this.CpuRegisters.SetRegisterPC(address);
                this.Cycles(1);

                Console.WriteLine($"* GOTO ADDRESS: '{pc.ToString("X2"),4}' TO '{address.ToString("X2"),4}'");
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
            sbyte rel = (sbyte)(this.FetchedData & 0xFF); // signed offset
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

                ushort value = Convert.ToUInt16(((hi << 8) | lo));
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
                    if (this.Instruction.Reg2.Is16Bits())
                    {
                        this.Cycles(1);
                        this.Bus.WriteB16(this.MemoryAdressDest, this.FetchedData);
                    }
                    else
                        this.Bus.Write(this.MemoryAdressDest, (byte)(this.FetchedData));
                    
                    //TODO: VALIDAR
                    this.Cycles(1);
                    return;
                }

                if (this.Instruction.Mode == AddrMode.AM_HL_SPR)
                {
                    int hflag = ((cpu.CpuReadRegister(this.Instruction.Reg2) & 0xF)
                            + (this.FetchedData & 0xF)) >= 0x10 ? 1 : 0;

                    int cflag = ((cpu.CpuReadRegister(this.Instruction.Reg2) & 0xFF)
                                + (this.FetchedData & 0xFF)) >= 0x100 ? 1 : 0;

                    cpu.CpuSetFlags(0, 0, hflag, cflag);
                    cpu.CpuWriteRegister(
                        this.Instruction.Reg1
                        , (ushort)(cpu.CpuReadRegister(this.Instruction.Reg2) + ((sbyte)this.FetchedData))
                    );

                    return;
                }

                cpu.CpuWriteRegister(this.Instruction.Reg1 , this.FetchedData);
            }
        }

        #endregion

        #region PROC LDH

        internal void PROC_LDH()
        {
            if (this is Cpu cpu)
            {
                if (cpu.Instruction.Reg1 == RegType.RT_A)
                    cpu.CpuWriteRegister(cpu.Instruction.Reg1, cpu.Bus.Read((ushort)(0xFF00 | cpu.FetchedData)));
               
                else
                    cpu.Bus.Write(this.MemoryAdressDest, cpu.CpuRegisters.A);

                this.Cycles(1);

                return;
            }

            throw ExceptionsUtil.NotSupportedCpu();
        }

        #endregion

        #region  PROC_OR

        public void PROC_OR()
        {
            if(this is Cpu cpu)
            {
                byte a = cpu.CpuRegisters.A;
                a |= (byte)(cpu.FetchedData & 0xFF);

                cpu.CpuRegisters.SetRegisterA(a);
                cpu.CpuSetFlags(z : cpu.CpuRegisters.A == 0 ? 1 : 0, n : 0, h : 0, c : 0);
                return;
            }

            throw ExceptionsUtil.NotSupportedCpu();
        }

        #endregion

        #region PROC XOR

        internal void PROC_XOR()
        {
            byte a = this.CpuRegisters.A;
            a ^= (byte)(this.FetchedData & 0xFF);
            this.CpuRegisters.SetRegisterA(a);

            // TODO: VALIDAR 
            int z = this.CpuRegisters.A == 0 ? 1 : 0;
            this.CpuSetFlags(z, 0, 0, 0);
        }

        #endregion

        #region PROC DI

        internal void PROC_DI()
        {
            this.IntMasterEnabled = false;
        }

        #endregion

        #region PROC POP

        public void PROC_POP()
        {
            if(this is Cpu cpu)
            {
                byte lo = this.Stack.Pop();
                this.Cycles(1);
                byte hi = this.Stack.Pop();
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

                if(cpu.Instruction.Reg1.Is16Bits())
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

                if (cpu.Instruction.Reg1.Is16Bits())
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
                int value = cpu.CpuReadRegister(cpu.Instruction.Reg1) + (char)cpu.FetchedData;

                if(cpu.Instruction.Reg1.Is16Bits())
                {
                    cpu.Cycles(1);
                }

                if(cpu.Instruction.Reg1 == RegType.RT_SP)
                    value = cpu.CpuReadRegister(cpu.Instruction.Reg1) + (char)cpu.FetchedData;

                int z = (value & 0xFF) == 0 ? 1 : 0; ;
                int h = (cpu.CpuReadRegister(cpu.Instruction.Reg1) & 0xF) + (cpu.FetchedData & 0xF) >= 0x10 ? 1 : 0; ;
                int c = (cpu.CpuReadRegister(cpu.Instruction.Reg1) & 0xFF) + (cpu.FetchedData & 0xFF) >= 0x100 ? 1 : 0; ;

                if (cpu.Instruction.Reg1.Is16Bits())
                {
                    z = 0;
                    h = (cpu.CpuReadRegister(cpu.Instruction.Reg1) & 0xFFF) + (cpu.FetchedData & 0xFFF) >= 0x1000 ? 1 : 0; ;
                    int n  = (cpu.CpuReadRegister(cpu.Instruction.Reg1)) + (cpu.FetchedData);
                    c = n >= 0x10000 ? 1 : 0;
                }

                if(cpu.Instruction.Reg1 == RegType.RT_SP)
                {
                    z = -1;
                    h = (cpu.CpuReadRegister(cpu.Instruction.Reg1) & 0xF) + (cpu.FetchedData & 0xF) >= 0x10 ? 1 : 0; ;
                    c = (cpu.CpuReadRegister(cpu.Instruction.Reg1) & 0xFF) + (cpu.FetchedData & 0xFF) >= 0x100 ? 1 : 0; ;
                }

                cpu.CpuWriteRegister(cpu.Instruction.Reg1, (ushort)(value & 0xFFFF));
                cpu.CpuSetFlags(z, n : 0, h, c);
                
                return;
            }

            throw ExceptionsUtil.NotSupportedCpu();
        }

        #endregion

        #region PROC ADC

        #endregion

        #region PROC SUB

        public void PROC_SUB()
        {
            if(this is Cpu cpu)
            {
                int value = cpu.CpuReadRegister(cpu.Instruction.Reg1) - cpu.FetchedData;

                int z = 0;
                int h = (cpu.CpuReadRegister(cpu.Instruction.Reg1) & 0xF) - (cpu.FetchedData & 0xF) < 0x10 ? 1 : 0; ;
                int c = cpu.CpuReadRegister(cpu.Instruction.Reg1) + (cpu.FetchedData & 0xFF) < 0 ? 1 : 0; ;

                cpu.CpuWriteRegister(cpu.Instruction.Reg1, (ushort)value);
                cpu.CpuSetFlags(z, n : 1, h, c);

                return;
            }

            throw ExceptionsUtil.NotSupportedCpu();
        }

        #endregion

        #region PROC SBC

        public void PROC_SBC()
        {
            if (this is Cpu cpu)
            {
                ushort value = cpu.FetchedData;
                value += Convert.ToUInt16(CPU_FLAG_C);

                int z = cpu.CpuReadRegister(cpu.Instruction.Reg1) - value < 0 ? 1 : 0;
                int h = ((cpu.CpuReadRegister(cpu.Instruction.Reg1) & 0xF) 
                        - (cpu.FetchedData & 0xF)
                        - (Convert.ToInt32(CPU_FLAG_C))) < 0 ? 1 : 0; 
                int c = (cpu.CpuReadRegister(cpu.Instruction.Reg1) 
                        - cpu.FetchedData
                        - Convert.ToInt32(CPU_FLAG_C)) < 0 ? 1 : 0;

                cpu.CpuWriteRegister(cpu.Instruction.Reg1, (ushort)(cpu.CpuReadRegister(cpu.Instruction.Reg1) - value));
                cpu.CpuSetFlags(z, n: 1, h, c);

                return;
            }

            throw ExceptionsUtil.NotSupportedCpu();

        }

        #endregion

        #region PROC CPL

        public void PROC_CPL()
        {
            if(this is Cpu cpu)
            {
                byte a = cpu.CpuRegisters.A;
                a = (byte)~a;

                cpu.CpuRegisters.SetRegisterA(a);
                cpu.CpuSetFlags(-1, 1, 1, -1);

                return;
            }

            throw ExceptionsUtil.NotSupportedCpu();
        }

        #endregion

        #region PROC ADC

        public void PROC_ADC()
        {
            if(this is Cpu cpu)
            {
                ushort u = cpu.FetchedData;
                ushort a = cpu.CpuRegisters.A;
                ushort flagC = Convert.ToUInt16(CPU_FLAG_C);

                cpu.CpuRegisters.SetRegisterA((byte)((a + u + flagC) & 0xFF));

                int z = this.CpuRegisters.A == 0 ? 1 : 0;
                int n = 0;
                int h = (a & 0xF) + (u & 0xF) + flagC > 0xF ? 1 : 0;
                int c = a + u + flagC > 0xFF ? 1 : 0;

                cpu.CpuSetFlags(z, n, h, flagC);
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

                default: return false;
            }
        }

        // TODO: VALIDAR 

        internal void CpuSetFlags(int z, int n, int h, int c)
        {
            if (z != -1)
            {
                this.CpuRegisters.SetRegisterF(BitHelper.BitSet(this.CpuRegisters.F, 7, z));
            }

            if (n != -1)
            {
                this.CpuRegisters.SetRegisterF(BitHelper.BitSet(this.CpuRegisters.F, 6, n));
            }

            if (h != -1)
            {
                this.CpuRegisters.SetRegisterF(BitHelper.BitSet(this.CpuRegisters.F, 5, h));
            }

            if (c != -1)
            {
                this.CpuRegisters.SetRegisterF(BitHelper.BitSet(this.CpuRegisters.F, 4, c));
            }
        }

        #endregion

        #endregion PROCESS
    }
}
