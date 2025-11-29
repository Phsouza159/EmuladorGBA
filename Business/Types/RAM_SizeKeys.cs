using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmuladorGBA.Business.Types
{
    internal static class ROM_Size
    {
        private static Dictionary<Decimal, string>? _values { get; set; } = null;

        public static Dictionary<Decimal, string> Values
        {
            get
            {
                if (ROM_Size._values is null)
                    ROM_Size._values = new Dictionary<Decimal, string>
                    {
                        { 0x00 , "32 KiB (2, no banking)" },
                        { 0x01 , "64 KiB (4)" },
                        { 0x02 , "128 KiB (8)" },
                        { 0x03 , "256 KiB (16)" },
                        { 0x04 , "512 KiB (32)" },
                        { 0x05 , "1 MiB (64)" },
                        { 0x06 , "2 MiB (128)" },
                        { 0x07 , "4 MiB (256)" },
                        { 0x08 , "8 MiB (512)" },
                        { 0x52 , "1.1 MiB (72, 13)" },
                        { 0x53 , "1.2 MiB (80, 13)" },
                        { 0x54 , "1.5 MiB (96, 13)" }
                    };

                return ROM_Size._values;
            }
        }
    }
}
