using EmuladorGBA.Business.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EmuladorGBA.Business.Process.CpuProcesses;

namespace EmuladorGBA.Business.Process.Load
{
    internal static class ProcessorsLoad
    {
        internal static void Load(Cpu cpu)
        {
            int len = System.Enum.GetValues(typeof(InType)).Length;
            cpu.Processors = new IN_PROC[len];

            cpu.Processors[(short)InType.IN_NONE]   = cpu.PROC_NONE;
            cpu.Processors[(short)InType.IN_NOP]    = cpu.PROC_NOP;
            cpu.Processors[(short)InType.IN_LD]     = cpu.PROC_LD;
            cpu.Processors[(short)InType.IN_JP]     = cpu.PROC_JP;
            cpu.Processors[(short)InType.IN_DI]     = cpu.PROC_DI;
            cpu.Processors[(short)InType.IN_XOR]    = cpu.PROC_XOR;
        }
    }
}
