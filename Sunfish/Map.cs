using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using Sunfish.TagStructures;

namespace Sunfish
{
    public class Map
    {
        public const string Extension = ".map";

        public HeaderStruct Header;
        public TagIndex Index;
        public UnicodeTable EnglishUnicode;
        public Stream BaseStream;
        public string[] Tagnames;
        public List<string> StringIdNames;

        public int PrimaryMagic;
        public int SecondaryMagic;

        public Map()
        {
            Header = HeaderStruct.Default;
            Index = new TagIndex();
            EnglishUnicode = new UnicodeTable();
            Tagnames = new string[0];
            StringIdNames = new List<string>();
        }

        public Map(Stream stream)
        {
            BaseStream = stream;
            BaseStream.Position = 0;
            BinaryReader br = new BinaryReader(BaseStream);
            Header = HeaderStruct.RawDeserialize(br.ReadBytes(Marshal.SizeOf(typeof(HeaderStruct))), 0);
            Index = new TagIndex(BaseStream, Header.IndexAddress);
            SecondaryMagic = Index.TagEntries[0].VirtualAddress - (Header.IndexAddress + Header.IndexLength);
            Goto(Index.ScenarioTagId);
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
                int sbspId = br.ReadInt32();
                BaseStream.Position += 4;
                int ltmpId = br.ReadInt32();
                BaseStream.Position = blockOffset + 8;
                int ltmpOffset = br.ReadInt32();
                int ltmpSize = (int)((blockOffset + blockSize) - (ltmpOffset - PrimaryMagic));
                Map.TagIndex.TagInfo tagEntry = Index.TagEntries[sbspId & 0x0000FFFF];
                tagEntry.VirtualAddress = virtualOffset;
                tagEntry.Length = blockSize - ltmpSize;
                Index.TagEntries[sbspId & 0x0000FFFF] = tagEntry;
                tagEntry = Index.TagEntries[ltmpId & 0x0000FFFF];
                tagEntry.VirtualAddress = ltmpOffset;
                tagEntry.Length = ltmpSize;
                Index.TagEntries[ltmpId & 0x0000FFFF] = tagEntry;
            }
            BaseStream.Position = Header.FilenameTableAddress;
            Tagnames = Encoding.UTF8.GetString(br.ReadBytes(Header.FilenameTableLength - 1)).Split(char.MinValue);
            BaseStream.Position = Header.StringIdTableAddress;
            StringIdNames = new List<string>(Encoding.UTF8.GetString(br.ReadBytes(Header.StringIdTableLength - 1)).Split(char.MinValue));
            EnglishUnicode = new UnicodeTable(this);
        }

        public void Goto(int tagIndex)
        {
            int index = tagIndex & 0x0000FFFF;
            if (Index.TagEntries[index].Type == "sbsp" || Index.TagEntries[index].Type == "ltmp")
                BaseStream.Position = Index.TagEntries[index].VirtualAddress - PrimaryMagic;
            else BaseStream.Position = Index.TagEntries[index].VirtualAddress - SecondaryMagic;
        }

        public int IndexOfTagEntry(string type, string tagname)
        {
            for (int i = 0; i < Index.TagEntries.Length; i++)
                if (Index.TagEntries[i].Type == type && Tagnames[i] == tagname) return i;
            return -1;
        }

        public class TagIndex
        {
            const int HeaderSize = 32;

            public int TagTypeArrayVirtualAddress;
            public int TagTypeCount;
            public int TagInfoArrayVirtualAddress;
            public int ScenarioTagId;
            public int GlobalsTagId;
            public int TagInfoCount;
            readonly byte[] FourCC;

            public static string[] Types;
            public TagInfo[] TagEntries;

            public int Length
            {
                get
                {
                    int indexSize = 32 + (TagTypeCount * 12) + (TagInfoCount * 16);
                    int padding = Padding.GetCount(indexSize, 512);
                    return indexSize + padding;
                }
            }

            static TagIndex()
            {
                Types = new string[] {                 
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
                for (int i = 0; i < Map.TagIndex.Types.Length; i++)
                    TagTypesDictionary.Add(GetCleanType(Map.TagIndex.Types[i]), Map.TagIndex.Types[i]);
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

            public TagIndex()
            {
                FourCC = Encoding.UTF8.GetBytes("sgat");
                TagTypeCount = Types.Length;
            }

            public TagIndex(Stream stream, int offset)
            {
                stream.Position = offset;
                BinaryReader br = new BinaryReader(stream);
                TagTypeArrayVirtualAddress = br.ReadInt32();
                TagTypeCount = br.ReadInt32();
                TagInfoArrayVirtualAddress = br.ReadInt32();
                ScenarioTagId = br.ReadInt32();
                GlobalsTagId = br.ReadInt32();
                br.ReadInt32();
                TagInfoCount = br.ReadInt32();
                FourCC = br.ReadBytes(4);
                byte[] buffer = br.ReadBytes(TagTypeCount * 12);
                Types = new string[TagTypeCount];
                for (int i = 0; i < TagTypeCount; i++)
                    Types[i] = Globals.ReverseString(Encoding.UTF8.GetString(buffer, i * 12, 4));
                buffer = br.ReadBytes(TagInfoCount * 16);
                TagEntries = new TagInfo[TagInfoCount];
                for (int i = 0; i < TagInfoCount; i++)
                    TagEntries[i] = new TagInfo(buffer, i * 16);
            }

            public struct TagType
            {
                public readonly string Name;
                public readonly string BaseType0;
                public readonly string BaseType1;

                public TagType(byte[] rawData, int offset)
                {
                    Name = Globals.ReverseString(Encoding.UTF8.GetString(rawData, offset, 4));
                    BaseType0 = Globals.ReverseString(Encoding.UTF8.GetString(rawData, offset + 4, 4));
                    BaseType1 = Globals.ReverseString(Encoding.UTF8.GetString(rawData, offset + 8, 4));
                }
            }

            public struct TagInfo
            {
                public string Type;
                public int Id;
                public int VirtualAddress;
                public int Length;

                public TagInfo(byte[] rawData, int position)
                {
                    Type = Globals.ReverseString(Encoding.UTF8.GetString(rawData, position, 4));
                    Id = BitConverter.ToInt32(rawData, position + 4);
                    VirtualAddress = BitConverter.ToInt32(rawData, position + 8);
                    Length = BitConverter.ToInt32(rawData, position + 12);
                }

                public byte[] ToArray()
                {
                    byte[] buffer = new byte[16];
                    MemoryStream ms = new MemoryStream(buffer);
                    BinaryWriter br = new BinaryWriter(ms);
                    byte[] typeBytes = Encoding.UTF8.GetBytes(Type);
                    Array.Reverse(typeBytes);
                    br.Write(typeBytes, 0, 4);
                    br.Write(Id);
                    br.Write(VirtualAddress);
                    br.Write(Length);
                    br.Close();
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

                string baseType;
                byte[] zero = new byte[] { 0x00, 0x00, 0x00, 0x00 };
                byte[] typeBuffer;
                foreach (string type in Types)
                {
                    typeBuffer = Encoding.UTF8.GetBytes(type);
                    Array.Reverse(typeBuffer);
                    bw.Write(typeBuffer, 0, 4);
                    baseType = GetBaseType(type);
                    if (baseType == null)
                    {
                        bw.Write(zero);
                        bw.Write(zero);
                    }
                    else
                    {
                        typeBuffer = Encoding.UTF8.GetBytes(baseType);
                        Array.Reverse(typeBuffer);
                        bw.Write(typeBuffer, 0, 4);
                        baseType = GetBaseType(baseType);
                        if (baseType == null)
                        {
                            bw.Write(zero);
                        }
                        else
                        {
                            typeBuffer = Encoding.UTF8.GetBytes(baseType);
                            Array.Reverse(typeBuffer);
                            bw.Write(typeBuffer, 0, 4);
                        }
                    }
                }

                foreach (TagInfo entry in TagEntries)
                    bw.Write(entry.ToArray());
                bw.Close();
                return buffer;
            }

            public string GetBaseType(string type)
            {
                switch (type)
                {
                    case "item":
                    case "unit":
                    case "ssce":
                    case "devi":
                    case "scen":
                    case "proj":
                    case "crea":
                    case "bloc":
                        return "obje";
                    case "weap":
                    case "garb":
                    case "eqip":
                        return "item";
                    case "vehi":
                    case "bipd":
                        return "unit";
                    case "mach":
                    case "lifi":
                    case "ctrl":
                        return "devi";
                    default: return null;
                }
            }
        }

        [StructLayout(LayoutKind.Explicit, Size = 2048)]
        public struct HeaderStruct
        {
            public string Name { get { return Encoding.UTF8.GetString(name).Trim(char.MinValue); } }
            public string Scenario { get { return Encoding.UTF8.GetString(scenario).Trim(char.MinValue); } }

            [FieldOffset(0)]
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] HeaderFourCC;
            [FieldOffset(4)]
            public int EngineVersion;
            [FieldOffset(8)]
            public int Filesize;
            [FieldOffset(16)]
            public int IndexAddress;
            [FieldOffset(20)]
            public int IndexLength;
            [FieldOffset(24)]
            public int MetaTableSize;
            [FieldOffset(28)]
            public int TagDataSize;
            [FieldOffset(288)]
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public byte[] BuildDate;
            [FieldOffset(320)]
            public int Type;
            [FieldOffset(340)]
            public int CrazyTableLength;
            [FieldOffset(344)]
            public int CrazyTableAddress;
            [FieldOffset(352)]
            public int StringId128TableAddress;
            [FieldOffset(356)]
            public int StringIdCount;
            [FieldOffset(360)]
            public int StringIdTableLength;
            [FieldOffset(364)]
            public int StringIdIndexAddress;
            [FieldOffset(368)]
            public int StringIdTableAddress;
            [FieldOffset(408)]
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 36)]
            byte[] name;
            [FieldOffset(444)]
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
            byte[] scenario;
            [FieldOffset(704)]
            public int TagCount;
            [FieldOffset(708)]
            public int FilenameTableAddress;
            [FieldOffset(712)]
            public int FilenameTableLength;
            [FieldOffset(716)]
            public int FilenameIndexOffset;
            [FieldOffset(720)]
            public uint Checksum;
            [FieldOffset(2044)]
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] FooterFourCC;

            public static HeaderStruct Default
            { get { return new HeaderStruct(true); } }

            private HeaderStruct(bool b)
            {
                HeaderFourCC = Encoding.UTF8.GetBytes("daeh");
                EngineVersion = 8;
                Filesize = 0;
                IndexAddress = 0;
                IndexLength = 0;
                MetaTableSize = 0;
                TagDataSize = 0;

                BuildDate = new byte[36];
                byte[] buildDate = Encoding.UTF8.GetBytes("02.09.27.09809");
                for (int i = 0; i < buildDate.Length; i++)
                    BuildDate[i] = buildDate[i];

                Type = 1;
                CrazyTableLength = 0;
                CrazyTableAddress = 0;
                StringId128TableAddress = 0;
                StringIdCount = 0;
                StringIdTableLength = 0;
                StringIdIndexAddress = 0;
                StringIdTableAddress = 0;
                name = new byte[36];
                scenario = new byte[256];
                TagCount = 0;
                FilenameTableAddress = 0;
                FilenameTableLength = 0;
                FilenameIndexOffset = 0;
                Checksum = 0;
                FooterFourCC = Encoding.UTF8.GetBytes("toof");
            }

            public static HeaderStruct RawDeserialize(byte[] rawData, int position)
            {
                int rawsize = Marshal.SizeOf(typeof(HeaderStruct));
                if (rawsize > rawData.Length) throw new Exception();
                IntPtr buffer = Marshal.AllocHGlobal(rawsize);
                Marshal.Copy(rawData, position, buffer, rawsize);
                HeaderStruct header = (HeaderStruct)Marshal.PtrToStructure(buffer, typeof(HeaderStruct));
                Marshal.FreeHGlobal(buffer);
                return header;
            }

            public static byte[] RawSerialize(HeaderStruct header)
            {
                int rawSize = Marshal.SizeOf(header);
                IntPtr buffer = Marshal.AllocHGlobal(rawSize);
                Marshal.StructureToPtr(header, buffer, false);
                byte[] rawDatas = new byte[rawSize];
                Marshal.Copy(buffer, rawDatas, 0, rawSize);
                Marshal.FreeHGlobal(buffer);
                return rawDatas;
            }
        }

        public class UnicodeTable
        {
            public List<UnicodeEntry> Items;

            public UnicodeTable()
            {
                Items = new List<UnicodeEntry>();
            }

            public UnicodeTable(Map map)
            {
                map.Goto(map.Index.GlobalsTagId);
                map.BaseStream.Position += 400;//English Location
                BinaryReader br = new BinaryReader(map.BaseStream);
                int stringCount = br.ReadInt32();
                int tableSize = br.ReadInt32();
                int indexOffset = br.ReadInt32();
                int tableOffset = br.ReadInt32();
                Items = new List<UnicodeEntry>(stringCount);
                for (int i = 0; i < stringCount; i++)
                {
                    map.BaseStream.Position = indexOffset + (i * 8);
                    int sid = br.ReadInt32();
                    int strOffset = br.ReadInt32();
                    map.BaseStream.Position = tableOffset + strOffset;
                    string value = "";
                    char c;
                    while (true)
                    {
                        c = br.ReadChar();
                        if (c == Char.MinValue) break;
                        value += c;
                    }
                    Items.Add(new UnicodeEntry() { stringID = sid, unicodeString = value });
                }
            }

            public class UnicodeEntry
            {
                public int stringID;
                public string unicodeString;

                public override string ToString()
                {
                    return unicodeString;
                }
            }
        }
    }
}
