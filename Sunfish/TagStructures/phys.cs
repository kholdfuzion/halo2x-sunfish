using System.Collections.Generic;
using System;
using System.Text;
using System.IO;

namespace Sunfish.TagStructures
{
	public class phys : TagBlock
	{
		public phys() : base("phys", 116)
		{
			Values = InitializeValues(new Value[]
			{
				new Data(92),
				new TagBlockArray(typeof(TagBlock0_0)),
				new TagBlockArray(typeof(TagBlock0_1)),
				new TagBlockArray(typeof(TagBlock0_2)),
			});
		}
		public class TagBlock0_0 : TagBlock
		{
			public TagBlock0_0() : base(36, 4)
			{
			}
		}
		public class TagBlock0_1 : TagBlock
		{
			public TagBlock0_1() : base(60, 4)
			{
			}
		}
		public class TagBlock0_2 : TagBlock
		{
			public TagBlock0_2() : base(128, 4)
			{
			}
		}
	}
}
