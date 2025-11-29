using EmuladorGBA.Business.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmuladorGBA.Business
{
    public class Cpu : ICpu
    {
        public Cpu()
        {
        }

        public const short ResolutionWeidth = 160;

        public const short ResolutionHeight = 144;

        internal int chuck = 0;

        internal void Init()
        {
            while(true)
            {
                Thread.Sleep(2000);

                this.chuck += 1;
            }
        }
    }
}
