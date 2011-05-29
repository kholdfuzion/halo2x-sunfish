using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Sunfish.Canvas
{
    public class Surface
    {
        public char[] Magic;
        public SurfaceHeader Header;
        public byte[] Data1;
        public byte[] Data2;

        public Surface(Stream stream)
        {
            BinaryReader binReader = new BinaryReader(stream);
            Magic = binReader.ReadChars(4);
            Header = new SurfaceHeader(stream, (int)stream.Position);
        }

        public class SurfaceHeader
        {
            public int Size;
            public int Height;
            public int Width;
            public int PitchOrLinearSize;
            public int Depth;
            public int MipMapCount;
            public PixelFormat Format;
            public Capibilities Caps;

            public SurfaceHeader(Stream stream, int offset)
            {
                BinaryReader binReader = new BinaryReader(stream);
                stream.Position = offset;

                Size = binReader.ReadInt32();
                Height = binReader.ReadInt32();
                Width = binReader.ReadInt32();
                PitchOrLinearSize = binReader.ReadInt32();
                Depth = binReader.ReadInt32();
                MipMapCount = binReader.ReadInt32();

                Format = new PixelFormat(stream, (int)stream.Position);
                Caps = new Capibilities(stream, (int)stream.Position);
            }

            public class PixelFormat
            {
                public int Size;
                public EdwFlags Flags;
                public char[] FourCC;
                public int BitCount;
                public int RBitMask;
                public int GBitMask;
                public int BBitMask;
                public int AlphaBitMask;

                public PixelFormat(Stream stream, int offset)
                {
                    BinaryReader binReader = new BinaryReader(stream);
                    stream.Position = offset;

                    Size = binReader.ReadInt32();
                    Flags = (EdwFlags)binReader.ReadInt32();
                    FourCC = binReader.ReadChars(4);
                    BitCount = binReader.ReadInt32();
                    RBitMask = binReader.ReadInt32();
                    GBitMask = binReader.ReadInt32();
                    BBitMask = binReader.ReadInt32();
                    AlphaBitMask = binReader.ReadInt32();
                }

                [Flags]
                public enum EdwFlags
                {
                    NONE = 0x00000000,
                    ALPHAPIXELS = 0x00000001,
                    ALPHA = 0x00000002,
                    FOURCC = 0x00000004,
                    RGB = 0x00000040,
                    LUMINANCE = 0x00020000,
                    YUV = 0x00080000,
                }
            }

            public class Capibilities
            {
                public EdwCaps1 Caps1;
                public EdwCaps2 Caps2;

                public Capibilities(Stream stream, int offset)
                {
                    BinaryReader binReader = new BinaryReader(stream);
                    stream.Position = offset;

                    Caps1 = (EdwCaps1)binReader.ReadInt32();
                    Caps2 = (EdwCaps2)binReader.ReadInt32();
                }

                [Flags]
                public enum EdwCaps1
                {
                    DDSCAPS_NONE = 0x00000000,
                    DDSCAPS_COMPLEX = 0x00000008,
                    DDSCAPS_TEXTURE = 0x00001000,
                    DDSCAPS_MIPMAP = 0x00400000,
                }

                [Flags]
                public enum EdwCaps2
                {
                    DDSCAPS2_NONE = 0x00000000,
                    DDSCAPS2_CUBEMAP = 0x00000200,
                    DDSCAPS2_CUBEMAP_POSITIVEX = 0x00000400,
                    DDSCAPS2_CUBEMAP_NEGATIVEX = 0x00000800,
                    DDSCAPS2_CUBEMAP_POSITIVEY = 0x00001000,
                    DDSCAPS2_CUBEMAP_NEGATIVEY = 0x00002000,
                    DDSCAPS2_CUBEMAP_POSITIVEZ = 0x00004000,
                    DDSCAPS2_CUBEMAP_NEGATIVEZ = 0x00008000,
                    DDSCAPS2_VOLUME = 0x00200000,
                }
            }
        }
    }
}