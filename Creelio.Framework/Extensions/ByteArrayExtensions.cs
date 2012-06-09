namespace Creelio.Framework.Core.Extensions.ByteArrayExtensions
{
    using System.Text;

    public static class ByteArrayExtensions
    {
        public static string ToHexString(this byte[] ba)
        {
            StringBuilder sb = new StringBuilder("0x");
            foreach (byte b in ba)
            {
                sb.AppendFormat("{0:X}", b);
            }

            return sb.ToString();
        }
    }
}