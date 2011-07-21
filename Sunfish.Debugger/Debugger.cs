using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sunfish.Developmental;
using System.IO;
using Sunfish.ValueTypes;

namespace Sunfish.Debugger
{
    public class Sector
    {
        public bool IsInternal;
        public bool IsAligned { get { return (StartAddress % Alignment) == 0; } }

        public int StartAddress;
        public int Length;
        public int Alignment;

        public int NextOffset { get { return StartAddress + Length; } }
        public Block Block;

        public Sector(int startaddress, int length, int alignment, Block block)
        {
            this.IsInternal = true;
            this.StartAddress = startaddress;
            this.Length = length;
            this.Alignment = alignment;
            this.Block = block;
        }
    }

    public struct Report
    {
        public string Tagname;
        public TagType Type;
        public List<Sector> Sectors;

        public Report(string tagname, TagType type)
        {
            Tagname = String.Empty;
            Type = TagType.Null;
            Sectors = new List<Sector>();
        }
    }

    public class Debugger
    {
        public List<string> Strings;
        BinaryReader br;
        int magic;
        int startAddress;
        int length;

        public Debugger()
        {
            Strings = new List<string>(GlobalStringIDs.Values);
            Strings.Sort();
        }


        public void DebugMap(Map map)
        {
            Strings = new List<string>();
            Strings.Sort();

            for (int i = 0; i < map.Index.TagEntries.Length; i++)
            {
                DebugTag(map.Index.TagEntries[i].Index.Index, map);
                System.Windows.Forms.Application.DoEvents();
            }
        }

        public Report DebugTag(TagIndex tagindex, Map map)
        {
            Block b = Blocks.Types[map.Index.TagEntries[tagindex.Index].Type.ToString()];
            int magic;
            if (map.Index.TagEntries[tagindex.Index].Type == "sbsp" || map.Index.TagEntries[tagindex.Index].Type == "sbsp")
                magic = map.PrimaryMagic;
            else
                magic = map.SecondaryMagic;
            startAddress = map.Index.TagEntries[tagindex.Index].VirtualAddress - magic;
            length = map.Index.TagEntries[tagindex.Index].Length;
            Report report = ProcessBlock(b, map, map.Index.TagEntries[tagindex.Index].VirtualAddress, magic);
            report.Tagname = map.Tagnames[tagindex.Index];
            report.Type = map.Index.TagEntries[tagindex.Index].Type;
            return report;
        }

        Report ProcessBlock(Block block, Map map, int address, int magic)
        {
            br = new BinaryReader(map.BaseStream);
            this.magic = magic;
            Report report = new Report();
            report.Sectors = new List<Sector>();
            ProcessBlockRecursive(block, map, address, 1, ref report);
            return report;
        }

        void ProcessBlockRecursive(Block block, Map map, int address, int count, ref Report report)
        {
            br.BaseStream.Position = address - magic;

            #region Check Alignement

            if (br.BaseStream.Position % block.Alignment != 0)
            {
                //if (OnAddressError != null)
                //{
                //    OnAddressError(this, new AddressingErrorArgs()
                //    {
                //        Block = block,
                //        Offset = startAddress,
                //        Length = length,
                //        Type = AddressingErrorArgs.ErrorType.InvalidAlignment
                //    });
                //}
            }

            #endregion

            #region Read Sector

            bool internalSector = (address - magic) >= startAddress && (address - magic) < startAddress + length;
            report.Sectors.Add(new Sector(address - magic, block.Size * count, block.Alignment, block) { IsInternal = internalSector });

            //if (OnSectorRead != null)
            //{
            //    bool internalSector = (address - magic) >= startAddress && (address - magic) < startAddress + length;
            //    OnSectorRead(this, new ReadSectorArgs()
            //    {
            //        Sector = new Sector(address - magic, block.Size * count, block.Alignment, block) { IsInternal = internalSector }
            //    });
            //}

            #endregion

            int blockAddress = (int)br.BaseStream.Position;

            #region Check Values

            for (int i = 0; i < count; i++)
            {
                for (int j = 0; j < block.Values.Length; j++)
                {
                    int offset = blockAddress + (block.Size * i) + block.Values[j].Offset;
                    br.BaseStream.Seek(offset, SeekOrigin.Begin);
                    switch (block.Values[j].Type)
                    {
                        #region Check StringReference Values

                        case Sunfish.Developmental.Value.ValueType.StringId:
                            ValueTypes.StringId stringRef = br.ReadStringReference();
                            int index = Strings.BinarySearch(map.StringIdNames[stringRef.Index]);
                            if (index < 0)
                                Strings.Insert(~index, map.StringIdNames[stringRef.Index]);
                            if (stringRef.Index < 0 || stringRef.Index > map.StringIdNames.Count || map.StringIdNames[stringRef.Index].Length != stringRef.Length)
                            {
                                //if (OnValueError != null)
                                //{
                                //    OnValueError(this, new ValueErrorArgs()
                                //    {
                                //        Type = ValueErrorArgs.ErrorType.InvalidStringReference,
                                //        Block = block,
                                //        ValueIndex = j,
                                //        Offset = offset
                                //    });
                                //}
                            }
                            break;

                        #endregion

                        #region Check TagIndex Values

                        case Sunfish.Developmental.Value.ValueType.TagId:
                            ValueTypes.TagIndex tagIndex = br.ReadTagIndex();
                            if (tagIndex != -1 && !(tagIndex.Index < map.Index.TagInfoCount && tagIndex.Index > 0 && map.Index.TagEntries[tagIndex.Index].Index.Index == tagIndex))
                            {
                                //if (OnValueError != null)
                                //{
                                //    OnValueError(this, new ValueErrorArgs()
                                //    {
                                //        Type = ValueErrorArgs.ErrorType.InvalidTagIndex,
                                //        Block = block,
                                //        ValueIndex = j,
                                //        Offset = offset,
                                //        Value = tagIndex,
                                //    });
                                //}
                            }
                            break;

                        #endregion

                        #region Check TagReference Value

                        case Sunfish.Developmental.Value.ValueType.TagReference:
                            TagType tagType = br.ReadTagType();
                            tagIndex = br.ReadTagIndex();
                            if (tagIndex == -1)
                            {
                                if (tagType == TagType.Null)
                                { break; }
                                bool matched = false;
                                foreach (TagType tt in Sunfish.Index.Types)
                                {
                                    if (tt == tagType) matched = true;
                                }
                                if (!matched)
                                {
                                    //if (OnValueError != null)
                                    //{
                                    //    OnValueError(this, new ValueErrorArgs()
                                    //    {
                                    //        Type = ValueErrorArgs.ErrorType.InvalidTagType,
                                    //        Block = block,
                                    //        ValueIndex = j,
                                    //        Offset = offset,
                                    //        Value = tagType,
                                    //    });
                                    //}
                                }
                            }
                            else if (!(tagIndex.Index < map.Index.TagInfoCount && tagIndex.Index > 0 && map.Index.TagEntries[tagIndex.Index].Index.Index == tagIndex))
                            {
                                //if (OnValueError != null)
                                //{
                                //    OnValueError(this, new ValueErrorArgs()
                                //    {
                                //        Type = ValueErrorArgs.ErrorType.InvalidTagIndex,
                                //        Block = block,
                                //        ValueIndex = j,
                                //        Offset = offset,
                                //        Value = tagIndex,
                                //    });
                                //}
                            }
                            break;

                        #endregion
                    }
                }
            }
            #endregion

            #region Cache Raws

            //if (block.Raws.Length > 0)
            //{
            //    for (int i = 0; i < count; i++)
            //    {
            //        foreach (ResourceReference r in block.Raws)
            //        {
            //            ResourceReference raw = r;
            //            raw.Offset0 = blockAddress + (block.Size * i) + r.Offset0;
            //            raw.Offset1 = blockAddress + (block.Size * i) + r.Offset1;
            //        }
            //    }
            //}

            #endregion

            #region Process Nested Blocks

            if (block.NestedBlocks.Length > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    foreach (Block nestedBlock in block.NestedBlocks)
                    {
                        br.BaseStream.Position = (address - magic) + (i * block.Size) + nestedBlock.Offset;
                        int nestedBlockCount = br.ReadInt32();
                        if (nestedBlockCount == 0) continue;
                        int nestedBlockAddress = br.ReadInt32();
                        int offset = nestedBlockAddress - magic;
                        if (nestedBlockCount < 0)
                        {
                            //if (OnValueError != null)
                            //{
                            //    OnValueError(this, new ValueErrorArgs()
                            //    {
                            //        Type = ValueErrorArgs.ErrorType.InvalidArrayCount,
                            //        Offset = offset
                            //    });
                            //}
                        }
                        if (offset < 0 || offset > map.BaseStream.Length)
                        {
                            //if (OnValueError != null)
                            //{
                            //    OnValueError(this, new ValueErrorArgs()
                            //    {
                            //        Type = ValueErrorArgs.ErrorType.InvalidPointer,
                            //        Offset = offset
                            //    });
                            //}
                        }
                        else ProcessBlockRecursive(nestedBlock, map, nestedBlockAddress, nestedBlockCount, ref report);
                    }
                }
            }

            #endregion
        }
    }
}
