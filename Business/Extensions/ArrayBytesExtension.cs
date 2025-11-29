using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmuladorGBA.Business.Extensions
{
    internal static class ArrayBytesExtension
    {
        public static string ToValueString(this byte[] bytes)
        {
            return Encoding.ASCII.GetString(bytes);
        }
    }
}
