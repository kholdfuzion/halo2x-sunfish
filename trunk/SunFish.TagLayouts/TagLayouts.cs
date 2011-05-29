using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Sunfish.TagLayouts
{
    public class Layouts
    {
        public TagDefinition LoadTagDefinitionFromStream(TagDefinition _tag, Stream _stream, int _offset, int _magic)
        {
            BinaryReader reader = new BinaryReader(_stream, Encoding.UTF8);
            _tag = (TagDefinition)RecursivelyReadTagBlock(_tag as ITagBlock, reader, _offset, _magic);
            return _tag;
        }

        private ITagBlock RecursivelyReadTagBlock(ITagBlock _block, BinaryReader _reader, int _offset, int _magic)
        {
            _reader.BaseStream.Position = _offset;
            _block.Data = _reader.ReadBytes(_block.Size);
            if (_block.Values == null) { return _block; }
                int Offset = 0;
            foreach (Value value in _block.Values)
            {
                _reader.BaseStream.Position = _offset + Offset;
                Offset += value.length;
                if(value is TagBlockArray)
                {
                    TagBlockArray tagBlockArray = value as TagBlockArray;
                    tagBlockArray.count = _reader.ReadInt32();
                    if (tagBlockArray.count < 1) continue;
                    int address = _reader.ReadInt32() - _magic;
                    TagBlock[] tagBlocks = new TagBlock[tagBlockArray.count];
                    for (int i = 0; i < tagBlocks.Length; i++)
                    {
                        tagBlocks[i] = tagBlockArray.tagBlocks[0];
                        tagBlocks[i] = (TagBlock)RecursivelyReadTagBlock(tagBlocks[0], _reader, address + (i * tagBlocks[0].size), _magic);
                    }
                    tagBlockArray.tagBlocks = tagBlocks;
                }
                else if (value is RawBlock)
                {
                    RawBlock rawReference = value as RawBlock;
                    _reader.BaseStream.Position = _offset + rawReference.pointerOffset;
                    uint rawDataOffset = _reader.ReadUInt32();
                    if (IsInternal(rawDataOffset))
                    {
                        _reader.BaseStream.Position = _offset + rawReference.lengthOffset;
                        int rawDataLength = _reader.ReadInt32();
                        _reader.BaseStream.Position = rawDataOffset;
                        rawReference.data = _reader.ReadBytes(rawDataLength);
                    }
                }
            }
            return _block;
        }

        public bool IsInternal(uint pointer)
        {
            return ((0xC0000000 & pointer) == 0);
        }
    }

    public abstract class TagDefinition : ITagBlock
    {
        int alignment = 4;
        int size;
        byte[] data;
        Value[] values;

        public TagDefinition LoadFromStream(TagDefinition tagDefinition, Stream stream, int offset, int magic)
        {
            BinaryReader reader = new BinaryReader(stream, Encoding.UTF8);
            tagDefinition = (TagDefinition)RecursivelyReadTagBlock(tagDefinition as ITagBlock, reader, offset, magic);
            return tagDefinition;
        }

        private ITagBlock RecursivelyReadTagBlock(ITagBlock tagBlock, BinaryReader binaryReader, int offset, int magic)
        {
            binaryReader.BaseStream.Position = offset;
            tagBlock.Data = binaryReader.ReadBytes(tagBlock.Size);
            if (tagBlock.Values == null) { return tagBlock; }
            int valueOffset = 0;
            foreach (Value value in tagBlock.Values)
            {
                binaryReader.BaseStream.Position = offset + valueOffset;
                valueOffset += value.length;
                if (value is TagBlockArray)
                {
                    TagBlockArray tagBlockArray = value as TagBlockArray;
                    tagBlockArray.count = binaryReader.ReadInt32();
                    if (tagBlockArray.count < 1) continue;
                    int address = binaryReader.ReadInt32() - magic;
                    TagBlock[] tagBlocks = new TagBlock[tagBlockArray.count];
                    for (int i = 0; i < tagBlocks.Length; i++)
                    {
                        tagBlocks[i] = tagBlockArray.tagBlocks[0];
                        tagBlocks[i] = (TagBlock)RecursivelyReadTagBlock(tagBlocks[0], binaryReader, address + (i * tagBlocks[0].size), magic);
                    }
                    tagBlockArray.tagBlocks = tagBlocks;
                }
                else if (value is RawBlock)
                {
                    RawBlock raw = value as RawBlock;
                    binaryReader.BaseStream.Position = offset + raw.pointerOffset;
                    uint rawDataOffset = binaryReader.ReadUInt32();
                    if (IsInternal(rawDataOffset))
                    {
                        binaryReader.BaseStream.Position = offset + raw.lengthOffset;
                        int rawDataLength = binaryReader.ReadInt32();
                        binaryReader.BaseStream.Position = rawDataOffset;
                        raw.data = binaryReader.ReadBytes(rawDataLength);
                    }
                }
            }
            return tagBlock;
        }

        private bool IsInternal(uint pointer)
        {
            return ((0xC0000000 & pointer) == 0);
        }

        #region ITagBlock Members

        public int Alignment { get { return alignment; } set { alignment = value; } }

        public int Size
        {
            get
            {
                return size;
            }
            set
            {
                size = value;
            }
        }

        public byte[] Data
        {
            get
            {
                return data;
            }
            set
            {
                data = value;
            }
        }

        public Value[] Values
        {
            get
            {
                return values;
            }
            set
            {
                values = value;
            }
        }

        #endregion
    }

    public interface IDataBound
    {
        TagDefinition Unbind(TagDefinition tagDefinition, object dataSource);
    }

    public interface ITagBlock
    {
        int Alignment { get; set; }
        int Size { get; set; }
        byte[] Data { get; set; }
        Value[] Values { get; set; }
    }

    public class Data : Value
    {
        public Data(int _length)
            : base(_length) { }
    }

    public class StringID : Value
    {
        public StringID()
            : base(4) { }
    }

    public class TagReference : Value
    {
        public TagReference()
            : base(8) { }
    }

    public class RawBlock : Value
    {
        public byte[] data;
        public int pointerOffset;
        public int lengthOffset;

        public RawBlock(int pointerOffset, int lengthOffset)
            : base(0)
        {
            this.pointerOffset = pointerOffset;
            this.lengthOffset = lengthOffset;
        }
    }

    public abstract class Value
    {
        public int length;

        public Value(int _length)
        { length = _length; }
    }

    public class ByteArray : Value
    {
        public int count;
        public byte[] bytes;

        public ByteArray()
            : base(8)
        {
            count = 0;
        }
    }

    public class TagBlockArray : Value
    {
        public int count;
        public TagBlock[] tagBlocks;

        public TagBlockArray(TagBlock _tagBlock)
            : base(8)
        {
            tagBlocks = new TagBlock[] { _tagBlock };
            count = 0;
        }
    }

    public struct TagBlock : ITagBlock
    {
        public int alignment;
        public int size;
        public byte[] data;
        public Value[] values;

        public TagBlock(int _size, Value[] _values)
        {
            alignment = 4;
            size = _size;
            data = null;
            values = _values;
        }

        #region ITagBlock Members

        public int Alignment { get { return alignment; } set { alignment = value; } }

        public int Size
        {
            get
            {
                return size;
            }
            set
            {
                size = value;
            }
        }

        public byte[] Data
        {
            get
            {
                return data;
            }
            set
            {
                data = value;
            }
        }

        public Value[] Values
        {
            get
            {
                return values;
            }
            set
            {
                values = value;
            }
        }

        #endregion
    }
}
