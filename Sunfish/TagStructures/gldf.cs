using System.Collections.Generic;
using System;
using System.Text;
using System.IO;

namespace Sunfish.TagStructures
{
    public class gldf : TagBlock
    {
        public gldf()
            : base("gldf", 8)
        {
            Values = InitializeValues(new Value[] { 
                new TagBlockArray(typeof(TagBlock0_0)),
            });
        }

        public class TagBlock0_0 : TagBlock
        {
            public TagBlock0_0()
                : base(144, 4)
            {
                Values = InitializeValues(new Value[] {       
                    new Data(36),
                    new ByteArray(),
                    new Data(52),
                    new ByteArray(),
                    new Data(24),
                    new ByteArray(),
                    new ByteArray(),
                });
            }
        }
    }
}