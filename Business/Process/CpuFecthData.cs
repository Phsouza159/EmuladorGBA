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
            ushort adress;
            ushort pc;
            ushort lo;
            ushort hi;

            cpu.SetMemoryAdressDest(0);
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
                    
                    pc += 2;
                    cpu.CpuRegisters.SetRegisterPC(pc);

                    return;

                case AddrMode.AM_MR_R:

                    cpu.FetchedData = cpu.CpuReadRegister(cpu.Instruction.Reg2);
                    cpu.SetMemoryAdressDest(cpu.CpuReadRegister(cpu.Instruction.Reg1));
                    cpu.DestIsMemory = true;

                    if(cpu.Instruction.Reg1 == RegType.RT_C)
                    {
                        //adress = 0xFF00; // DEFAULT AM_MR_R
                        cpu.SetMemoryAdressDest((ushort)(cpu.MemoryAdressDest | 0xFF00));
                    }

                    return;

                case AddrMode.AM_R_MR:

                    adress = cpu.CpuReadRegister(cpu.Instruction.Reg2);

                    if (cpu.Instruction.Reg2 == RegType.RT_C)
                    {
                        adress = (ushort)(adress | 0xFF00); // DEFAULT AM_R_MR
                    }

                    cpu.FetchedData = cpu.Bus.Read(adress);
                    cpu.Cycles(1);

                    return;


                case AddrMode.AM_R_HLI:

                    cpu.FetchedData = cpu.Bus.Read(cpu.CpuReadRegister(cpu.Instruction.Reg2));
                    cpu.Cycles(1);
                   
                    ushort hli = cpu.CpuReadRegister(RegType.RT_HL);
                    hli += 1;

                    cpu.CpuWriteRegister(RegType.RT_HL, hli);

                    return;

                case AddrMode.AM_R_HLD:

                    cpu.FetchedData = cpu.Bus.Read(cpu.CpuReadRegister(cpu.Instruction.Reg2));
                    cpu.Cycles(1);

                    ushort hld = cpu.CpuReadRegister(RegType.RT_HL);
                    hld -= 1;

                    cpu.CpuWriteRegister(RegType.RT_HL, hld);

                    return;

                case AddrMode.AM_HLI_R:

                    cpu.FetchedData = cpu.CpuReadRegister(cpu.Instruction.Reg2);
                    cpu.SetMemoryAdressDest(cpu.CpuReadRegister(cpu.Instruction.Reg1));
                    cpu.DestIsMemory = true;

                    ushort hliR = cpu.CpuReadRegister(RegType.RT_HL);
                    hliR += 1;

                    cpu.CpuWriteRegister(RegType.RT_HL, hliR);

                    return;

                case AddrMode.AM_HLD_R:

                    cpu.FetchedData = cpu.Bus.Read(cpu.CpuReadRegister(cpu.Instruction.Reg2));
                    cpu.SetMemoryAdressDest(cpu.Bus.Read(cpu.CpuReadRegister(cpu.Instruction.Reg1)));
                    cpu.DestIsMemory = true;

                    ushort hldR = cpu.CpuReadRegister(RegType.RT_HL);
                    hldR -= 1;

                    cpu.CpuWriteRegister(RegType.RT_HL, hldR);

                    return;

                case AddrMode.AM_R_A8:

                    cpu.FetchedData = cpu.Bus.Read(cpu.CpuRegisters.PC);
                    cpu.Cycles(1);
                    cpu.CpuRegisters.IncrementPC();

                    return;

                case AddrMode.AM_A8_R:

                    cpu.SetMemoryAdressDest((ushort)(cpu.Bus.Read(cpu.CpuRegisters.PC) | 0xFF00));
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

                    cpu.SetMemoryAdressDest(Convert.ToUInt16(lo | (hi << 8)));
                    cpu.DestIsMemory = true;

                    pc += 2;
                    cpu.CpuRegisters.SetRegisterPC(pc);
                    cpu.FetchedData = cpu.CpuReadRegister(cpu.Instruction.Reg2);

                    return;

                case AddrMode.AM_MR_D8:

                    cpu.FetchedData = cpu.Bus.Read(cpu.CpuRegisters.PC);
                    cpu.Cycles(1);
                    cpu.CpuRegisters.IncrementPC();
                    cpu.SetMemoryAdressDest(cpu.CpuReadRegister(cpu.Instruction.Reg1));
                    cpu.DestIsMemory = true;
                    
                    return;

                case AddrMode.AM_MR:

                    cpu.SetMemoryAdressDest(cpu.CpuReadRegister(cpu.Instruction.Reg1));
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

                    adress = Convert.ToUInt16(lo | (hi << 8));

                    pc += 2;
                    cpu.CpuRegisters.SetRegisterPC(pc);
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
