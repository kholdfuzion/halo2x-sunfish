using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;

namespace Sunfish.ValueTypes
{
    public struct TagIndex
    {
        private const short TagDatum = -7820;
        public short Index;
        short Salt;

        public TagIndex(short index, short salt)
        {
            Index = index;
            Salt = salt;
        }

        public TagIndex(int index)
        {
            Index = (short)index;
            Salt = (short)(TagDatum + index);
        }

        public static implicit operator int(TagIndex tagIndex)
        {
            return (tagIndex.Salt << 16) | (ushort)tagIndex.Index;
        }

        public static implicit operator TagIndex(int i)
        {
            return new TagIndex((short)(i & 0x0000FFFF), (short)((i & 0xFFFF0000) >> 16));
        }

        public override string ToString()
        {
            return Index.ToString();
        }
    }

    public struct StringId
    {
        public short Index;
        public sbyte Length;

        public StringId(short index, sbyte length)
        {
            Index = index;
            Length = length;
        }

        public static implicit operator int(StringId strRef)
        {
            return (strRef.Length << 24) | (ushort)strRef.Index;
        }

        public static implicit operator StringId(int i)
        {
            return new StringId((short)(i & 0x0000FFFF), (sbyte)((i & 0xFF000000) >> 24));
        }

        public override string ToString()
        {
            return string.Format("Index:{0}, Length:{1}", Index, Length);
        }
    }

    public struct TagType
    {
        public string Type;

        public TagType(byte[] typefourcc)
        {
            Type = Encoding.UTF8.GetString(typefourcc, 0, 4);
        }

        public TagType(string typefourcc)
        {
            Type = typefourcc.Substring(0, 4);
        }

        public static bool operator ==(TagType tagType, string value)
        {
            return tagType.Type == value;
        }

        public static bool operator !=(TagType tagType, string value)
        {
            return tagType.Type != value;
        }

        public static bool operator ==(TagType tagType, TagType value)
        {
            return tagType.Type == value.Type;
        }

        public static bool operator !=(TagType tagType, TagType value)
        {
            return tagType.Type != value.Type;
        }

        public byte[] ToByteArray()
        {
            byte[] buffer =Encoding.UTF8.GetBytes(Type);
            Array.Reverse(buffer);
            return buffer;
        }

        public static implicit operator TagType(string str)
        {
            return new TagType(Encoding.UTF8.GetBytes(str));
        }

        public static TagType Null { get { return new TagType(new byte[] { 0xFF, 0xFF, 0xFF, 0xFF }); } }

        public override string ToString()
        {
            return Type;
        }

        public string ToPathSafeString()
        {
            StringBuilder builder = new StringBuilder(this.ToString());
            foreach (char c in Path.GetInvalidPathChars())
                builder.Replace(c, ' ');
            return builder.ToString().Trim();
        }
    }

    public struct TagBlockPointer
    {
        public int Count;
        public int Address;
    }

    public static class Extensions
    {
        #region Read Extensions

        public static TagBlockPointer ReadTagBlockPointer(this BinaryReader br)
        {
            return new TagBlockPointer() { Count = br.ReadInt32(), Address = br.ReadInt32() };
        }

        public static TagIndex ReadTagIndex(this BinaryReader br)
        {
            return new TagIndex(br.ReadInt16(), br.ReadInt16());
        }

        public static StringId ReadStringReference(this BinaryReader br)
        {
            short index = br.ReadInt16();
            br.ReadByte();
            sbyte length = br.ReadSByte();
            return new StringId(index, length);
        }

        public static TagType ReadTagType(this BinaryReader br)
        {
            byte[] b = br.ReadBytes(4);
            Array.Reverse(b);
            return new TagType(b);
        }

        #endregion

        #region Write Extensions

        public static void Write(this BinaryWriter bw, string s)
        {
            bw.Write(s);
        }

        public static void Write(this BinaryWriter bw, TagIndex tagindex)
        {
            bw.Write(tagindex);
        }

        public static void Write(this BinaryWriter bw, StringId stringreference)
        {
            bw.Write(stringreference);
        }

        public static void Write(this BinaryWriter bw, TagType tagtype)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(tagtype.ToString());
            Array.Reverse(buffer, 0, 4);
            bw.Write(buffer, 0, 4);
        }

        #endregion
    }
}
