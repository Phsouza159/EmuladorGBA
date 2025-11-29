using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmuladorGBA.Business.Extensions
{
    internal static class GetValueByDiconary
    {
        public static string GetValueOrDefault(this Dictionary<Decimal, string> dictionary, byte value)
        {
            if (Decimal.TryParse(value.ToString(), out decimal key) 
                && dictionary.ContainsKey(key))

                return dictionary[key];

            return "UNKNOWN";
        }

        public static string GetValueOrDefault(this Dictionary<Decimal, string> dictionary, byte[] values)
        {
            if (Decimal.TryParse(Encoding.ASCII.GetString(values), out decimal key)
                && dictionary.ContainsKey(key))

                return dictionary[key];

            return "UNKNOWN";
        }

        public static string GetValueOrDefault(this Dictionary<string, string> dictionary, string key)
        {
            if(dictionary.ContainsKey(key))
            {
                return dictionary[key];
            }

            return "UNKNOWN";
        }
    }
}
