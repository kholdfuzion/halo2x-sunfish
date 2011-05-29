using System.Collections.Generic;
using System;
using System.Text;
using System.IO;

namespace Sunfish.TagStructures
{
	public class ctrl : TagBlock
	{
		public ctrl() : base("ctrl", 320)
		{
            List<Value> values = new List<Value>(new devi().Values);
            values.AddRange(new Value[]{
                new Data(8),
                new StringReference(), 
                new TagReference(),
                new TagReference(),
            });
            Values = InitializeValues(values.ToArray());
        }
	}
}
