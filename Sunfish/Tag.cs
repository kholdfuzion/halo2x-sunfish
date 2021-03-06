﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Sunfish.TagStructures;
using Sunfish.Developmental;

namespace Sunfish
{
    public class CompilerTag:Tag
    {
        public CompilerTag(string filename)
        :base(filename)
        { }

        public int ID;
    }

    public class Tag
    {
        /// <summary>
        /// Relative filename of the tag
        /// </summary>
        public string Filename
        {
            get { return filename; }
            set
            {
                    for (int i = 0; i < TagReferences.Count; i++)
                    {
                        if (TagReferences[i] == filename)
                            TagReferences[i] = value;
                    }
                filename = value;
            }
        }
        string filename;
        /// <summary>
        /// FourCC type
        /// </summary>
        public string Type;
        /// <summary>
        /// Stream containing tag meta data
        /// </summary>
        public MemoryStream TagStream;
        /// <summary>
        /// Stream containing tag raw data(s)
        /// </summary>
        public MemoryStream ResourceStream;
        /// <summary>
        /// Array of RawInfos, use to get offset of raw in RawStream and raw length
        /// </summary>
        public ResourceInfo[] ResourceInformation;
        /// <summary>
        /// Array of stringId names
        /// </summary>
        public List<string> Strings = new List<string>();
        /// <summary>
        /// Array of filenames, which are the tags this tag has references to
        /// </summary>
        public List<string> TagReferences = new List<string>();

        public bool IsCached;
        public int[] RawOffsets;
        public int[] StringIdOffsets;
        public int[] TagReferenceOffsets;
        public int[] PointerOffsets;

        public Tag()
        {
            TagStream = new MemoryStream();
            ResourceStream = new MemoryStream();
            ResourceInformation = new ResourceInfo[0];
            Strings = new List<string>();
            TagReferences = new List<string>();
        }

        /// <summary>
        /// Create Tag from filestream
        /// </summary>
        /// <param name="filename">path to file which contains Tag data</param>
        public Tag(string filename)
        {
            FileStream file = new FileStream(filename, FileMode.Open);
            using (file)
            {
                BinaryReader binReader = new BinaryReader(file);

                Info header = new Info(file);
                Filename = filename;
                Type = header.type;

                file.Position = header.cacheInformationOffset;
                IsCached = binReader.ReadBoolean();
                file.Seek(3, SeekOrigin.Current);
                RawOffsets = new int[binReader.ReadInt32()];
                for (int i = 0; i < RawOffsets.Length; i++)
                    RawOffsets[i] = binReader.ReadInt32();
                StringIdOffsets = new int[binReader.ReadInt32()];
                for (int i = 0; i < StringIdOffsets.Length; i++)
                    StringIdOffsets[i] = binReader.ReadInt32();
                TagReferenceOffsets = new int[binReader.ReadInt32()];
                for (int i = 0; i < TagReferenceOffsets.Length; i++)
                    TagReferenceOffsets[i] = binReader.ReadInt32();
                PointerOffsets = new int[binReader.ReadInt32()];
                for (int i = 0; i < PointerOffsets.Length; i++)
                    PointerOffsets[i] = binReader.ReadInt32();

                file.Position = header.metaOffset;
                TagStream = new MemoryStream(binReader.ReadBytes(header.metaLength));
                file.Position = header.rawOffset;
                ResourceStream = new MemoryStream(binReader.ReadBytes(header.rawLength));
                file.Position = header.rawReferencesOffset;
                ResourceInformation = new ResourceInfo[header.rawReferencesCount];
                for (int i = 0; i < ResourceInformation.Length; i++)
                    ResourceInformation[i] = new ResourceInfo() { Address = binReader.ReadInt32(), Length = binReader.ReadInt32() };
                Strings = new List<string>(header.stringReferencesCount);
                if (header.stringReferencesLength > 0)
                {
                    file.Position = header.stringReferencesOffset;
                    Strings.AddRange(Encoding.UTF8.GetString(binReader.ReadBytes(header.stringReferencesLength)).Split('\0'));
                }
                TagReferences = new List<string>(header.idReferencesCount);
                if (header.idReferencesLength > 0)
                {
                    file.Position = header.idReferencesOffset;
                    TagReferences.AddRange(Encoding.UTF8.GetString(binReader.ReadBytes(header.idReferencesLength)).Split('\0'));
                }
            }
        }

        #region Raw Methods

        /// <summary>
        /// Adds new raw data to RawStream and creates new RawInfo entry
        /// </summary>
        /// <param name="data">raw data bytes</param>
        public int AddRaw(byte[] data)
        {
            ResourceInfo newRawInfo = new ResourceInfo() { Address = (int)ResourceStream.Length, Length = data.Length };
            byte[] buffer = ResourceStream.ToArray();
            MemoryStream memStream = new MemoryStream(buffer.Length + Padding.Pad(newRawInfo.Length));
            BinaryWriter binWriter = new BinaryWriter(memStream);
            binWriter.Write(buffer);
            binWriter.Write(data);
            binWriter.Write(Padding.GetBytes(memStream.Position));
            Array.Resize<ResourceInfo>(ref ResourceInformation, ResourceInformation.Length + 1);
            ResourceInformation[ResourceInformation.Length - 1] = newRawInfo;
            ResourceStream = memStream;
            return ResourceInformation.Length - 1;
        }

        /// <summary>
        /// Inserts new raw data into RawStream at index, updates all RawInfo's
        /// </summary>
        /// <param name="index">index of RawInfo</param>
        /// <param name="data">raw data bytes</param>
        public void InsertRaw(int index, byte[] data)
        {
            AddRawAtIndex(index, data, true);
        }

        /// <summary>
        /// Removes raw data from RawStream at index, updates all RawInfo's
        /// </summary>
        /// <param name="index">index of RawInfo</param>
        public void RemoveRaw(int index)
        {
            ResourceInfo rawInfo = ResourceInformation[index];
            byte[] buffer = ResourceStream.ToArray();
            MemoryStream memStream = new MemoryStream();
            BinaryWriter binWriter = new BinaryWriter(memStream);
            binWriter.Write(buffer, 0, rawInfo.Address);
            int nextAddress = Padding.Pad(rawInfo.Address + rawInfo.Length);
            binWriter.Write(buffer, nextAddress, buffer.Length - nextAddress);
            ShiftRawOffsets(index, -Padding.Pad(rawInfo.Length));
            List<ResourceInfo> rawInfos = new List<ResourceInfo>(ResourceInformation);
            rawInfos.RemoveAt(index);
            ResourceInformation = rawInfos.ToArray();
            ResourceStream = memStream;
        }
        
        /// <summary>
        /// Replaces raw data at index in RawStream, updates all RawInfo's
        /// </summary>
        /// <param name="index">index of RawInfo</param>
        /// <param name="data">raw data bytes</param>
        public void ReplaceRaw(int index, byte[] data)
        {
            AddRawAtIndex(index, data, false);
        }

        private void AddRawAtIndex(int index, byte[] data, bool insert)
        {
            ResourceInfo rawInfo = ResourceInformation[index];
            byte[] buffer = ResourceStream.ToArray();
            MemoryStream memStream = new MemoryStream();
            BinaryWriter binWriter = new BinaryWriter(memStream);
            binWriter.Write(buffer, 0, rawInfo.Address);
            binWriter.Write(data);
            binWriter.Write(Padding.GetCount(binWriter.BaseStream.Position, 512));
            if (insert)
            {
                binWriter.Write(buffer, rawInfo.Address, buffer.Length - rawInfo.Address);
                List<ResourceInfo> rawInfos = new List<ResourceInfo>(ResourceInformation);
                rawInfos.Insert(index, new ResourceInfo() { Address = rawInfo.Address, Length = data.Length });
                ResourceInformation = rawInfos.ToArray();
                ShiftRawOffsets(index + 1, Padding.Pad(ResourceInformation[index].Length));
            }
            else
            {
                int nextAddress = (int)(rawInfo.Address + rawInfo.Length + Padding.GetCount(rawInfo.Address + rawInfo.Length, 512));
                binWriter.Write(buffer, nextAddress, buffer.Length - nextAddress);
                int shift = (int)((data.Length + Padding.GetCount(data.Length, 512)) - (ResourceInformation[index].Length + Padding.GetCount(ResourceInformation[index].Length, 512)));
                ResourceInformation[index].Length = data.Length;
                ShiftRawOffsets(index + 1, shift);
            }            
            ResourceStream = memStream;
        }

        private void ShiftRawOffsets(int index, int shift)
        {
            for (int i = index; i < ResourceInformation.Length; i++)
                ResourceInformation[i].Address += shift;
        }

        #endregion

        /// <summary>
        /// Saves Tag with this.Filename
        /// </summary>
        public void Save()
        {
            if (Filename == null) throw new Exception();
            Save(Filename);
        }

        /// <summary>
        /// Saves Tag to the specified File (creates it if it doesn't exist)
        /// </summary>
        /// <param name="filename">filename of file to save tag to</param>
        public void Save(string filename)
        {
            FileStream File = new FileStream(filename, FileMode.Create);
            BinaryWriter binWriter = new BinaryWriter(File);
            Info header = new Info();
            this.Filename = filename;
            header.type = this.Type;

            File.Position = Info.Size;
            //header.cacheInformationOffset = (int)File.Position;
            //binWriter.Write(IsCached);
            //binWriter.Write(Padding.GetBytes(File.Position, 4));
            //binWriter.Write(RawOffsets.Length);
            //foreach (int i in RawOffsets)
            //    binWriter.Write(i);
            //binWriter.Write(StringIdOffsets.Length);
            //foreach (int i in StringIdOffsets)
            //    binWriter.Write(i);
            //binWriter.Write(TagReferenceOffsets.Length);
            //foreach (int i in TagReferenceOffsets)
            //    binWriter.Write(i);
            //binWriter.Write(PointerOffsets.Length);
            //foreach (int i in PointerOffsets)
            //    binWriter.Write(i);

            header.metaOffset = (int)File.Position;
            binWriter.Write(TagStream.ToArray());
            header.metaLength = (int)File.Position - header.metaOffset;

            if (ResourceStream.Length > 0)
            {
                binWriter.Write(Padding.GetBytes(File.Position, 512));

                header.rawOffset = (int)File.Position;
                binWriter.Write(ResourceStream.ToArray());
                header.rawLength = (int)File.Position - header.rawOffset;

                header.rawReferencesCount = ResourceInformation.Length;
                header.rawReferencesOffset = (int)File.Position;
                for (int i = 0; i < ResourceInformation.Length; i++)
                {
                    binWriter.Write(ResourceInformation[i].Address);
                    binWriter.Write(ResourceInformation[i].Length);
                }
            }

            if (Strings.Count > 0)
            {
                binWriter.Write(Padding.GetBytes(File.Position, 512));

                header.stringReferencesCount = Strings.Count;
                header.stringReferencesOffset = (int)File.Position;
                for (int i = 0; i < Strings.Count; i++)
                {
                    binWriter.Write(Encoding.UTF8.GetBytes(Strings[i]));
                    binWriter.Write(byte.MinValue);
                }
                if (Strings.Count > 1)
                    File.Position -= 1;
                header.stringReferencesLength = (int)File.Position - header.stringReferencesOffset;
            }

            if (TagReferences.Count > 0)
            {

                binWriter.Write(Padding.GetBytes(File.Position, 512));

                header.idReferencesOffset = (int)File.Position;
                header.idReferencesCount = TagReferences.Count;
                for (int i = 0; i < TagReferences.Count; i++)
                {
                    if (TagReferences[i] == "{THIS}")
                    {
                        binWriter.Write(Encoding.UTF8.GetBytes(this.Filename));
                    }
                    else
                    {
                        binWriter.Write(Encoding.UTF8.GetBytes(TagReferences[i]));
                    }
                    binWriter.Write(byte.MinValue);
                }
                File.Position -= 1;
                header.idReferencesLength = (int)File.Position - header.idReferencesOffset;
            }
            binWriter.Write(Padding.GetBytes(File.Position, 512));
            File.Position = 0;
            binWriter.Write(header.ToByteArray());

            binWriter.Flush();
            File.Close();
        }

        public struct Info
        {
            public const int Size = 64;
            public string type;
            public int filesize;
            public int metaOffset;
            public int metaLength;
            public int rawOffset;
            public int rawLength;
            public int rawReferencesCount;
            public int rawReferencesOffset;
            public int stringReferencesCount;
            public int stringReferencesOffset;
            public int stringReferencesLength;
            public int idReferencesCount;
            public int idReferencesOffset;
            public int idReferencesLength;
            public int cacheInformationOffset;

            public Info(Stream stream)
            {
                BinaryReader binReader = new BinaryReader(stream);
                type = Encoding.UTF8.GetString(binReader.ReadBytes(4));
                filesize = binReader.ReadInt32();
                metaOffset = binReader.ReadInt32();
                metaLength = binReader.ReadInt32();
                rawOffset = binReader.ReadInt32();
                rawLength = binReader.ReadInt32();
                rawReferencesCount = binReader.ReadInt32();
                rawReferencesOffset = binReader.ReadInt32();
                stringReferencesCount = binReader.ReadInt32();
                stringReferencesOffset = binReader.ReadInt32();
                stringReferencesLength = binReader.ReadInt32();
                idReferencesCount = binReader.ReadInt32();
                idReferencesOffset = binReader.ReadInt32();
                idReferencesLength = binReader.ReadInt32();
                cacheInformationOffset = binReader.ReadInt32();
            }

            public byte[] ToByteArray()
            {
                MemoryStream memStream = new MemoryStream(Size);
                BinaryWriter binWriter = new BinaryWriter(memStream);
                binWriter.Write(Encoding.UTF8.GetBytes(type), 0, 4);
                binWriter.Write(filesize);
                binWriter.Write(metaOffset);
                binWriter.Write(metaLength);
                binWriter.Write(rawOffset);
                binWriter.Write(rawLength);
                binWriter.Write(rawReferencesCount);
                binWriter.Write(rawReferencesOffset);
                binWriter.Write(stringReferencesCount);
                binWriter.Write(stringReferencesOffset);
                binWriter.Write(stringReferencesLength);
                binWriter.Write(idReferencesCount);
                binWriter.Write(idReferencesOffset);
                binWriter.Write(idReferencesLength);
                binWriter.Write(cacheInformationOffset);
                binWriter.Flush();
                return memStream.ToArray();
            }
        }

        public static class Path
        {
            public const string Extension = ".sf";

            private static string[] GetPathComponents(string tagpath)
            {
                int index = tagpath.LastIndexOf("\\");
                if (index == -1) { index = 0; } else { index++; }
                string[] parts = tagpath.Substring(index, tagpath.Length - index).Split(new char[] { '.' });
                if (parts.Length != 3) throw new Exception();
                if (parts[2] != Extension.Substring(1)) throw new Exception();
                return parts;
            }

            public static string GetTagType(string tagpath)
            {
                return GetPathComponents(tagpath)[1];
            }

            public static string GetTagName(string tagpath)
            {
                return GetPathComponents(tagpath)[0];
            }

            public static string GetTagpath(string filepath)
            {
                string[] parts = filepath.Split(new char[] { '.' });
                if (parts.Length != 3) throw new Exception();
                if (parts[2] != Extension.Substring(1)) throw new Exception();
                return parts[0];
            }

            public static string Create(string filename, string type)
            {
                return System.IO.Path.ChangeExtension(filename, type) + Extension;
            }
        }
    }
}
