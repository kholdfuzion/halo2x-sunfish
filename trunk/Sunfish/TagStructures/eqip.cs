using System.Collections.Generic;
using System;
using System.Text;
using System.IO;

namespace Sunfish.TagStructures
{
	public class eqip : TagBlock
	{
		public eqip() : base("eqip", 316)
		{
            List<Value> values = new List<Value>(new item().Values);
            values.AddRange(new Value[]{
                new Data(8),
                new TagReference(),
            });
            Values = InitializeValues(values.ToArray());
		}
	}
}
