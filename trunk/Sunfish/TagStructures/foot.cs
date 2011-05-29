using System.Collections.Generic;
using System;
using System.Text;
using System.IO;

namespace Sunfish.TagStructures
{
	public class foot : TagBlock
	{
		public foot() : base("foot", 8)
		{
			Values = InitializeValues(new Value[]
			{
				new TagBlockArray(typeof(TagBlock0_0)),
			});
		}
		public class TagBlock0_0 : TagBlock
		{
			public TagBlock0_0() : base(24, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new TagBlockArray(typeof(TagBlock1_0)),
					new TagBlockArray(typeof(TagBlock1_1)),
					new TagBlockArray(typeof(TagBlock1_2)),
				});
			}
			public class TagBlock1_0 : TagBlock
			{
				public TagBlock1_0() : base(24, 4)
				{
					Values = InitializeValues(new Value[]
					{
						new TagReference(),
						new TagReference(),
						new StringReference(),
					});
				}
			}
			public class TagBlock1_1 : TagBlock
			{
				public TagBlock1_1() : base(24, 4)
				{
					Values = InitializeValues(new Value[]
					{
						new TagReference(),
						new TagReference(),
						new StringReference(),
					});
				}
			}
			public class TagBlock1_2 : TagBlock
			{
				public TagBlock1_2() : base(24, 4)
				{
					Values = InitializeValues(new Value[]
					{
						new TagReference(),
						new TagReference(),
						new StringReference(),
					});
				}
			}
		}
	}
}
