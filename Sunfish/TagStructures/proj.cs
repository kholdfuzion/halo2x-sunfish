using System.Collections.Generic;
using System;
using System.Text;
using System.IO;

namespace Sunfish.TagStructures
{
	public class proj : TagBlock
	{
		public proj() : base("proj", 420)
		{
            List<Value> values = new List<Value>(new obje().Values);
            values.AddRange(new Value[] { 
                new Data(44),
                new TagReference(),
                new TagReference(),
                new TagReference(),
                new TagReference(),
                new TagReference(),
                new TagReference(),
                new TagReference(),
                new TagReference(),
                new Data(4),                
                new TagReference(),
                new Data(4),      
                new TagReference(),
                new TagReference(),
                new TagReference(),
                new Data(4),      
                new TagReference(),
                new TagReference(),
                new Data(56),  
                new TagBlockArray(typeof(TagBlock0_0)),
            });
            Values = InitializeValues(values.ToArray());
		}

        public class TagBlock0_0 : TagBlock
        {
            public TagBlock0_0()
                : base(88, 4)
            {
                Values = InitializeValues(new Value[]
				{
					new Data(4),
                    new TagReference(),
                    new StringReference(),
                    new Data(28),
                    new TagReference(),
                    new Data(12),
                    new TagReference(),
				});
            }
        }
	}
}
