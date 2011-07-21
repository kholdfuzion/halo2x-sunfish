using System.Collections.Generic;
using System;
using System.Text;
using System.IO;

namespace Sunfish.TagStructures
{
	public class unic : TagBlock
	{
        public unic()
            : base("unic", 52)
        { }

        public unic ExpandUnicode(unic unicodeTagblock, UnicodeTable unicodeTable)
        {
            List<UnicodeTable.Entry> entries = new List<UnicodeTable.Entry>();
            byte[] unicodeBytes = (unicodeTagblock.Values[1] as ByteArray).Data;
            TagBlockArray array  =(unicodeTagblock.Values[0] as TagBlockArray);
            for (int i = 0; i < array.Length; i++)
            {
                int sId = BitConverter.ToInt32(array.TagBlocks[i].Data, 0);
                short offset = BitConverter.ToInt16(array.TagBlocks[i].Data, 4);
                List<byte> strBytes = new List<byte>();
                int index = 0;
                while (true)
                {
                    if (unicodeBytes[offset + index] == 0x00) break;
                    strBytes.Add(unicodeBytes[offset + index]);
                    index++;
                }
                string str = Encoding.UTF8.GetString(strBytes.ToArray());
                entries.Add(new UnicodeTable.Entry() { StringReference = sId, Value = str, });
            }
            int start = unicodeTable.Count;
            //unicodeTable.AddRange(entries);
            unic unic = new unic();
            unic.Data = new byte[unic.Size];
            MemoryStream ms = new MemoryStream(unic.Data);
            BinaryWriter bw = new BinaryWriter(ms);
            ms.Position = 16;
            bw.Write((short)start);
            bw.Write((short)entries.Count);
            return unic;
        }

        public static unic CollapseUnicode(unic unicodeTagblock, UnicodeTable dataSource)
        {
            unic unic = new unic(true);
            TagBlockArray array = unic.Values[0] as TagBlockArray;
            unic.Data = new byte[16];
            MemoryStream ms = new MemoryStream(unicodeTagblock.Data);
            BinaryReader reader = new BinaryReader(ms);
            ms.Position = 16;
            int offset = reader.ReadInt16();
            int count = reader.ReadInt16();

            TagBlock[] tagBlocks = new TagBlock[count];
            TagBlockArray tagBlockArray = unic.Values[0] as TagBlockArray;
            for (int i = 0; i < count; i++)
                tagBlocks[i] = (TagBlock)Activator.CreateInstance(array.TagBlockType);

            List<byte> unicodeBytes = new List<byte>();
            for (int i = 0; i < count; i++)
            {
                byte[] buffer = new byte[48];
                MemoryStream blockStream = new MemoryStream(buffer);
                using (blockStream)
                {
                    BinaryWriter bw = new BinaryWriter(blockStream);
                    blockStream.Position = 0;
                    bw.Write(dataSource[offset + i].StringReference);
                    bw.Write(unicodeBytes.Count);
                }
                tagBlocks[i].Data = buffer;
                unicodeBytes.AddRange(Encoding.UTF8.GetBytes(dataSource[offset + i].Value));
                unicodeBytes.Add((byte)0x00);
            }
            array.TagBlocks = tagBlocks;
            (unic.Values[1] as ByteArray).Length = unicodeBytes.Count;
            (unic.Values[1] as ByteArray).Data = unicodeBytes.ToArray();
            return unic;
        }

		public unic(bool decompiled) : base("unic", 16)
		{
			Values = InitializeValues(new Value[]
			{
				new TagBlockArray(typeof(TagBlock0_0)),
				new ByteArray(),
			});
		}

		public class TagBlock0_0 : TagBlock
		{
			public TagBlock0_0() : base(40, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new StringReferenceValue(),
					new Data(36),
				});
			}
		}
	}
}
