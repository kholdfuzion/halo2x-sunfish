using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Sunfish.TagStructures;
using System.IO;
using System.Collections;
using System.Runtime.InteropServices;

namespace Sunfish.Developmental
{
    public class Blocks
    {
        public static Dictionary<string, Block> Types;

        static Blocks()
        {
            Blocks b = new Blocks();
            Types = b.CreateBlocks();
        }

        Dictionary<string, Block> CreateBlocks()
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            List<Type> Tags = new List<Type>(118);
            foreach (Type t in asm.GetTypes())
            {
                if (t.Namespace == "Sunfish.TagStructures" && t.BaseType == typeof(TagBlock) && !t.IsNested)
                    Tags.Add(t);
            }
            Tags.TrimExcess();
            Dictionary<string, Block> Blocks = new Dictionary<string, Block>(Tags.Count);
            foreach (Type T in Tags)
            {
                TagBlock Tag;
                Block block;
                if (T == typeof(unic))
                {
                    Tag = (TagBlock)Activator.CreateInstance(T, new object[] { true });
                    block = RecursivelyParseTagBlock(Tag);
                    Blocks.Add("utf8", block);
                }
                Tag = (TagBlock)Activator.CreateInstance(T);
                block = RecursivelyParseTagBlock(Tag);
                Blocks.Add(Tag.Name, block);
            }

            return Blocks;
        }

        Block RecursivelyParseTagBlock(TagBlock Tag)
        {
            Block block = new Block() { Size = Tag.Size, Alignment = Tag.Alignment };
            List<Block> nestedBlocks = new List<Block>();
            List<Value> values = new List<Value>();
            List<Raw> raws = new List<Raw>();
            if (Tag.Values != null)
            {
                foreach (TagStructures.Value val in Tag.Values)
                {
                    if (val is TagBlockArray)
                    {
                        Block nestedBlock = RecursivelyParseTagBlock((TagBlock)Activator.CreateInstance((val as TagBlockArray).TagBlockType));
                        nestedBlock.Offset = val.Offset;
                        nestedBlocks.Add(nestedBlock);
                    }
                    else if (val is ByteArray)
                    {
                        ByteArray array = val as ByteArray;
                        Block nestedBlock = new Block() { 
                            Alignment = array.Alignment, 
                            NestedBlocks = new Block[0], 
                            Offset = array.Offset, 
                            Raws = new Raw[0], 
                            Size = 1, 
                            Values = new Value[0] 
                        };
                        nestedBlocks.Add(nestedBlock);
                    }
                    else if (val is StringReference)
                    {
                        values.Add(new Value() { Offset = val.Offset, Type = Value.ValueType.StringId });
                    }
                    else if (val is TagIdentifier)
                    {
                        values.Add(new Value() { Offset = val.Offset, Type = Value.ValueType.TagId });
                    }
                    else if (val is TagReference)
                    {
                        values.Add(new Value() { Offset = val.Offset, Type = Value.ValueType.TagReference });
                    }
                    else if (val is RawBlock)
                    {
                        RawBlock rawBlock = val as RawBlock;
                        raws.Add(new Raw() { Offset0 = rawBlock.lengthOffset, Offset1 = rawBlock.addressOffset });
                    }
                }
            }
            block.NestedBlocks = nestedBlocks.ToArray();
            block.Values = values.ToArray();
            block.Raws = raws.ToArray();
            return block;
        }
    }

    public class Cache<T>
    {
        public T[] Values;
        public int Count;

        public Cache(int capacity)
        {
            Values = new T[capacity];
            Count = 0;
        }

        public void Clear()
        {
            Count = 0;
        }

        public void Add(T value)
        {
            Values[Count] = value;
            Count++;
        }
    }

    public struct RawInfo
    {
        public int Length;
        public int Address;
    }

    public struct Pointer
    {
        public int Offset;
        public int Address;

        public override string ToString()
        {
            return Offset.ToString();
        }
    }

    public struct Block
    {
        public int Size;
        public int Alignment;
        public int Offset;
        public Value[] Values;
        public Block[] NestedBlocks;
        public Raw[] Raws;
    }

    public struct Raw
    {
        public int Offset0;
        public int Offset1;

        public override string ToString()
        {
            return string.Format("Offset0:{0}, Offset1:{1}", Offset0, Offset1);
        }
    }

    public struct Value
    {
        public ValueType Type;
        public int Offset;

        public override string ToString()
        {
            return string.Format("Offset:{0}, Type:{1}", Offset, Type);
        }

        public enum ValueType
        {
            StringId,
            TagId,
            TagReference,
        }
    }    
}
