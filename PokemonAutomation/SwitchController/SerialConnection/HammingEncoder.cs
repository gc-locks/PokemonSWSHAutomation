using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace SwitchController.SerialConnection
{
    /*
     * from https://en.wikipedia.org/wiki/Hamming_code
     */
    public static class HammingEncoder
    {
        static readonly int g0 = 0b1101;
        static readonly int g1 = 0b1011;
        static readonly int g2 = 0b1000;
        static readonly int g3 = 0b0111;
        static readonly int g4 = 0b0100;
        static readonly int g5 = 0b0010;
        static readonly int g6 = 0b0001;
        static readonly int g7 = 0b1110;
        static readonly int[] G = new int[] { g7, g6, g5, g4, g3, g2, g1, g0 };

        static readonly int h0 = 0b10101010;
        static readonly int h1 = 0b01100110;
        static readonly int h2 = 0b00011110;
        static readonly int h3 = 0b11111111;
        static readonly int[] H = new int[] { h3, h2, h1, h0 };

        private static int Mul(int[] M, int x, int columns)
        {
            var Mix = M.Select(Mi => x & Mi).ToArray();
            return (byte)Enumerable.Range(0, columns).Select(i =>
            {
                return Mix
                .Select((m, j) => ((m >> i) & 1) << j)
                .Aggregate((p, q) => p | q);
            })
            .Aggregate((p, q) => p ^ q);
        }

        public static IEnumerable<byte> Encode(IEnumerable<byte> data) =>
            data.SelectMany(x => (new int[] { ((x & 0xf0) >> 4), (x & 0x0f) }).Select(y => (byte)Mul(G, y, 4)));

        private static byte SampleData(byte x)
        {
            return (byte)(((x & 0b00100000) >> 2) | ((x & 0b1110) >> 1));
        }

        public static bool TryDecodeHalfByte(byte x, out byte decoded)
        {
            var Hx = Mul(H, x, 8);
            if (Hx == 0)
            {
                decoded = SampleData(x);
                return true;
            }

            // 誤り
            int find = H.Select((h, i) => h ^ ((Hx & (1 << i)) != 0 ? 0xff : 0x00))
                .Aggregate((p, q) => p | q);

            var isZero = Enumerable.Range(0, 8)
                .Select(i =>
                {
                    return (find & (1 << i)) == 0 ? i : (int?)null;
                })
                .Where(i => i != null)
                .Cast<int>()
                .ToArray();

            if (isZero.Count() == 1)
            {
                // 修正
                decoded = SampleData((byte)(x ^ (1 << isZero.Single())));
                return true;
            }

            // 2誤り
            decoded = default;
            return false;
        }
    }
}
