using System.Collections.Generic;
using System;
using System.Text;
using System.IO;

namespace Sunfish.TagStructures
{
	public class clwd : TagBlock
	{
		public clwd() : base("clwd", 108)
		{
			Values = InitializeValues(new Value[]
			{
				new Data(4),
				new StringReference(),
				new TagReference(),
				new Data(60),
				new TagBlockArray(typeof(TagBlock0_0)),
				new TagBlockArray(typeof(TagBlock0_1)),
				new TagBlockArray(typeof(TagBlock0_2)),
				new TagBlockArray(typeof(TagBlock0_3)),
			});
		}
		public class TagBlock0_0 : TagBlock
		{
			public TagBlock0_0() : base(20, 4)
			{
			}
		}
		public class TagBlock0_1 : TagBlock
		{
			public TagBlock0_1() : base(2, 4)
			{
			}
		}
		public class TagBlock0_2 : TagBlock
		{
			public TagBlock0_2() : base(2, 4)
			{
			}
		}
		public class TagBlock0_3 : TagBlock
		{
			public TagBlock0_3() : base(16, 4)
			{
			}
		}
	}
}
