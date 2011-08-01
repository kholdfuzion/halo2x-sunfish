using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Sunfish.Developmental;
using Sunfish.ValueTypes;

namespace Sunfish
{
    public class ProjectCompiler
    {
        Header Header;
        RawCache SoundCache;
        RawCache ModelCache;
        RawCache SbspCache;
        RawCache LtmpCache;
        RawCache DECRCache;
        RawCache WeatherCache;
        RawCache AnimationCache;
        StructureMetaCache SbspLtmpMetaCache;
        StringsCache StringIDsCache;
        TagnameCache TagnameCache;
        UnicodeCache EnglishUnicodeCache;
        RawCache BitmapCache;
        IndexCache IndexCache;
        MetaCache MetaCache;

        List<CompilerTag> Tags;
        Cache<Pointer> pointerCache;
        Cache<Value> valueCache;
        Cache<ResourceReference> rawCache;

        public ProjectCompiler()
        {
            SoundCache = new RawCache();
            ModelCache = new RawCache();
            SbspCache = new RawCache();
            LtmpCache = new RawCache();
            DECRCache = new RawCache();
            WeatherCache = new RawCache();
            AnimationCache = new RawCache();
            BitmapCache = new RawCache();
            TagnameCache = new TagnameCache();
            IndexCache = new IndexCache();
            MetaCache = new MetaCache();
            SbspLtmpMetaCache = new StructureMetaCache();
            EnglishUnicodeCache = new UnicodeCache();
            StringIDsCache = new StringsCache();
            valueCache = new Cache<Value>(10000);
            pointerCache = new Cache<Pointer>(50000);
            rawCache = new Cache<ResourceReference>(10000);
        }

        public void Compile(Project project, bool rebuildcaches)
        {
            List<string> filenames = new List<string>(Directory.GetFiles(project.SourceDirectory, String.Format("*{0}", Sunfish.Tag.Path.Extension), SearchOption.AllDirectories));
            project.SourceFiles = new List<string>(filenames);
            //for(int i=0;i<project.SourceFiles.Count;i++)
            //    project.SourceFiles[i] = project.SourceFiles[i].Substring(project.SourceDirectory.Length);
            project.SortSourceFiles();
            Benchmark mark = new Benchmark();
            mark.Begin();
            if (!rebuildcaches && CheckCacheStatus(project)) { LoadCaches(project); }
            else CreateCaches(project);
            CreateMap(project);
            mark.End();
            System.Windows.Forms.MessageBox.Show(String.Format("finished in: {0}", mark.Result), "Solution Compiled");
        }

        public void Compile(Project project)
        {
            Compile(project, false);
        }

        private void LoadCaches(Project project)
        {
            SoundCache.Load(Path.Combine(project.BinDirectory, "SOUND_RAW_CACHE.BIN"));
            ModelCache.Load(Path.Combine(project.BinDirectory, "MODEL_RAW_CACHE.BIN"));
            SbspCache.Load(Path.Combine(project.BinDirectory, "STRUCTURE_RAW_CACHE.BIN"));
            LtmpCache.Load(Path.Combine(project.BinDirectory, "LIGHTMAP_RAW_CACHE.BIN"));
            DECRCache.Load(Path.Combine(project.BinDirectory, "DECORATER_RAW_CACHE.BIN"));
            WeatherCache.Load(Path.Combine(project.BinDirectory, "WEATHER_RAW_CACHE.BIN"));
            AnimationCache.Load(Path.Combine(project.BinDirectory, "ANIMATION_RAW_CACHE.BIN"));
            BitmapCache.Load(Path.Combine(project.BinDirectory, "TEXTURE_RAW_CACHE.BIN"));
            StringIDsCache.Load(Path.Combine(project.BinDirectory, "STRINGS_CACHE.BIN"));
            IndexCache.Load(Path.Combine(project.BinDirectory, "INDEX.BIN"));
            TagnameCache.Load(Path.Combine(project.BinDirectory, "TAGNAMES.BIN"));
            EnglishUnicodeCache.Load(Path.Combine(project.BinDirectory, "ENGLISH_UNICODE.BIN"));
            MetaCache.Load(Path.Combine(project.BinDirectory, "META.BIN"));
            SbspLtmpMetaCache.Load(Path.Combine(project.BinDirectory, "STRUCTURE_LIGHTMAP_META.BIN"));
            Header = new Header(File.OpenRead(Path.Combine(project.BinDirectory, "HEADER.BIN")));
        }

        private bool CheckCacheStatus(Project project)
        {
            return (File.Exists(Path.Combine(project.BinDirectory, "SOUND_RAW_CACHE.BIN")) && File.GetLastWriteTime(Path.Combine(project.BinDirectory, "SOUND_RAW_CACHE.BIN")) == project.CacheCreationDate
               && File.Exists(Path.Combine(project.BinDirectory, "MODEL_RAW_CACHE.BIN")) && File.GetLastWriteTime(Path.Combine(project.BinDirectory, "MODEL_RAW_CACHE.BIN")) == project.CacheCreationDate
               && File.Exists(Path.Combine(project.BinDirectory, "STRUCTURE_RAW_CACHE.BIN")) && File.GetLastWriteTime(Path.Combine(project.BinDirectory, "STRUCTURE_RAW_CACHE.BIN")) == project.CacheCreationDate
               && File.Exists(Path.Combine(project.BinDirectory, "LIGHTMAP_RAW_CACHE.BIN")) && File.GetLastWriteTime(Path.Combine(project.BinDirectory, "LIGHTMAP_RAW_CACHE.BIN")) == project.CacheCreationDate
               && File.Exists(Path.Combine(project.BinDirectory, "DECORATER_RAW_CACHE.BIN")) && File.GetLastWriteTime(Path.Combine(project.BinDirectory, "DECORATER_RAW_CACHE.BIN")) == project.CacheCreationDate
               && File.Exists(Path.Combine(project.BinDirectory, "WEATHER_RAW_CACHE.BIN")) && File.GetLastWriteTime(Path.Combine(project.BinDirectory, "WEATHER_RAW_CACHE.BIN")) == project.CacheCreationDate
               && File.Exists(Path.Combine(project.BinDirectory, "ANIMATION_RAW_CACHE.BIN")) && File.GetLastWriteTime(Path.Combine(project.BinDirectory, "ANIMATION_RAW_CACHE.BIN")) == project.CacheCreationDate
               && File.Exists(Path.Combine(project.BinDirectory, "TEXTURE_RAW_CACHE.BIN")) && File.GetLastWriteTime(Path.Combine(project.BinDirectory, "TEXTURE_RAW_CACHE.BIN")) == project.CacheCreationDate
               && File.Exists(Path.Combine(project.BinDirectory, "STRINGS_CACHE.BIN")) && File.GetLastWriteTime(Path.Combine(project.BinDirectory, "STRINGS_CACHE.BIN")) == project.CacheCreationDate
               && File.Exists(Path.Combine(project.BinDirectory, "INDEX.BIN")) && File.GetLastWriteTime(Path.Combine(project.BinDirectory, "INDEX.BIN")) == project.CacheCreationDate
               && File.Exists(Path.Combine(project.BinDirectory, "TAGNAMES.BIN")) && File.GetLastWriteTime(Path.Combine(project.BinDirectory, "TAGNAMES.BIN")) == project.CacheCreationDate
               && File.Exists(Path.Combine(project.BinDirectory, "ENGLISH_UNICODE.BIN")) && File.GetLastWriteTime(Path.Combine(project.BinDirectory, "ENGLISH_UNICODE.BIN")) == project.CacheCreationDate
               && File.Exists(Path.Combine(project.BinDirectory, "META.BIN")) && File.GetLastWriteTime(Path.Combine(project.BinDirectory, "META.BIN")) == project.CacheCreationDate
               && File.Exists(Path.Combine(project.BinDirectory, "STRUCTURE_LIGHTMAP_META.BIN")) && File.GetLastWriteTime(Path.Combine(project.BinDirectory, "STRUCTURE_LIGHTMAP_META.BIN")) == project.CacheCreationDate
               && File.Exists(Path.Combine(project.BinDirectory, "HEADER.BIN")) && File.GetLastWriteTime(Path.Combine(project.BinDirectory, "HEADER.BIN")) == project.CacheCreationDate);
        }

        public void CreateCaches(Project project)
        {
            #region STAGE ONE - Load Data To Caches

            #region Load Tags

            Tags = new List<CompilerTag>(project.SourceFiles.Count);
            Directory.SetCurrentDirectory(project.SourceDirectory);
            foreach (string filename in project.SourceFiles)
                Tags.Add(new CompilerTag(filename));

            #endregion

            Header = new Header() { Type = Header.MapType.Multiplayer, Name = project.Name, Scenario = project.Scenario };

            foreach (CompilerTag tag in Tags)
            { 
                #region Cache StringReference Values

                StringIDsCache.CacheData(tag);

                #endregion

                #region Process Unicode Tags

                if (tag.Type == "utf8")
                    EnglishUnicodeCache.CacheData(tag, StringIDsCache);

                #endregion

                #region Initialize Tag in Filetable and Index

                TagnameCache.CacheData(tag);
                IndexCache.CacheData(tag);

                #endregion

                #region Cache Resource Data

                switch (tag.Type)
                {
                    case "ugh!":
                        SoundCache.CacheData(tag);
                        break;
                    case "mode":
                        ModelCache.CacheData(tag);
                        break;
                    case "sbsp":
                        SbspCache.CacheData(tag);
                        break;
                    case "ltmp":
                        LtmpCache.CacheData(tag);
                        break;
                    case "DECR":
                        DECRCache.CacheData(tag);
                        break;
                    case "weat":
                        WeatherCache.CacheData(tag);
                        break;
                    case "jmad":
                        AnimationCache.CacheData(tag);
                        break;
                    case "bitm":
                        BitmapCache.CacheData(tag);
                        break;
                }

                #endregion

                #region Cache Tag Data

                switch (tag.Type)
                {
                    case "sbsp":
                        SbspLtmpMetaCache.CacheData(tag);
                        break;
                    case "ltmp":
                        SbspLtmpMetaCache.CacheData(tag);
                        break;
                    default:
                        MetaCache.CacheData(tag);
                        break;

                }
                #endregion
            }

            #endregion

            #region STAGE TWO - Setup And Sort Caches
            
            SoundCache.Offset = 2048;
            ModelCache.Offset = SoundCache.NextOffset;
            SbspCache.Offset = ModelCache.NextOffset;
            LtmpCache.Offset = SbspCache.NextOffset;
            DECRCache.Offset = LtmpCache.NextOffset;
            WeatherCache.Offset = DECRCache.NextOffset;
            AnimationCache.Offset = WeatherCache.NextOffset;
            SbspLtmpMetaCache.Offset = AnimationCache.NextOffset;

            #region Update Scenario Tag

            SbspLtmpMetaCache.SetVirtualOffset(IndexCache.NextVirtualOffset);
            SbspLtmpMetaCache.UpdateScenario(Tags[3]);
            MetaCache[3] = Tags[3].TagStream.ToArray();

            #endregion

            StringIDsCache.Offset = SbspLtmpMetaCache.NextOffset;
            TagnameCache.Offset = StringIDsCache.NextOffset;
            EnglishUnicodeCache.Offset = TagnameCache.NextOffset;
            BitmapCache.Offset = EnglishUnicodeCache.NextOffset;
            IndexCache.Offset = BitmapCache.NextOffset;
            MetaCache.Offset = IndexCache.NextOffset;

            #region Update Globals with Unicode information

            BinaryWriter bw = new BinaryWriter(Tags[0].TagStream);
            Tags[0].TagStream.Seek(400, SeekOrigin.Begin);
            bw.Write(EnglishUnicodeCache.Values.Count);
            bw.Write(EnglishUnicodeCache.UnicodeTableLength);
            bw.Write(EnglishUnicodeCache.UnicodeIndexOffset);
            bw.Write(EnglishUnicodeCache.UnicodeTableOffset);
            Tags[0].TagStream.Seek(4, SeekOrigin.Current);
            for (int i = 0; i < 8; i++)
            {
                Tags[0].TagStream.Seek(8, SeekOrigin.Current);
                bw.Write(new byte[8]);
                bw.Write(BitmapCache.Offset);
                bw.Write(BitmapCache.Offset);
                Tags[0].TagStream.Seek(4, SeekOrigin.Current);
            }
            MetaCache[0] = Tags[0].TagStream.ToArray();

            #endregion

            #region Set Virtual Addresses

            MetaCache.VirtualOffset = SbspLtmpMetaCache.NextVirtualOffset;

            #endregion

            #region Update IndexCache

            SbspLtmpMetaCache.UpdateIndexCache(IndexCache);
            MetaCache.UpdateIndexCache(IndexCache);

            #endregion

            #endregion

            #region STAGE THREE - Update Meta Values

            foreach (CompilerTag tag in Tags)
            {
                BinaryReader br;
                MetaCache Cache = null;

                if (SbspLtmpMetaCache.TagIndexers.Contains(tag.ID))
                    Cache = SbspLtmpMetaCache;
                else if (MetaCache.TagIndexers.Contains(tag.ID))
                    Cache = MetaCache;
                else throw new Exception();

                br = new BinaryReader(Cache.Stream);
                bw = new BinaryWriter(Cache.Stream);

                int Index = Cache.TagIndexers.IndexOf(tag.ID);

                RawCache ResourceCache = null;
                switch (tag.Type)
                {
                    case "ugh!":
                        ResourceCache = SoundCache;
                        break;
                    case "mode":
                        ResourceCache = ModelCache;
                        break;
                    case "sbsp":
                        ResourceCache = SbspCache;
                        break;
                    case "ltmp":
                        ResourceCache = LtmpCache;
                        break;
                    case "DECR":
                        ResourceCache = DECRCache;
                        break;
                    case "weat":
                        ResourceCache = WeatherCache;
                        break;
                    case "jmad":
                        ResourceCache = AnimationCache;
                        break;
                    case "bitm":
                        ResourceCache = BitmapCache;
                        break;
                }

                #region Update Pointers and Other Values

                valueCache.Clear();
                pointerCache.Clear();
                Block block = Blocks.Types[tag.Type];
                ProcessBlock(block, 1, 0, tag.TagStream);

                for (int i = 0; i < pointerCache.Count; i++)
                {
                    Cache.Stream.Position = Cache.Entries[Index].Offset + pointerCache.Values[i].Offset;
                    int address = br.ReadInt32();
                    if (address != -1)
                    {
                        Cache.Stream.Seek(-4, SeekOrigin.Current);
                        bw.Write(IndexCache.Entries[tag.ID].Address + address);
                    }
                }

                for (int i = 0; i < valueCache.Count; i++)
                {
                    switch (valueCache.Values[i].Type)
                    {
                        case Value.ValueType.StringId:
                            Cache.Stream.Position = Cache.Entries[Index].Offset + valueCache.Values[i].Offset;
                            int index = br.ReadInt32();
                            Cache.Stream.Seek(-4, SeekOrigin.Current);
                            StringId strRef = new StringId((short)StringIDsCache.Values.IndexOf(tag.StringReferenceNames[index]), (sbyte)Encoding.UTF8.GetByteCount(tag.StringReferenceNames[index]));
                            bw.Write(strRef);
                            break;
                            TagIndex tagIndex;
                        case Value.ValueType.TagId:
                            Cache.Stream.Position = Cache.Entries[Index].Offset + valueCache.Values[i].Offset;
                            index = br.ReadInt32();
                            if (index == -1) continue;
                            Cache.Stream.Seek(-4, SeekOrigin.Current);
                            tagIndex = IndexCache.Entries[TagnameCache.Tagnames.IndexOf(tag.TagReferences[index])].Index;
                            bw.Write(tagIndex);
                            break;
                        case Value.ValueType.TagReference:
                            Cache.Stream.Position = Cache.Entries[Index].Offset + valueCache.Values[i].Offset + 4;
                            index = br.ReadInt32();
                            if (index == -1) continue;
                            Cache.Stream.Seek(-8, SeekOrigin.Current);
                            byte[] bytes = Encoding.UTF8.GetBytes(Globals.ReverseString(Sunfish.Index.GetDirtyType(Tag.Path.GetTagType(tag.TagReferences[index]))));
                            bw.Write(bytes, 0, 4);
                            tagIndex = IndexCache.Entries[TagnameCache.Tagnames.IndexOf(tag.TagReferences[index])].Index;
                            bw.Write(tagIndex);
                            break;
                    }
                }

                #endregion

                #region Update Resource Values

                if (tag.ResourceInformation.Length > 0)
                {
                    rawCache.Clear();
                    ProcessBlockRaw(block, 1, 0, tag.TagStream);

                    int ResourceCacheIndex = ResourceCache.TagIDs.IndexOf(tag.ID);
                    int ResourceIndex = 0;
                    for (int i = 0; i < rawCache.Count; i++)
                    {
                        Cache.Stream.Position = Cache.Entries[Index].Offset + rawCache.Values[i].Offset1;
                        int address = br.ReadInt32();
                        if (!(address == -1 || Globals.IsExternalResource(address)))
                        {
                            Cache.Stream.Seek(-4, SeekOrigin.Current);
                            bw.Write(ResourceCache.Offset + ResourceCache.Tags[ResourceCacheIndex].Offset + tag.ResourceInformation[ResourceIndex].Address);
                            Cache.Stream.Position = Cache.Entries[Index].Offset + rawCache.Values[i].Offset0;
                            bw.Write(tag.ResourceInformation[ResourceIndex].Length);
                            ResourceIndex++;
                        }
                    }
                }

                #endregion
            }

            #endregion

            #region STAGE FOUR - Save Caches

            project.CacheCreationDate = DateTime.Now;
            SoundCache.Save(Path.Combine(project.BinDirectory, "SOUND_RAW_CACHE.BIN"), project.CacheCreationDate);
            ModelCache.Save(Path.Combine(project.BinDirectory, "MODEL_RAW_CACHE.BIN"), project.CacheCreationDate);
            SbspCache.Save(Path.Combine(project.BinDirectory, "STRUCTURE_RAW_CACHE.BIN"), project.CacheCreationDate);
            LtmpCache.Save(Path.Combine(project.BinDirectory, "LIGHTMAP_RAW_CACHE.BIN"), project.CacheCreationDate);
            DECRCache.Save(Path.Combine(project.BinDirectory, "DECORATER_RAW_CACHE.BIN"), project.CacheCreationDate);
            WeatherCache.Save(Path.Combine(project.BinDirectory, "WEATHER_RAW_CACHE.BIN"), project.CacheCreationDate);
            AnimationCache.Save(Path.Combine(project.BinDirectory, "ANIMATION_RAW_CACHE.BIN"), project.CacheCreationDate);
            BitmapCache.Save(Path.Combine(project.BinDirectory, "TEXTURE_RAW_CACHE.BIN"), project.CacheCreationDate);
            StringIDsCache.Save(Path.Combine(project.BinDirectory, "STRINGS_CACHE.BIN"), project.CacheCreationDate);
            IndexCache.Save(Path.Combine(project.BinDirectory, "INDEX.BIN"), project.CacheCreationDate);
            TagnameCache.Save(Path.Combine(project.BinDirectory, "TAGNAMES.BIN"), project.CacheCreationDate);
            EnglishUnicodeCache.Save(Path.Combine(project.BinDirectory, "ENGLISH_UNICODE.BIN"), project.CacheCreationDate);
            MetaCache.Save(Path.Combine(project.BinDirectory, "META.BIN"), project.CacheCreationDate);
            SbspLtmpMetaCache.Save(Path.Combine(project.BinDirectory, "STRUCTURE_LIGHTMAP_META.BIN"), project.CacheCreationDate);
            using (FileStream file = new FileStream(Path.Combine(project.BinDirectory, "HEADER.BIN"), FileMode.Create, FileAccess.Write, FileShare.ReadWrite | FileShare.Delete))
            {
                byte[] buffer = Header.ToByteArray();
                file.Write(buffer, 0, buffer.Length);
            }
            File.SetLastWriteTime(Path.Combine(project.BinDirectory, "HEADER.BIN"), project.CacheCreationDate);
            project.Save();

            #endregion
        }

        public void CreateMap(Project project)
        {
            FileStream output = new FileStream(Path.ChangeExtension(Path.Combine(project.BinDirectory, project.Name), ".map"), FileMode.Create, FileAccess.ReadWrite, FileShare.None);
            using (output)
            {
                BinaryWriter bw = new BinaryWriter(output);

                output.Position = SoundCache.Offset;
                SoundCache.Stream.WriteTo(output);

                output.Position = ModelCache.Offset;
                ModelCache.Stream.WriteTo(output);

                output.Position = SbspCache.Offset;
                SbspCache.Stream.WriteTo(output);

                output.Position = LtmpCache.Offset;
                LtmpCache.Stream.WriteTo(output);

                output.Position = DECRCache.Offset;
                DECRCache.Stream.WriteTo(output);

                output.Position = WeatherCache.Offset;
                WeatherCache.Stream.WriteTo(output);

                output.Position = AnimationCache.Offset;
                AnimationCache.Stream.WriteTo(output);

                output.Position = SbspLtmpMetaCache.Offset;
                SbspLtmpMetaCache.WriteTo(output);

                output.Position = StringIDsCache.Offset;
                StringIDsCache.WriteTo(output, ref Header);
                output.Position = Padding.Pad(output.Position);

                output.Position = TagnameCache.Offset;
                TagnameCache.WriteIndexTo(output, ref Header);
                TagnameCache.WriteTableTo(output, ref Header);
                output.Position = Padding.Pad(output.Position);

                output.Position = EnglishUnicodeCache.Offset;
                EnglishUnicodeCache.WriteUnicodeIndexTo(output);
                EnglishUnicodeCache.WriteUnicodeTableTo(output);
                output.Position = Padding.Pad(output.Position);

                output.Position = BitmapCache.Offset;
                BitmapCache.Stream.WriteTo(output);

                output.Position = IndexCache.Offset;
                IndexCache.WriteTo(output, ref Header, IndexCache.Entries[0].Index, IndexCache.Entries[3].Index);

                output.Position = MetaCache.Offset;
                MetaCache.Stream.WriteTo(output);
                bw.Write(Padding.GetBytes(output.Position, 2048));

                Header.Checksum = CalculateChecksum(output);
                Header.TagCacheLength = MetaCache.Length;
                Header.TotalCacheLength = MetaCache.Length + SbspLtmpMetaCache.Length + IndexCache.Length;

                output.Position = 0;
                bw.Write(Header.ToByteArray());
            }
        }

        /// <summary>
        /// Xbox7887's Faster Resign Code, thanks ;)!
        /// </summary>
        /// <param name="stream"></param>
        unsafe private uint CalculateChecksum(Stream stream)
        {
            // initial declarations
            uint checksum = 0;
            int blockSize = 0x10000;    // 64kb
            byte[] block = new byte[blockSize];
            int hashSize = (int)(stream.Length - 2048);
            int blockCount = hashSize / blockSize;
            int blockPasses = blockSize / 16;
            int remainder = hashSize % blockSize;
            int remainingPasses = remainder / 16;
            stream.Position = 2048;

            fixed (byte* buf = &block[0])
            {
                // loop through the map hashing one full block at a time
                uint* p;
                for (int i = 0; i < blockCount; i++)
                {
                    p = (uint*)buf;
                    stream.Read(block, 0, blockSize);
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
                stream.Read(block, 0, remainder);
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

        private void ProcessBlock(Block block, int count, int address, Stream stream)
        {
            BinaryReader binReader = new BinaryReader(stream);
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

        private void ProcessBlockRaw(Block block, int count, int address, Stream stream)
        {
            BinaryReader binReader = new BinaryReader(stream);

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
    }

    public class UnicodeCache : Cache
    {
        public override int Length { get { return Padding.Pad(UnicodeIndexLength) + Padding.Pad(UnicodeTableLength); } }

        #region Things that don't really matter

        public int UnicodeStringCount { get { return Values.Count; } }
        public int UnicodeTableOffset { get { return Padding.Pad(Offset + UnicodeIndexLength); } }
        public int UnicodeTableLength
        {
            get
            {
                int len = 0;
                foreach (Entry e in Values)
                    len += Encoding.UTF8.GetByteCount(e.UnicodeString) + 1;
                return len;
            }
        }
        public int UnicodeIndexOffset { get { return Offset; } }
        public int UnicodeIndexLength { get { return Values.Count * 8; } }

        #endregion

        public List<Entry> Values;

        public UnicodeCache()
        {
            Stream = new MemoryStream();
            Values = new List<Entry>();
        }

        public void WriteUnicodeIndexTo(Stream stream)
        {
            stream.Position = UnicodeIndexOffset;
            BinaryWriter bw = new BinaryWriter(stream);

            int start = 0;
            for (int i = 0; i < Values.Count; i++)
            {
                bw.Write(Values[i].StringId);
                bw.Write(start);
                start += Encoding.UTF8.GetByteCount(Values[i].UnicodeString) + 1;
            }
        }

        public void WriteUnicodeTableTo(Stream stream)
        {
            stream.Position = UnicodeTableOffset;
            BinaryWriter bw = new BinaryWriter(stream);

            foreach (Entry val in Values)
            {
                bw.Write(Encoding.UTF8.GetBytes(val.UnicodeString));
                bw.Write(byte.MinValue);
            }
        }

        public void CacheData(CompilerTag tag, StringsCache cache)
        {
            int index = Values.Count;
            CacheData(tag);
            int count = Values.Count;
            for (int i = index; i < count; i++)
            {
                string str = tag.StringReferenceNames[Values[i].StringId];
                Values[i].StringId = new StringId((short)cache.Values.IndexOf(str), (sbyte)Encoding.UTF8.GetByteCount(str));
            }
        }

        public override void CacheData(CompilerTag tag)
        {
            BinaryReader binReader = new BinaryReader(tag.TagStream);
            BinaryWriter binWriter = new BinaryWriter(tag.TagStream);
            tag.TagStream.Position = 0;
            int count = binReader.ReadInt32();
            int address = binReader.ReadInt32();
            int strLength = binReader.ReadInt32();
            int strAddress = binReader.ReadInt32();
            tag.TagStream.Position = strAddress;
            byte[] unicodeBytes = binReader.ReadBytes(strLength);
            int index = Values.Count;
            for (int i = 0; i < count; i++)
            {
                tag.TagStream.Position = address + (i * 40);
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
                Values.Add(new Entry() { StringId = stringid, UnicodeString = Encoding.UTF8.GetString(strBytes.ToArray()) });
            }
            tag.TagStream.SetLength(52);
            tag.TagStream.Position = 0;
            binWriter.Write(new byte[16]);
            binWriter.Write((short)index);
            binWriter.Write((short)count);
            binWriter.Write(new byte[32]);
            tag.Type = "unic";
        }

        public class Entry
        {
            public string UnicodeString;
            public StringId StringId;
        }

        public override void Save(string filename, DateTime time)
        {
            using (FileStream file = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.ReadWrite | FileShare.Delete))
            {
                BinaryWriter binWriter = new BinaryWriter(file);

                //Write Cache Header
                binWriter.Write(new TagType("UNIC"));
                binWriter.Write(time.ToBinary());
                binWriter.Write(Offset);
                binWriter.Write(Values.Count);

                //Write Cache Index
                for (int i = 0; i < Values.Count; i++)
                {
                    binWriter.Write(Values[i].StringId);
                    binWriter.Write(Values[i].UnicodeString);
                }
                binWriter.Write(Padding.GetBytes(file.Position));
                binWriter.Flush();
                file.Close();
            }
            File.SetLastWriteTime(filename, time);
        }

        public override void Load(string filename)
        {
            using (FileStream file = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete))
            {
                BinaryReader binReader = new BinaryReader(file);
                if (binReader.ReadTagType() != "UNIC") throw new Exception(); 
                DateTime time = DateTime.FromBinary(binReader.ReadInt64());
                Offset = binReader.ReadInt32();
                int count = binReader.ReadInt32();
                Values = new List<Entry>(count);
                for (int i = 0; i < count; i++)
                {
                    Values.Add(new Entry() { StringId = binReader.ReadStringReference(), UnicodeString = binReader.ReadString() });
                }
            }
        }
    }

    public class TagnameCache : Cache
    {
        public override int Length { get { return Padding.Pad(TagnameIndexLength) + Padding.Pad(TagnameTableLength); } }

        public int TagnameIndexOffset { get { return Offset; } }
        public int TagnameIndexLength { get { return Tagnames.Count * 4; } }
        public int TagnameTableOffset { get { return Padding.Pad(Offset + TagnameIndexLength); } }
        public int TagnameTableLength
        {
            get
            {
                int len = 0;
                foreach (string str in Tagnames)
                    len += Encoding.UTF8.GetByteCount(Tag.Path.GetTagpath(str)) + 1;
                return len;
            }
        }

        public List<string> Tagnames;

        public TagnameCache()
        {
            Stream = new MemoryStream();
            Tagnames = new List<string>();
        }

        public void WriteIndexTo(Stream stream, ref Header header)
        {
            stream.Position = header.PathIndexAddress = TagnameIndexOffset;
            header.PathCount = Tagnames.Count;
            BinaryWriter bw = new BinaryWriter(stream);
            int start = 0;
            for (int i = 0; i < Tagnames.Count; i++)
            {
                bw.Write(start);
                start += Encoding.UTF8.GetByteCount(Tag.Path.GetTagpath(Tagnames[i])) + 1;
            }
        }

        public void WriteTableTo(Stream stream, ref Header header)
        {
            stream.Position = header.PathTableAddress = TagnameTableOffset;
            BinaryWriter bw = new BinaryWriter(stream);
            foreach (string val in Tagnames)
            {
                bw.Write(Encoding.UTF8.GetBytes(Tag.Path.GetTagpath(val)));
                bw.Write(byte.MinValue);
            }
            header.PathTableLength = TagnameTableLength;
        }

        public override void CacheData(CompilerTag tag)
        {
            Tagnames.Add(tag.Filename);
        }

        public override void Save(string filename, DateTime time)
        {
            using (FileStream file = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.ReadWrite | FileShare.Delete))
            {
                BinaryWriter binWriter = new BinaryWriter(file);

                //Write Cache Header
                binWriter.Write(new TagType("TAGN"));
                binWriter.Write(time.ToBinary());
                binWriter.Write(Offset);
                binWriter.Write(Tagnames.Count);

                //Write Cache Index
                for (int i = 0; i < Tagnames.Count; i++)
                    binWriter.Write(Tagnames[i]);
                binWriter.Write(Padding.GetBytes(file.Position));

                binWriter.Flush();
                file.Close();
            }
            File.SetLastWriteTime(filename, time);
        }

        public override void Load(string filename)
        {
            using (FileStream file = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete))
            {
                BinaryReader binReader = new BinaryReader(file);
                if (binReader.ReadTagType() != "TAGN") throw new Exception();
                DateTime time = DateTime.FromBinary(binReader.ReadInt64());
                Offset = binReader.ReadInt32();
                int count = binReader.ReadInt32();
                Tagnames = new List<string>(count);
                for (int i = 0; i < count; i++)
                    Tagnames.Add(binReader.ReadString());
            }
        }
    }

    public class IndexCache : Cache
    {
        public override int Length { get { return Padding.Pad(32 + (Index.Types.Length * 12) + (Entries.Count * 16)); } }

        int VirtualOffset = Index.VirtualOffset;
        public int NextVirtualOffset { get { return VirtualOffset + Length; } }

        public List<Entry> Entries;

        public IndexCache()
        {
            Stream = new MemoryStream();
            Entries = new List<Entry>();
        }

        public void SetEntry(int index, Entry entry)
        {
            Entries[index] = entry;
        }

        public class Entry
        {
            public TagType Type;
            public TagIndex Index;
            public int Address;
            public int Length;
        }

        public void WriteTo(Stream stream, ref Header header, int globalstagindex, int scenariotagindex)
        {
            stream.Position = header.IndexAddress = Offset;

            BinaryWriter bw = new BinaryWriter(stream);

            Index index = new Index();
            index.TagTypeArrayVirtualAddress =Index.VirtualOffset + 32;
            index.TagInfoArrayVirtualAddress = Index.VirtualOffset + 32 + (index.TagTypeCount * 12);
            index.GlobalsTagId = globalstagindex;
            index.ScenarioTagId = scenariotagindex;
            index.TagEntries = new Index.TagInformation[Entries.Count];
            index.TagInfoCount = Entries.Count;
            for (int i = 0; i < index.TagEntries.Length; i++)
            {
                index.TagEntries[i] = new Index.TagInformation()
                {
                    Type = Entries[i].Type,
                    Index = Entries[i].Index,
                    VirtualAddress = Entries[i].Address,
                    Length = Entries[i].Length
                };
            }

            bw.Write(index.ToArray());

            header.IndexLength = Length;
        }

        public override void CacheData(CompilerTag tag)
        {
            tag.ID = Entries.Count;
            Entries.Add(new Entry()
            {
                Type = tag.Type,
                Index = new TagIndex(Entries.Count),
                Length = (int)tag.TagStream.Length
            });
        }

        public override void Save(string filename, DateTime time)
        {
            using (FileStream file = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.ReadWrite | FileShare.Delete))
            {
                BinaryWriter binWriter = new BinaryWriter(file);

                //Write Cache Header
                binWriter.Write(new TagType("INDX"));
                binWriter.Write(time.ToBinary());
                binWriter.Write(Offset);
                binWriter.Write(Entries.Count);
                //Write Cache Index
                for (int i = 0; i < Entries.Count; i++)
                {
                    binWriter.Write(Entries[i].Type);
                    binWriter.Write(Entries[i].Index);
                    binWriter.Write(Entries[i].Address);
                    binWriter.Write(Entries[i].Length);
                }

                binWriter.Write(Padding.GetBytes(file.Position));
                binWriter.Flush();
                file.Close();
            }
            File.SetLastWriteTime(filename, time);
        }

        public override void Load(string filename)
        {
            using (FileStream file = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Delete | FileShare.ReadWrite))
            {
                BinaryReader binReader = new BinaryReader(file);
                if (binReader.ReadTagType() != "INDX") throw new Exception();
                DateTime time = DateTime.FromBinary(binReader.ReadInt64());
                Offset = binReader.ReadInt32();
                int count = binReader.ReadInt32();
                Entries = new List<Entry>(count);
                for (int i = 0; i < count; i++)
                    Entries.Add(new Entry() { Type = binReader.ReadTagType(), Index = binReader.ReadInt32(), Address = binReader.ReadInt32(), Length = binReader.ReadInt32() });
            }
        }
    }

    public class StructureMetaCache : MetaCache
    {
        public override int Length { get { return Padding.Pad(Stream.Length); } }

        List<Tag> Tags;

        public StructureMetaCache()
            : base()
        {
            Tags = new List<Tag>();
        }


        public void SetVirtualOffset(int virtualoffset)
        {
            VirtualOffset = virtualoffset;
        }

        public void UpdateScenario(Tag scenariotag)
        {
            List<Tag> tags = new List<Tag>(2);
            if (Tags[0].Type == "sbsp")
                tags.AddRange(Tags);
            else
            {
                tags.Add(Tags[1]);
                tags.Add(Tags[0]);
            }
            Tags = tags;

            BinaryWriter bw = new BinaryWriter(Stream);
            foreach (CompilerTag tag in Tags)
            {
                bw.Write(Padding.GetBytes(Stream.Position));
                Entry entry = new Entry()
                {
                    Offset = (int)Stream.Position,
                    Length = (int)tag.TagStream.Length,
                    PointerOffsets = tag.PointerOffsets,
                    RawOffsets = tag.RawOffsets,
                    StringIDOffsets = tag.StringIdOffsets,
                    TagIDOffsets = tag.TagReferenceOffsets
                };
                tag.TagStream.WriteTo(Stream);
                bw.Write(Padding.GetBytes(Stream.Position));
                bw.Flush();
                Entries.Add(entry);
                TagIndexers.Add(tag.ID);
            }
            BinaryReader br = new BinaryReader(scenariotag.TagStream);
            bw = new BinaryWriter(scenariotag.TagStream);
            scenariotag.TagStream.Position = 532;
            int address = br.ReadInt32();
            scenariotag.TagStream.Position = address;
            bw.Write(Offset);
            bw.Write(Length);
            bw.Write(VirtualOffset);
        }

        public void WriteTo(Stream stream)
        {
            BinaryWriter bw = new BinaryWriter(Stream);
            Stream.Position = 0;
            int ltmpVirtualOffset = VirtualOffset + Entries[1].Offset;
            bw.Write(Length);
            bw.Write(VirtualOffset + 16);
            bw.Write(ltmpVirtualOffset);
            bw.Write(Encoding.UTF8.GetBytes("psbs"));
            bw.Flush();

            stream.Position = Offset;
            Stream.WriteTo(stream);
        }

        public override void CacheData(CompilerTag tag)
        {
            Tags.Add(tag);
        }
    }

    public class MetaCache : Cache
    {
        public override int Length { get { return Padding.Pad(Stream.Length, 2048); } }

        public int VirtualOffset;
        public int NextVirtualOffset { get { return VirtualOffset + Length; } }

        public List<Entry> Entries;
        public List<int> TagIndexers;

        public class Entry
        {
            public int Offset;
            public int Length;
            public int[] RawOffsets;
            public int[] StringIDOffsets;
            public int[] TagIDOffsets;
            public int[] PointerOffsets;
        }

        public MetaCache()
        {
            Stream = new MemoryStream();
            Entries = new List<Entry>();
            TagIndexers = new List<int>();
        }

        public byte[] this[int index]
        {
            get
            {
                byte[] buffer = new byte[Entries[index].Length];
                Stream.Read(buffer, Entries[index].Offset, Entries[index].Length);
                return buffer;
            }
            set
            {
                if (value.Length == Entries[index].Length)
                {
                    Stream.Seek(Entries[index].Offset, SeekOrigin.Begin);
                    Stream.Write(value, 0, value.Length);
                }
                else throw new Exception();
            }
        }

        public void UpdateIndexCache(IndexCache indexcache)
        {
            for (int i = 0; i < TagIndexers.Count; i++)
                indexcache.Entries[TagIndexers[i]].Address = Entries[i].Offset + VirtualOffset;
        }

        public override void CacheData(CompilerTag tag)
        {
            BinaryWriter bw = new BinaryWriter(Stream);
            bw.Write(Padding.GetBytes(Stream.Position));
            Entry entry = new Entry()
            {
                Offset = (int)Stream.Position,
                Length = (int)tag.TagStream.Length,
                PointerOffsets = tag.PointerOffsets,
                RawOffsets = tag.RawOffsets,
                StringIDOffsets = tag.StringIdOffsets,
                TagIDOffsets = tag.TagReferenceOffsets
            };
            tag.TagStream.WriteTo(Stream);
            bw.Flush();
            Entries.Add(entry);
            TagIndexers.Add(tag.ID);
        }

        public override void Save(string filename, DateTime time)
        {
            using (FileStream file = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.ReadWrite | FileShare.Delete))
            {
                const int headerLength = 36;
                int tagCount = Entries.Count;
                BinaryWriter binWriter = new BinaryWriter(file);

                //Write Cache Header
                binWriter.Write(new TagType("METC")); 
                binWriter.Write(time.ToBinary());
                binWriter.Write(Offset);
                binWriter.Write(VirtualOffset);
                binWriter.Write(headerLength);
                binWriter.Write(tagCount);
                binWriter.Write(-1);
                binWriter.Write((int)Stream.Length);

                //Write Cache Index
                for (int i = 0; i < tagCount; i++)
                {
                    binWriter.Write(TagIndexers[i]);
                    binWriter.Write(Entries[i].Offset);
                    binWriter.Write(Entries[i].Length);
                    binWriter.Write(Entries[i].PointerOffsets.Length);
                    for (int x = 0; x < Entries[i].PointerOffsets.Length; x++)
                        binWriter.Write(Entries[i].PointerOffsets[x]);
                    binWriter.Write(Entries[i].RawOffsets.Length);
                    for (int x = 0; x < Entries[i].RawOffsets.Length; x++)
                        binWriter.Write(Entries[i].RawOffsets[x]);
                    binWriter.Write(Entries[i].StringIDOffsets.Length);
                    for (int x = 0; x < Entries[i].StringIDOffsets.Length; x++)
                        binWriter.Write(Entries[i].StringIDOffsets[x]);
                    binWriter.Write(Entries[i].TagIDOffsets.Length);
                    for (int x = 0; x < Entries[i].TagIDOffsets.Length; x++)
                        binWriter.Write(Entries[i].TagIDOffsets[x]);
                }

                binWriter.Write(Padding.GetBytes(file.Position));
                int streamAddress = (int)file.Position;

                //Write Cache Data
                binWriter.Write(Stream.ToArray());
                binWriter.Write(Padding.GetBytes(file.Position));

                file.Seek(28, SeekOrigin.Begin);
                binWriter.Write(streamAddress);

                binWriter.Flush();
                file.Close();
            }
            File.SetLastWriteTime(filename, time);
        }

        public override void Load(string filename)
        {
            using (FileStream file = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Delete | FileShare.ReadWrite))
            {
                BinaryReader binReader = new BinaryReader(file);
                if (binReader.ReadTagType() != "METC") throw new Exception();
                DateTime time = DateTime.FromBinary(binReader.ReadInt64());
                Offset = binReader.ReadInt32();
                VirtualOffset = binReader.ReadInt32();
                int indexAddress = binReader.ReadInt32();
                int count = binReader.ReadInt32();
                int streamAddress = binReader.ReadInt32();
                int streamLength = binReader.ReadInt32();

                TagIndexers = new List<int>(count);
                Entries = new List<Entry>(count);

                for (int i = 0; i < count; i++)
                {
                    TagIndexers.Add(binReader.ReadInt32());
                    Entry e = new Entry();
                    e.Offset = binReader.ReadInt32();
                    e.Length = binReader.ReadInt32();
                    e.PointerOffsets = new int[binReader.ReadInt32()];
                    for (int x = 0; x < e.PointerOffsets.Length; x++)
                        e.PointerOffsets[x] = binReader.ReadInt32();
                    e.RawOffsets = new int[binReader.ReadInt32()];
                    for (int x = 0; x < e.RawOffsets.Length; x++)
                        e.RawOffsets[x] = binReader.ReadInt32();
                    e.StringIDOffsets = new int[binReader.ReadInt32()];
                    for (int x = 0; x < e.StringIDOffsets.Length; x++)
                        e.StringIDOffsets[x] = binReader.ReadInt32();
                    e.TagIDOffsets = new int[binReader.ReadInt32()];
                    for (int x = 0; x < e.TagIDOffsets.Length; x++)
                        e.TagIDOffsets[x] = binReader.ReadInt32();
                    Entries.Add(e);
                }

                file.Seek(streamAddress, SeekOrigin.Begin);
                Stream = new MemoryStream(binReader.ReadBytes(streamLength));
            }
        }
    }

    public class RawCache : Cache
    {
        public List<Entry> Tags;
        public List<int> TagIDs;

        public RawCache()
        {
            Stream = new MemoryStream();
            Tags = new List<Entry>();
            TagIDs = new List<int>();
        }

        public struct Entry
        {
            public int Offset;
            public int Length;
        }

        public override int Length { get { return (int)Stream.Length; } }

        public override void CacheData(CompilerTag tag)
        {
            if (tag.ResourceStream.Length == 0) return;
            Entry entry = new Entry() { Offset = (int)Stream.Position, Length = (int)tag.ResourceStream.Length };
            for (int i = 0; i < tag.RawOffsets.Length; i++)
                tag.RawOffsets[i] += entry.Offset;
            Tags.Add(entry);
            TagIDs.Add(tag.ID);
            BinaryWriter bw = new BinaryWriter(Stream);
            bw.Write(tag.ResourceStream.ToArray());
            bw.Flush();
        }

        public override void Save(string filename, DateTime time)
        {
            using (FileStream file = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.ReadWrite | FileShare.Delete))
            {
                const int headerLength = 32;
                const int indexEntryLength = 12;

                int tagCount = Tags.Count;
                int streamAddress = Padding.Pad(headerLength + (tagCount * indexEntryLength));
                BinaryWriter binWriter = new BinaryWriter(file);

                //Write Cache Header
                binWriter.Write(new TagType("RAWC"));
                binWriter.Write(time.ToBinary());
                binWriter.Write(Offset);
                binWriter.Write(headerLength);
                binWriter.Write(tagCount);                
                binWriter.Write(streamAddress);
                binWriter.Write((int)Stream.Length);

                //Write Cache Index
                for (int i = 0; i < tagCount; i++)
                {
                    binWriter.Write(TagIDs[i]);
                    binWriter.Write(Tags[i].Offset);
                    binWriter.Write(Tags[i].Length);
                }

                //Write Cache Data
                file.Seek(streamAddress, SeekOrigin.Begin);
                binWriter.Write(Stream.ToArray());
                binWriter.Write(Padding.GetBytes(file.Position));
                binWriter.Flush();
                file.Close();
            }
            File.SetLastWriteTime(filename, time);
        }

        public override void Load(string filename)
        {
            using (FileStream file = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete))
            {
                BinaryReader binReader = new BinaryReader(file);

                //Write Cache Header
                if (binReader.ReadTagType() != "RAWC") throw new Exception();
                DateTime time = DateTime.FromBinary(binReader.ReadInt64());
                Offset = binReader.ReadInt32();
                int indexAddress = binReader.ReadInt32();
                int count = binReader.ReadInt32();
                int streamAddress = binReader.ReadInt32();
                int streamLength = binReader.ReadInt32();

                Tags = new List<Entry>(count);
                TagIDs = new List<int>(count);

                //Write Cache Index
                for (int i = 0; i < count; i++)
                {
                    TagIDs.Add(binReader.ReadInt32());
                    Tags.Add(new Entry() { Offset = binReader.ReadInt32(), Length = binReader.ReadInt32() });
                }

                //Write Cache Data
                file.Seek(streamAddress, SeekOrigin.Begin);
                Stream = new MemoryStream(binReader.ReadBytes(streamLength));
            }
        }
    }

    public class StringsCache : Cache
    {
        public int String128TableAddress { get { return Offset; } }
        public int String128TableLength { get { return Values.Count * 128; } }
        public int StringIndexAddress { get { return Padding.Pad(String128TableAddress + String128TableLength); } }
        public int StringIndexLength { get { return Values.Count * 4; } }
        public int StringTableAddress { get { return Padding.Pad(StringIndexAddress + StringIndexLength); } }
        public int StringTableLength
        {
            get
            {
                int len = 0;
                foreach (string str in Values)
                    len += Encoding.UTF8.GetByteCount(str) + 1;
                return len;
            }
        }

        public List<string> Values;

        public void OptimizeCache()
        {
        }

        public StringsCache()
        {
            Values = new List<string>(GlobalStringIDs.Values);
        }

        public void WriteTo(Stream stream, ref Header header)
        {
            Write128TableTo(stream, ref header);
            WriteIndexTo(stream, ref header);
            WriteTableTo(stream, ref header);
        }

        void Write128TableTo(Stream stream, ref Header header)
        {
            stream.Position = header.String128TableAddress = String128TableAddress;
            header.StringCount = Values.Count;

            BinaryWriter bw = new BinaryWriter(stream);

            List<byte> buffer = new List<byte>();
            for (int i = 0; i < Values.Count; i++)
            {
                buffer.AddRange(Encoding.UTF8.GetBytes(Values[i]));
                buffer.AddRange(new byte[128 - Values[i].Length]);
            }
            bw.Write(buffer.ToArray());
        }

        void WriteIndexTo(Stream stream, ref Header header)
        {
            stream.Position = header.StringIndexAddress = StringIndexAddress;
            BinaryWriter bw = new BinaryWriter(stream);

            int start = 0;
            for (int i = 0; i < Values.Count; i++)
            {
                bw.Write(start);
                start += Values[i].Length + 1;
            }
        }

        void WriteTableTo(Stream stream, ref Header header)
        {
            stream.Position = header.StringTableAddress = StringTableAddress;
            BinaryWriter bw = new BinaryWriter(stream);

            List<byte> buffer = new List<byte>();
            for (int i = 0; i < Values.Count; i++)
            {
                buffer.AddRange(Encoding.UTF8.GetBytes(Values[i]));
                buffer.Add(byte.MinValue);
            }
            bw.Write(buffer.ToArray());

            header.StringTableLength = StringTableLength;
        }

        public override int Length { get { return Padding.Pad(String128TableLength) + Padding.Pad(StringIndexLength) + Padding.Pad(StringTableLength); } }

        public override void CacheData(CompilerTag tag)
        {
            foreach (string value in tag.StringReferenceNames)
            {
                if (!Values.Contains(value))
                {
                    Values.Add(value);
                }
            }
        }

        public override void Save(string filename, DateTime time)
        {
            using (FileStream file = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                const int headerLength = 32;

                BinaryWriter binWriter = new BinaryWriter(file);

                //Write Header
                binWriter.Write(new TagType("STRC"));
                binWriter.Write(time.ToBinary());
                binWriter.Write(Offset);
                binWriter.Write(Values.Count);
                binWriter.Write(headerLength);
                binWriter.Write(0);
                binWriter.Write(0);

                //Write Index
                int start = 0;
                for (int i = 0; i < Values.Count; i++)
                {
                    binWriter.Write(start);
                    start += Encoding.UTF8.GetByteCount(Values[i]) + 1;
                }

                int stringsAddress = file.Pad(4);

                //Write Strings
                for (int i = 0; i < Values.Count; i++)
                {
                    binWriter.Write(Encoding.UTF8.GetBytes(Values[i]));
                    binWriter.Write(byte.MinValue);
                }
                file.Pad(4);
                file.Seek(24, SeekOrigin.Begin);
                binWriter.Write(start);
                binWriter.Write(stringsAddress);
                binWriter.Flush();
                file.Close();
            }
            File.SetLastWriteTime(filename, time);
        }

        public override void Load(string filename)
        {
            using (FileStream file = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.None))
            {
                BinaryReader binReader = new BinaryReader(file);
                if (binReader.ReadTagType() != "STRC") throw new Exception();
                DateTime time = DateTime.FromBinary(binReader.ReadInt64());
                Offset = binReader.ReadInt32();
                int count = binReader.ReadInt32();
                int indexAddress = binReader.ReadInt32();
                int stringsLength = binReader.ReadInt32();
                int stringsAddress = binReader.ReadInt32();
                Values = new List<string>(count);
                int[] strIndices = new int[count];
                file.Seek(indexAddress, SeekOrigin.Begin);
                for (int i = 0; i < count; i++)
                    strIndices[i] = binReader.ReadInt32();
                file.Seek(stringsAddress, SeekOrigin.Begin);
                for (int i = 0; i < count - 1; i++)
                {
                    file.Seek(stringsAddress + strIndices[i], SeekOrigin.Begin);
                    Values.Add(binReader.ReadUTF8String(strIndices[i + 1] - strIndices[i] - 1));
                }
                file.Seek(stringsAddress + strIndices[strIndices.Length-1], SeekOrigin.Begin);
                Values.Add(binReader.ReadUTF8String(stringsLength - strIndices[strIndices.Length - 1] - 1));
            }
        }
    }

    public abstract class Cache
    {
        public int Offset;
        public abstract int Length { get; }
        public int NextOffset { get { return Offset + Length; } }

        public MemoryStream Stream;

        public Cache()
        {
            Stream = new MemoryStream();
        }

        public abstract void CacheData(CompilerTag tag);

        public abstract void Save(string filename, DateTime time);

        public abstract void Load(string filename);
    }
}