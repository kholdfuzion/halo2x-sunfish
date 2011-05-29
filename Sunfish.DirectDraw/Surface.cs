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
        public SurfaceDescription Description;
        public byte[] Data1;
        public byte[] Data2;
        
        static Dictionary<H2BitmapCollection.BitmapData.EFormat, SurfaceDescription.PixelFormat> Lookup;

        static Surface()
        {
            Lookup = new Dictionary<H2BitmapCollection.BitmapData.EFormat, SurfaceDescription.PixelFormat>();
            SurfaceDescription.PixelFormat Format = new SurfaceDescription.PixelFormat();

            Format.FourCC = "DXT1".ToCharArray();
            Lookup[H2BitmapCollection.BitmapData.EFormat.DXT1] = Format;

            Format = new    SurfaceDescription.PixelFormat();
            Format.AlphaBitMask = 0x000000FF;
            Format.BitCount = 8;
            Lookup[H2BitmapCollection.BitmapData.EFormat.A8] = Format;

            Format = new    SurfaceDescription.PixelFormat();
            Format.BitCount = 8;
            Format.AlphaBitMask = 0x000000FF;
            Lookup[H2BitmapCollection.BitmapData.EFormat.AL8] = Format;

            Format = new    SurfaceDescription.PixelFormat();
            Format.LuminanceBitMask = 0x000000FF;
            Format.BitCount = 8;
            Lookup[H2BitmapCollection.BitmapData.EFormat.L8] = Format;

            Format = new    SurfaceDescription.PixelFormat();
            Format.LuminanceBitMask = 0x000000FF;
            Format.BitCount = 8;
            Lookup[H2BitmapCollection.BitmapData.EFormat.P8] = Format;

            Format = new    SurfaceDescription.PixelFormat();
            Format.FourCC = "DXT3".ToCharArray();
            Lookup[H2BitmapCollection.BitmapData.EFormat.DXT3] = Format;

            Format = new    SurfaceDescription.PixelFormat();
            Format.FourCC = "DXT5".ToCharArray();
            Lookup[H2BitmapCollection.BitmapData.EFormat.DXT5] = Format;

            Format = new    SurfaceDescription.PixelFormat();
            Format.BitCount = 16;
            Format.RGBAlphaBitMask = 0x0000FF00;
            Format.LuminanceBitMask = 0x000000FF;
            Lookup[H2BitmapCollection.BitmapData.EFormat.A8L8] = Format;

            Format = new    SurfaceDescription.PixelFormat();
            Format.BitCount = 16;
            Format.UBitMask = 0x0000FF00;
            Format.YBitMask = 0x000000FF;
            Lookup[H2BitmapCollection.BitmapData.EFormat.U8V8] = Format;

            Format = new    SurfaceDescription.PixelFormat();
            Format.BitCount = 16;
            Format.RGBAlphaBitMask = 0x0000F000;
            Format.RBitMask = 0x00000F00;
            Format.GBitMask = 0x000000F0;
            Format.BBitMask = 0x0000000F;
            Lookup[H2BitmapCollection.BitmapData.EFormat.ARGB4444] = Format;

            Format = new    SurfaceDescription.PixelFormat();
            Format.BitCount = 16;
            Format.RGBAlphaBitMask = 0x00008000;
            Format.RBitMask = 0x00007C00;
            Format.GBitMask = 0x000003E0;
            Format.BBitMask = 0x0000001F;
            Lookup[H2BitmapCollection.BitmapData.EFormat.ARGB1555] = Format;

            Format = new    SurfaceDescription.PixelFormat();
            Format.BitCount = 16;
            Format.RBitMask = 0x0000F800;
            Format.GBitMask = 0x000007E0;
            Format.BBitMask = 0x0000001F;
            Lookup[H2BitmapCollection.BitmapData.EFormat.R5G6B5] = Format;

            Format = new    SurfaceDescription.PixelFormat();
            Format.BitCount = 32;
            Format.RGBAlphaBitMask = 0xFF000000;
            Format.RBitMask = 0x00FF0000;
            Format.GBitMask = 0x0000FF00;
            Format.BBitMask = 0x000000FF;
            Lookup[H2BitmapCollection.BitmapData.EFormat.ARGB8888] = Format;

            Format = new    SurfaceDescription.PixelFormat();
            Format.BitCount = 32;
            Format.RBitMask = 0x00FF0000;
            Format.GBitMask = 0x0000FF00;
            Format.BBitMask = 0x000000FF;
            Lookup[H2BitmapCollection.BitmapData.EFormat.XRGB8888] = Format;
        }

        public byte[] GetData()
        {
            if ((Description.Caps.Caps2 & SurfaceDescription.Capibilities.EdwCaps2.DDSCAPS2_CUBEMAP) == SurfaceDescription.Capibilities.EdwCaps2.DDSCAPS2_CUBEMAP ||
                (Description.Caps.Caps2 & SurfaceDescription.Capibilities.EdwCaps2.DDSCAPS2_VOLUME) == SurfaceDescription.Capibilities.EdwCaps2.DDSCAPS2_VOLUME)
            {
                throw new Exception();
            }
            else
            {
                int length = Data1.Length;
                if ((Description.Caps.Caps1 & SurfaceDescription.Capibilities.EdwCaps1.DDSCAPS_COMPLEX) == SurfaceDescription.Capibilities.EdwCaps1.DDSCAPS_COMPLEX)
                    length += Data2.Length;
                length = Padding.Pad(length, 128);
                byte[] data = new byte[length];
                Array.Copy(Data1, data, Data1.Length);
                if ((Description.Caps.Caps1 & SurfaceDescription.Capibilities.EdwCaps1.DDSCAPS_COMPLEX) == SurfaceDescription.Capibilities.EdwCaps1.DDSCAPS_COMPLEX)
                    Array.Copy(Data2, 0, data, Data1.Length, Data2.Length);
                return data;
            }
        }

        public H2BitmapCollection.BitmapData.EFormat Format
        {
            get
            {
                H2BitmapCollection.BitmapData.EFormat[] Formats = Lookup.Keys.ToArray<H2BitmapCollection.BitmapData.EFormat>();
                for (int i = 0; i < Formats.Length; i++)
                {
                    if (Lookup[Formats[i]].Equals(Description.Format))
                        return Formats[i];
                }
                throw new Exception();
            }
        }

        public Surface(Stream stream)
        {
            BinaryReader binReader = new BinaryReader(stream);
            stream.Position = 0;
            Magic = binReader.ReadChars(4);
            Description = new SurfaceDescription(stream, 4);
            stream.Position = 128;
            if ((Description.Flags & SurfaceDescription.EFlags.LINEARSIZE) == SurfaceDescription.EFlags.LINEARSIZE)
            {
                Data1 = binReader.ReadBytes(Description.PitchOrLinearSize);
            }
            else
            {
                Data1 = binReader.ReadBytes(Description.PitchOrLinearSize * Description.Height);
            }
            if ((Description.Caps.Caps1 & SurfaceDescription.Capibilities.EdwCaps1.DDSCAPS_COMPLEX) == SurfaceDescription.Capibilities.EdwCaps1.DDSCAPS_COMPLEX)
            {
                Data2 = binReader.ReadBytes((int)stream.Length - Data1.Length - 128);
            }
        }

        public class SurfaceDescription
        {
            public int Size;
            public EFlags Flags;
            public int Height;
            public int Width;
            public int PitchOrLinearSize;
            public int Depth;
            public int MipMapCount;
            public PixelFormat Format;
            public Capibilities Caps;

            public SurfaceDescription(Stream stream, int offset)
            {
                BinaryReader binReader = new BinaryReader(stream);
                stream.Position = offset;

                Size = binReader.ReadInt32();
                Flags = (EFlags)binReader.ReadInt32();
                Height = binReader.ReadInt32();
                Width = binReader.ReadInt32();
                PitchOrLinearSize = binReader.ReadInt32();
                Depth = binReader.ReadInt32();
                MipMapCount = binReader.ReadInt32();

                Format = new PixelFormat(stream, 76);
                Caps = new Capibilities(stream, 108);
            }

            public class PixelFormat: IEquatable<PixelFormat>
            {
                public int Size;
                public EdwFlags Flags;
                public char[] FourCC { get { return fourCC; } set { fourCC = value; Flags |= EdwFlags.FOURCC; } }
                public int BitCount;
                public uint RBitMask { get { return rBitMask; } set { rBitMask = value; Flags |= EdwFlags.RGB; } }
                public uint GBitMask { get { return gBitMask; } set { gBitMask = value; Flags |= EdwFlags.RGB; } }
                public uint BBitMask { get { return bBitMask; } set { bBitMask = value; Flags |= EdwFlags.RGB; } }
                public uint RGBAlphaBitMask { get { return rgbAlphaBitMask; } set { rgbAlphaBitMask = value; Flags |= EdwFlags.ALPHAPIXELS; } }
                public uint AlphaBitMask { get { return rgbAlphaBitMask; } set { rgbAlphaBitMask = value; Flags |= EdwFlags.ALPHA; } }
                public uint LuminanceBitMask { get { return rBitMask; } set { rBitMask = value; Flags |= EdwFlags.LUMINANCE; } }
                public uint YBitMask { get { return rBitMask; } set { rBitMask = value; Flags |= EdwFlags.YUV; } }
                public uint UBitMask { get { return gBitMask; } set { gBitMask = value; Flags |= EdwFlags.YUV; } }
                public uint VBitMask { get { return bBitMask; } set { bBitMask = value; Flags |= EdwFlags.YUV; } }

                char[] fourCC;
                uint rBitMask;
                uint gBitMask;
                uint bBitMask;
                uint rgbAlphaBitMask;

                internal PixelFormat()
                {
                    fourCC = new char[] { char.MinValue, char.MinValue, char.MinValue, char.MinValue };
                }

                public PixelFormat(Stream stream, int offset)
                {
                    BinaryReader binReader = new BinaryReader(stream);
                    stream.Position = offset;

                    Size = binReader.ReadInt32();
                    Flags = (EdwFlags)binReader.ReadInt32();
                    fourCC = binReader.ReadChars(4);
                    BitCount = binReader.ReadInt32();
                    rBitMask = binReader.ReadUInt32();
                    gBitMask = binReader.ReadUInt32();
                    bBitMask = binReader.ReadUInt32();
                    rgbAlphaBitMask = binReader.ReadUInt32();
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

                #region IEquatable<PixelFormat> Members

                public bool Equals(PixelFormat other)
                {
                    return (other.Flags == this.Flags
                       && new string(other.FourCC) == new string(this.FourCC)
                       && other.BitCount == this.BitCount
                       && other.RBitMask == this.RBitMask
                       && other.GBitMask == this.GBitMask
                       && other.BBitMask == this.BBitMask
                       && other.AlphaBitMask == this.RGBAlphaBitMask);
                }

                #endregion
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


            [Flags]
            public enum EFlags
            {
                NONE = 0x00000000,
                CAPS = 0x00000001,
                HEIGHT = 0x00000002,
                WIDTH = 0x00000004,
                PITCH = 0x00000008,
                PIXELFORMAT = 0x00001000,
                MIPMAPCOUNT = 0x00020000,
                LINEARSIZE = 0x00080000,
                DEPTH = 0x00800000,
            }
        }
    }
}