using System;
using System.Collections.Generic;
using System.Text;

namespace JuliHelper
{
    public static class ByteExtension
    {
        public static void SetBit(ref byte value, int index)
        {
            value |= (byte)(1 << index);
        }

        public static void UnsetBit(ref byte value, int index)
        {
            value &= (byte)~(1 << index);
        }

        public static bool IsBitSet(byte value, int index)
        {
            return (value & (1 << index)) != 0;
        }
    }
}
