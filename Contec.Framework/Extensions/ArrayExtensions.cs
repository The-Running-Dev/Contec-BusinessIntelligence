namespace Contec.Framework.Extensions
{
    using System.Linq;
    using System.Text;

    public static class ArrayExtensions
    {
        public static string ToHexString(this byte[] value)
        {
            return value.Aggregate(new StringBuilder(32),
                                   (sb, b) => sb.Append(b.ToString("X2")))
                        .ToString().ToLower();
        }
    }
}