using System.Collections.Generic;
using System;
using System.Text;
using System.IO;

namespace Sunfish.TagStructures
{
	public class trak : TagBlock
	{
		public trak() : base("trak", 12)
		{
			Values = InitializeValues(new Value[]
			{
				new Data(4),
				new TagBlockArray(typeof(TagBlock0_0)),
			});
		}
		public class TagBlock0_0 : TagBlock
		{
			public TagBlock0_0() : base(28, 4)
			{
			}
		}
	}
}
