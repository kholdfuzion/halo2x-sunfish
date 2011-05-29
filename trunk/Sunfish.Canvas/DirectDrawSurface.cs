using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using Microsoft.Xna.Framework;
using System.IO;
using System.Reflection;
using System.Collections;

namespace Sunfish.Canvas
{
    

    public class DirectDrawSurfaceStream : MemoryStream
    {
        BinaryReader Reader;
        BinaryWriter Writer;

        public char[] dwMagic
        {
            get { Reader.BaseStream.Position = 0; return Reader.ReadChars(4); }
            set { Writer.BaseStream.Position = 0; Writer.Write(new char[] { value[0], value[1], value[2], value[3] }); }
        }
        public DDSURFACEDESC2Stream SurfaceDescription { get; set; }
        public int bData1_Length;
        public byte[] bData1
        {
            get { Reader.BaseStream.Position = 128; return Reader.ReadBytes(bData1_Length); }
            set
            {
                if (value.Length != bData1_Length) { MovebData2(value.Length - bData1_Length); }
                Writer.BaseStream.Position = 128; 
                Writer.Write(value);
                bData1_Length = value.Length;
            }
        }
        public int bdata2_Length;
        public byte[] bData2
        {
            get { Reader.BaseStream.Position = 128 + bData1_Length; return Reader.ReadBytes((int)(Reader.BaseStream.Length - Reader.BaseStream.Position)); }
            set
            {
                Writer.BaseStream.Position = 128 + bData1_Length;
                if (value.Length != (Reader.BaseStream.Length - Reader.BaseStream.Position)) { Writer.BaseStream.SetLength(Writer.BaseStream.Length + (value.Length - (Reader.BaseStream.Length - Reader.BaseStream.Position))); }
                Writer.Write(value);
                bdata2_Length = value.Length;
            }
        }

        public byte[] GetData()
        {
            byte[] retBytes = new byte[bData1_Length + bdata2_Length];
            MemoryStream memStream = new MemoryStream(retBytes);
            memStream.Write(bData1, 0, bData1_Length);
            memStream.Write(bData2, 0, bdata2_Length);
            return retBytes;
        }

        private void MovebData2(int Shift)
        {
            byte[] Buffer = new byte[Reader.BaseStream.Length - (128 + bData1_Length)];
            Buffer = Reader.ReadBytes(Buffer.Length);
            Writer.BaseStream.Position = 128 + bData1_Length + Shift;
            Writer.Write(Buffer);
            Writer.BaseStream.SetLength(Writer.BaseStream.Position);
        }

        public DirectDrawSurfaceStream(byte[] buffer)
            : base(buffer)
        {
            Reader = new BinaryReader(this);
            Writer = new BinaryWriter(this);
            SurfaceDescription = new DDSURFACEDESC2Stream(this);
            if ((SurfaceDescription.dwFlags & DDSURFACEDESC2Stream.EdwFlags.DDSD_LINEARSIZE) == DDSURFACEDESC2Stream.EdwFlags.DDSD_LINEARSIZE)
                bData1_Length = SurfaceDescription.dwPitchOrLinearSize;
            else bData1_Length = SurfaceDescription.dwPitchOrLinearSize * SurfaceDescription.dwHeight;
            bdata2_Length = (int)(this.Length - bData1_Length - 128);
            Position = 0;
        }

        public DirectDrawSurfaceStream()
        {
            SetLength(128);
            Reader = new BinaryReader(this);
            Writer = new BinaryWriter(this);
            dwMagic = "DDS ".ToCharArray();
            SurfaceDescription = new DDSURFACEDESC2Stream(this);
            bData1 = new byte[0];
            bData2 = new byte[0];
        }

        public class DDSURFACEDESC2Stream
        {
            BinaryReader Reader;
            BinaryWriter Writer;
            Stream Stream;
            int StreamOffset = 4;

            #region Properties

            public int dwSize
            {
                get { Reader.BaseStream.Position = StreamOffset + 0; return Reader.ReadInt32(); }
                set { Writer.BaseStream.Position = StreamOffset + 0; Writer.Write(value); }
            }

            public EdwFlags dwFlags
            {
                get { Reader.BaseStream.Position = StreamOffset + 4; return (EdwFlags)Reader.ReadInt32(); }
                set { Writer.BaseStream.Position = StreamOffset + 4; Writer.Write((int)value); }
            }

            public int dwHeight
            {
                get { Reader.BaseStream.Position = StreamOffset + 8; return Reader.ReadInt32(); }
                set { Writer.BaseStream.Position = StreamOffset + 8; Writer.Write(value); }
            }

            public int dwWidth
            {
                get { Reader.BaseStream.Position = StreamOffset + 12; return Reader.ReadInt32(); }
                set { Writer.BaseStream.Position = StreamOffset + 12; Writer.Write(value); }
            }

            public int dwPitchOrLinearSize
            {
                get { Reader.BaseStream.Position = StreamOffset + 16; return Reader.ReadInt32(); }
                set { Writer.BaseStream.Position = StreamOffset + 16; Writer.Write(value); }
            }

            public int dwDepth
            {
                get { Reader.BaseStream.Position = StreamOffset + 20; return Reader.ReadInt32(); }
                set { Writer.BaseStream.Position = StreamOffset + 20; Writer.Write(value); }

            }

            public int dwMipMapCount
            {
                get { Reader.BaseStream.Position = StreamOffset + 24; return Reader.ReadInt32(); }
                set { Writer.BaseStream.Position = StreamOffset + 24; Writer.Write(value); }
            }

            public DDPIXELFORMAT PixelFormat { get; set; }

            public DDCAPS Capibilties { get; set; }

            #endregion

            public DDSURFACEDESC2Stream(Stream stream)
            {
                Stream = stream;
                Reader = new BinaryReader(Stream);
                Writer = new BinaryWriter(Stream);
                dwSize = 124;
                dwFlags |= EdwFlags.DDSD_CAPS | EdwFlags.DDSD_PIXELFORMAT;
                PixelFormat = new DDPIXELFORMAT(stream);
                Capibilties = new DDCAPS(stream);
            }

            public class DDPIXELFORMAT
            {
                BinaryReader Reader;
                BinaryWriter Writer;
                Stream Stream;
                int StreamOffset = 76;

                public DDPIXELFORMAT(Stream stream)
                {
                    Stream = stream;
                    Reader = new BinaryReader(Stream);
                    Writer = new BinaryWriter(Stream);
                    dwSize = 32;
                }

                public int dwSize
                {
                    get { Reader.BaseStream.Position = StreamOffset + 0; return Reader.ReadInt32(); }
                    set { Writer.BaseStream.Position = StreamOffset + 0; Writer.Write(value); }
                }

                public EdwFlags dwFlags
                {
                    get { Reader.BaseStream.Position = StreamOffset + 4; return (EdwFlags)Reader.ReadInt32(); }
                    set { Writer.BaseStream.Position = StreamOffset + 4; Writer.Write((int)value); }
                }

                public char[] dwFourCC
                {
                    get { Reader.BaseStream.Position = StreamOffset + 8; return Reader.ReadChars(4); }
                    set { Writer.BaseStream.Position = StreamOffset + 8; Writer.Write(new char[] { value[0], value[1], value[2], value[3] }); }
                }

                public int dwBitCount
                {
                    get { Reader.BaseStream.Position = StreamOffset + 12; return Reader.ReadInt32(); }
                    set { Writer.BaseStream.Position = StreamOffset + 12; Writer.Write(value); }
                }

                public int dwRBitMask
                {
                    get { Reader.BaseStream.Position = StreamOffset + 16; return Reader.ReadInt32(); }
                    set { Writer.BaseStream.Position = StreamOffset + 16; Writer.Write(value); }
                }

                public int dwGBitMask
                {
                    get { Reader.BaseStream.Position = StreamOffset + 20; return Reader.ReadInt32(); }
                    set { Writer.BaseStream.Position = StreamOffset + 20; Writer.Write(value); }
                }

                public int dwBBitMask
                {
                    get { Reader.BaseStream.Position = StreamOffset + 24; return Reader.ReadInt32(); }
                    set { Writer.BaseStream.Position = StreamOffset + 24; Writer.Write(value); }
                }

                public int dwAlphaBitMask
                {
                    get { Reader.BaseStream.Position = StreamOffset + 28; return Reader.ReadInt32(); }
                    set { Writer.BaseStream.Position = StreamOffset + 28; Writer.Write(value); }
                }


                [Flags]
                public enum EdwFlags
                {
                    DDPF_NONE = 0x00000000,
                    DDPF_ALPHAPIXELS = 0x00000001,
                    DDPF_ALPHA = 0x00000002,
                    DDPF_FOURCC = 0x00000004,
                    DDPF_PALETTEINDEXED8 = 0x00000020,
                    DDPF_RGB = 0x00000040,
                    DDPF_LUMINANCE = 0x00020000,
                    DDPF_YUV = 0x00080000,
                }
            }

            public class DDCAPS
            {
                BinaryReader Reader;
                BinaryWriter Writer;
                Stream Stream;
                int StreamOffset = 108;

                public DDCAPS(Stream stream)
                {
                    Stream = stream;
                    Reader = new BinaryReader(Stream);
                    Writer = new BinaryWriter(Stream);
                }

                public EdwCaps1 dwCaps1
                {
                    get { Reader.BaseStream.Position = StreamOffset + 0; return (EdwCaps1)Reader.ReadInt32(); }
                    set { Writer.BaseStream.Position = StreamOffset + 0; Writer.Write((int)value); }
                }

                public EdwCaps2 dwCaps2
                {
                    get { Reader.BaseStream.Position = StreamOffset + 4; return (EdwCaps2)Reader.ReadInt32(); }
                    set { Writer.BaseStream.Position = StreamOffset + 4; Writer.Write((int)value); }
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
            public enum EdwFlags
            {
                DDSD_NONE = 0x00000000,
                DDSD_CAPS = 0x00000001,
                DDSD_HEIGHT = 0x00000002,
                DDSD_WIDTH = 0x00000004,
                DDSD_PITCH = 0x00000008,
                DDSD_PIXELFORMAT = 0x00001000,
                DDSD_MIPMAPCOUNT = 0x00020000,
                DDSD_LINEARSIZE = 0x00080000,
                DDSD_DEPTH = 0x00800000,
            }
        }

        private static Dictionary<H2BitmapCollection.BitmapData.EFormat, DDPIXELFORMAT> CreateLookupTableInstamce()
        {
            Dictionary<H2BitmapCollection.BitmapData.EFormat, DDPIXELFORMAT> lookup;
            lookup = new Dictionary<H2BitmapCollection.BitmapData.EFormat, DDPIXELFORMAT>();
            DDPIXELFORMAT Format = DDPIXELFORMAT.CreateInstance();
            Format.FourCC = "DXT1";
            lookup.Add(H2BitmapCollection.BitmapData.EFormat.DXT1, Format);
            Format = DDPIXELFORMAT.CreateInstance();
            Format.AlphaBitMask = 0x000000FF;
            Format.BitsPerPixel = 8;
            lookup.Add(H2BitmapCollection.BitmapData.EFormat.A8, Format);
            Format = DDPIXELFORMAT.CreateInstance();
            Format.AlphaBitMask = 0x000000FF;
            Format.BitsPerPixel = 8;
            lookup.Add(H2BitmapCollection.BitmapData.EFormat.AY8, Format);
            Format = DDPIXELFORMAT.CreateInstance();
            Format.LuminanceBitMask = 0x000000FF;
            Format.BitsPerPixel = 8;
            lookup.Add(H2BitmapCollection.BitmapData.EFormat.Y8, Format);
            Format = DDPIXELFORMAT.CreateInstance();
            Format.LuminanceBitMask = 0x000000FF;
            Format.BitsPerPixel = 8;
            lookup.Add(H2BitmapCollection.BitmapData.EFormat.P8, Format);
            Format = DDPIXELFORMAT.CreateInstance();
            Format.FourCC = "DXT3";
            lookup.Add(H2BitmapCollection.BitmapData.EFormat.DXT3, Format);
            Format = DDPIXELFORMAT.CreateInstance();
            Format.FourCC = "DXT5";
            lookup.Add(H2BitmapCollection.BitmapData.EFormat.DXT5, Format);
            Format = DDPIXELFORMAT.CreateInstance();
            Format.BitsPerPixel = 16;
            Format.RGBAlphaBitMask = 0x0000FF00;
            Format.LuminanceBitMask = 0x000000FF;
            lookup.Add(H2BitmapCollection.BitmapData.EFormat.A8Y8, Format);
            Format = DDPIXELFORMAT.CreateInstance();
            Format.BitsPerPixel = 16;
            Format.UBitMask = 0x0000FF00;
            Format.YBitMask = 0x000000FF;
            lookup.Add(H2BitmapCollection.BitmapData.EFormat.U8V8, Format);
            Format = DDPIXELFORMAT.CreateInstance();
            Format.BitsPerPixel = 16;
            Format.RGBAlphaBitMask = 0x0000F000;
            Format.RedBitMask = 0x00000F00;
            Format.GreenBitMask = 0x000000F0;
            Format.BlueBitMask = 0x0000000F;
            lookup.Add(H2BitmapCollection.BitmapData.EFormat.A4R4G4B4, Format);
            Format = DDPIXELFORMAT.CreateInstance();
            Format.BitsPerPixel = 16;
            Format.RGBAlphaBitMask = 0x00008000;
            Format.RedBitMask = 0x00007C00;
            Format.GreenBitMask = 0x000003E0;
            Format.BlueBitMask = 0x0000001F;
            lookup.Add(H2BitmapCollection.BitmapData.EFormat.A1R5G5B5, Format);
            Format = DDPIXELFORMAT.CreateInstance();
            Format.BitsPerPixel = 16;
            Format.RedBitMask = 0x0000F800;
            Format.GreenBitMask = 0x000007E0;
            Format.BlueBitMask = 0x0000001F;
            lookup.Add(H2BitmapCollection.BitmapData.EFormat.R5G6B5, Format);
            Format = DDPIXELFORMAT.CreateInstance();
            Format.BitsPerPixel = 32;
            Format.RGBAlphaBitMask = 0xFF000000;
            Format.RedBitMask = 0x00FF0000;
            Format.GreenBitMask = 0x0000FF00;
            Format.BlueBitMask = 0x000000FF;
            lookup.Add(H2BitmapCollection.BitmapData.EFormat.A8R8G8B8, Format);
            Format = DDPIXELFORMAT.CreateInstance();
            Format.BitsPerPixel = 32;
            Format.RedBitMask = 0x00FF0000;
            Format.GreenBitMask = 0x0000FF00;
            Format.BlueBitMask = 0x000000FF;
            lookup.Add(H2BitmapCollection.BitmapData.EFormat.X8R8G8B8, Format);
            return lookup;
        }

        [StructLayout(LayoutKind.Explicit)]
        private struct DDPIXELFORMAT : IEquatable<DDSURFACEDESC2Stream.DDPIXELFORMAT>
        {
            public const int Size = 32;

            [FieldOffset(0)]
            int dwSize;
            [FieldOffset(4)]
            EdwFlags dwFlags;
            [FieldOffset(8)]
            char[] dwFourCC;

            [FieldOffset(12)]
            int dwBitCount;

            #region RGB

            [FieldOffset(12)]
            int dwRGBBitCount;
            [FieldOffset(16)]
            uint dwRBitMask;
            [FieldOffset(20)]
            uint dwGBitMask;
            [FieldOffset(24)]
            uint dwBBitMask;

            #endregion

            #region YUV

            [FieldOffset(12)]
            int dwYUVBitCount;
            [FieldOffset(16)]
            uint dwYBitMask;
            [FieldOffset(20)]
            uint dwUBitMask;
            [FieldOffset(24)]
            uint dwVBitMask;

            #endregion

            #region Alpha

            [FieldOffset(28)]
            uint dwAlphaBitMask;

            #endregion

            #region Luminance

            [FieldOffset(16)]
            uint dwLuminanceBitMask;

            #endregion

            [FieldOffset(28)]
            uint dwRGBAlphaBitMask;

            public EdwFlags Flags
            {
                get { return dwFlags; }
                set { dwFlags = value; }
            }

            public string FourCC
            {
                get
                {
                    if ((dwFlags & EdwFlags.DDPF_FOURCC) == EdwFlags.DDPF_FOURCC)
                        return new string(dwFourCC);
                    else throw new NotSupportedException();
                }
                set
                {
                    dwFlags |= EdwFlags.DDPF_FOURCC;
                    dwFourCC = value.ToCharArray(0, 4);
                }
            }

            public int BitsPerPixel
            {
                get
                {
                    //if ((dwFlags & EdwFlags.DDPF_RGB) == EdwFlags.DDPF_RGB ||
                    //    (dwFlags & EdwFlags.DDPF_YUV) == EdwFlags.DDPF_YUV ||
                    //    (dwFlags & EdwFlags.DDPF_ALPHA) == EdwFlags.DDPF_ALPHA ||
                    //    (dwFlags & EdwFlags.DDPF_LUMINANCE) == EdwFlags.DDPF_LUMINANCE)
                        return dwRGBBitCount;
                   // else throw new NotSupportedException();
                }
                set
                {
                    dwRGBBitCount = value;
                }
            }

            public uint RedBitMask
            {
                get
                {
                    if ((dwFlags & EdwFlags.DDPF_RGB) == EdwFlags.DDPF_RGB)
                        return dwRBitMask;
                    else throw new NotSupportedException();
                }
                set
                {
                    dwFlags |= EdwFlags.DDPF_RGB;
                    dwRBitMask = value;
                }
            }

            public uint GreenBitMask
            {
                get
                {
                    if ((dwFlags & EdwFlags.DDPF_RGB) == EdwFlags.DDPF_RGB)
                        return dwGBitMask;
                    else throw new NotSupportedException();
                }
                set
                {
                    dwFlags |= EdwFlags.DDPF_RGB;
                    dwGBitMask = value;
                }
            }

            public uint BlueBitMask
            {
                get
                {
                    if ((dwFlags & EdwFlags.DDPF_RGB) == EdwFlags.DDPF_RGB)
                        return dwBBitMask;
                    else throw new NotSupportedException();
                }
                set
                {
                    dwFlags |= EdwFlags.DDPF_RGB;
                    dwBBitMask = value;
                }
            }

            public uint YBitMask
            {
                get
                {
                    if ((dwFlags & EdwFlags.DDPF_YUV) == EdwFlags.DDPF_YUV)
                        return dwYBitMask;
                    else throw new NotSupportedException();
                }
                set
                {
                    dwFlags |= EdwFlags.DDPF_YUV;
                    dwYBitMask = value;
                }
            }

            public uint UBitMask
            {
                get
                {
                    if ((dwFlags & EdwFlags.DDPF_YUV) == EdwFlags.DDPF_YUV)
                        return dwUBitMask;
                    else throw new NotSupportedException();
                }
                set
                {
                    dwFlags |= EdwFlags.DDPF_YUV;
                    dwUBitMask = value;
                }
            }

            public uint VBitMask
            {
                get
                {
                    if ((dwFlags & EdwFlags.DDPF_YUV) == EdwFlags.DDPF_YUV)
                        return dwVBitMask;
                    else throw new NotSupportedException();
                }
                set
                {
                    dwFlags |= EdwFlags.DDPF_YUV;
                    dwVBitMask = value;
                }
            }

            public uint RGBAlphaBitMask
            {
                get
                {
                    if ((dwFlags & EdwFlags.DDPF_ALPHAPIXELS) == EdwFlags.DDPF_ALPHAPIXELS)
                        return dwRGBAlphaBitMask;
                    else throw new NotSupportedException();
                }
                set
                {
                    dwFlags |= EdwFlags.DDPF_ALPHAPIXELS;
                    dwRGBAlphaBitMask = value;
                }
            }

            public uint AlphaBitMask
            {
                get
                {
                    if ((dwFlags & EdwFlags.DDPF_ALPHA) == EdwFlags.DDPF_ALPHA)
                        return dwAlphaBitMask;
                    else throw new NotSupportedException();
                }
                set
                {
                    dwFlags |= EdwFlags.DDPF_ALPHA;
                    dwAlphaBitMask = value;
                }
            }

            public uint LuminanceBitMask
            {
                get
                {
                    if ((dwFlags & EdwFlags.DDPF_LUMINANCE) == EdwFlags.DDPF_LUMINANCE)
                        return dwLuminanceBitMask;
                    else throw new NotSupportedException();
                }
                set
                {
                    dwFlags |= EdwFlags.DDPF_LUMINANCE;
                    dwLuminanceBitMask = value;
                }
            }

            [Flags]
            public enum EdwFlags
            {
                DDPF_NONE = 0x00000000,
                DDPF_ALPHAPIXELS = 0x00000001,
                DDPF_ALPHA = 0x00000002,
                DDPF_FOURCC = 0x00000004,
                DDPF_PALETTEINDEXED8 = 0x00000020,
                DDPF_RGB = 0x00000040,
                DDPF_LUMINANCE = 0x00020000,
                DDPF_YUV = 0x00080000,
            }

            public static DDPIXELFORMAT CreateInstance()
            {
                DDPIXELFORMAT ddPixelFormat = new DDPIXELFORMAT();
                ddPixelFormat.dwSize = 32;
                ddPixelFormat.dwFlags = EdwFlags.DDPF_NONE;
                ddPixelFormat.dwRGBBitCount = 0;
                ddPixelFormat.dwRBitMask = 0x00000000;
                ddPixelFormat.dwGBitMask = 0x00000000;
                ddPixelFormat.dwBBitMask = 0x00000000;
                ddPixelFormat.dwRGBAlphaBitMask = 0x00000000;
                ddPixelFormat.dwFourCC = new char[] { '\0', '\0', '\0', '\0' };
                return ddPixelFormat;
            }

            #region IEquatable<DDPIXELFORMAT> Members

            public bool Equals(DirectDrawSurfaceStream.DDSURFACEDESC2Stream.DDPIXELFORMAT other)
            {
                return (other.dwFlags == (DDSURFACEDESC2Stream.DDPIXELFORMAT.EdwFlags)this.dwFlags
                    && new string(other.dwFourCC) == new string(this.dwFourCC)
                    && other.dwBitCount == this.dwBitCount
                    && (uint)other.dwRBitMask == (uint)this.dwRBitMask
                    && (uint)other.dwGBitMask == (uint)this.dwGBitMask
                    && (uint)other.dwBBitMask == (uint)this.dwBBitMask
                    && (uint)other.dwAlphaBitMask == (uint)this.dwRGBAlphaBitMask);
            }

            #endregion
        }

        public void ImportHalo2BitmapData(H2BitmapCollection.BitmapData Meta, byte[] Raw)
        {
            SurfaceDescription = new DDSURFACEDESC2Stream(this);
            SurfaceDescription.Capibilties = new DDSURFACEDESC2Stream.DDCAPS(this);
            SurfaceDescription.PixelFormat = new DDSURFACEDESC2Stream.DDPIXELFORMAT(this);
            if (Meta.Type  == H2BitmapCollection.EType.TEXTURES_2D)
            {
                Dictionary<H2BitmapCollection.BitmapData.EFormat, DDPIXELFORMAT> Lookup = CreateLookupTableInstamce();
                SurfaceDescription.Capibilties.dwCaps1 |= DDSURFACEDESC2Stream.DDCAPS.EdwCaps1.DDSCAPS_TEXTURE;
                SurfaceDescription.Capibilties.dwCaps2 |= DDSURFACEDESC2Stream.DDCAPS.EdwCaps2.DDSCAPS2_NONE;
                SurfaceDescription.dwFlags |= DDSURFACEDESC2Stream.EdwFlags.DDSD_HEIGHT | DDSURFACEDESC2Stream.EdwFlags.DDSD_WIDTH;
                
                if (Meta.MIPMapCount > 0)
                {
                    SurfaceDescription.Capibilties.dwCaps1 |= DDSURFACEDESC2Stream.DDCAPS.EdwCaps1.DDSCAPS_MIPMAP;
                    SurfaceDescription.dwFlags |= DDSURFACEDESC2Stream.EdwFlags.DDSD_MIPMAPCOUNT;
                    SurfaceDescription.dwMipMapCount = Meta.MIPMapCount;
                }

                if ((Meta.Flags & H2BitmapCollection.BitmapData.EFlags.Power_Of_2_Dimensions) != H2BitmapCollection.BitmapData.EFlags.Power_Of_2_Dimensions)
                {
                    SurfaceDescription.dwWidth = Meta.Width + (16 - (Meta.Width % 16) == 16 ? 0 : 16 - (Meta.Width % 16)); 
                    int x = SurfaceDescription.dwWidth;
                    for (int i = 0; i < SurfaceDescription.dwMipMapCount; i++)
                    {
                        x += x / 2;//broken
                    }
                    SurfaceDescription.dwHeight = Raw.Length / (x * (Lookup[Meta.Format].BitsPerPixel / 8));
                }
                else
                {
                    SurfaceDescription.dwWidth = Meta.Width;
                    SurfaceDescription.dwHeight = Meta.Height;
                }

                
                //Meta.Height + (2 - (Meta.Height % 2) == 2 ? 0 : 2 - (Meta.Height % 2));

                if (Meta.Format == H2BitmapCollection.BitmapData.EFormat.DXT1 || Meta.Format == H2BitmapCollection.BitmapData.EFormat.DXT3 || Meta.Format == H2BitmapCollection.BitmapData.EFormat.DXT5)
                {
                    SurfaceDescription.dwFlags |= DDSURFACEDESC2Stream.EdwFlags.DDSD_LINEARSIZE;
                    if (Meta.Format == H2BitmapCollection.BitmapData.EFormat.DXT1)
                        SurfaceDescription.dwPitchOrLinearSize = (int)(SurfaceDescription.dwWidth * SurfaceDescription.dwHeight * 0.5F);
                    else
                        SurfaceDescription.dwPitchOrLinearSize = SurfaceDescription.dwWidth * SurfaceDescription.dwHeight * 1;
                }
                else
                {
                    SurfaceDescription.dwFlags |= DDSURFACEDESC2Stream.EdwFlags.DDSD_LINEARSIZE;
                    SurfaceDescription.dwPitchOrLinearSize = SurfaceDescription.dwWidth * Lookup[Meta.Format].BitsPerPixel / 8 * SurfaceDescription.dwHeight;
                }
                SurfaceDescription.PixelFormat.dwFlags = (DDSURFACEDESC2Stream.DDPIXELFORMAT.EdwFlags)Lookup[Meta.Format].Flags;
                if ((SurfaceDescription.PixelFormat.dwFlags & DDSURFACEDESC2Stream.DDPIXELFORMAT.EdwFlags.DDPF_FOURCC) == DDSURFACEDESC2Stream.DDPIXELFORMAT.EdwFlags.DDPF_FOURCC)
                    SurfaceDescription.PixelFormat.dwFourCC = Lookup[Meta.Format].FourCC.ToCharArray();
                if ((SurfaceDescription.PixelFormat.dwFlags & DDSURFACEDESC2Stream.DDPIXELFORMAT.EdwFlags.DDPF_RGB) == DDSURFACEDESC2Stream.DDPIXELFORMAT.EdwFlags.DDPF_RGB)
                {
                    SurfaceDescription.PixelFormat.dwRBitMask = (int)Lookup[Meta.Format].RedBitMask;
                    SurfaceDescription.PixelFormat.dwGBitMask = (int)Lookup[Meta.Format].GreenBitMask;
                    SurfaceDescription.PixelFormat.dwBBitMask = (int)Lookup[Meta.Format].BlueBitMask;
                    SurfaceDescription.PixelFormat.dwBitCount = Lookup[Meta.Format].BitsPerPixel;
                }
                if ((SurfaceDescription.PixelFormat.dwFlags & DDSURFACEDESC2Stream.DDPIXELFORMAT.EdwFlags.DDPF_YUV) == DDSURFACEDESC2Stream.DDPIXELFORMAT.EdwFlags.DDPF_YUV)
                {
                    SurfaceDescription.PixelFormat.dwRBitMask = (int)Lookup[Meta.Format].YBitMask;
                    SurfaceDescription.PixelFormat.dwGBitMask = (int)Lookup[Meta.Format].UBitMask;
                    SurfaceDescription.PixelFormat.dwBBitMask = (int)Lookup[Meta.Format].VBitMask;
                    SurfaceDescription.PixelFormat.dwBitCount = Lookup[Meta.Format].BitsPerPixel;
                }
                if ((SurfaceDescription.PixelFormat.dwFlags & DDSURFACEDESC2Stream.DDPIXELFORMAT.EdwFlags.DDPF_ALPHAPIXELS) == DDSURFACEDESC2Stream.DDPIXELFORMAT.EdwFlags.DDPF_ALPHAPIXELS)
                    SurfaceDescription.PixelFormat.dwAlphaBitMask = (int)Lookup[Meta.Format].RGBAlphaBitMask;
                if ((SurfaceDescription.PixelFormat.dwFlags & DDSURFACEDESC2Stream.DDPIXELFORMAT.EdwFlags.DDPF_LUMINANCE) == DDSURFACEDESC2Stream.DDPIXELFORMAT.EdwFlags.DDPF_LUMINANCE)
                {
                    SurfaceDescription.PixelFormat.dwRBitMask = (int)Lookup[Meta.Format].LuminanceBitMask;
                    SurfaceDescription.PixelFormat.dwBitCount = Lookup[Meta.Format].BitsPerPixel;
                }
                if ((SurfaceDescription.PixelFormat.dwFlags & DDSURFACEDESC2Stream.DDPIXELFORMAT.EdwFlags.DDPF_ALPHA) == DDSURFACEDESC2Stream.DDPIXELFORMAT.EdwFlags.DDPF_ALPHA)
                {
                    SurfaceDescription.PixelFormat.dwAlphaBitMask = (int)Lookup[Meta.Format].AlphaBitMask;
                    SurfaceDescription.PixelFormat.dwBitCount = Lookup[Meta.Format].BitsPerPixel;
                }
                MemoryStream Stream = new MemoryStream(Raw);
                BinaryReader Reader = new BinaryReader(Stream);
                Reader.BaseStream.Position = 0;
                if ((SurfaceDescription.dwFlags & DDSURFACEDESC2Stream.EdwFlags.DDSD_LINEARSIZE) == DDSURFACEDESC2Stream.EdwFlags.DDSD_LINEARSIZE)
                {
                    bData1 = Reader.ReadBytes(SurfaceDescription.dwPitchOrLinearSize);
                }
                else bData1 = Reader.ReadBytes(SurfaceDescription.dwPitchOrLinearSize * SurfaceDescription.dwHeight);
                bData2 = Reader.ReadBytes((int)(Stream.Length - Stream.Position));
                if (bData2.Length > 0) SurfaceDescription.Capibilties.dwCaps1 |= DDSURFACEDESC2Stream.DDCAPS.EdwCaps1.DDSCAPS_COMPLEX;
                if ((Meta.Flags & H2BitmapCollection.BitmapData.EFlags.Swizzled) == H2BitmapCollection.BitmapData.EFlags.Swizzled)
                {
                    bData1 = Swizzler.Swizzle(bData1, SurfaceDescription.dwWidth, SurfaceDescription.dwHeight, SurfaceDescription.dwDepth, SurfaceDescription.PixelFormat.dwBitCount, true);
                }
            }
        }

        private uint unswizzle(uint offset, int log2_w)
        {
            if (log2_w <= 4)
                return offset;

            uint w_mask = (uint)(1 << log2_w) - 1;

            uint mx = offset & 0xf;
            uint by = (uint)(offset & (~7 << log2_w));
            uint bx = offset & ((w_mask & 0xf) << 7);
            uint my = offset & 0x70;

            return by | (bx >> 3) | (my << (log2_w - 4)) | mx;
        }

        public sealed class Swizzler
        {
            public static byte[] Swizzle(byte[] raw, int offset, int width, int height, int depth, int bitCount, bool deswizzle)
            {
                if (depth < 1) depth = 1;
                bitCount /= 8;
                int a = 0, b = 0;
                int tempsize = raw.Length;//width * height * bitCount;
                byte[] data = new byte[tempsize];
                MaskSet masks = new MaskSet(width, height, depth);

                offset = 0;


                for (int z = 0; z < depth; z++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        for (int x = 0; x < width; x++)
                        {
                            if (deswizzle)
                            {
                                a = ((((z * height) + y) * width) + x) * bitCount;
                                b = (Swizzle(x, y, z, masks)) * bitCount;
                                //a = ((y * width) + x) * bitCount;
                                //b = (Swizzle(x, y, -1, masks)) * bitCount;
                            }
                            else
                            {
                                b = ((((z * height) + y) * width) + x) * bitCount;
                                a = (Swizzle(x, y, z, masks)) * bitCount;
                                //b = ((y * width) + x) * bitCount;
                                //a = (Swizzle(x, y, -1, masks)) * bitCount;
                            }

                            for (int i = offset; i < bitCount + offset; i++)
                                data[a + i] = raw[b + i];


                        }
                    }
                }


                //for(int u = 0; u < offset; u++)
                //data[u] = raw[u];
                //for(int v = offset + (height * width * depth * bitCount); v < data.Length; v++)
                //	data[v] = raw[v];

                return data;
            }

            public static byte[] Swizzle(byte[] raw, int width, int height, int depth, int bitCount, bool deswizzle)
            {
                return Swizzle(raw, 0, width, height, depth, bitCount, deswizzle);
            }

            private static int Swizzle(int x, int y, int z, MaskSet masks)
            {
                return SwizzleAxis(x, masks.x) | SwizzleAxis(y, masks.y) | (z == -1 ? 0 : SwizzleAxis(z, masks.z));
            }

            private static int SwizzleAxis(int val, int mask)
            {
                int bit = 1;
                int result = 0;

                while (bit <= mask)
                {
                    int test = mask & bit;
                    if (test != 0)
                    {
                        result |= (val & bit);
                    }
                    else
                    {
                        val <<= 1;
                    }
                    bit <<= 1;
                }

                return result;
            }

            private class MaskSet
            {
                public int x = 0;
                public int y = 0;
                public int z = 0;

                public MaskSet(int w, int h, int d)
                {
                    int bit = 1;
                    int index = 1;


                    while (bit < w || bit < h || bit < d)
                    {
                        //if (bit == 0) { break; }
                        if (bit < w)
                        {
                            x |= index;
                            index <<= 1;
                        }
                        if (bit < h)
                        {
                            y |= index;
                            index <<= 1;
                        }
                        if (bit < d)
                        {
                            z |= index;
                            index <<= 1;
                        }
                        bit <<= 1;
                    }
                }
            }
        }

        internal H2BitmapCollection.BitmapData.EFormat GetFormat()
        {
            Dictionary<H2BitmapCollection.BitmapData.EFormat, DDPIXELFORMAT> Lookup = CreateLookupTableInstamce();
            H2BitmapCollection.BitmapData.EFormat[] Formats = Lookup.Keys.ToArray<H2BitmapCollection.BitmapData.EFormat>();
            for (int i = 0; i < Formats.Length; i++)
            {
                if (Lookup[Formats[i]].Equals(this.SurfaceDescription.PixelFormat)) 
                    return Formats[i];
            }
            throw new Exception();
        }
    }
}
