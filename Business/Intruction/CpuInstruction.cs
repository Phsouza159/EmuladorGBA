using EmuladorGBA.Business.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmuladorGBA.Business.Intruction
{
    internal struct CpuInstruction
    {
        public InType Type { get; set; }
        public AddrMode Mode { get; set; }
        public RegType Reg1 { get; set; }
        public RegType Reg2 { get; set; }
        public CondType Cond { get; set; }
        public byte Param { get; set; }

    }
}
