using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Sunfish.Developmental;
using System.Threading;

namespace Sunfish
{
    public class Decompiler
    {
        Map map;
        MemoryStream bufferStream;
        BinaryReader binReader;
        BinaryWriter binWriter;
        Cache<Pointer> pointerCache;
        Cache<Value> valueCache;
        Cache<ResourceReference> rawCache;
        string type;
        int metaLength;
        int rawAddress;
        List<int> stringReferences;
        List<int> idReferences;
        List<ResourceInfo> rawReferences;

        public Decompiler(Map map)
        {
            this.map = map;
            Initialize();
        }

        void Initialize()
        {
            bufferStream = new MemoryStream(1048576);
            pointerCache = new Cache<Pointer>(10240);
            valueCache = new Cache<Value>(10240);
            rawCache = new Cache<ResourceReference>(10240);
            rawReferences = new List<ResourceInfo>(1024);
            stringReferences = new List<int>(4096);
            idReferences = new List<int>(1024);
        }

        void ResetBuffers()
        {
            bufferStream.Position = 0;
            stringReferences.Clear();
            idReferences.Clear();
            rawReferences.Clear();
            valueCache.Clear();
            pointerCache.Clear();
            rawCache.Clear();
        }

        public void DecompileUnic(Index.TagInformation tag, string filename, int magic, UnicodeTable unicodeTable)
        {
            ResetBuffers();

            binReader = new BinaryReader(map.BaseStream);
            binWriter = new BinaryWriter(bufferStream);

            type = tag.Type.ToString();

            Block block = Blocks.Types[type];

            pointerCache.Add(new Pointer());
            ProcessBlock(block, 1, tag.VirtualAddress, magic);

            binReader = new BinaryReader(bufferStream);
            bufferStream.Position = 16;
            int offset = binReader.ReadInt16();
            int count = binReader.ReadInt16();

            List<byte> unicodeBytes = new List<byte>();
            for (int i = 0; i < count; i++)
            {
                bufferStream.Position = 16 + (i * 40);
                binWriter.Write(map.Unicode[UnicodeTable.Language.English][offset + i].StringReference);
                binWriter.Write(unicodeBytes.Count);
                binWriter.Write(new byte[32]);
                unicodeBytes.AddRange(Encoding.UTF8.GetBytes(map.Unicode[UnicodeTable.Language.English][offset + i].Value));
                unicodeBytes.Add(byte.MinValue);
            }

            bufferStream.Position = 16 + (count * 40);
            binWriter.Write(unicodeBytes.ToArray());
            binWriter.Write(Padding.GetBytes(bufferStream.Position, 4));

            metaLength = (int)bufferStream.Position;

            bufferStream.Position = 0;
            binWriter.Write(count);
            binWriter.Write(16);
            binWriter.Write(unicodeBytes.Count);
            binWriter.Write(16 + (count * 40));

            stringReferences.Clear();
            idReferences.Clear();
            rawReferences.Clear();
            valueCache.Clear();
            pointerCache.Clear();
            rawCache.Clear();

            type = "utf8";
            block = Blocks.Types[type];

            pointerCache.Add(new Pointer());
            ProcessBlock(block, 1, 0);

            ProcessValues();
            ProcessPointers();
            ProcessRaws();

            binWriter.Flush();

            BufferedWriteFile(filename, stringReferences.ToArray(), idReferences.ToArray(), rawReferences.ToArray());
        }

        public void Decompile(Index.TagInformation tag, string filename, int magic)
        {
            ResetBuffers();

            binReader = new BinaryReader(map.BaseStream);
            binWriter = new BinaryWriter(bufferStream);

            type = tag.Type.ToString();


            Block block = Blocks.Types[type];

            pointerCache.Add(new Pointer());

            ProcessBlock(block, 1, tag.VirtualAddress, magic);

            metaLength = (int)bufferStream.Position;

            binReader = new BinaryReader(bufferStream);
            ProcessValues();
            ProcessPointers();
            ProcessRaws();

            BufferedWriteFile(filename, stringReferences.ToArray(), idReferences.ToArray(), rawReferences.ToArray());

            //FileWriter fw = new FileWriter(WriteFile);
            //queuedWrites++;
            //fw.BeginInvoke(Path.ChangeExtension(map.Tagnames[tag.Id & 0x0000FFFF], Globals.GetCleanType(tag.Type).Trim()), stringReferences.ToArray(), idReferences.ToArray(), rawReferences.ToArray(), AfterWrite, null);

            //binWriter.Flush();
        }

        private delegate void FileWriter(string filename, byte[] buffer);

        public void BufferedWriteFile(string filename, int[] stringReferences, int[] idReferences, ResourceInfo[] rawReferences)
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter memWriter = new BinaryWriter(stream);
            BinaryReader binReader = new BinaryReader(map.BaseStream);

            Tag.Info fileHeader = new Tag.Info();
            stream.Position = Tag.Info.Size;
            fileHeader.type = this.type;

            fileHeader.cacheInformationOffset = (int)stream.Position;
            memWriter.Write(false);
            memWriter.Write(Padding.GetBytes(stream.Position, 4));
            memWriter.Write(rawCache.Count);
            for (int i = 0; i < rawCache.Count; i++)
                memWriter.Write(rawCache.Values[i].Offset0);
            int sidCount = 0, tidCount = 0;
            for (int i = 0; i < valueCache.Count; i++)
                switch (valueCache.Values[i].Type)
                {
                    case Value.ValueType.StringId:
                        sidCount++; break;
                    case Value.ValueType.TagId:
                        tidCount++; break;
                    case Value.ValueType.TagReference:
                        tidCount++; break;
                }
            memWriter.Write(sidCount);
            for (int i = 0; i < valueCache.Count; i++)
                if (valueCache.Values[i].Type == Value.ValueType.StringId)
                    memWriter.Write(valueCache.Values[i].Offset); 
            memWriter.Write(tidCount);
            for (int i = 0; i < valueCache.Count; i++)
                if (valueCache.Values[i].Type == Value.ValueType.TagId || valueCache.Values[i].Type == Value.ValueType.TagReference)
                    memWriter.Write(valueCache.Values[i].Offset);

            memWriter.Write(Padding.GetBytes(stream.Position, 512));


            fileHeader.metaLength = metaLength;
            fileHeader.metaOffset = (int)stream.Position;
            memWriter.Write(bufferStream.GetBuffer(), 0, fileHeader.metaLength);

            if (rawReferences.Length > 0)
            {
                memWriter.Write(Padding.GetBytes(stream.Position, 512));
                fileHeader.rawOffset = (int)stream.Position;
                fileHeader.rawLength = (int)stream.Position;
                for (int i = 0; i < rawReferences.Length; i++)
                {
                    map.BaseStream.Position = rawReferences[i].Address;
                    memWriter.Write(binReader.ReadBytes(rawReferences[i].Length & 0x7FFFFFFF));
                    memWriter.Write(Padding.GetBytes(stream.Position, 512));
                }
                fileHeader.rawLength = (int)stream.Position - fileHeader.rawLength;

                binReader = new BinaryReader(bufferStream);

                memWriter.Write(Padding.GetBytes(stream.Position, 512));
                fileHeader.rawReferencesCount = rawReferences.Length;
                fileHeader.rawReferencesOffset = (int)stream.Position;
                rawAddress = 0;
                for (int i = 0; i < rawReferences.Length; i++)
                {
                    memWriter.Write(rawAddress);
                    memWriter.Write(rawReferences[i].Length);

                    rawAddress += rawReferences[i].Length & 0x7FFFFFFF;
                    rawAddress += Padding.GetCount(rawAddress, 512);
                }
            }

            if (stringReferences.Length > 0)
            {
                memWriter.Write(Padding.GetBytes(stream.Position, 512));
                fileHeader.stringReferencesOffset = (int)stream.Position;
                fileHeader.stringReferencesCount = stringReferences.Length;
                for (int i = 0; i < stringReferences.Length; i++)
                {
                    string str = map.StringIdNames[stringReferences[i] & 0x0000FFFF];
                    memWriter.Write(Encoding.UTF8.GetBytes(str));
                    memWriter.Write(byte.MinValue);
                }
                if (stringReferences.Length > 1)
                    stream.Position -= 1;
                fileHeader.stringReferencesLength = (int)stream.Position - fileHeader.stringReferencesOffset;
            }

            if (idReferences.Length > 0)
            {
                memWriter.Write(Padding.GetBytes(stream.Position, 512));
                fileHeader.idReferencesOffset = (int)stream.Position;
                fileHeader.idReferencesCount = idReferences.Length;
                for (int i = 0; i < idReferences.Length; i++)
                {
                    string type = map.Index.TagEntries[idReferences[i] & 0x0000FFFF].Type.ToString();
                    type = Index.GetCleanType(type).Trim();
                    string tagname = map.Tagnames[idReferences[i] & 0x0000FFFF];
                    memWriter.Write(Encoding.UTF8.GetBytes(Path.ChangeExtension(tagname, type)));
                    memWriter.Write(Encoding.UTF8.GetBytes(Tag.Path.Extension));
                    memWriter.Write(byte.MinValue);
                }
                stream.Position -= 1;
                fileHeader.idReferencesLength = (int)stream.Position - fileHeader.idReferencesOffset;
            }

            memWriter.Write(Padding.GetBytes(stream.Position, 512));
            stream.Position = 0;
            memWriter.Write(fileHeader.ToByteArray());

            WriteFileAsync(filename, stream.ToArray());
            //if (result != null)
            //    fw.EndInvoke(result);
            //result = fw.BeginInvoke(filename, stream.ToArray(), null, null);
        }

        public void WriteFileAsync(string filename, byte[] buffer)
        {
            string dir = Path.GetDirectoryName(filename);
            if (!Directory.Exists(dir) && dir != string.Empty)
                Directory.CreateDirectory(dir);
            FileStream file = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
            
            using (file)
            {
                file.Write(buffer, 0, (int)buffer.Length);
                file.Flush();
                file.SetLength(buffer.Length);
            }
        }

        public void WriteFile(string filename, int[] stringReferences, int[] idReferences, ResourceInfo[] rawReferences)
        {
            if (!Directory.Exists(filename))
                Directory.CreateDirectory(filename);
            FileStream file = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None);
            using (file)
            {
                BinaryWriter binWriter = new BinaryWriter(file);
                BinaryReader binReader = new BinaryReader(map.BaseStream);

                Tag.Info fileHeader = new Tag.Info();
                file.Position = Tag.Info.Size;
                fileHeader.type = this.type;

                fileHeader.metaLength = metaLength;
                fileHeader.metaOffset = (int)file.Position;
                binWriter.Write(bufferStream.GetBuffer(), 0, fileHeader.metaLength);

                if (rawReferences.Length > 0)
                {
                    binWriter.Write(Padding.GetBytes(file.Position, 512));
                    fileHeader.rawOffset = (int)file.Position;
                    fileHeader.rawLength = (int)file.Position;
                    for (int i = 0; i < rawReferences.Length; i++)
                    {
                        map.BaseStream.Position = rawReferences[i].Address;
                        binWriter.Write(binReader.ReadBytes(rawReferences[i].Length & 0x7FFFFFFF));
                        binWriter.Write(Padding.GetBytes(file.Position, 512));
                    }
                    fileHeader.rawLength = (int)file.Position - fileHeader.rawLength;

                    binReader = new BinaryReader(bufferStream);

                    binWriter.Write(Padding.GetBytes(file.Position, 512));
                    fileHeader.rawReferencesCount = rawReferences.Length;
                    fileHeader.rawReferencesOffset = (int)file.Position;
                    rawAddress = 0;
                    for (int i = 0; i < rawReferences.Length; i++)
                    {
                        binWriter.Write(rawAddress);
                        binWriter.Write(rawReferences[i].Length);

                        rawAddress += rawReferences[i].Length & 0x7FFFFFFF;
                        rawAddress += Padding.GetCount(rawAddress, 512);
                    }
                }

                if (stringReferences.Length > 0)
                {
                    binWriter.Write(Padding.GetBytes(file.Position, 512));
                    fileHeader.stringReferencesOffset = (int)file.Position;
                    fileHeader.stringReferencesCount = stringReferences.Length;
                    for (int i = 0; i < stringReferences.Length; i++)
                    {
                        string str = map.StringIdNames[stringReferences[i] & 0x0000FFFF];
                        binWriter.Write(Encoding.UTF8.GetBytes(str));
                        binWriter.Write(byte.MinValue);
                    }
                    if (stringReferences.Length > 1)
                        file.Position -= 1;
                    fileHeader.stringReferencesLength = (int)file.Position - fileHeader.stringReferencesOffset;
                }

                if (idReferences.Length > 0)
                {
                    binWriter.Write(Padding.GetBytes(file.Position, 512));
                    fileHeader.idReferencesOffset = (int)file.Position;
                    fileHeader.idReferencesCount = idReferences.Length;
                    for (int i = 0; i < idReferences.Length; i++)
                    {
                        string type = map.Index.TagEntries[idReferences[i] & 0x0000FFFF].Type.ToString();
                        type = Index.GetCleanType(type).Trim();
                        string tagname = map.Tagnames[idReferences[i] & 0x0000FFFF];
                        binWriter.Write(Encoding.UTF8.GetBytes(Path.ChangeExtension(tagname, type)));
                        binWriter.Write(byte.MinValue);
                    }
                    file.Position -= 1;
                    fileHeader.idReferencesLength = (int)file.Position - fileHeader.idReferencesOffset;
                }

                binWriter.Write(Padding.GetBytes(file.Position, 512));
                file.Position = 0;
                binWriter.Write(fileHeader.ToByteArray());
                binWriter.Flush();
            }
        }

        void ProcessPointers()
        {
            for (int i = 1; i < pointerCache.Count; i++)
            {
                bufferStream.Position = pointerCache.Values[i].Offset;
                binWriter.Write(pointerCache.Values[i].Address);
            }
        }

        void ProcessValues()
        {
            int value;
            for (int i = 0; i < valueCache.Count; i++)
            {
                bufferStream.Position = valueCache.Values[i].Offset;
                switch (valueCache.Values[i].Type)
                {
                    case Value.ValueType.StringId:
                        value = binReader.ReadInt32();
                        if (!stringReferences.Contains(value))
                            stringReferences.Add(value);
                        binReader.BaseStream.Position -= 4;
                        binWriter.Write(stringReferences.IndexOf(value));
                        break;
                    case Value.ValueType.TagId:
                        value = binReader.ReadInt32();
                        if (value == -1) continue;
                        if (!idReferences.Contains(value))
                            idReferences.Add(value);
                        binReader.BaseStream.Position -= 4;
                        binWriter.Write(idReferences.IndexOf(value));
                        break;
                    case Value.ValueType.TagReference:
                        binReader.BaseStream.Position += 4;
                        value = binReader.ReadInt32();
                        if (value == -1) continue;
                        if (!idReferences.Contains(value))
                            idReferences.Add(value);
                        binReader.BaseStream.Position -= 8;
                        binWriter.Write(0xFFFFFFFF);
                        binWriter.Write(idReferences.IndexOf(value));
                        break;
                }
            }
        }

        void ProcessRaws()
        {
            for (int i = 0; i < rawCache.Count; i++)
            {
                bufferStream.Position = rawCache.Values[i].Offset0;
                int count = binReader.ReadInt32();
                bufferStream.Position = rawCache.Values[i].Offset1;
                int address = binReader.ReadInt32();
                if ((address & 0xC0000000) == 0)
                {
                    bufferStream.Position = rawCache.Values[i].Offset0;
                    binWriter.Write(0x00000000);
                    bufferStream.Position = rawCache.Values[i].Offset1;
                    binWriter.Write(rawReferences.Count);

                    rawReferences.Add(new ResourceInfo() { Length = count, Address = address });
                }
            }
        }

        private void ProcessBlock(Block block, int count, int address)
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

            for (int i = 0; i < count; i++)
            {
                foreach (Block b in block.NestedBlocks)
                {
                    binReader.BaseStream.Position = address + (i * block.Size) + b.Offset;
                    int nestedBlockCount = binReader.ReadInt32();
                    if (nestedBlockCount == 0) continue;
                    int nestedBlockAddress = binReader.ReadInt32();
                    pointerCache.Add(new Pointer() { Offset = address + (i * block.Size) + b.Offset + 4, Address = nestedBlockAddress });
                    ProcessBlock(b, nestedBlockCount, nestedBlockAddress);
                }
            }

            #endregion
        }

        void ProcessBlock(Block curBlock, int count, int address, int magic)
        {
            binReader.BaseStream.Position = address - magic;

            binWriter.Write(Padding.GetBytes(bufferStream.Position, curBlock.Alignment));

            int blockAddress = (int)binWriter.BaseStream.Position;

            pointerCache.Values[pointerCache.Count - 1].Address = blockAddress;
            binWriter.Write(binReader.ReadBytes(curBlock.Size * count));

            #region Cache Values

            if (curBlock.Values.Length > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    foreach (Value v in curBlock.Values)
                    {
                        Value val = v;
                        val.Offset = blockAddress + (curBlock.Size * i) + v.Offset;
                        valueCache.Add(val);
                    }
                }
            }

            #endregion

            #region Cache Raws

            if (curBlock.Raws.Length > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    foreach (ResourceReference r in curBlock.Raws)
                    {
                        ResourceReference raw = r;
                        raw.Offset0 = blockAddress + (curBlock.Size * i) + r.Offset0;
                        raw.Offset1 = blockAddress + (curBlock.Size * i) + r.Offset1;
                        rawCache.Add(raw);
                    }
                }
            }

            #endregion

            #region Process Nested Blocks

            if (curBlock.NestedBlocks.Length > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    foreach (Block b in curBlock.NestedBlocks)
                    {
                        binReader.BaseStream.Position = (address - magic) + (i * curBlock.Size) + b.Offset;
                        int nestedBlockCount = binReader.ReadInt32();
                        if (nestedBlockCount == 0) continue;
                        pointerCache.Add(new Pointer() { Offset = blockAddress + (i * curBlock.Size) + b.Offset + 4 });
                        int nestedBlockAddress = binReader.ReadInt32();
                        ProcessBlock(b, nestedBlockCount, nestedBlockAddress, magic);
                    }
                }
            }

            #endregion
        }
    }
}
