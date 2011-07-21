using System.Collections.Generic;
using System;
using System.Text;
using System.IO;

namespace Sunfish.TagStructures
{
	public class skin : TagBlock
	{
		public skin() : base("skin", 60)
		{
			Values = InitializeValues(new Value[]
			{
				new Data(4),
				new TagReference(),
				new Data(8),
				new TagBlockArray(typeof(TagBlock0_0)),
				new TagBlockArray(typeof(TagBlock0_1)),
				new TagBlockArray(typeof(TagBlock0_2)),
				new TagBlockArray(typeof(TagBlock0_3)),
				new TagBlockArray(typeof(TagBlock0_4)),
			});
		}
		public class TagBlock0_0 : TagBlock
		{
			public TagBlock0_0() : base(16, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new Data(8),
					new TagBlockArray(typeof(TagBlock1_0)),
				});
			}
			public class TagBlock1_0 : TagBlock
			{
				public TagBlock1_0() : base(20, 4)
				{
				}
			}
		}
		public class TagBlock0_1 : TagBlock
		{
			public TagBlock0_1() : base(44, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new Data(36),
					new StringReferenceValue(),
				});
			}
		}
		public class TagBlock0_2 : TagBlock
		{
			public TagBlock0_2() : base(56, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new Data(24),
					new TagReference(),
					new Data(12),
					new StringReferenceValue(),
				});
			}
		}
		public class TagBlock0_3 : TagBlock
		{
			public TagBlock0_3() : base(36, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new Data(12),
					new TagReference(),
					new TagReference(),
				});
			}
		}
		public class TagBlock0_4 : TagBlock
		{
			public TagBlock0_4() : base(24, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new Data(4),
					new TagReference(),
				});
			}
		}
	}
}
