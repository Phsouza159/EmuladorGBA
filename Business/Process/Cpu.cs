using EmuladorGBA.Business.Enum;
using EmuladorGBA.Business.Extensions;
using EmuladorGBA.Business.Interface;
using EmuladorGBA.Business.Intruction;
using EmuladorGBA.Business.Memory;
using EmuladorGBA.Business.Process.Load;
using EmuladorGBA.Business.Register;

namespace EmuladorGBA.Business.Process
{
    internal class Cpu : CpuProcesses
    {
        public Cpu()
        {
            this.CpuRegisters = new CpuRegisters();
            this.Stack = new Stack(this);

            // CONFIG INIT
            this.CpuRegisters.SetRegisterPC(0x100);
            this.CpuRegisters.SetRegisterA(0x01);

            this.LoadInstruction();
            this.LoadProcess();
        }


        #region LOAD INSTRUCTION / PROCESS

        protected void LoadInstruction() => InstructionsLoad.Load(this);

        protected void LoadProcess() => ProcessorsLoad.Load(this);

        protected void LoadLoockUp()
        {
            //TODO..
        }

        #endregion

        #region STEP

        internal bool Step()
        {
            if (!this.Halted)
            {
                ushort pc = this.CpuRegisters.PC;

                this.FetchInstruction();
                this.FecthData();

                Console.WriteLine(
                    $"* OK Ticekt {this.Tickets, -5} | PC {pc:X4}: {this.InstName(this.Instruction.Type),-7} | " +
                    $"({this.CpuOpeCode:X2} {this.Bus.Read((ushort)(pc + 1)):X2} {this.Bus.Read((ushort)(pc + 2)):X2}) | " +
                    $"AF: {this.CpuRegisters.A:X2}{this.CpuRegisters.F:X2} " +
                    $"- BC: {this.CpuRegisters.B:X2}{this.CpuRegisters.C:X2} " +
                    $"- DE: {this.CpuRegisters.D:X2}{this.CpuRegisters.E:X2} " +
                    $"- HL: {this.CpuRegisters.H:X2}{this.CpuRegisters.L:X2} " +
                    $"- SP: {this.CpuRegisters.SP.ToString("X2"), -4} " 
                );

                if (this.Instruction.IsEmpty())
                {
                    throw new ArgumentException($"Instrução não suportada em: {this.CpuOpeCode:X2}");
                }

                this.Execute();
            }

            return true;
        }

        #endregion
    }
}
