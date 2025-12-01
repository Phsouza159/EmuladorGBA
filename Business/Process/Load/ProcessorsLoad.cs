using EmuladorGBA.Business.Enum;

namespace EmuladorGBA.Business.Process.Load
{
    internal static class ProcessorsLoad
    {
        internal static void Load(Cpu cpu)
        {
            int len = System.Enum.GetValues(typeof(InType)).Length;
            cpu.Processors = new CpuProcesses.IN_PROC[len];

            cpu.Processors[(short)InType.IN_NONE]   = cpu.PROC_NONE;
            cpu.Processors[(short)InType.IN_NOP]    = cpu.PROC_NOP;
            cpu.Processors[(short)InType.IN_LD]     = cpu.PROC_LD;
            cpu.Processors[(short)InType.IN_LDH]    = cpu.PROC_LDH;
            cpu.Processors[(short)InType.IN_JP]     = cpu.PROC_JP;
            cpu.Processors[(short)InType.IN_JR]     = cpu.PROC_JR;
            cpu.Processors[(short)InType.IN_DI]     = cpu.PROC_DI;
            cpu.Processors[(short)InType.IN_XOR]    = cpu.PROC_XOR;
            cpu.Processors[(short)InType.IN_POP]    = cpu.PROC_POP;
            cpu.Processors[(short)InType.IN_PUSH]   = cpu.PROC_PUSH;
            cpu.Processors[(short)InType.IN_CALL]   = cpu.PROC_CALL;
            cpu.Processors[(short)InType.IN_RET]    = cpu.PROC_RET;
            cpu.Processors[(short)InType.IN_RETI]   = cpu.PROC_RETI;
            cpu.Processors[(short)InType.IN_RST]    = cpu.PROC_RST;
        }
    }
}
