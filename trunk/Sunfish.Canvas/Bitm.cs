using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;
using System.Reflection;
using Sunfish.TagStructures;

namespace Sunfish.Canvas
{
    public class H2BitmapCollection : ISerializeable
    {
        public const int SizeOf = 76;

        public EType Type;
        public EFormat Format;
        public EUsage Usage;
        public EFlags Flags;
        public float DetailFadeFactor;
        public float SharpenAmount;
        public float BumpHeight;
        public ESpriteBudgetSize SpriteBudgetSize;
        public short SpriteBudgetCount;
        public short ColourPlateWidth;
        public short ColourPlateHeight;
        public int CompressedColourPlateDataCount;
        public int CompressedColourPlateDataPointer;
        public int ProcessedPixelDataCount;
        public int ProcessedPixelDataPointer;
        public float BlueFilterSize;
        public float AlphaBias;
        public short MipmapCount;
        public ESpriteUsage SpriteUsage;
        public short SpriteSpacing;
        public short ForceFormat;
        public Sequence[] Sequences;
        public BitmapData[] Bitmaps;

        public H2BitmapCollection(Stream stream, int offset, int magic)
        {
            BinaryReader binReader = new BinaryReader(stream);
            stream.Position = offset;

            Type = (EType)binReader.ReadInt16();
            Format = (EFormat)binReader.ReadInt16();
            Usage = (EUsage)binReader.ReadInt16();
            Flags = (EFlags)binReader.ReadInt16();
            DetailFadeFactor = binReader.ReadSingle();
            SharpenAmount = binReader.ReadSingle();
            BumpHeight = binReader.ReadSingle();
            SpriteBudgetSize = (ESpriteBudgetSize)binReader.ReadInt16();
            SpriteBudgetCount = binReader.ReadInt16();
            ColourPlateWidth = binReader.ReadInt16();
            ColourPlateHeight = binReader.ReadInt16();
            CompressedColourPlateDataCount = binReader.ReadInt32();
            CompressedColourPlateDataPointer = binReader.ReadInt32();
            ProcessedPixelDataCount = binReader.ReadInt32();
            ProcessedPixelDataPointer = binReader.ReadInt32();
            BlueFilterSize = binReader.ReadSingle();
            AlphaBias = binReader.ReadSingle();
            MipmapCount = binReader.ReadInt16();
            SpriteUsage = (ESpriteUsage)binReader.ReadInt16();
            SpriteSpacing = binReader.ReadInt16();
            ForceFormat = binReader.ReadInt16();

            int sequenceCount = binReader.ReadInt32();
            int sequencePointer = binReader.ReadInt32();
            int bitmapCount = binReader.ReadInt32();
            int bitmapPointer = binReader.ReadInt32();

            Sequences = new Sequence[sequenceCount];
            for (int i = 0; i < sequenceCount; i++)
                Sequences[i] = new Sequence(stream, (sequencePointer - magic) + (i * Sequence.SizeOf), magic);

            Bitmaps = new BitmapData[bitmapCount];
            for (int i = 0; i < bitmapCount; i++)
                Bitmaps[i] = new BitmapData(stream, (bitmapPointer - magic) + (i * BitmapData.SizeOf));
        }

        public class Sequence : ISerializeable
        {
            public const int SizeOf = 60;

            public string Name;
            public short FirstBitmapIndex;
            public short BitmapCount;
            byte[] unsused;
            public Sprite[] Sprites;

            public Sequence(Stream stream, int offset, int magic)
            {
                BinaryReader binReader = new BinaryReader(stream);
                stream.Position = offset;

                Name = Encoding.UTF8.GetString(binReader.ReadBytes(32)).Trim(char.MinValue);
                FirstBitmapIndex = binReader.ReadInt16();
                BitmapCount = binReader.ReadInt16();
                unsused = binReader.ReadBytes(16);
                int count = binReader.ReadInt32();
                int pointer = binReader.ReadInt32();
                Sprites = new Sprite[count];
                for (int i = 0; i < count; i++)
                    Sprites[i] = new Sprite(stream, (pointer - magic) + (i * Sprite.SizeOf));

            }

            public struct Sprite : ISerializeable
            {
                public const int SizeOf = 32;

                public int BitmapIndex;
                public float Left;
                public float Right;
                public float Top;
                public float Bottom;
                public float RegistrationPointX;
                public float RegistrationPointY;

                public Sprite(Stream stream, int offset)
                {
                    BinaryReader binReader = new BinaryReader(stream);
                    stream.Position = offset;

                    BitmapIndex = binReader.ReadInt32();
                    Left = binReader.ReadSingle();
                    Right = binReader.ReadSingle();
                    Top = binReader.ReadSingle();
                    Bottom = binReader.ReadSingle();
                    RegistrationPointX = binReader.ReadSingle();
                    RegistrationPointY = binReader.ReadSingle();
                }

                #region ISerializeable Members

                public void Deserialize(Stream stream, long startAddress, int magic)
                {
                    throw new NotImplementedException();
                }

                public long Serialize(Stream stream, long startAddress, long nextAddress, int magic)
                {
                    BinaryWriter binWriter = new BinaryWriter(stream);
                    stream.Position = startAddress;

                    #region Deserialize values

                    binWriter.Write(BitmapIndex);
                    binWriter.Write(Left);
                    binWriter.Write(Right);
                    binWriter.Write(Top);
                    binWriter.Write(Bottom);
                    binWriter.Write(RegistrationPointX);
                    binWriter.Write(RegistrationPointY);

                    #endregion

                    return nextAddress;
                }

                #endregion
            }

            #region ISerializeable Members

            public void Deserialize(Stream stream, long startAddress, int magic)
            {
                throw new NotImplementedException();
            }

            public long Serialize(Stream stream, long startAddress, long nextAddress, int magic)
            {
                BinaryWriter binWriter = new BinaryWriter(stream);
                stream.Position = startAddress;

                #region Deserialize values

                byte[] nameBytes = Encoding.UTF8.GetBytes(Name);
                binWriter.Write(nameBytes);
                binWriter.Write(new byte[32 - nameBytes.Length]);
                binWriter.Write(FirstBitmapIndex);
                binWriter.Write(BitmapCount);
                binWriter.Write(unsused, 0, 16);

                long address = nextAddress;
                stream.Position = startAddress + 52;
                binWriter.Write(Sprites.Length);
                if (Sprites.Length > 0)
                    binWriter.Write(address);
                else binWriter.Write(0);
                nextAddress += Sprite.SizeOf * Sprites.Length;
                for (int i = 0; i < Sprites.Length; i++)
                    nextAddress = Sprites[i].Serialize(stream, address + (i * Sprite.SizeOf), nextAddress, 0);

                #endregion

                return nextAddress;
            }

            #endregion
        }

        public class BitmapData : ISerializeable
        {
            public const int SizeOf = 116;

            public char[] Tag = "mtib".ToCharArray();
            public short Width;
            public short Height;
            public short Depth;
            public EType Type;
            public EFormat Format;
            public EFlags Flags;
            public short RegistrationPointX;
            public short RegistrationPointY;
            public short MIPMapCount;
            public int PixelOffset;
            short unused00;
            public int LOD1Offset = -1;
            public int LOD2Offset = -1;
            public int LOD3Offset = -1;
            int unused0 = -1;
            int unused1 = -1;
            int unused2 = -1;
            public int LOD1Size;
            public int LOD2Size;
            public int LOD3Size;
            int unused3;
            int unused4;
            int unused5;
            public int ID;
            byte[] unused6;

            public BitmapData() { }

            public BitmapData(Stream stream, int offset)
            {
                BinaryReader binReader = new BinaryReader(stream);
                stream.Position = offset;

                Tag = binReader.ReadChars(4);
                Width = binReader.ReadInt16();
                Height = binReader.ReadInt16();
                Depth = binReader.ReadInt16();
                Type = (EType)binReader.ReadInt16();
                Format = (EFormat)binReader.ReadInt16();
                Flags = (EFlags)binReader.ReadUInt16();
                RegistrationPointX = binReader.ReadInt16();
                RegistrationPointY = binReader.ReadInt16();
                MIPMapCount = binReader.ReadInt16();
                PixelOffset = binReader.ReadInt32();
                unused00 = binReader.ReadInt16();
                LOD1Offset = binReader.ReadInt32();
                LOD2Offset = binReader.ReadInt32();
                LOD3Offset = binReader.ReadInt32();
                unused0 = binReader.ReadInt32();
                unused1 = binReader.ReadInt32();
                unused2 = binReader.ReadInt32();
                LOD1Size = binReader.ReadInt32();
                LOD2Size = binReader.ReadInt32();
                LOD3Size = binReader.ReadInt32();
                unused3 = binReader.ReadInt32();
                unused4 = binReader.ReadInt32();
                unused5 = binReader.ReadInt32();
                ID = binReader.ReadInt32();
                unused6 = binReader.ReadBytes(32);
            } 

            public enum EFormat : short
            {
                A8 = 0,
                Y8 = 1,
                AY8 = 2,
                A8Y8 = 3,
                R5G6B5 = 6,
                A1R5G5B5 = 8,
                A4R4G4B4 = 9,
                X8R8G8B8 = 10,
                A8R8G8B8 = 11,
                DXT1 = 14,
                DXT3 = 15,
                DXT5 = 16,
                P8 = 17,
                LIGHTMAP = 18,
                U8V8 = 22,
            }

            [Flags]
            public enum EFlags : ushort
            {
                Power_Of_2_Dimensions = 0x0001,
                Compressed = 0x0002,
                Palettized = 0x0004,
                Swizzled = 0x0008,
                Linear = 0x0010,
                V16U16 = 0x0020,
                MIP_Map_Debug_Level = 0x0040,
                Prefer_Stutter = 0x0080,
                Unused1 = 0x0100,
                Unused2 = 0x0200,
                Unused3 = 0x0400,
                Unused4 = 0x0800,
                Unused5 = 0x1000,
                Unused6 = 0x2000,
                Unused7 = 0x4000,
                Unused8 = 0x8000,
            }

            #region ISerializeable Members

            public void Deserialize(Stream stream, long startAddress, int magic)
            {
                throw new NotImplementedException();
            }

            public long Serialize(Stream stream, long startAddress, long nextAddress, int magic)
            { 
                BinaryWriter binWriter = new BinaryWriter(stream);
                stream.Position = startAddress;

                #region Deserialize values

                binWriter.Write(Encoding.UTF8.GetBytes(Tag));
                binWriter.Write(Width);
                binWriter.Write(Height);
                binWriter.Write(Depth);
                binWriter.Write((short)Type);
                binWriter.Write((short)Format);
                binWriter.Write((short)Flags);
                binWriter.Write(RegistrationPointX);
                binWriter.Write(RegistrationPointY);
                binWriter.Write(MIPMapCount);
                binWriter.Write(PixelOffset);
                binWriter.Write(unused00);
                binWriter.Write(LOD1Offset);
                binWriter.Write(LOD2Offset);
                binWriter.Write(LOD3Offset);
                binWriter.Write(unused0);
                binWriter.Write(unused1);
                binWriter.Write(unused2);
                binWriter.Write(LOD1Size);
                binWriter.Write(LOD2Size);
                binWriter.Write(LOD3Size);
                binWriter.Write(unused3);
                binWriter.Write(unused4);
                binWriter.Write(unused5);
                binWriter.Write(ID);
                binWriter.Write(unused6, 0, 32);

                #endregion

                return nextAddress;
            }

            #endregion
        }

        public enum EType : short
        {
            TEXTURES_2D,
            TEXTURES_3D,
            CUBEMAPS,
            SPRITES,
            INTERFACE_BITMAPS,
        }

        public enum EFormat : short
        {
            DXT1,
            DXT2_3,
            DXT4_5,
            COLOUR_16_BIT,
            COLOUR_32_BIT,
            MONOCHROME,
        }

        public enum EUsage : short
        {
            ALPHA_BLEND,
            DEFAULT,
            HEIGHT_MAP,
            DETAIL_MAP,
            LIGHT_MAP,
            VECTOR_MAP,
        }

        [Flags]
        public enum EFlags : short
        {
            ENABLE_DIFFUSION_DITHERING,
            DISABLE_HEIGHT_MAP_COMPRESSION,
            UNIFORM_SPRITE_SEQUENCES,
            FILTHY_SPRITE_BUG_FIX,
            USE_SHARP_BUMP_FILTER,
            UNUSED,
            USE_CLAMPED_MIRRORED_BUMP_FILTER,
            INVERT_DETAIL_FADE,
            SWAP_X_Y_VECTOR_COMPONENTS,
            CONVERT_FROM_SIGNED,
            CONVERT_TO_SIGNED,
            IMPORT_MIPMAP_CHAIN,
            INTENTIONALLY_TRUE_COLOUR
        }

        public enum ESpriteUsage : short
        {
            BLEND_ADD_SUBTRACT_MAX,
            MULTIPLY_MIN,
            DOUBLE_MULTIPLY,
        }

        public enum ESpriteBudgetSize : short
        {
            Size32x32,
            Size64x64,
            Size128x128,
            Size256x256,
            Size512x512,
        }

        //public void Load(Stream stream)
        //{
        //    this = (H2BitmapMeta)Load(stream, typeof(H2BitmapMeta), 0);
        //}

        //private object Load(Stream stream, Type t, int StartPosition)
        //{
        //    BinaryReader Reader = new BinaryReader(stream);
        //    Object O = Activator.CreateInstance(t);
        //    FieldInfo[] Fields = t.GetFields();
        //    foreach (FieldInfo Field in Fields)
        //    {
        //        object[] FieldOffset = Field.GetCustomAttributes(typeof(FieldOffsetAttribute), false);
        //        int Offset = (FieldOffset[0] as FieldOffsetAttribute).Value;
        //        Reader.BaseStream.Position = StartPosition + Offset;
        //        if (Field.FieldType.IsArray)
        //        {
        //            if (Field.FieldType == typeof(Char[]))
        //            {
        //                int Count = 4;
        //                Type ElementType = Field.FieldType.GetElementType();
        //                Array Chars = Array.CreateInstance(ElementType, Count);
        //                for (int i = 0; i < Count; i++)
        //                    Chars.SetValue(Reader.ReadChar(), i);
        //                Field.SetValue(O, Chars);
        //            }
        //            else
        //            {
        //                int Count = Reader.ReadInt32();
        //                Type ElementType = Field.FieldType.GetElementType();
        //                int Size = Marshal.SizeOf(ElementType);
        //                Array Structs = Array.CreateInstance(ElementType, Count);
        //                int StructStartPositition = Reader.ReadInt32();
        //                for (int i = 0; i < Count; i++)
        //                    Structs.SetValue(Load(stream, ElementType, StructStartPositition + (i * Size)), i);
        //                Field.SetValue(O, Structs);
        //            }
        //        }
        //        else
        //        {
        //            if (Field.FieldType.IsEnum)
        //            {
        //                Type BaseType = Enum.GetUnderlyingType(Field.FieldType);
        //                if (BaseType == typeof(int))
        //                    Field.SetValue(O, Reader.ReadInt32());
        //                else if (BaseType == typeof(short))
        //                    Field.SetValue(O, Reader.ReadInt16());
        //                else if (BaseType == typeof(ushort))
        //                    Field.SetValue(O, Reader.ReadUInt16());
        //                else throw new Exception();
        //            }
        //            else if (Field.FieldType.IsPrimitive)
        //            {
        //                if (Field.FieldType == typeof(short))
        //                { Field.SetValue(O, Reader.ReadInt16()); }
        //                else if (Field.FieldType == typeof(int))
        //                { Field.SetValue(O, Reader.ReadInt32()); }
        //                else if (Field.FieldType == typeof(float))
        //                { Field.SetValue(O, Reader.ReadSingle()); }
        //            }
        //            else if (Field.FieldType == typeof(String))
        //            {
        //                string s = new string(Reader.ReadChars(32));
        //                s = s.Remove(s.IndexOf('\0'));
        //                Field.SetValue(O, s);
        //            }
        //            else throw new NotSupportedException();
        //        }
        //    }
        //    return O;
        //}

        #region ISerializeable Members

        public void Deserialize(Stream stream, long startAddress, int magic)
        {
            throw new NotImplementedException();
        }

        public long Serialize(Stream stream, long startAddress, long nextAddress, int magic)
        {
            BinaryWriter binWriter = new BinaryWriter(stream);
            stream.Position = startAddress;

            #region Deserialize values

            binWriter.Write((short)Type);
            binWriter.Write((short)Format);
            binWriter.Write((short)Usage);
            binWriter.Write((short)Flags);
            binWriter.Write(DetailFadeFactor);
            binWriter.Write(SharpenAmount);
            binWriter.Write(BumpHeight);
            binWriter.Write((short)SpriteBudgetSize);
            binWriter.Write(SpriteBudgetCount);
            binWriter.Write(ColourPlateWidth);
            binWriter.Write(ColourPlateHeight);
            binWriter.Write(CompressedColourPlateDataCount);
            binWriter.Write(CompressedColourPlateDataPointer);
            binWriter.Write(ProcessedPixelDataCount);
            binWriter.Write(ProcessedPixelDataPointer);
            binWriter.Write(BlueFilterSize);
            binWriter.Write(AlphaBias);
            binWriter.Write(MipmapCount);
            binWriter.Write((short)SpriteUsage);
            binWriter.Write(SpriteSpacing);
            binWriter.Write(ForceFormat);

            long address = nextAddress;
            stream.Position = startAddress + 60;
            binWriter.Write(Sequences.Length);
            if (Sequences.Length > 0)
                binWriter.Write(address);
            else binWriter.Write(0);
            nextAddress += Sequence.SizeOf * Sequences.Length;
            for (int i = 0; i < Sequences.Length; i++)
                nextAddress = Sequences[i].Serialize(stream, address + (i * Sequence.SizeOf), nextAddress, 0);

            address = nextAddress;
            stream.Position = startAddress + 68;
            binWriter.Write(Bitmaps.Length);
            if (Bitmaps.Length > 0)
                binWriter.Write(address);
            else binWriter.Write(0);
            nextAddress += BitmapData.SizeOf * Bitmaps.Length;
            for (int i = 0; i < Bitmaps.Length; i++)
                nextAddress = Bitmaps[i].Serialize(stream, address + (i * BitmapData.SizeOf), nextAddress, 0);


            #endregion

            return nextAddress;
        }

        #endregion
    }
}
