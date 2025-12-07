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
        private static Cpu ctx {  get; set; }

        public static Cpu Create()
        {
            if(ctx is null)
                ctx = new Cpu();

            return ctx;
        }

        public static Cpu GetContext() => Cpu.ctx;

        private Cpu()
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

                byte f = this.CpuRegisters.F;

                string flags = 
                    $"{((f & (1 << 7)) != 0 ? 'Z' : '-')}" +
                    $"{((f & (1 << 6)) != 0 ? 'N' : '-')}" +
                    $"{((f & (1 << 5)) != 0 ? 'H' : '-')}" +
                    $"{((f & (1 << 4)) != 0 ? 'C' : '-')}";


                Console.WriteLine(
                    $"* OK Ticekt {this.Tickets.ToString("X8"), -5} | PC {pc:X4}: {this.InstName(this.Instruction.Type),-7} | " +
                    $"({this.CpuOpeCode:X2} {this.Bus.Read((ushort)(pc + 1)):X2} {this.Bus.Read((ushort)(pc + 2)):X2}) | " +
                    $"A: {this.CpuRegisters.A:X2} " +
                    $"F: {flags} " +
                    $" | BC: {this.CpuRegisters.B:X2}{this.CpuRegisters.C:X2} " +
                    $"- DE: {this.CpuRegisters.D:X2}{this.CpuRegisters.E:X2} " +
                    $"- HL: {this.CpuRegisters.H:X2}{this.CpuRegisters.L:X2} " +
                    $"- SP: {this.CpuRegisters.SP.ToString("X2"), -4} " +
                    $"- M-DEST: {this.MemoryAdressDest.ToString("X2"), -4} " +
                    $"- DATA: {this.FetchedData.ToString("X2"),-4} "


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
