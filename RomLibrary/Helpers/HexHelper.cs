using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RomLibrary.Helpers
{
    public static class HexHelper
    {
        public static IEnumerable<byte> AsByteArray(this string value)
        {
            return Enumerable.Range(0, value.Length)
                .Where(x => x % 2 == 0)
                .Select(x => Convert.ToByte(value.Substring(x, 2), 16));
        }

        public static IEnumerable<string> AsHex(this byte[] bytes)
        {
            return bytes.ToList()
                .Select(b => b.ToString("X2"));
        }

        public static string AsHexMerged(this byte[] bytes)
        {
            return string.Concat(bytes.AsHex());
        }

        public static int IndexOf(this byte[] bytes, string hex)
        {
            return bytes.AsHexMerged()
                .IndexOf(hex) / 2;
        }

        public static IEnumerable<byte> Replace(this byte[] bytes, IEnumerable<byte> replace, int start)
        {
            int maxLength = bytes.Length;
            if ((replace.Count() + start) > maxLength)
                throw new ArgumentOutOfRangeException();

            replace.ToList()
                .ForEach(nv => bytes[start++] = nv);

            return bytes;
        }

        public static IEnumerable<byte> Substring(this byte[] bytes, int start, int length)
        {
            return bytes.AsEnumerable()
                .Skip(start)
                .Take(length);
        }
    }
}
