using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Sunfish
{
    public static class Buffering
    {
        public static void Copy(Stream source, Stream destination)
        {
            Copy(source, source.Position, source.Length, destination, destination.Position);
        }

        public static void Copy(Stream source, long sourceAddress, long byteCount, Stream destination, long destinationAddress)
        {
            const int blockSize = 512;

            source.Position = sourceAddress;
            destination.Position = destinationAddress;

            byte[] buffer;
            if (byteCount > blockSize)
            {
                long blockCount = byteCount / blockSize;
                buffer = new byte[blockSize];
                for (int i = 0; i < blockCount; i++)
                {
                    source.Read(buffer, 0, blockSize);
                    destination.Write(buffer, 0, blockSize);
                }
            }
            byteCount = byteCount % blockSize;
            if (byteCount == 0) return;
            buffer = new byte[byteCount];
            source.Read(buffer, 0, buffer.Length);
            destination.Write(buffer, 0, buffer.Length);
        }
    }
}
