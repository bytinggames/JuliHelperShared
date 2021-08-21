using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JuliHelper.Test
{
    [TestClass]
    public class ByteExtensionTest
    {
        [TestMethod]
        public void Test()
        {
            byte b = 0;

            Assert.AreEqual((byte)0, b);

            Assert.IsFalse(ByteExtension.IsBitSet(b, 1));
            Assert.IsFalse(ByteExtension.IsBitSet(b,0));

            ByteExtension.SetBit(ref b,1);

            Assert.IsTrue(ByteExtension.IsBitSet(b,1));
            Assert.IsFalse(ByteExtension.IsBitSet(b,0));

            ByteExtension.SetBit(ref b,0);

            Assert.IsTrue(ByteExtension.IsBitSet(b,1));
            Assert.IsTrue(ByteExtension.IsBitSet(b,0));


            ByteExtension.UnsetBit(ref b,1);

            Assert.IsFalse(ByteExtension.IsBitSet(b,1));
            Assert.IsTrue(ByteExtension.IsBitSet(b,0));

            ByteExtension.UnsetBit(ref b,0);

            Assert.IsFalse(ByteExtension.IsBitSet(b,1));
            Assert.IsFalse(ByteExtension.IsBitSet(b,0));

            Assert.AreEqual((byte)0, b);
        }
    }
}
