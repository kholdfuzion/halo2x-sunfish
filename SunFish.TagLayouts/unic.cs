using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Sunfish.TagLayouts
{
    public class unic : TagDefinition, IDataBound
    {
        public unic()
        {
            Size = 52;
            Values = null;
        }

        #region IDataBound Members

        public TagDefinition Unbind(TagDefinition tagDefinition, object dataSource)
        {
            UnicodeTable languageStrings = dataSource as string[][];
            unic unic = new unic();
            unic.Size = 8;
            unic.Values = new Value[] { 
                new TagBlockArray(new TagBlock(48, new Value[]{
                    new StringID(),
                    new Data(36),
                    new ByteArray(),
                }))
            };
            MemoryStream ms = new MemoryStream(tagDefinition.Data);
            BinaryReader reader = new BinaryReader(ms);
            ms.Position = 2;
            int count = reader.ReadInt16();
            TagBlock[] tagBlocks = new TagBlock[count];
            TagBlockArray tagBlockArray = unic.Values[0] as TagBlockArray;
            for (int i = 0; i < count; i++)
                tagBlocks[i] = tagBlockArray.tagBlocks[0];
            ms.Position = 0;
            int offset = reader.ReadInt16();
            count = reader.ReadInt16();
            byte[] buffer = new byte[48];
            MemoryStream blockStream = new MemoryStream(buffer);
            BinaryWriter bw = new BinaryWriter(blockStream);
            for (int i = 0; i < count; i++)
            {
                blockStream.Position = 0;
                bw.Write(
            }
            return unic;
        }

        #endregion
    }
}
