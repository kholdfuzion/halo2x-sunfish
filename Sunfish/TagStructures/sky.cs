using System.Collections.Generic;
using System;
using System.Text;
using System.IO;

namespace Sunfish.TagStructures
{
	public class sky : TagBlock
	{
		public sky() : base("sky ", 172)
		{
			Values = InitializeValues(new Value[]
			{
				new TagReference(),
				new TagReference(),
				new Data(12),
				new TagBlockArray(typeof(TagBlock0_0)),
				new Data(36),
				new TagBlockArray(typeof(TagBlock0_1)),
				new TagBlockArray(typeof(TagBlock0_2)),
				new TagBlockArray(typeof(TagBlock0_3)),
				new TagBlockArray(typeof(TagBlock0_4)),
				new Data(16),
				new TagBlockArray(typeof(TagBlock0_5)),
			});
		}
		public class TagBlock0_0 : TagBlock
		{
			public TagBlock0_0() : base(12, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new TagReference(),
				});
			}
		}
		public class TagBlock0_1 : TagBlock
		{
			public TagBlock0_1() : base(24, 4)
			{
			}
		}
		public class TagBlock0_2 : TagBlock
		{
			public TagBlock0_2() : base(24, 4)
			{
			}
		}
		public class TagBlock0_3 : TagBlock
		{
			public TagBlock0_3() : base(16, 4)
			{
			}
		}
		public class TagBlock0_4 : TagBlock
		{
			public TagBlock0_4() : base(80, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new Data(72),
					new TagReference(),
				});
			}
		}
		public class TagBlock0_5 : TagBlock
		{
			public TagBlock0_5() : base(52, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new Data(20),
					new TagReference(),
					new TagBlockArray(typeof(TagBlock1_0)),
					new TagBlockArray(typeof(TagBlock1_1)),
					new TagBlockArray(typeof(TagBlock1_2)),
				});
			}
			public class TagBlock1_0 : TagBlock
			{
				public TagBlock1_0() : base(44, 4)
				{
				}
			}
			public class TagBlock1_1 : TagBlock
			{
				public TagBlock1_1() : base(44, 4)
				{
				}
			}
			public class TagBlock1_2 : TagBlock
			{
				public TagBlock1_2() : base(40, 4)
				{
				}
			}
		}
	}
}
