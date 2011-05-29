
using System;
using System.Collections.Generic;
namespace Sunfish.TagLayouts
{
    public class bipd : TagDefinition
    {
        public bipd()
        {
            Size = 788;
            obje obje = new obje();
            unit unit = new unit();
            List<Value> values = new List<Value>();
            values.AddRange(obje.Values);
            values.AddRange(unit.Values);
            values.AddRange(new Value[]{
                    new Data(152),
                    new TagBlockArray(new TagBlock(128, null)),
                    new TagBlockArray(new TagBlock(80, null)),
                    new TagBlockArray(new TagBlock(128, null)),
                    new Data(92),
                    new TagBlockArray(new TagBlock(4, null)),
                });
            Values = values.ToArray();
        }
    }
}