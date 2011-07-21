using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using Sunfish.TagStructures;
using Sunfish.ValueTypes;

namespace Sunfish
{
    public class Map
    {
        public const string Extension = ".map";

        public Header Header;
        public Index Index;
        public Dictionary<UnicodeTable.Language, UnicodeTable> Unicode;
        public Stream BaseStream;
        public string[] Tagnames;
        public List<string> StringIdNames;

        public int PrimaryMagic;
        public int SecondaryMagic;

        public Map()
        {
            Header = new Header();
            Index = new Index();
            Unicode = new Dictionary<UnicodeTable.Language, UnicodeTable>();
            Tagnames = new string[0];
            StringIdNames = new List<string>();
        }

        public Map(Stream stream)
        {
            BaseStream = stream;
            BaseStream.Position = 0;
            BinaryReader br = new BinaryReader(BaseStream);
            Header = new Header(this);
            Index = new Index(this);
            SecondaryMagic = Index.TagEntries[0].VirtualAddress - (Header.IndexAddress + Header.IndexLength);
            Seek(Index.ScenarioTagId);
            BaseStream.Position += 528;
            int Count = br.ReadInt32();
            int Address = br.ReadInt32();
            for (int i = 0; i < Count; i++)
            {
                BaseStream.Position = Address - SecondaryMagic;
                int blockOffset = br.ReadInt32();
                int blockSize = br.ReadInt32();
                int virtualOffset = br.ReadInt32();
                if (i == 0)
                    PrimaryMagic = (int)(virtualOffset - blockOffset);
                BaseStream.Position += 8;
                Sunfish.ValueTypes.TagIndex sbspId = br.ReadInt32();
                BaseStream.Position += 4;
                Sunfish.ValueTypes.TagIndex ltmpId = br.ReadInt32();
                BaseStream.Position = blockOffset + 8;
                int ltmpOffset = br.ReadInt32();
                int ltmpSize = (int)((blockOffset + blockSize) - (ltmpOffset - PrimaryMagic));
                Index.TagInformation tagEntry = Index.TagEntries[sbspId.Index];
                tagEntry.VirtualAddress = virtualOffset;
                tagEntry.Length = blockSize - ltmpSize;
                tagEntry.Magic = PrimaryMagic;
                Index.TagEntries[sbspId & 0x0000FFFF] = tagEntry;
                if (ltmpId == -1) { continue; }
                tagEntry = Index.TagEntries[ltmpId.Index];
                tagEntry.VirtualAddress = ltmpOffset;
                tagEntry.Length = ltmpSize;
                tagEntry.Magic = PrimaryMagic;
                Index.TagEntries[ltmpId.Index] = tagEntry;
            }
            BaseStream.Position = Header.PathTableAddress;
            Tagnames = Encoding.UTF8.GetString(br.ReadBytes(Header.PathTableLength - 1)).Split(char.MinValue);
            BaseStream.Position = Header.StringTableAddress;
            StringIdNames = new List<string>(Encoding.UTF8.GetString(br.ReadBytes(Header.StringTableLength - 1)).Split(char.MinValue));
            Unicode = new Dictionary<UnicodeTable.Language, UnicodeTable>(8);
            Unicode.Add(UnicodeTable.Language.English, new UnicodeTable(this, UnicodeTable.Language.English));
            Unicode.Add(UnicodeTable.Language.Japanese, new UnicodeTable(this, UnicodeTable.Language.Japanese));
            Unicode.Add(UnicodeTable.Language.Chinese, new UnicodeTable(this, UnicodeTable.Language.Chinese));
            Unicode.Add(UnicodeTable.Language.Dutch, new UnicodeTable(this, UnicodeTable.Language.Dutch));
            Unicode.Add(UnicodeTable.Language.French, new UnicodeTable(this, UnicodeTable.Language.French));
            Unicode.Add(UnicodeTable.Language.Spanish, new UnicodeTable(this, UnicodeTable.Language.Spanish));
            Unicode.Add(UnicodeTable.Language.Italian, new UnicodeTable(this, UnicodeTable.Language.Italian));
            Unicode.Add(UnicodeTable.Language.Korean, new UnicodeTable(this, UnicodeTable.Language.Korean));
            Unicode.Add(UnicodeTable.Language.Portuguese, new UnicodeTable(this, UnicodeTable.Language.Portuguese));
        }

        public void Seek(int tagIndex)
        {
            int index = tagIndex & 0x0000FFFF;
            BaseStream.Seek(Index.TagEntries[index].VirtualAddress - Index.TagEntries[index].Magic, SeekOrigin.Begin);
        }

        public int IndexOfTagEntry(string type, string tagname)
        {
            for (int i = 0; i < Index.TagEntries.Length; i++)
                if (Index.TagEntries[i].Type == type && Tagnames[i] == tagname) return i;
            return -1;
        }       
    }
    public class Index
    {
        const int HeaderSize = 32;

        public const int VirtualOffset = -2147086336;

        public int TagTypeArrayVirtualAddress;
        public int TagTypeCount;
        public int TagInfoArrayVirtualAddress;
        public int ScenarioTagId;
        public int GlobalsTagId;
        public int TagInfoCount;
        readonly byte[] FourCC;

        public static TagType[] Types;
        public TagInformation[] TagEntries;

        public int Length
        {
            get
            {
                int indexSize = 32 + (TagTypeCount * 12) + (TagInfoCount * 16);
                int padding = Padding.GetCount(indexSize, 512);
                return indexSize + padding;
            }
        }

        static Index()
        {
            Types = new TagType[] {                 
                    "$#!+",
                    "*cen","*eap","*ehi","*igh","*ipd","*qip","*rea","*sce",
                    "/**/",
                    "<fx>",
                    "BooM",
                    "DECP","DECR",
                    "MGS2",
                    "PRTM",
                    "adlg",
                    "ai**","ant!",
                    "bipd","bitm","bloc","bsdt",
                    "char","cin*","clu*","clwd","coll","coln","colo","cont","crea","ctrl",
                    "dc*s","dec*","deca","devi","devo","dgr*","dobc",
                    "effe","egor","eqip",
                    "fog ","foot","fpch",
                    "garb","gldf","goof","grhi",
                    "hlmt","hmt ","hsc*","hud#","hudg",
                    "item","itmc",
                    "jmad","jpt!",
                    "lens","lifi","ligh","lsnd","ltmp",
                    "mach","matg","mdlg","metr","mode","mpdt","mply","mulg",
                    "nhdt",
                    "obje",
                    "phmo","phys","pmov","pphy","proj","prt3",
                    "sbsp","scen","scnr","sfx+","shad","sily","skin","sky ","slit","sncl","snd!","snde","snmx","spas","spk!","ssce","sslt","stem","styl",
                    "tdtl","trak","trg*",
                    "udlg","ugh!","unhi","unic","unit",
                    "vehc","vehi","vrtx",
                    "weap","weat","wgit","wgtz","whip","wigl","wind","wphi",
                };
            TagTypesDictionary = new Dictionary<string, string>(118);
            for (int i = 0; i < Index.Types.Length; i++)
                TagTypesDictionary.Add(GetCleanType(Index.Types[i].ToString()), Index.Types[i].ToString());
        }

        public static string GetCleanType(string dirtyType)
        {
            string cleanType = dirtyType.Replace('*', ' ');
            cleanType = cleanType.Replace('!', ' ');
            cleanType = cleanType.Replace('+', ' ');
            cleanType = cleanType.Replace('<', ' ');
            cleanType = cleanType.Replace('>', ' ');
            cleanType = cleanType.Replace('$', ' ');
            cleanType = cleanType.Replace('#', ' ');
            return cleanType.Trim();
        }

        public static string GetDirtyType(string cleanType)
        {
            return TagTypesDictionary[cleanType];
        }

        static Dictionary<string, string> TagTypesDictionary;

        public Index()
        {
            FourCC = Encoding.UTF8.GetBytes("sgat");
            TagTypeCount = Types.Length;
        }

        public Index(Map map)
        {
            BinaryReader br = new BinaryReader(map.BaseStream);
            map.BaseStream.Seek(map.Header.IndexAddress, SeekOrigin.Begin);
            this.TagTypeArrayVirtualAddress = br.ReadInt32();
            this.TagTypeCount = br.ReadInt32();
            this.TagInfoArrayVirtualAddress = br.ReadInt32();
            this.ScenarioTagId = br.ReadInt32();
            this.GlobalsTagId = br.ReadInt32();
            map.BaseStream.Seek(4, SeekOrigin.Current);
            this.TagInfoCount = br.ReadInt32();
            if (br.ReadTagType() != "tags") throw new Exception();
            for (int i = 0; i < TagTypeCount; i++)
            {
                if (Types[i] != br.ReadTagType()) throw new Exception("InvalidTagType");
                map.BaseStream.Seek(8, SeekOrigin.Current);
            }
            TagEntries = new TagInformation[TagInfoCount];
            for (int i = 0; i < TagInfoCount; i++)
            {
                TagEntries[i] = new TagInformation() { Type = br.ReadTagType(), Index = br.ReadTagIndex(), VirtualAddress = br.ReadInt32(), Length = br.ReadInt32() };
                TagEntries[i].Magic = TagEntries[0].VirtualAddress - (map.Header.IndexAddress + map.Header.IndexLength);
            }
        }

        public class TagInformation
        {
            public TagType Type;
            public TagIndex Index;
            public int VirtualAddress;
            public int Length;

            public int Magic;

            public byte[] ToByteArray()
            {
                byte[] buffer = new byte[16];
                Array.Copy(Type.ToByteArray(), 0, buffer, 0, 4);
                Array.Copy(BitConverter.GetBytes(Index), 0, buffer, 4, 4);
                Array.Copy(BitConverter.GetBytes(VirtualAddress), 0, buffer, 8, 4);
                Array.Copy(BitConverter.GetBytes(Length), 0, buffer, 12, 4);
                return buffer;
            }
        }

        public byte[] ToArray()
        {
            byte[] buffer = new byte[this.Length];
            MemoryStream ms = new MemoryStream(buffer);
            BinaryWriter bw = new BinaryWriter(ms);

            bw.Write(TagTypeArrayVirtualAddress);
            bw.Write(TagTypeCount);
            bw.Write(TagInfoArrayVirtualAddress);
            bw.Write(ScenarioTagId);
            bw.Write(GlobalsTagId);
            bw.Write(0x00000000);
            bw.Write(TagInfoCount);
            bw.Write(FourCC, 0, 4);

            TagType baseType;
            foreach (TagType type in Types)
            {
                bw.Write(type.ToByteArray());
                baseType = GetBaseType(type);
                if (baseType == TagType.Null)
                {
                    bw.Write(0);
                    bw.Write(0);
                }
                else
                {
                    bw.Write(baseType.ToByteArray());
                    baseType = GetBaseType(baseType);
                    if (baseType == TagType.Null)
                    {
                        bw.Write(0);
                    }
                    else
                    {
                        bw.Write(baseType.ToByteArray());
                    }
                }
            }

            foreach (TagInformation entry in TagEntries)
                bw.Write(entry.ToByteArray());
            bw.Close();
            return buffer;
        }

        TagType GetBaseType(TagType type)
        {
            switch (type.ToString())
            {
                case "item":
                case "unit":
                case "ssce":
                case "devi":
                case "scen":
                case "proj":
                case "crea":
                case "bloc":
                    return new TagType("obje");
                case "weap":
                case "garb":
                case "eqip":
                    return new TagType("item");
                case "vehi":
                case "bipd":
                    return new TagType("unit");
                case "mach":
                case "lifi":
                case "ctrl":
                    return new TagType("devi");
                default: return TagType.Null;
            }
        }
    }

    public class Header
    {
        const int Length = 2048;

        public const int EngineVersion = 8;
        public int IndexAddress = -1;
        public int IndexLength;
        public int TagCacheLength;
        public int TotalCacheLength;
        public const string BuildDate = "02.09.27.09809";
        public MapType Type;
        public int String128TableAddress = -1;
        public int StringCount;
        public int StringTableLength;
        public int StringIndexAddress = -1;
        public int StringTableAddress = -1;
        public string Name;
        public string Scenario;
        public int PathCount;
        public int PathTableAddress = -1;
        public int PathTableLength;
        public int PathIndexAddress = -1;
        public uint Checksum;

        public Header() { }

        public Header(Stream stream)
        {
            BinaryReader br = new BinaryReader(stream);
            stream.Seek(0, SeekOrigin.Begin);
            if (br.ReadTagType() != "head") throw new InvalidDataException();
            if (EngineVersion != br.ReadInt32()) throw new InvalidDataException();
            stream.Seek(8, SeekOrigin.Current);
            IndexAddress = br.ReadInt32();
            IndexLength = br.ReadInt32();
            TagCacheLength = br.ReadInt32();
            TotalCacheLength = br.ReadInt32();
            stream.Seek(256, SeekOrigin.Current);
            if (BuildDate != Encoding.UTF8.GetString(br.ReadBytes(32)).TrimEnd(char.MinValue)) throw new InvalidDataException();
            Type = (MapType)br.ReadInt32();
            stream.Seek(28, SeekOrigin.Current);
            String128TableAddress = br.ReadInt32();
            StringCount = br.ReadInt32();
            StringTableLength = br.ReadInt32();
            StringIndexAddress = br.ReadInt32();
            StringTableAddress = br.ReadInt32();
            stream.Seek(36, SeekOrigin.Current);
            Name = Encoding.UTF8.GetString(br.ReadBytes(36)).TrimEnd(char.MinValue);
            Scenario = Encoding.UTF8.GetString(br.ReadBytes(256)).TrimEnd(char.MinValue);
            stream.Seek(4, SeekOrigin.Current);
            PathCount = br.ReadInt32();
            PathTableAddress = br.ReadInt32();
            PathTableLength = br.ReadInt32();
            PathIndexAddress = br.ReadInt32();
            Checksum = br.ReadUInt32();
            stream.Seek(1320, SeekOrigin.Current);
            if (br.ReadTagType() != "foot") throw new InvalidDataException();
        }

        public Header(Map map)
            : this(map.BaseStream) { }

        public byte[] ToByteArray()
        {
            MemoryStream stream = new MemoryStream(Header.Length);
            BinaryWriter bw = new BinaryWriter(stream);
            stream.Seek(0, SeekOrigin.Begin);
            bw.Write(new TagType("head"));
            bw.Write(EngineVersion);
            stream.Seek(8, SeekOrigin.Current);
            bw.Write(IndexAddress);
            bw.Write(IndexLength);
            bw.Write(TagCacheLength);
            bw.Write(TotalCacheLength);
            stream.Seek(256, SeekOrigin.Current);
            bw.Write(Encoding.UTF8.GetBytes(BuildDate));
            stream.Seek(32 - Encoding.UTF8.GetByteCount(BuildDate), SeekOrigin.Current);
            bw.Write((int)Type);
            stream.Seek(28, SeekOrigin.Current);
            bw.Write(String128TableAddress);
            bw.Write(StringCount);
            bw.Write(StringTableLength);
            bw.Write(StringIndexAddress);
            bw.Write(StringTableAddress);
            stream.Seek(36, SeekOrigin.Current);
            if (Encoding.UTF8.GetByteCount(Name) > 36) throw new IndexOutOfRangeException();
            bw.Write(Encoding.UTF8.GetBytes(Name));
            stream.Seek(36 - Encoding.UTF8.GetByteCount(Name), SeekOrigin.Current);
            if (Encoding.UTF8.GetByteCount(Scenario) > 256) throw new IndexOutOfRangeException();
            bw.Write(Encoding.UTF8.GetBytes(Scenario));
            stream.Seek(256 - Encoding.UTF8.GetByteCount(Scenario), SeekOrigin.Current);
            stream.Seek(4, SeekOrigin.Current);
            bw.Write(PathCount);
            bw.Write(PathTableAddress);
            bw.Write(PathTableLength);
            bw.Write(PathIndexAddress);
            bw.Write(Checksum);
            stream.Seek(1320, SeekOrigin.Current);
            bw.Write(new TagType("foot"));
            return stream.ToArray();
        }

        public enum MapType : int
        {
            Campaign = 0,
            Multiplayer = 1,
            Mainmenu = 2,
            Shared = 3,
            Single_Player_Shared = 4,
        }
    }

    public class UnicodeTable : List<UnicodeTable.Entry>
    {
        public UnicodeTable()
            : base() { }

        public UnicodeTable(Map map, Language language)
            : base()
        {
            map.Seek(map.Index.GlobalsTagId);
            map.BaseStream.Seek(400 + ((int)language * 28), SeekOrigin.Current);
            BinaryReader br = new BinaryReader(map.BaseStream);
            int count = br.ReadInt32();
            int tableLength = br.ReadInt32();
            int indexAddress = br.ReadInt32();
            int tableAddress = br.ReadInt32();
            this.Capacity = count;
            StringId[] strRefs = new StringId[count];
            int[] strOffsets = new int[count];
            map.BaseStream.Seek(indexAddress, SeekOrigin.Begin);
            for (int i = 0; i < count; i++)
            {
                strRefs[i] = br.ReadStringReference();
                strOffsets[i] = br.ReadInt32();
            }
            for (int i = 0; i < count; i++)
            {
                map.BaseStream.Seek(tableAddress + strOffsets[i], SeekOrigin.Begin);
#if DEBUG
                if (map.BaseStream.Position >= tableAddress + tableLength) throw new Exception();
#endif
                StringBuilder unicodeString = new StringBuilder(byte.MaxValue);
                while (br.PeekChar() != char.MinValue)
                    unicodeString.Append(br.ReadChar());
                this.Add(new Entry() { StringReference = strRefs[i], Value = unicodeString.ToString() });
            }
        }

        public enum Language
        {
            English = 0,
            Japanese = 1,
            Dutch = 2,
            French = 3,
            Spanish = 4,
            Italian = 5,
            Korean = 6,
            Chinese = 7,
            Portuguese = 8,
        }

        public class Entry
        {
            public StringId StringReference;
            public string Value;

            public override string ToString()
            {
                return Value;
            }
        }
    }
}