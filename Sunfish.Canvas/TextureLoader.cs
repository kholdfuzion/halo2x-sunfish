using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Sunfish.Canvas
{
    public static class TextureLoader
    {
        public static Dictionary<H2BitmapCollection.BitmapData.EFormat, SurfaceFormat> SurfaceFormatDictionary;

        static int GetBBP(H2BitmapCollection.BitmapData.EFormat format)
        {
            switch (format)
            {
                case H2BitmapCollection.BitmapData.EFormat.XRGB8888:
                case H2BitmapCollection.BitmapData.EFormat.ARGB8888:
                    return 32;
                case H2BitmapCollection.BitmapData.EFormat.ARGB4444:
                case H2BitmapCollection.BitmapData.EFormat.ARGB1555:
                case H2BitmapCollection.BitmapData.EFormat.A8L8:
                case H2BitmapCollection.BitmapData.EFormat.R5G6B5:
                case H2BitmapCollection.BitmapData.EFormat.U8V8:
                    return 16;
                case H2BitmapCollection.BitmapData.EFormat.P8:
                case H2BitmapCollection.BitmapData.EFormat.L8:
                case H2BitmapCollection.BitmapData.EFormat.A8:
                case H2BitmapCollection.BitmapData.EFormat.DXT5:
                case H2BitmapCollection.BitmapData.EFormat.DXT3:
                case H2BitmapCollection.BitmapData.EFormat.AL8:
                case H2BitmapCollection.BitmapData.EFormat.LIGHTMAP:
                    return 8;
                case H2BitmapCollection.BitmapData.EFormat.DXT1:
                    return 4;
                default: throw new Exception();
            }
        }

        static TextureLoader()
        {
            SurfaceFormatDictionary = new Dictionary<H2BitmapCollection.BitmapData.EFormat, SurfaceFormat>(14); 
            SurfaceFormatDictionary[H2BitmapCollection.BitmapData.EFormat.LIGHTMAP] = Microsoft.Xna.Framework.Graphics.SurfaceFormat.Luminance8;
            SurfaceFormatDictionary[H2BitmapCollection.BitmapData.EFormat.ARGB8888] = Microsoft.Xna.Framework.Graphics.SurfaceFormat.Color;
            SurfaceFormatDictionary[H2BitmapCollection.BitmapData.EFormat.XRGB8888] = Microsoft.Xna.Framework.Graphics.SurfaceFormat.Bgr32;
            SurfaceFormatDictionary[H2BitmapCollection.BitmapData.EFormat.AL8] = Microsoft.Xna.Framework.Graphics.SurfaceFormat.LuminanceAlpha8;
            SurfaceFormatDictionary[H2BitmapCollection.BitmapData.EFormat.ARGB1555] = Microsoft.Xna.Framework.Graphics.SurfaceFormat.Bgra5551;
            SurfaceFormatDictionary[H2BitmapCollection.BitmapData.EFormat.ARGB4444] = Microsoft.Xna.Framework.Graphics.SurfaceFormat.Bgra4444;
            SurfaceFormatDictionary[H2BitmapCollection.BitmapData.EFormat.R5G6B5] = Microsoft.Xna.Framework.Graphics.SurfaceFormat.Bgr565;
            SurfaceFormatDictionary[H2BitmapCollection.BitmapData.EFormat.A8L8] = Microsoft.Xna.Framework.Graphics.SurfaceFormat.LuminanceAlpha16;
            SurfaceFormatDictionary[H2BitmapCollection.BitmapData.EFormat.U8V8] = Microsoft.Xna.Framework.Graphics.SurfaceFormat.NormalizedByte2;
            SurfaceFormatDictionary[H2BitmapCollection.BitmapData.EFormat.A8] = Microsoft.Xna.Framework.Graphics.SurfaceFormat.Alpha8;
            SurfaceFormatDictionary[H2BitmapCollection.BitmapData.EFormat.P8] = Microsoft.Xna.Framework.Graphics.SurfaceFormat.Luminance8;
            SurfaceFormatDictionary[H2BitmapCollection.BitmapData.EFormat.L8] = Microsoft.Xna.Framework.Graphics.SurfaceFormat.Luminance8;
            SurfaceFormatDictionary[H2BitmapCollection.BitmapData.EFormat.DXT1] = Microsoft.Xna.Framework.Graphics.SurfaceFormat.Dxt1;
            SurfaceFormatDictionary[H2BitmapCollection.BitmapData.EFormat.DXT3] = Microsoft.Xna.Framework.Graphics.SurfaceFormat.Dxt3;
            SurfaceFormatDictionary[H2BitmapCollection.BitmapData.EFormat.DXT5] = Microsoft.Xna.Framework.Graphics.SurfaceFormat.Dxt5;
        }

        public static Texture2D LoadTexture(GraphicsDevice device, Sunfish.Canvas.H2BitmapCollection.BitmapData meta, byte[] raw)
        {
            int width, height;
            int bitsPerPixel = GetBBP(meta.Format);
            if ((meta.Flags & H2BitmapCollection.BitmapData.EFlags.Power_Of_2_Dimensions) == H2BitmapCollection.BitmapData.EFlags.Power_Of_2_Dimensions)
            {
                width = meta.Width;
                height = meta.Height;
            }
            else
            {
                width = meta.Width + (16 - (meta.Width % 16) == 16 ? 0 : 16 - (meta.Width % 16));
                int x = width;
                int mipWidth = x;
                for (int i = 0; i < meta.MIPMapCount; i++)
                {
                    mipWidth /= 2;
                    x += mipWidth;//broken, not broken? Probably still broken but WHATEVER.
                }
                height = raw.Length / (x * (bitsPerPixel / 8));
            }
            SurfaceFormat format = SurfaceFormatDictionary[meta.Format];
            Texture2D t = new Texture2D(device, width, height, 1, TextureUsage.None, format);
            byte[] newRaw = new byte[(int)(width * height * ((float)bitsPerPixel / 8.0f))];
            Array.Copy(raw, newRaw, newRaw.Length);
            if ((meta.Flags & H2BitmapCollection.BitmapData.EFlags.Swizzled) == H2BitmapCollection.BitmapData.EFlags.Swizzled)
                newRaw = Swizzler.Swizzle(newRaw, width, height, 1, bitsPerPixel, true);
            t.SetData<byte>(newRaw);
            return t;
        }

        public static Texture2D[] LoadCubemap(GraphicsDevice device, Sunfish.Canvas.H2BitmapCollection.BitmapData meta, byte[] raw)
        {
            int size = 0;
            int bitsPerPixel = GetBBP(meta.Format);
            if ((meta.Flags & H2BitmapCollection.BitmapData.EFlags.Power_Of_2_Dimensions) == H2BitmapCollection.BitmapData.EFlags.Power_Of_2_Dimensions)
            {
                size = meta.Width;
            }

            SurfaceFormat format = SurfaceFormatDictionary[meta.Format];
            Texture2D[] textures = new Texture2D[6];

            for (int i = 0; i < 6; i++)
            {
                textures[i] = new Texture2D(device, size, size, 1, TextureUsage.None, format);
                byte[] newRaw = new byte[(int)(size * size * ((float)bitsPerPixel / 8.0f))];
                Array.Copy(raw, GetSurfaceLength(size, meta.MIPMapCount, bitsPerPixel) * i, newRaw, 0, newRaw.Length);
                if ((meta.Flags & H2BitmapCollection.BitmapData.EFlags.Swizzled) == H2BitmapCollection.BitmapData.EFlags.Swizzled)
                    newRaw = Swizzler.Swizzle(newRaw, size, size, 1, bitsPerPixel, true);
                textures[i].SetData<byte>(newRaw);
            }
            return textures;
        }

        static int GetSurfaceLength(int size, int levels, int bbp)
        {
            int len = size * size;
            for (int i = 0; i < levels; i++)
            {
                size /= 2;
                len += size * size;
            }
            len = (int)(len * ((float)bbp / 8.0f));
            len += Padding.GetCount(len, 128);
            return len;
        }
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
}
