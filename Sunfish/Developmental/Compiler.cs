using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sunfish
{
    public class Compiler
    {
        Dictionary<string, int> loadedTagnames;
        int[] stringIds;
        Cache<Pointer> pointerCache;
        Cache<Value> valueCache;
        Cache<Raw> rawCache;
        List<Tag> tags;
        Map map;
        BinaryReader binReader;
        BinaryWriter binWriter;
        FileStream file;

        int virtualOffset;

        public Compiler()
        {
            Initialize();
        }

        void Initialize()
        {
            Directory.SetCurrentDirectory(@"E:\Users\root\Documents\tags");
            map = new Map();
            tags = new List<Tag>(10000);
            loadedTagnames = new Dictionary<string, int>(10000);
            pointerCache = new Cache<Pointer>(10240);
            valueCache = new Cache<Value>(10240);
            rawCache = new Cache<Raw>(10240);
        }

        public void Compile(string scenario)
        {
            StaticBenchmark.Begin();

            virtualOffset = -2147086336;
            loadedTagnames.Clear();
            tags.Clear();

            LoadTag(@"globals\globals.matg");
            LoadTag(scenario);

            SortTags();

            GenerateTagStuffs();

            CalculateTagIds();

            file = new FileStream(@"E:\Users\root\Documents\Halo 2 Modding\Working Maps\headlong_rebuild_01.map", FileMode.Create);
            binWriter = new BinaryWriter(file);
            map.BaseStream = file;

            //reserve header space
            file.Position = Marshal.SizeOf(typeof(Map.HeaderStruct));

            //write sound raw
            ProcessRaw(tags[tags.Count - 1], (int)file.Position);
            binWriter.Write(tags[tags.Count - 1].RawStream.ToArray());
            //write model raw
            WriteRawTable("mode");
            //write sbsp raw
            WriteRawTable("sbsp");
            //write ltmp raw
            WriteRawTable("ltmp");
            //write DECR raw
            WriteRawTable("DECR");
            //write weather raw
            WriteRawTable("weat");
            //write animation raw
            WriteRawTable("jmad");


            //1. writer sbsp & ltmp meta
            virtualOffset += 32;
            map.Index.TagTypeArrayVirtualAddress = virtualOffset;
            virtualOffset += map.Index.TagTypeCount * 12;
            map.Index.TagInfoArrayVirtualAddress = virtualOffset;
            virtualOffset += map.Index.TagInfoCount * 16;

            WriteMapStructureMetaBlock();

            WriteStringIdTables();

            WriteTagnameTable();


            //1. write unicode///////////////////////////


            //write bitmap raw
            WriteRawTable("bitm");

            map.Header.IndexAddress = (int)file.Position;

            file.Position += map.Index.Length;

            int metaLength = (int)file.Position;

            int secondaryMagic = virtualOffset - (int)file.Position;

            for (int i = 0; i < tags.Count; i++)
            {
                if (map.Index.TagEntries[i].Type == "sbsp" || map.Index.TagEntries[i].Type == "ltmp") continue;
                map.Index.TagEntries[i].VirtualAddress = (int)file.Position + secondaryMagic;
                ProcessPointersAndValues(tags[i], (int)file.Position, secondaryMagic);
                binWriter.Write(tags[i].TagStream.ToArray());
            }

            Pad(file);

            metaLength = (int)file.Position - metaLength;

            file.Position = map.Header.IndexAddress;
            binWriter.Write(map.Index.ToArray());

            map.Header.IndexLength = map.Index.Length;

            file.Position = 0;
            binWriter.Write(Map.HeaderStruct.RawSerialize(map.Header));

            binWriter.Flush();
            binWriter.Close();

            StaticBenchmark.End();

            System.Windows.Forms.MessageBox.Show(String.Format("Rebuilding Done\nTime: {0}", StaticBenchmark.Result));
        }

        void WriteTagnameTable()
        {
            int[] startIndices = new int[map.Tagnames.Length];
            int start = 0;
            for (int i = 0; i < map.Tagnames.Length; i++)
            {
                startIndices[i] = start;
                start += Encoding.UTF8.GetByteCount(map.Tagnames[i]) + 1;
            }

            Pad(file);

            map.Header.FilenameIndexOffset = (int)file.Position;
            foreach (int i in startIndices)
                binWriter.Write(i);

            Pad(file);

            map.Header.FilenameTableAddress = (int)file.Position;
            foreach (string s in map.Tagnames)
            {
                binWriter.Write(Encoding.UTF8.GetBytes(s));
                binWriter.Write(Byte.MinValue);
            }
            map.Header.FilenameTableLength = (int)file.Position - map.Header.FilenameTableAddress - 1;
        }

        void WriteStringIdTables()
        {
            Benchmark mark = new Benchmark();
            mark.Begin();

            Pad(file);

            map.Header.StringId128TableAddress = (int)file.Position;

            byte[] blank = new byte[128];
            for (int i = 0; i < map.StringIdNames.Count; i++)
            {
                byte[] buffer = Encoding.UTF8.GetBytes(map.StringIdNames[i]);
                binWriter.Write(buffer);
                binWriter.Write(blank, 0, blank.Length - buffer.Length);
            }

            Pad(file);

            map.Header.StringIdIndexAddress = (int)file.Position;
            map.Header.StringIdCount = map.StringIdNames.Count;

            int start = 0;
            for (int i = 0; i < map.StringIdNames.Count; i++)
            {
                binWriter.Write(start);
                start += Encoding.UTF8.GetByteCount(map.StringIdNames[i]) + 1;
            }

            Pad(file);

            map.Header.StringIdTableAddress = (int)file.Position;
            for (int i = 0; i < map.StringIdNames.Count; i++)
            {
                binWriter.Write(Encoding.UTF8.GetBytes(map.StringIdNames[i]));
                binWriter.Write((byte)0x00);
            }
            map.Header.StringIdTableLength = (int)(file.Position - map.Header.StringIdTableAddress - 1);

            mark.End();
        }

        void WriteMapStructureMetaBlock()
        {
            Pad(file);

            int blockVirtualOffset = virtualOffset;
            int blockOffset = (int)file.Position;
            int blockSize = (int)file.Position;
            int sbspId = -1;
            int ltmpId = -1;
            int ltmpVirtualOffset = 0;

            int magic = blockVirtualOffset - blockOffset;

            for (int i = 0; i < tags.Count; i++)
                if (tags[i].Type == "sbsp")
                {
                    sbspId = map.Index.TagEntries[i].Id;
                    ProcessPointersAndValues(tags[i], (int)file.Position, magic);
                    binWriter.Write(tags[i].TagStream.ToArray());
                    break;
                }
            for (int i = 0; i < tags.Count; i++)
                if (tags[i].Type == "ltmp")
                {
                    ltmpVirtualOffset = (int)file.Position + magic;
                    ltmpId = map.Index.TagEntries[i].Id;
                    ProcessPointersAndValues(tags[i], (int)file.Position, magic);
                    binWriter.Write(tags[i].TagStream.ToArray());
                    break;
                }

            blockSize = (int)file.Position - blockSize;

            virtualOffset += blockSize;

            file.Position = blockOffset;
            binWriter.Write(blockSize);
            binWriter.Write(blockVirtualOffset + 16);
            binWriter.Write(ltmpVirtualOffset);
            binWriter.Write(Encoding.UTF8.GetBytes("psbs"));

            file.Position = blockOffset + blockSize;

            binWriter = new BinaryWriter(tags[3].TagStream);
            binReader = new BinaryReader(tags[3].TagStream);
            tags[3].TagStream.Position = 532;
            int address = binReader.ReadInt32();
            tags[3].TagStream.Position = address;
            binWriter.Write(blockOffset);
            binWriter.Write(blockSize);
            binWriter.Write(blockVirtualOffset);

            binWriter = new BinaryWriter(file);
        }

        void GenerateTagStuffs()
        {
            map.Tagnames = new string[tags.Count];
            map.Index.TagEntries = new Map.TagIndex.TagInfo[tags.Count];
            List<string> stringIDNames = new List<string>(10000);
            for (int i = 0; i < tags.Count; i++)
            {
                map.Tagnames[i] = Path.ChangeExtension(tags[i].Filename, null);
                map.Index.TagEntries[i] = new Map.TagIndex.TagInfo() { Type = tags[i].Type, Length = (int)tags[i].TagStream.Length };
                foreach (string s in tags[i].StringIdNames)
                {
                    int index = stringIDNames.BinarySearch(s);
                    if (index < 0) stringIDNames.Insert(~index, s);
                }
            }
            map.Index.TagInfoCount = map.Index.TagEntries.Length;
            map.Header.TagCount = map.Index.TagEntries.Length;
            map.StringIdNames = stringIDNames;
            map.Header.StringIdCount = stringIDNames.Count;
            CalculateStringIds();
        }

        void WriteRawTable(string type)
        {
            foreach (Tag t in tags)
                if (t.Type == type)
                {
                    ProcessRaw(t, (int)file.Position);
                    binWriter.Write(t.RawStream.ToArray());
                }
        }

        void ProcessRaw(Tag tag, int rawOffset)
        {
            rawCache.Clear();
            valueCache.Clear();
            pointerCache.Clear();
            Block block = Blocks.Types[tag.Type];
            binReader = new BinaryReader(tag.TagStream);
            binWriter = new BinaryWriter(tag.TagStream);
            ProcessBlock(block, 1, 0, tag.TagStream);
            for (int i = 0; i < rawCache.Count; i++)
            {
                tag.TagStream.Position = rawCache.Values[i].Offset1;
                int index = binReader.ReadInt32();
                if ((index & 0xC0000000) == 0)
                {
                    tag.TagStream.Position = rawCache.Values[i].Offset0;
                    binWriter.Write(tag.RawOffsets[index].Length);
                    tag.TagStream.Position = rawCache.Values[i].Offset1;
                    binWriter.Write((int)(tag.RawOffsets[index].Address + rawOffset));
                }
            }
            binWriter = new BinaryWriter(file);
        }

        void ProcessPointersAndValues(Tag tag, int offset, int magic)
        {
            valueCache.Clear();
            pointerCache.Clear();
            rawCache.Clear();
            Block block = Blocks.Types[tag.Type];
            binReader = new BinaryReader(tag.TagStream);
            binWriter = new BinaryWriter(tag.TagStream);
            ProcessBlock(block, 1, 0, tag.TagStream);
            for (int i = 0; i < valueCache.Count; i++)
            {
                int index;
                switch (valueCache.Values[i].Type)
                {
                    case ValueType.StringId:
                        tag.TagStream.Position = valueCache.Values[i].Offset;
                        index = binReader.ReadInt32();
                        tag.TagStream.Position -= 4;
                        binWriter.Write(GetStringId(tag.StringIdNames[index]));
                        break;
                    case ValueType.TagId:
                        tag.TagStream.Position = valueCache.Values[i].Offset;
                        index = binReader.ReadInt32();
                        if (index == -1) continue;
                        tag.TagStream.Position -= 4;
                        binWriter.Write(GetTagId(tag.TagReferences[index]));
                        break;
                    case ValueType.TagReference:
                        tag.TagStream.Position = valueCache.Values[i].Offset + 4;
                        index = binReader.ReadInt32();
                        if (index == -1) continue;
                        tag.TagStream.Position -= 8;
                        byte[] bytes = Encoding.UTF8.GetBytes(Globals.GetDirtyType(Path.GetExtension(tag.TagReferences[index]).Substring(1)));
                        binWriter.Write(bytes, 0, 4);
                        binWriter.Write(GetTagId(tag.TagReferences[index]));
                        break;
                }
            }
            for (int i = 0; i < pointerCache.Count; i++)
            {
                tag.TagStream.Position = pointerCache.Values[i].Offset;
                binWriter.Write(pointerCache.Values[i].Address + offset + magic);
            }
            binWriter = new BinaryWriter(file);
        }

        void CalculateTagIds()
        {
            uint id = 0xE1740000;
            uint salt = 0x00010001;
            for (int i = 0; i < map.Header.TagCount; i++)
            {
                map.Index.TagEntries[i].Id = (int)id;
                id += salt;
            }
            map.Index.GlobalsTagId = map.Index.TagEntries[0].Id;
            map.Index.ScenarioTagId = map.Index.TagEntries[3].Id;
        }

        int GetTagId(string tagReference)
        {
            int index = loadedTagnames[tagReference];
            return map.Index.TagEntries[index].Id;
        }

        void CalculateStringIds()
        {
            stringIds = new int[map.StringIdNames.Count];
            for (int i = 0; i < stringIds.Length; i++)
            {
                int index = map.StringIdNames.BinarySearch(map.StringIdNames[i]);
                stringIds[i] = index | map.StringIdNames[i].Length << 24;
            }
        }

        int GetStringId(string name)
        {
            int index = map.StringIdNames.BinarySearch(name);
            return stringIds[index];
        }

        void ProcessBlock(Block block, int count, int address, Stream stream)
        {
            #region Cache Values

            for (int i = 0; i < count; i++)
            {
                foreach (Value v in block.Values)
                {
                    Value val = v;
                    val.Offset = address + (block.Size * i) + v.Offset;
                    valueCache.Add(val);
                }
            }

            #endregion

            #region Cache Raws

            for (int i = 0; i < count; i++)
            {
                foreach (Raw r in block.Raws)
                {
                    Raw raw = r;
                    raw.Offset0 = address + (block.Size * i) + r.Offset0;
                    raw.Offset1 = address + (block.Size * i) + r.Offset1;
                    rawCache.Add(raw);
                }
            }

            #endregion

            #region Process Nested Blocks

            for (int i = 0; i < count; i++)
            {
                foreach (Block b in block.NestedBlocks)
                {
                    binReader.BaseStream.Position = address + (i * block.Size) + b.Offset;
                    int nestedBlockCount = binReader.ReadInt32();
                    if (nestedBlockCount == 0) continue;
                    int nestedBlockAddress = binReader.ReadInt32();
                    pointerCache.Add(new Pointer() { Offset = address + (i * block.Size) + b.Offset + 4, Address = nestedBlockAddress });
                    ProcessBlock(b, nestedBlockCount, nestedBlockAddress, stream);
                }
            }

            #endregion
        }

        void Pad(Stream stream)
        {
            stream.Position += Globals.GetPaddingByteCount(stream.Position, 512);
        }

        void SortTags()
        {
            Tag scenarioTag = null;
            Tag soundGlobalsTag = null;
            string scenarioFilename = string.Empty;
            string soundFilename = string.Empty;
            bool f1 = false;
            bool f2 = false;
            for (int i = 0; i < tags.Count; i++)
            {
                if (tags[i].Type == "scnr")
                {
                    scenarioTag = tags[i];
                    tags.RemoveAt(i);
                    f1 = true;
                }
                else if (tags[i].Type == "ugh!")
                {
                    soundGlobalsTag = tags[i];
                    tags.RemoveAt(i);
                    f2 = true;
                }
                if (f1 && f2) break;
            }
            tags.Insert(3, scenarioTag);
            tags.Add(soundGlobalsTag);

            loadedTagnames.Clear();
            foreach (Tag t in tags)
                loadedTagnames.Add(t.Filename, loadedTagnames.Count);
        }

        void LoadTag(string filename)
        {
            if (!loadedTagnames.ContainsKey(filename)) loadedTagnames.Add(filename, loadedTagnames.Count);
            else return;
            filename += ".h2tag";
            Tag tag = new Tag(filename);
            this.tags.Add(tag);
            foreach (string s in tag.TagReferences)
                LoadTag(s);
        }
    }
}
