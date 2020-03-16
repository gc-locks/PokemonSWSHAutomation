using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SwitchController.SerialConnection;
using System.Linq;

namespace SwitchControllerTest
{
    [TestClass]
    public class HammingEncoderTest
    {
        [TestMethod]
        public void TestEncode()
        {
            var val = HammingEncoder.Encode(new byte[] { 0b00001011 }).ToArray();
            Assert.AreEqual(2, val.Count());
            Assert.AreEqual(0b00000000, val.Skip(0).Take(1).FirstOrDefault());
            Assert.AreEqual(0b01100110, val.Skip(1).Take(1).FirstOrDefault());
        }

        [TestMethod]
        public void TestDecode()
        {
            var original = (byte)0b00001011;
            var x = HammingEncoder.Encode(new byte[] { original }).Skip(1).First();

            var success = HammingEncoder.TryDecodeHalfByte(x, out byte decoded);
            Assert.IsTrue(success);
            Assert.AreEqual(original, decoded);

            // 1誤り
            foreach (int i in Enumerable.Range(0, 8))
            {
                var y = (byte)(x ^ (1 << i));

                var success2 = HammingEncoder.TryDecodeHalfByte(y, out byte decoded2);
                Assert.IsTrue(success2);
                Assert.AreEqual(original, decoded2);

                // 2誤り
                foreach (int j in Enumerable.Range(0, 8))
                {
                    if (i == j)
                        continue;
                    var z = (byte)(y ^ (1 << j));

                    var success3 = HammingEncoder.TryDecodeHalfByte(z, out byte decoded3);
                    Assert.IsFalse(success3);
                }
            }
        }
    }
}
