namespace EmuladorGBA.Business.Extensions
{
    internal static class StructExtension
    {
        public static bool IsEmpty<Struct>(this Struct item)
            where Struct : struct
        {
            return item.Equals(default(Struct));
        }
    }
}
