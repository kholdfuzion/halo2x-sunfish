using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sunfish.TagLayouts
{
    public class unit : TagDefinition
    {
        public unit()
        {
            Size = 312;
            Values = new Value[] {
                new Data(44),
                new TagBlockArray(new TagBlock(8, null)),
                new Data(168),
                new TagBlockArray(new TagBlock(16, null)),
                new TagBlockArray(new TagBlock(8, null)),
                new TagBlockArray(new TagBlock(24, null)),
                new Data(8),
                new TagBlockArray(new TagBlock(8, null)),
                new TagBlockArray(new TagBlock(8, null)),
                new TagBlockArray(new TagBlock(176, new Value[] {
                    new Data(116),
                    new TagBlockArray(new TagBlock(8, null)),
                    new TagBlockArray(new TagBlock(8, null)),
                })),
                new Data(28),
            };
        }
    }
}