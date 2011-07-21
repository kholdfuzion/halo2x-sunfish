using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using Sunfish.Developmental;

namespace Sunfish
{
    public class Compiler
    {
        #region Stuff

        private Dictionary<string, int> filenames;
        private Cache<Pointer> pointerCache;
        private Cache<Value> valueCache;
        private Cache<ResourceReference> rawCache;
        private List<Tag> tags;
        private Map map;
        private BinaryReader binReader;
        private BinaryWriter binWriter;
        private Stream output;
        private int virtualOffset;

        #endregion

        public Compiler(Stream output)
        {
            this.output = output;
            Initialize();
        }

        public void SetTagsDirectory(string dir)
        {
            Directory.SetCurrentDirectory(dir);
        }

        public void CompileFromScenario(string scenario)
        {
            //Setup this Compiler to compile new map
            Setup();

            //Recursively Load Tags
            LoadTagsRecursively(@"globals\globals.matg");
            LoadTagsRecursively(Path.ChangeExtension(scenario, null));

            SortTags();

            ProcessTagInfoAndStringIds();

            ProcessUnicodeTags();

            CalculateTagIds();

            binWriter = new BinaryWriter(output);
            map.BaseStream = output;

            //reserve header space
            output.Position = Marshal.SizeOf(typeof(Header));

            //write sound raw
            ProcessTagRawValues(tags[tags.Count - 1], (int)output.Position);
            binWriter.Write(tags[tags.Count - 1].ResourceStream.ToArray());
            //write model raw
            WriteRawSection("mode");
            //write sbsp raw
            WriteRawSection("sbsp");
            //write ltmp raw
            WriteRawSection("ltmp");
            //write DECR raw
            WriteRawSection("DECR");
            //write weather raw
            WriteRawSection("weat");
            //write animation raw
            WriteRawSection("jmad");

            //calculate virtual offset
            map.Header.TotalCacheLength = map.Index.Length;
            map.Index.TagTypeArrayVirtualAddress = virtualOffset + 32;
            map.Index.TagInfoArrayVirtualAddress = virtualOffset + 32 + (map.Index.TagTypeCount * 12);
            virtualOffset += map.Index.Length;

            //write sbsp & ltmp meta
            WriteBSPBlock();

            //write stringid tables
            CreateStringIdTables();

            //write filename table
            CreateTagnameTable();

            //write unicode
            CreateUnicodeTables();   

            //write bitmap raw
            WriteRawSection("bitm");

            //reserve tagIndex space
            map.Header.IndexAddress = (int)output.Position;
            map.Header.IndexLength = map.Index.Length;
            output.Position += map.Index.Length;

            //write other tag meta
            WriteTagBlock();

            //write tagIndex
            output.Position = map.Header.IndexAddress;
            binWriter.Write(map.Index.ToArray());

            //write header
            map.Header.Checksum = CalculateChecksum();
            output.Position = 0;
            binWriter.Write(map.Header.ToByteArray());

            //commit write cache
            binWriter.Flush();
            binWriter.Close();
        }

        #region Private

        private void ProcessUnicodeTags()
        {
            int tagIndex = 0;
            foreach (Tag t in tags)
            {
                if (t.Type == "utf8")
                {
                    BinaryReader binReader = new BinaryReader(t.TagStream);
                    BinaryWriter binWriter = new BinaryWriter(t.TagStream);
                    t.TagStream.Position = 0;
                    int count = binReader.ReadInt32();
                    int address = binReader.ReadInt32();
                    int strLength = binReader.ReadInt32();
                    int strAddress = binReader.ReadInt32();
                    t.TagStream.Position = strAddress;
                    byte[] unicodeBytes = binReader.ReadBytes(strLength);
                    List<UnicodeTable.Entry> entries = new List<UnicodeTable.Entry>(count);
                    for (int i = 0; i < count; i++)
                    {
                        t.TagStream.Position = address + (i * 40);
                        int stringid = binReader.ReadInt32();
                        int offset = binReader.ReadInt32();
                        List<byte> strBytes = new List<byte>();
                        byte c;
                        while (true)
                        {
                            c = unicodeBytes[offset];
                            if (c == byte.MinValue) break;
                            strBytes.Add(c);
                            offset++;
                        }
                        entries.Add(new UnicodeTable.Entry()
                        {
                            StringReference = GetStringId(t.StringReferenceNames[stringid]),
                            Value = Encoding.UTF8.GetString(strBytes.ToArray())
                        });
                    }
                    int index = map.Unicode.Count;
                    //map.EnglishUnicode.Items.AddRange(entries);
                    t.TagStream.SetLength(52);
                    t.TagStream.Position = 0;
                    binWriter.Write(new byte[16]);
                    binWriter.Write((short)index);
                    binWriter.Write((short)count);
                    binWriter.Write(new byte[32]);
                    t.Type = "unic";
                    map.Index.TagEntries[tagIndex].Type = "unic";
                }
                tagIndex++;
            }
        }

        private void Initialize()
        {
            SetTagsDirectory(@"E:\Users\root\Documents\tags");
            map = new Map();
            tags = new List<Tag>(10000);
            filenames = new Dictionary<string, int>(10000);
            pointerCache = new Cache<Pointer>(10240);
            valueCache = new Cache<Value>(10240);
            rawCache = new Cache<ResourceReference>(10240);
        }

        private void WriteTagBlock()
        {
            int metaLength = (int)output.Position;

            int secondaryMagic = virtualOffset - (map.Header.IndexAddress + map.Header.IndexLength);

            for (int i = 0; i < tags.Count; i++)
            {
                if (!(map.Index.TagEntries[i].Type == "sbsp" || map.Index.TagEntries[i].Type == "ltmp"))
                {
                    map.Index.TagEntries[i].VirtualAddress = (int)output.Position + secondaryMagic;
                    map.Index.TagEntries[i].Length = (int)tags[i].TagStream.Length;
                    ProcessTagPointersAndValues(tags[i], (int)output.Position, secondaryMagic);
                    map.Header.TotalCacheLength += (int)tags[i].TagStream.Length;
                    binWriter.Write(tags[i].TagStream.ToArray());
                }
            }

            byte[] buf = Padding.GetBytes(output.Position, 512);
            binWriter.Write(buf);
            map.Header.TotalCacheLength += buf.Length;

            metaLength = (int)output.Position - metaLength;

            map.Header.TagCacheLength = metaLength;
        }

        private void Setup()
        {
            virtualOffset = -2147086336;
            filenames.Clear();
            tags.Clear();
        }

        private void CreateTagnameTable()
        {
            int[] startIndices = new int[map.Tagnames.Length];
            int start = 0;
            for (int i = 0; i < map.Tagnames.Length; i++)
            {
                startIndices[i] = start;
                start += Encoding.UTF8.GetByteCount(map.Tagnames[i]) + 1;
            }

            Pad();

            map.Header.PathIndexAddress = (int)output.Position;
            foreach (int i in startIndices)
                binWriter.Write(i);

            Pad();

            map.Header.PathTableAddress = (int)output.Position;
            foreach (string s in map.Tagnames)
            {
                binWriter.Write(Encoding.UTF8.GetBytes(s));
                binWriter.Write(Byte.MinValue);
            }
            map.Header.PathTableLength = (int)output.Position - map.Header.PathTableAddress - 1;
        }

        private void CreateStringIdTables()
        {
            Benchmark mark = new Benchmark();
            mark.Begin();

            Pad();

            map.Header.String128TableAddress = (int)output.Position;

            byte[] blank = new byte[128];
            for (int i = 0; i < map.StringIdNames.Count; i++)
            {
                byte[] buffer = Encoding.UTF8.GetBytes(map.StringIdNames[i]);
                binWriter.Write(buffer);
                binWriter.Write(blank, 0, blank.Length - buffer.Length);
            }

            Pad();

            map.Header.StringIndexAddress = (int)output.Position;
            map.Header.StringCount = map.StringIdNames.Count;

            int start = 0;
            for (int i = 0; i < map.StringIdNames.Count; i++)
            {
                binWriter.Write(start);
                start += Encoding.UTF8.GetByteCount(map.StringIdNames[i]) + 1;
            }

            Pad();

            map.Header.StringTableAddress = (int)output.Position;
            for (int i = 0; i < map.StringIdNames.Count; i++)
            {
                binWriter.Write(Encoding.UTF8.GetBytes(map.StringIdNames[i]));
                binWriter.Write((byte)0x00);
            }
            map.Header.StringTableLength = (int)(output.Position - map.Header.StringTableAddress - 1);

            mark.End();
        }

        private void CreateUnicodeTables()
        {
            Pad();

            int unicodeIndexAddress = (int)output.Position;
            int offset = 0;
            foreach (UnicodeTable.Entry item in map.Unicode[UnicodeTable.Language.English])
            {
                binWriter.Write(item.StringReference);
                binWriter.Write(offset);
                offset += Encoding.UTF8.GetByteCount(item.Value) + 1;
            }

            Pad();

            int unicodeTableAddress = (int)output.Position;
            int unicodeTableLength = unicodeTableAddress;
            foreach (UnicodeTable.Entry item in map.Unicode[UnicodeTable.Language.English])
            {
                binWriter.Write(Encoding.UTF8.GetBytes(item.Value));
                binWriter.Write((byte)0x00);
            }
            unicodeTableLength = (int)output.Position - unicodeTableAddress;

            //update Globals tag
            binWriter = new BinaryWriter(tags[0].TagStream);
            binWriter.BaseStream.Position = 400;

            binWriter.Write(map.Unicode.Count);
            binWriter.Write(unicodeTableLength);
            binWriter.Write(unicodeIndexAddress);
            binWriter.Write(unicodeTableAddress);
            binWriter.BaseStream.Position += 4;
            for (int i = 0; i < 7; i++)
            {
                binWriter.BaseStream.Position += 8;
                binWriter.Write(new byte[16]);
                binWriter.BaseStream.Position += 4;
            }

            binWriter = new BinaryWriter(output);
        }

        private void WriteBSPBlock()
        {
            Pad();

            int blockVirtualOffset = virtualOffset;
            int blockOffset = (int)output.Position;
            int blockSize = (int)output.Position;
            int sbspId = -1;
            int ltmpId = -1;
            int ltmpVirtualOffset = 0;

            int magic = blockVirtualOffset - blockOffset;

            for (int i = 0; i < tags.Count; i++)
                if (tags[i].Type == "sbsp")
                {
                    sbspId = map.Index.TagEntries[i].Index;
                    ProcessTagPointersAndValues(tags[i], (int)output.Position, magic);
                    binWriter.Write(tags[i].TagStream.ToArray());
                    break;
                }

            Pad();

            for (int i = 0; i < tags.Count; i++)
                if (tags[i].Type == "ltmp")
                {
                    ltmpVirtualOffset = (int)output.Position + magic;
                    ltmpId = map.Index.TagEntries[i].Index;
                    ProcessTagPointersAndValues(tags[i], (int)output.Position, magic);
                    binWriter.Write(tags[i].TagStream.ToArray());
                    break;
                }

            blockSize = (int)output.Position - blockSize;

            map.Header.TotalCacheLength += blockSize;

            virtualOffset += blockSize;

            output.Position = blockOffset;
            binWriter.Write(blockSize);
            binWriter.Write(blockVirtualOffset + 16);
            binWriter.Write(ltmpVirtualOffset);
            binWriter.Write(Encoding.UTF8.GetBytes("psbs"));

            output.Position = blockOffset + blockSize;

            binWriter = new BinaryWriter(tags[3].TagStream);
            binReader = new BinaryReader(tags[3].TagStream);
            tags[3].TagStream.Position = 532;
            int address = binReader.ReadInt32();
            tags[3].TagStream.Position = address;
            binWriter.Write(blockOffset);
            binWriter.Write(blockSize);
            binWriter.Write(blockVirtualOffset);

            binWriter = new BinaryWriter(output);
        }

        private void ProcessTagInfoAndStringIds()
        {
            map.Tagnames = new string[tags.Count];
            map.Index.TagEntries = new Index.TagInformation[tags.Count];
            List<string> stringIDNames = new List<string>(10000);
            for (int i = 0; i < tags.Count; i++)
            {
                map.Tagnames[i] = Path.ChangeExtension(tags[i].Filename, null);
                map.Index.TagEntries[i] = new Index.TagInformation() { Type = tags[i].Type };
                foreach (string s in tags[i].StringReferenceNames)
                {
                    int index = stringIDNames.BinarySearch(s);
                    if (index < 0) stringIDNames.Insert(~index, s);
                }
            }
            map.Index.TagInfoCount = map.Index.TagEntries.Length;
            map.Header.PathCount = map.Index.TagEntries.Length;
            map.StringIdNames = stringIDNames;
            map.Header.StringCount = stringIDNames.Count;
        }

        private void WriteRawSection(string type)
        {
            Pad();
            foreach (Tag t in tags)
                if (t.Type == type)
                {
                    ProcessTagRawValues(t, (int)output.Position);
                    binWriter.Write(t.ResourceStream.ToArray());
                }
        }

        private void ProcessTagRawValues(Tag tag, int rawOffset)
        {
            rawCache.Clear();
            binReader = new BinaryReader(tag.TagStream);
            binWriter = new BinaryWriter(tag.TagStream);
            Block block = Blocks.Types[tag.Type];
            ProcessBlockRaw(block, 1, 0, tag.TagStream);
            for (int i = 0; i < rawCache.Count; i++)
            {
                tag.TagStream.Position = rawCache.Values[i].Offset1;
                int index = binReader.ReadInt32();
                if ((index & 0xC0000000) == 0)
                {
                    tag.TagStream.Position = rawCache.Values[i].Offset0;
                    binWriter.Write(tag.ResourceInformation[index].Length);
                    tag.TagStream.Position = rawCache.Values[i].Offset1;
                    binWriter.Write((int)(tag.ResourceInformation[index].Address + rawOffset));
                }
            }
            binWriter = new BinaryWriter(output);
        }

        private void ProcessTagPointersAndValues(Tag tag, int offset, int magic)
            {
                valueCache.Clear();
                pointerCache.Clear();
            Block block = Blocks.Types[tag.Type];
            binReader = new BinaryReader(tag.TagStream);
            binWriter = new BinaryWriter(tag.TagStream);
            ProcessBlock(block, 1, 0, tag.TagStream);
            for (int i = 0; i < valueCache.Count; i++)
            {
                int index;
                switch (valueCache.Values[i].Type)
                {
                    case Value.ValueType.StringId:
                        tag.TagStream.Position = valueCache.Values[i].Offset;
                        index = binReader.ReadInt32();
                        tag.TagStream.Position -= 4;
                        binWriter.Write(GetStringId(tag.StringReferenceNames[index]));
                        break;
                    case Value.ValueType.TagId:
                        tag.TagStream.Position = valueCache.Values[i].Offset;
                        index = binReader.ReadInt32();
                        if (index == -1) continue;
                        tag.TagStream.Position -= 4;
                        binWriter.Write(GetTagId(tag.TagReferences[index]));
                        break;
                    case Value.ValueType.TagReference:
                        tag.TagStream.Position = valueCache.Values[i].Offset + 4;
                        index = binReader.ReadInt32();
                        if (index == -1) continue;
                        tag.TagStream.Position -= 8;
                        byte[] bytes = Encoding.UTF8.GetBytes(Globals.ReverseString(Index.GetDirtyType(Path.GetExtension(tag.TagReferences[index]).Substring(1))));
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
            binWriter = new BinaryWriter(output);
        }

        private void CalculateTagIds()
        {
            uint id = 0xE1740000;
            uint salt = 0x00010001;
            for (int i = 0; i < map.Header.PathCount; i++)
            {
                map.Index.TagEntries[i].Index = (int)id;
                id += salt;
            }
            map.Index.GlobalsTagId = map.Index.TagEntries[0].Index;
            map.Index.ScenarioTagId = map.Index.TagEntries[3].Index;
        }

        private int GetTagId(string tagReference)
        {
            int index = filenames[tagReference];
            return map.Index.TagEntries[index].Index;
        }

        private int GetStringId(string name)
        {
            int index = map.StringIdNames.BinarySearch(name);
            return index | map.StringIdNames[index].Length << 24; ;
        }

        private void ProcessBlockRaw(Block block, int count, int address, Stream stream)
        {
            #region Cache Raws

            for (int i = 0; i < count; i++)
            {
                foreach (ResourceReference r in block.Raws)
                {
                    ResourceReference raw = r;
                    raw.Offset0 = address + (block.Size * i) + r.Offset0;
                    raw.Offset1 = address + (block.Size * i) + r.Offset1;
                    rawCache.Add(raw);
                }
            }

            #endregion

            #region Process Nested Blocks

            if (block.NestedBlocks.Length > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    foreach (Block b in block.NestedBlocks)
                    {
                        binReader.BaseStream.Position = address + (i * block.Size) + b.Offset;
                        int nestedBlockCount = binReader.ReadInt32();
                        if (nestedBlockCount == 0) continue;
                        int nestedBlockAddress = binReader.ReadInt32();
                        ProcessBlockRaw(b, nestedBlockCount, nestedBlockAddress, stream);
                    }
                }
            }

            #endregion
        }

        private void ProcessBlock(Block block, int count, int address, Stream stream)
        {
            #region Cache Values

            if (block.Values.Length > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    foreach (Value v in block.Values)
                    {
                        Value val = v;
                        val.Offset = address + (block.Size * i) + v.Offset;
                        valueCache.Add(val);
                    }
                }
            }

            #endregion

            #region Process Nested Blocks

            if (block.NestedBlocks.Length > 0)
            {
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
            }

            #endregion
        }

        /// <summary>
        /// Writes bytes until output.Position is on a 512 byte boundary
        /// </summary>
        private void Pad()
        {
            byte[] paddingBytes = Padding.GetBytes(output.Position, 512);
            output.Write(paddingBytes, 0, paddingBytes.Length);
        }

        /// <summary>
        /// Xbox7887's Faster Resign Code, thanks ;)!
        /// </summary>
        /// <param name="stream"></param>
        unsafe private uint CalculateChecksum()
        {
            // initial declarations
            uint checksum = 0;
            int blockSize = 0x10000;    // 64kb
            byte[] block = new byte[blockSize];
            int hashSize = (int)(output.Length - 2048);
            int blockCount = hashSize / blockSize;
            int blockPasses = blockSize / 16;
            int remainder = hashSize % blockSize;
            int remainingPasses = remainder / 16;
            output.Position = 2048;

            fixed (byte* buf = &block[0])
            {
                // loop through the map hashing one full block at a time
                uint* p;
                for (int i = 0; i < blockCount; i++)
                {
                    p = (uint*)buf;
                    output.Read(block, 0, blockSize);
                    for (int j = 0; j < blockPasses; j++)
                    {
                        checksum ^= p[0];
                        checksum ^= p[1];
                        checksum ^= p[2];
                        checksum ^= p[3];
                        p += 4;
                    }
                }

                // hash the remainder
                p = (uint*)buf;
                output.Read(block, 0, remainder);
                for (int j = 0; j < remainingPasses; j++)
                {
                    checksum ^= p[0];
                    checksum ^= p[1];
                    checksum ^= p[2];
                    checksum ^= p[3];
                    p += 4;
                }

                // save the final signature
                return checksum;
            }
        }

        /// <summary>
        /// Sorts tags such that the Scenario is at [3], and the SoundGlobals is at [n - 1] where n is the number of tags
        /// </summary>
        private void SortTags()
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

            filenames.Clear();
            foreach (Tag t in tags)
                filenames.Add(Path.ChangeExtension(t.Filename, null), filenames.Count);
        }

        /// <summary>
        /// Loads tag from filename and then loads all referenced tags tag references
        /// </summary>
        /// <param name="filename">filename in format of dir/filename.class, where class is CLEAN. Do NOT include file extension .h2tag</param>
        private void LoadTagsRecursively(string filename)
        {
            if (!filenames.ContainsKey(filename))
            {
                filenames.Add(filename, filenames.Count);
                Tag tag = new Tag(filename + Tag.Path.Extension);
                this.tags.Add(tag);
                foreach (string s in tag.TagReferences)
                    LoadTagsRecursively(s);
            }
        }

        #endregion
    }
}