using EmuladorGBA.Business.Enum;
using EmuladorGBA.Business.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmuladorGBA.Business.Process
{
    internal static class CpuFecthData
    {
        internal static void FecthData(this Cpu cpu)
        {
            ushort adress = 0;
            ushort pc;
            byte lo;
            ushort hi;

            cpu.MemoryAdressDest = 0;
            cpu.DestIsMemory = false;

            if (cpu.Instruction.IsEmpty())
                return;

            switch (cpu.Instruction.Mode)
            {
                case AddrMode.AM_IMP:
                    return;

                case AddrMode.AM_R:
                    cpu.FetchedData = cpu.CpuReadRegister(cpu.Instruction.Reg1);
                    return;

                case AddrMode.AM_R_R:
                    cpu.FetchedData = cpu.CpuReadRegister(cpu.Instruction.Reg2);
                    return;

                case AddrMode.AM_R_D8:
                    cpu.FetchedData = cpu.Bus.Read(cpu.CpuRegisters.PC);
                    cpu.Cycles(1);
                    cpu.CpuRegisters.IncrementPC();
                    return;

                case AddrMode.AM_R_D16:
                case AddrMode.AM_D16:

                    pc = cpu.CpuRegisters.PC;
                    lo = cpu.Bus.Read(pc);
                    cpu.Cycles(1);

                    hi = cpu.Bus.Read((ushort)(pc + 1));
                    cpu.Cycles(1);

                    cpu.FetchedData = (ushort)(lo | (hi << 8));
                    cpu.CpuRegisters.SetRegisterPC((ushort)(pc + 2));

                    return;

                case AddrMode.AM_MR_R:

                    cpu.FetchedData = cpu.CpuReadRegister(cpu.Instruction.Reg2);
                    cpu.MemoryAdressDest = cpu.CpuReadRegister(cpu.Instruction.Reg1);
                    cpu.DestIsMemory = true;

                    if(cpu.Instruction.Reg1 == RegType.RT_C)
                    {
                        adress = 0xFF00; // DEFAULT AM_MR_R
                        cpu.MemoryAdressDest = (ushort)(cpu.MemoryAdressDest | adress);
                    }

                    return;

                case AddrMode.AM_R_MR:

                    adress = cpu.CpuReadRegister(cpu.Instruction.Reg2);

                    if (cpu.Instruction.Reg1 == RegType.RT_C)
                    {
                        adress = (ushort)(adress | 0xFF00); // DEFAULT AM_R_MR
                    }

                    cpu.FetchedData = cpu.Bus.Read(adress);
                    cpu.Cycles(1);

                    return;


                case AddrMode.AM_R_HLI:

                    cpu.FetchedData = cpu.Bus.Read(cpu.CpuReadRegister(cpu.Instruction.Reg2));
                    cpu.Cycles(1);

                    cpu.CpuWriteRegister(RegType.RT_HL, (ushort)(cpu.CpuReadRegister(RegType.RT_HL) + 1));

                    return;

                case AddrMode.AM_R_HLD:

                    cpu.FetchedData = cpu.Bus.Read(cpu.CpuReadRegister(cpu.Instruction.Reg2));
                    cpu.Cycles(1);

                    cpu.CpuWriteRegister(RegType.RT_HL, (ushort)(cpu.CpuReadRegister(RegType.RT_HL) - 1));

                    return;

                case AddrMode.AM_HLI_R:

                    cpu.FetchedData = cpu.CpuReadRegister(cpu.Instruction.Reg2);
                    cpu.MemoryAdressDest = cpu.CpuReadRegister(cpu.Instruction.Reg1);
                    cpu.DestIsMemory = true;

                    cpu.CpuWriteRegister(RegType.RT_HL, (ushort)(cpu.CpuReadRegister(RegType.RT_HL) + 1));

                    return;

                case AddrMode.AM_HLD_R:

                    cpu.FetchedData = cpu.Bus.Read(cpu.CpuReadRegister(cpu.Instruction.Reg2));
                    cpu.MemoryAdressDest = cpu.Bus.Read(cpu.CpuReadRegister(cpu.Instruction.Reg1));
                    cpu.DestIsMemory = true;

                    cpu.CpuWriteRegister(RegType.RT_HL, (ushort)(cpu.CpuReadRegister(RegType.RT_HL) - 1));

                    return;

                case AddrMode.AM_R_A8:

                    cpu.FetchedData = cpu.Bus.Read(cpu.CpuRegisters.PC);
                    cpu.Cycles(1);
                    cpu.CpuRegisters.IncrementPC();

                    return;

                case AddrMode.AM_A8_R:

                    cpu.MemoryAdressDest = (ushort)(cpu.Bus.Read(cpu.CpuRegisters.PC) | 0xFF00);
                    cpu.DestIsMemory = true;
                    cpu.Cycles(1);
                    cpu.CpuRegisters.IncrementPC();

                    return;

                case AddrMode.AM_HL_SPR:

                    cpu.FetchedData = cpu.Bus.Read(cpu.CpuRegisters.PC);
                    cpu.Cycles(1);
                    cpu.CpuRegisters.IncrementPC();

                    return;

                case AddrMode.AM_D8:

                    cpu.FetchedData = cpu.Bus.Read(cpu.CpuRegisters.PC);
                    cpu.Cycles(1);
                    cpu.CpuRegisters.IncrementPC();

                    return;

                case AddrMode.AM_A16_R:
                case AddrMode.AM_D16_R:
                    pc = cpu.CpuRegisters.PC;
                    lo = cpu.Bus.Read(pc);
                    cpu.Cycles(1);

                    hi = cpu.Bus.Read((ushort)(pc + 1));
                    cpu.Cycles(1);

                    cpu.MemoryAdressDest = (ushort)(lo | (hi << 8));
                    cpu.DestIsMemory = true;

                    cpu.CpuRegisters.SetRegisterPC((ushort)(pc + 2));
                    cpu.FetchedData = cpu.CpuReadRegister(cpu.Instruction.Reg2);

                    //cpu.FetchedData = (ushort)(lo | (hi << 8));
                    //cpu.DestIsMemory = true;
                    //cpu.CpuRegisters.SetRegisterPC((ushort)(pc + 2));
                    //cpu.FetchedData = cpu.CpuReadRegister(cpu.Instruction.Reg2);

                    return;

                case AddrMode.AM_MR_D8:

                    cpu.FetchedData = cpu.Bus.Read(cpu.CpuRegisters.PC);
                    cpu.Cycles(1);
                    cpu.CpuRegisters.IncrementPC();
                    cpu.MemoryAdressDest = cpu.CpuReadRegister(cpu.Instruction.Reg1);
                    cpu.DestIsMemory = true;
                    
                    return;

                case AddrMode.AM_MR:

                    cpu.MemoryAdressDest = cpu.CpuReadRegister(cpu.Instruction.Reg1);
                    cpu.DestIsMemory = true;
                    cpu.FetchedData = cpu.CpuReadRegister(cpu.Instruction.Reg1);
                    cpu.Cycles(1);

                    return;

                case AddrMode.AM_R_A16:

                    pc = cpu.CpuRegisters.PC;
                    lo = cpu.Bus.Read(pc);
                    cpu.Cycles(1);

                    hi = cpu.Bus.Read((ushort)(pc + 1));
                    cpu.Cycles(1);

                    adress = (ushort)(lo | (hi << 8));
                    cpu.CpuRegisters.SetRegisterPC((ushort)(pc + 2));
                    cpu.FetchedData = cpu.Bus.Read(adress);
                    cpu.Cycles(1);

                    return;

                default:
                    Console.Write($"Mode sem suporte '{cpu.Instruction.Mode}' em '{cpu.CpuOpeCode}'");
                    throw new ArgumentException("STOP CPU");
            }
        }
    }
}
