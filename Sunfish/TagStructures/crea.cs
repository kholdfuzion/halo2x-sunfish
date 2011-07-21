using System.Collections.Generic;
using System;
using System.Text;
using System.IO;

namespace Sunfish.TagStructures
{
    public class crea : TagBlock
    {
        public crea()
            : base("crea", 188 + 188)
        {
            List<Value> values = new List<Value>(new obje().Values);
            values.AddRange(new Value[] { 
                new Data(44),
                new StringReferenceValue(),
                new StringReferenceValue(),
                new Data(4),        
                new TagBlockArray(typeof(TagBlock0_0)),
                new TagBlockArray(typeof(TagBlock0_1)),
                new TagBlockArray(typeof(TagBlock0_2)),
                new Data(92),      
                new TagReference(),
                new TagReference(),
            });
            Values = InitializeValues(values.ToArray());
        }

        public class TagBlock0_0 : TagBlock
        {
            public TagBlock0_0()
                : base(128, 4)
            {
                Values = InitializeValues(new Value[]
				{
                    new StringReferenceValue(),
				});
            }
        }

        public class TagBlock0_1 : TagBlock
        {
            public TagBlock0_1()
                : base(80, 4)
            {
                Values = InitializeValues(new Value[]
				{
                    new StringReferenceValue(),
				});
            }
        }

        public class TagBlock0_2 : TagBlock
        {
            public TagBlock0_2()
                : base(128, 4)
            {
                Values = InitializeValues(new Value[]
				{
                    new StringReferenceValue(),
				});
            }
        }
    }
}
