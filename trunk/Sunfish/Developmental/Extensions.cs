using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sunfish.ValueTypes;
using System.IO;

namespace Sunfish
{
    public static class Extensions
    {
        public static string ReadUTF8String(this BinaryReader reader, int bytecount)
        { return Encoding.UTF8.GetString(reader.ReadBytes(bytecount)); }
    }
}
