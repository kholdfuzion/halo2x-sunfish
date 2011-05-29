using System.Collections.Generic;
using System;
using System.Text;
using System.IO;

namespace Sunfish.TagStructures
{
	public class stem : TagBlock
	{
		public stem() : base("stem", 96)
		{
			Values = InitializeValues(new Value[]
			{
				new Data(8),
				new StringReference(),
				new Data(20),
				new TagReference(),
				new Data(24),
				new TagReference(),
				new Data(4),
				new TagReference(),
				new Data(4),
				new TagBlockArray(typeof(TagBlock0_0)),
			});
		}
		public class TagBlock0_0 : TagBlock
		{
			public TagBlock0_0() : base(40, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new TagBlockArray(typeof(TagBlock1_0)),
					new TagBlockArray(typeof(TagBlock1_1)),
					new TagBlockArray(typeof(TagBlock1_2)),
					new TagBlockArray(typeof(TagBlock1_3)),
					new TagBlockArray(typeof(TagBlock1_4)),
				});
			}
			public class TagBlock1_0 : TagBlock
			{
				public TagBlock1_0() : base(10, 4)
				{
				}
			}
			public class TagBlock1_1 : TagBlock
			{
				public TagBlock1_1() : base(2, 4)
				{
				}
			}
			public class TagBlock1_2 : TagBlock
			{
				public TagBlock1_2() : base(10, 4)
				{
					Values = InitializeValues(new Value[]
					{
						new TagReference(),
					});
				}
			}
			public class TagBlock1_3 : TagBlock
			{
				public TagBlock1_3() : base(6, 4)
				{
				}
			}
			public class TagBlock1_4 : TagBlock
			{
				public TagBlock1_4() : base(4, 4)
				{
				}
			}
		}
	}
}
