using EmuladorGBA.Business.Interface;
using EmuladorGBA.Business.Process;
using EmuladorGBA.Business.Util;

namespace EmuladorGBA.Business.Memory
{
    internal class Stack : IStack
    {
        public Stack(ICpu cpu)
        {
            Cpu = cpu;
        }

        private ICpu Cpu { get; }

        public void Push(byte data)
        {
            if(this.Cpu is Cpu cpu)
            {
                cpu.CpuRegisters.DescrementSP();
                cpu.Bus.Write(cpu.CpuRegisters.SP, data);
                return;
            }

            throw ExceptionsUtil.NotSupportedCpu();
        }

        public void Push(ushort data)
        {
            this.Push((byte)((data >> 8) & 0xFF));
            this.Push((byte)(data  & 0xFF));
        }

        public byte Pop()
        {
            if (this.Cpu is Cpu cpu)
            {
                ushort sp = cpu.CpuRegisters.SP;
                cpu.CpuRegisters.IncrementSP();
                return cpu.Bus.Read(sp);
            }

            throw ExceptionsUtil.NotSupportedCpu();
        }

        public ushort Pop16Bits()
        {
            byte lo = this.Pop();
            byte hi = this.Pop();

            return (ushort)((hi << 8) | lo);
        }
    }
}
