using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmuladorGBA.Business.Util
{
    internal static class ExceptionsUtil
    {
        
        public static Exception NotSupportedCpu()
        {
            return new NotSupportedException("Sem suporte para tipo de CPU.");
        }
    }
}
