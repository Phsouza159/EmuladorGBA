using EmuladorGBA.Business.Intruction;

namespace EmuladorGBA.Business.Extensions
{
    internal static class StructExtension
    {
        public static bool IsEmpty<Struct>(this Struct item)
            where Struct : struct
        {
            return item.Equals(default(Struct));
        }

        public static bool Is16Bits(this CpuInstruction instruction)
        {
            return instruction.Reg1 >= Enum.RegType.RT_AF;
        }
    }
}
