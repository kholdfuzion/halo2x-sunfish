using System.Collections.Generic;
using System;
using System.Text;
using System.IO;

namespace Sunfish.TagStructures
{
	public class spas : TagBlock
	{
		public spas() : base("spas", 36)
		{
			Values = InitializeValues(new Value[]
			{
				new Data(28),
				new TagBlockArray(typeof(TagBlock0_0)),
			});
		}
		public class TagBlock0_0 : TagBlock
		{
			public TagBlock0_0() : base(88, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new TagBlockArray(typeof(TagBlock1_0)),
					new TagBlockArray(typeof(TagBlock1_1)),
					new TagBlockArray(typeof(TagBlock1_2)),
					new TagBlockArray(typeof(TagBlock1_3)),
					new TagBlockArray(typeof(TagBlock1_4)),
					new TagBlockArray(typeof(TagBlock1_5)),
					new TagBlockArray(typeof(TagBlock1_6)),
					new TagBlockArray(typeof(TagBlock1_7)),
				});
			}
			public class TagBlock1_0 : TagBlock
			{
				public TagBlock1_0() : base(306, 4)
				{
					Values = InitializeValues(new Value[]
					{
						new Data(252),
						new TagReference(),
					});
				}
			}
			public class TagBlock1_1 : TagBlock
			{
				public TagBlock1_1() : base(4, 4)
				{
				}
			}
			public class TagBlock1_2 : TagBlock
			{
				public TagBlock1_2() : base(5, 4)
				{
				}
			}
			public class TagBlock1_3 : TagBlock
			{
				public TagBlock1_3() : base(24, 4)
				{
				}
			}
			public class TagBlock1_4 : TagBlock
			{
				public TagBlock1_4() : base(4, 4)
				{
				}
			}
			public class TagBlock1_5 : TagBlock
			{
				public TagBlock1_5() : base(6, 4)
				{
				}
			}
			public class TagBlock1_6 : TagBlock
			{
				public TagBlock1_6() : base(32, 4)
				{
				}
			}
			public class TagBlock1_7 : TagBlock
			{
				public TagBlock1_7() : base(4, 4)
				{
				}
			}
		}
	}
}
