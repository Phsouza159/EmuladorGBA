using EmuladorGBA.Business.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmuladorGBA.Business
{
    internal static class BitHelper
    {
        static bool _isReverseAtive = true;
        
        public static ushort Reverse(this ushort value, bool isReverseAtive = true)
        {
            if (!BitHelper._isReverseAtive && isReverseAtive)
                return value;

            return (ushort)(
                ((value & 0xFF00) >> 8) |  // pega o high byte e move para o low
                ((value & 0x00FF) << 8)    // pega o low byte e move para o high
            );

        }

        public static byte HighBit(this ushort value)
        {
            return (byte)(value >> 8);  // High byte
        }

        public static byte LowBit(this ushort value)
        {
            return (byte)(value & 0xFF);  // Low byte
        }

        public static int Bit(int a, int n)
        {
            return (a & (1 << n)) != 0 ? 1 : 0;
        }

        public static byte BitSet(byte a, byte n, int val)
        {
            if (val != 0)
                a |= (byte)(1 << n);   // ON BIT
            else
                a &= (byte)~(1 << n);  // OFF BIT

            return a;
        }

        public static bool Between(int a, int b, int c)
        {
            return a >= b && a <= c;
        }
    }
}
