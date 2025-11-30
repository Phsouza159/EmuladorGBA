using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmuladorGBA.Business
{
    internal static class BitHelper
    {
        public static int Bit(int a, int n)
        {
            return (a & (1 << n)) != 0 ? 1 : 0;
        }

        public static void BitSet(ref byte a, byte n, bool on)
        {
            if (on)
                a |= (byte)(1 << n);   // ON BIT
            else
                a &= (byte)~(1 << n);  // OFF BIT
        }

        public static bool Between(int a, int b, int c)
        {
            return a >= b && a <= c;
        }
    }
}
