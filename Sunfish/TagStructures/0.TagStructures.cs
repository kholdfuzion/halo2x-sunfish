using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Reflection;
using Sunfish.TagStructures;

namespace Sunfish.TagStructures
{
    public interface ISerializeable
    {
        void Deserialize(Stream stream, long startAddress, int magic);
        long Serialize(Stream stream, long startAddress, long nextAddress, int magic);
    }


    public abstract class TagBlock : ISerializeable
    {
        public readonly string Name;
        public readonly int Size;
        public readonly int Alignment = 4;
        public byte[] Data
        {
            get { return data; }
            set
            {
                data = value;
                if (Values == null) return;
                foreach (Value val in Values)
                    val.SetDataReference(data);
            }
        }
        public Value[] Values;

        #region static members
        
        static Dictionary<string, Type> Lookup;

        static TagBlock()
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            List<Type> Tags = new List<Type>(118);
            foreach (Type t in asm.GetTypes())
            {
                if (t.Namespace == "Sunfish.TagStructures" && t.BaseType == typeof(TagBlock) && !t.IsNested)
                    Tags.Add(t);
            }
            Tags.TrimExcess();
            Lookup = new Dictionary<string, Type>(Tags.Count);
            foreach (Type T in Tags)
            {
                TagBlock Tag = (TagBlock)Activator.CreateInstance(T);
                Lookup.Add(Tag.Name, T);
            }
        }

        public static TagBlock CreateInstance(string type, bool decompiled)
        {
            Type T = Lookup[type];
            TagBlock Tag = (TagBlock)Activator.CreateInstance(T, new object[] { decompiled });
            return Tag;
        }

        public static TagBlock CreateInstance(string type)
        {
            Type T = Lookup[type];
            TagBlock Tag = (TagBlock)Activator.CreateInstance(T);
            return Tag;
        }

        public static TagBlock CreateFromTag (Tag tag)
        {
            TagBlock tagBlock = TagBlock.CreateInstance(tag.Type);
            tagBlock.Deserialize(tag.TagStream, 0, 0);
            return tagBlock;
        }

        #endregion

        private byte[] data;

        public TagBlock(string name, int size, int alignment)
        {
            Name = name;
            Size = size;
            Alignment = alignment;
        }

        public TagBlock(int size, int alignment)
        {
            Name = "";
            Size = size;
            Alignment = alignment;
        }

        public TagBlock(string name, int size)
        {
            Name = name;
            Size = size;
        }

        protected Value[] InitializeValues(Value[] values)
        {
            List<Value> retValues = new List<Value>(values.Length);
            int offset = 0;
            foreach (Value val in values)
            {
                val.Offset = offset;
                offset += val.Size;
                if (!(val is Data))
                    retValues.Add(val);
                if (offset > Size) throw new Exception();
            }
            return retValues.ToArray();
        }

        protected Value[] InitializeValues(Value[] values, bool processRawBlocks)
        {
            if (!processRawBlocks) return InitializeValues(values);

            List<Value> retValues = new List<Value>(values.Length);
            int offset = 0;
            foreach (Value val in values)
            {
                val.Offset = offset;
                offset += val.Size;
                if (!(val is Data))
                    retValues.Add(val);
                if (offset > Size) throw new Exception();
            }
            List<RawBlockAddress> rawBlockAddressValues = new List<RawBlockAddress>();
            List<RawBlockLength> rawBlockLengthValues = new List<RawBlockLength>();

            foreach (Value val in values)
                if (val is RawBlockAddress) { rawBlockAddressValues.Add(val as RawBlockAddress); retValues.Remove(val); }
                else if (val is RawBlockLength) { rawBlockLengthValues.Add(val as RawBlockLength); retValues.Remove(val); }

            List<Value> rawBlocks = new List<Value>(rawBlockAddressValues.Count);

            for (int i = 0; i < rawBlocks.Capacity; i++)
                rawBlocks.Add((RawBlock)Activator.CreateInstance(rawBlockAddressValues[i].RawBlockType, new object[] { rawBlockAddressValues[i], rawBlockLengthValues[i] }));
            retValues.AddRange(rawBlocks);
            return retValues.ToArray();
        }

        public override string ToString()
        {
            return string.Format("Name:{0}, Size:{1}", Name, Size);
        }

        public void Save(Tag tag)
        {
            tag.TagStream = new MemoryStream();
            Serialize(tag.TagStream, 0, this.Size, 0);
        }

        public int IndexOfValue(int offset)
        {
            for (int i = 0; i < Values.Length; i++)
                if (Values[i].Offset == offset) return i;
            return -1;
        }

        public int IndexOfValue(string name)
        {
            for (int i = 0; i < Values.Length; i++)
                if (Values[i] is TagBlockArray)
                    if ((Values[i] as TagBlockArray).Default.Name == name) return i;
            return -1;
        }

        public void Update()
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            Serialize(ms, 0, this.Size, 0);
            Deserialize(ms, 0, 0);
        }

        #region ISerializeable Members

        public void Deserialize(Stream stream, long startAddress, int magic)
        {
            stream.Position = startAddress;
            Data = new byte[Size];
            stream.Read(Data, 0, Size);
            if (Values == null) return;
            foreach (Value val in Values)
            {
                val.SetDataReference(Data);
                if (val is ISerializeable)
                    (val as ISerializeable).Deserialize(stream, startAddress + val.Offset, magic);
            }
        }

        public long Serialize(Stream stream, long startAddress, long nextAddress, int magic)
        {
            if (Values != null)
            {
                foreach (Value val in Values)
                {
                    if (val is ISerializeable && !(val is RawBlock))
                    {
                        nextAddress = (val as ISerializeable).Serialize(stream, nextAddress, nextAddress, magic);
                    }
                }
            }
            stream.Position = startAddress;
            stream.Write(Data, 0, data.Length);
            return nextAddress;
        }

        #endregion
    }

    public abstract class RawBlock : Value, ISerializeable
    {
        public byte[] Data;

        public int addressOffset;
        public int lengthOffset;

        int address
        {
            get { return BitConverter.ToInt32(dataRef, addressOffset); }
            set
            {
                byte[] setBytes = BitConverter.GetBytes(value);
                dataRef[addressOffset] = setBytes[0];
                dataRef[addressOffset + 1] = setBytes[1];
                dataRef[addressOffset + 2] = setBytes[2];
                dataRef[addressOffset + 3] = setBytes[3];
            }
        }
        protected virtual int length
        {
            get { return BitConverter.ToInt32(dataRef, lengthOffset); }
            set
            {
                byte[] setBytes = BitConverter.GetBytes(value);
                dataRef[lengthOffset] = setBytes[0];
                dataRef[lengthOffset + 1] = setBytes[1];
                dataRef[lengthOffset + 2] = setBytes[2];
                dataRef[lengthOffset + 3] = setBytes[3];
            }
        }

        public RawBlock(RawBlockAddress addressValue, RawBlockLength lengthValue)
        {
            Size = 0;
            addressOffset = addressValue.Offset;
            lengthOffset = lengthValue.Offset;
        }

        protected bool IsInternal { get { return ((this.address & 0xC0000000) == 0x00000000); } }

        #region ISerializeable Members

        public virtual void Deserialize(Stream stream, long startAddress, int magic)
        {
            if (!IsInternal) return;
            stream.Position = this.address;
            Data = new byte[this.length];
            stream.Read(Data, 0, this.length);
        }

        public long Serialize(Stream stream, long startAddress, long nextAddress, int magic)
        {
            if (!IsInternal) return nextAddress;
            stream.Position = startAddress;
            stream.Write(Data, 0, this.length);
            nextAddress = startAddress + nextAddress;
            return nextAddress;
        }

        #endregion
    }

    public abstract class Value
    {
        protected byte[] dataRef;
        public int Offset;
        public int Size;

        public void SetDataReference(byte[] dataRef)
        {
            this.dataRef = dataRef;
        }

        public override string ToString()
        {
            return string.Format("Offset:{0}, Size:{1}", Offset, Size);
        }
    }


    public class ByteArray : Value, ISerializeable
    {
        public int Length
        {
            get { return BitConverter.ToInt32(base.dataRef, base.Offset); }
            set
            {
                byte[] setBytes = BitConverter.GetBytes(value);
                dataRef[Offset] = setBytes[0];
                dataRef[Offset + 1] = setBytes[1];
                dataRef[Offset + 2] = setBytes[2];
                dataRef[Offset + 3] = setBytes[3];
            }
        }
        public int Address
        {
            get { return BitConverter.ToInt32(base.dataRef, base.Offset + 4); }
            set
            {
                byte[] setBytes = BitConverter.GetBytes(value);
                dataRef[Offset + 4] = setBytes[0];
                dataRef[Offset + 5] = setBytes[1];
                dataRef[Offset + 6] = setBytes[2];
                dataRef[Offset + 7] = setBytes[3];
            }
        }
        public byte[] Data
        {
            get { return byteArrayData; }
            set { byteArrayData = value; Length = byteArrayData.Length; }
        }

        private byte[] byteArrayData;
        public int Alignment = 4;

        public ByteArray(int alignment)
        {
            Size = 8;
            this.Alignment = alignment;
        }

        public ByteArray()
        { Size = 8; }

        public int CalculatePointers(int virtualOffset, int nextVirtualOffset)
        {
            Address = virtualOffset;
            nextVirtualOffset += Length;
            return nextVirtualOffset;
        }

        #region ISerializeable Members

        public void Deserialize(Stream stream, long startAddress, int magic)
        {
            if (Length == 0) return;
            stream.Position = Address - magic;
            byteArrayData = new byte[Length];
            stream.Read(byteArrayData, 0, Length);
        }

        public long Serialize(Stream stream, long startAddress, long nextAddress, int magic)
        {
            if (Length == 0) return nextAddress;
            int padding = Padding.GetCount(startAddress, Alignment);
            startAddress += padding;
            stream.Position = startAddress;
            nextAddress += padding;
            Address = (int)startAddress + magic;
            nextAddress += Length;
            stream.Write(byteArrayData, 0, Length);
            return nextAddress;
        }

        #endregion
    }

    public class StringReference : Value
    {
        public StringReference()
        { Size = 4; }
    }

    public class TagReference : Value
    {
        public TagReference()
        { Size = 8; }
    }

    public class TagIdentifier : Value
    {
        public TagIdentifier()
        { Size = 4; }
    }

    public class TagBlockArray : Value, ISerializeable, ICollection<TagBlock>
    {
        public int Length
        {
            get { return BitConverter.ToInt32(base.dataRef, base.Offset); }
            set
            {
                byte[] setBytes = BitConverter.GetBytes(value);
                dataRef[Offset] = setBytes[0];
                dataRef[Offset + 1] = setBytes[1];
                dataRef[Offset + 2] = setBytes[2];
                dataRef[Offset + 3] = setBytes[3];
            }
        }
        public int Address
        {
            get { return BitConverter.ToInt32(base.dataRef, base.Offset + 4); }
            set
            {
                byte[] setBytes = BitConverter.GetBytes(value);
                dataRef[Offset + 4] = setBytes[0];
                dataRef[Offset + 5] = setBytes[1];
                dataRef[Offset + 6] = setBytes[2];
                dataRef[Offset + 7] = setBytes[3];
            }
        }
        public TagBlock[] TagBlocks
        {
            get { return tagBlocks; }
            set { Length = value.Length; tagBlocks = value; }
        }
        public readonly Type TagBlockType;

        private TagBlock[] tagBlocks;

        public TagBlockArray(Type tagBlockType)
        {
            Size = 8;
            TagBlockType = tagBlockType;
        }

        public override string ToString()
        {
            return string.Format("Offset:{0}, Size:{1}, Address:{2}, Length:{3}", Offset, Size, Address, Length);
        }

        public TagBlock Default
        {
            get
            {
                TagBlock block = (TagBlock)Activator.CreateInstance(TagBlockType, true);
                block.Data = new byte[block.Size];
                return block;
            }
        }

        private List<TagBlock> GetTagBlockList()
        {
            List<TagBlock> blocks = new List<TagBlock>();
            if (TagBlocks != null)
                blocks.AddRange(TagBlocks);
            return blocks;
        }

        #region ISerializeable Members

        public void Deserialize(Stream stream, long startAddress, int magic)
        {
            if (Length == 0) return;
            stream.Position = Address - magic;
            TagBlocks = new TagBlock[Length];
            for (int i = 0; i < Length; i++)
            {
                TagBlocks[i] = (TagBlock)Activator.CreateInstance(TagBlockType);
                TagBlocks[i].Deserialize(stream, (Address - magic) + (i * TagBlocks[i].Size), magic);
            }
        }

        public long Serialize(Stream stream, long startAddress, long nextAddress, int magic)
        {
            if (Length == 0) return nextAddress;

            long padding = Padding.GetCount(startAddress, tagBlocks[0].Alignment);
            startAddress += padding;
            Address = (int)startAddress + magic;
            nextAddress += padding + (Length * TagBlocks[0].Size);

            for (int i = 0; i < Length; i++)
            {
                nextAddress = TagBlocks[i].Serialize(stream, startAddress + (i * TagBlocks[i].Size), nextAddress, magic);
            }
            return nextAddress;
        }

        #endregion

        #region ICollection<TagBlock> Members

        public void Add(TagBlock item)
        {
            List<TagBlock> blocks = GetTagBlockList();
            blocks.Add(item);
            TagBlocks = blocks.ToArray();
        }

        public void Insert(int index, TagBlock item)
        {
            List<TagBlock> blocks = GetTagBlockList();
            blocks.Insert(index, item);
            TagBlocks = blocks.ToArray();
        }

        public void Remove(TagBlock item)
        {
            if (TagBlocks == null) { return; }
            List<TagBlock> blocks = new List<TagBlock>(TagBlocks);
            blocks.Remove(item);
            TagBlocks = blocks.ToArray();
        }

        public void RemoveAt(int index)
        {
            if (TagBlocks == null) { return; }
            List<TagBlock> blocks = new List<TagBlock>(TagBlocks);
            blocks.RemoveAt(index);
            TagBlocks = blocks.ToArray();
        }

        public void RemoveAll()
        {
            Clear();
        }

        public void Clear()
        {
            TagBlocks = new TagBlock[0];
        }

        public bool Contains(TagBlock item)
        {
            foreach (TagBlock t in TagBlocks)
                if (Object.ReferenceEquals(item, t)) return true;
            return false;
        }

        public void CopyTo(TagBlock[] array, int arrayIndex)
        {
            Array.Copy(TagBlocks, 0, array, arrayIndex, TagBlocks.Length);
        }

        public int Count
        {
            get { return TagBlocks.Length; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        bool ICollection<TagBlock>.Remove(TagBlock item)
        {
            if (TagBlocks == null) { return false; }
            List<TagBlock> blocks = new List<TagBlock>(TagBlocks);
            bool b = blocks.Remove(item);
            TagBlocks = blocks.ToArray();
            return b;
        }

        #endregion

        #region IEnumerable<TagBlock> Members

        public IEnumerator<TagBlock> GetEnumerator()
        {
            foreach (TagBlock t in TagBlocks)
                yield return t;
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return TagBlocks.GetEnumerator();
        }

        #endregion
    }


    public class RawBlockAddress : Value
    {
        public Type RawBlockType;

        public RawBlockAddress(Type rawBlockType)
        {
            RawBlockType = rawBlockType;
            Size = 4;
        }
    }

    public class RawBlockLength : Value
    {
        public RawBlockLength()
        {
            Size = 4;
        }
    }

    public class Data : Value
    {
        public Data(int size)
        { base.Size = size; }
    }
}
