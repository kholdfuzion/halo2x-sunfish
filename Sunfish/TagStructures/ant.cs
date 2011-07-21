using System.Collections.Generic;
using System;
using System.Text;
using System.IO;

namespace Sunfish.TagStructures
{
	public class ant : TagBlock
	{
        
		public ant() : base("ant!", 160)
		{
			Values = InitializeValues(new Value[]
			{
				new StringReferenceValue(),
				new TagReference(),
				new TagReference(),
				new Data(132),
				new TagBlockArray(typeof(TagBlock0_0)),
			});
		}
		public class TagBlock0_0 : TagBlock
		{
			public TagBlock0_0() : base(128, 4)
			{
			}
		}
	}
}
