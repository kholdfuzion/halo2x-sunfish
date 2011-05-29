using System.Collections.Generic;
using System;
using System.Text;
using System.IO;

namespace Sunfish.TagStructures
{
    public class devi : TagBlock
    {
        public devi()
            : base("devi", 188 + 92)
        {            
            List<Value> values = new List<Value>(new obje().Values);
            values.AddRange(new Value[] { 
                new Data(32),
                new TagReference(),
                new TagReference(),
                new TagReference(),
                new TagReference(),
                new TagReference(),
                new TagReference(),
                new Data(4),
                new TagReference(),
            });
            Values = values.ToArray();
        }
    }
}
