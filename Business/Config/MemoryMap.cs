using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmuladorGBA.Business.Config
{
    public struct MemoryMap
    {
        public int WRAM_Length { get; set; }

        public int HRAM_Length { get; set; }
    }
}
