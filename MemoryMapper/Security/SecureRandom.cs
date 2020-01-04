/*
==============================================================================
Copyright © Jason Drawdy (CloneMerge)

All rights reserved.

The MIT License (MIT)

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.

Except as contained in this notice, the name of the above copyright holder
shall not be used in advertising or otherwise to promote the sale, use or
other dealings in this Software without prior written authorization.
==============================================================================
*/

#region Imports

using System;
using System.Security.Cryptography;

#endregion
namespace MemoryMapper
{
    /// <summary>
    /// Provides an interface for generating cryptographically strong data.
    /// </summary>
    public class SecureRandom
    {
        #region Variables

        private const int BufferSize = 1024;  // must be a multiple of 4
        private byte[] RandomBuffer;
        private int BufferOffset;
        private RNGCryptoServiceProvider rng;

        #endregion
        #region Initialization

        /// <summary>
        /// Provides an interface for generating cryptographically strong data.
        /// </summary>
        public SecureRandom()
        {
            RandomBuffer = new byte[BufferSize];
            rng = new RNGCryptoServiceProvider();
            BufferOffset = RandomBuffer.Length;
        }

        #endregion
        #region Methods

        private void FillBuffer()
        {
            rng.GetBytes(RandomBuffer);
            BufferOffset = 0;
        }
        /// <summary>
        /// Returns a non-negative random integer.
        /// </summary>
        /// <returns></returns>
        public int Next()
        {
            if (BufferOffset >= RandomBuffer.Length)
            {
                FillBuffer();
            }
            int val = BitConverter.ToInt32(RandomBuffer, BufferOffset) & 0x7fffffff;
            BufferOffset += sizeof(int);
            return val;
        }
        /// <summary>
        /// Returns a non-negative random integer that is less than the specified maximum value.
        /// </summary>
        /// <param name="maxValue">The exclusive upper bound of the random number to be generated. maxValue must be greater than or equal to 0.</param>
        public int Next(int maxValue)
        {
            return Next() % maxValue;
        }
        /// <summary>
        /// Returns a random integer that is within a specified range.
        /// </summary>
        /// <param name="minValue">The inclusive lower bound of the random number returned.</param>
        /// <param name="maxValue">The exclusive upper bound of the random number returned. maxValue must be greater than or equal to minValue.</param>
        /// <returns></returns>
        public int Next(int minValue, int maxValue)
        {
            if (maxValue < minValue)
            {
                throw new ArgumentOutOfRangeException("maxValue must be greater than or equal to minValue");
            }
            int range = maxValue - minValue;
            return minValue + Next(range);
        }
        /// <summary>
        /// Returns a random floating-point number that is greater than or equal to 0.0, and less than 1.0.
        /// </summary>
        public double NextDouble()
        {
            int val = Next();
            return (double)val / int.MaxValue;
        }
        /// <summary>
        /// Fills a specified array with cryptographically strong random data.
        /// </summary>
        /// <param name="buff">The buffer to write random data to.</param>
        public void GetBytes(byte[] buff)
        {
            rng.GetBytes(buff);
        }
        /// <summary>
        /// Returns an array filled with cryptographically strong random data.
        /// </summary>
        /// <param name="bufferLength">The length of the buffer to return.</param>
        public byte[] GetBytes(int bufferLength)
        {
            byte[] data = new byte[bufferLength];
            rng.GetBytes(data);
            return data;
        }

        #endregion
    }
}
