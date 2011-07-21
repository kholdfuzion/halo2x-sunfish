using System.Collections.Generic;
using System;
using System.Text;
using System.IO;

namespace Sunfish.TagStructures
{
	public class goof : TagBlock
	{
		public goof() : base("goof", 368)
		{
			Values = InitializeValues(new Value[]
			{
				new Data(24),
				new TagBlockArray(typeof(TagBlock0_0)),
				new TagReference(),
				new TagBlockArray(typeof(TagBlock0_1)),
				new Data(8),
				new TagBlockArray(typeof(TagBlock0_2)),
				new Data(12),
				new TagBlockArray(typeof(TagBlock0_3)),
				new Data(32),
				new TagBlockArray(typeof(TagBlock0_4)),
				new Data(12),
				new TagBlockArray(typeof(TagBlock0_5)),
				new Data(32),
				new TagBlockArray(typeof(TagBlock0_6)),
				new Data(12),
				new TagBlockArray(typeof(TagBlock0_7)),
				new Data(12),
				new TagBlockArray(typeof(TagBlock0_8)),
			});
		}
		public class TagBlock0_0 : TagBlock
		{
			public TagBlock0_0() : base(24, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new Data(8),
					new TagBlockArray(typeof(TagBlock1_0)),
				});
			}
			public class TagBlock1_0 : TagBlock
			{
				public TagBlock1_0() : base(8, 4)
				{
					Values = InitializeValues(new Value[]
					{
						new TagReference(),
					});
				}
			}
		}
		public class TagBlock0_1 : TagBlock
		{
			public TagBlock0_1() : base(20, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new StringReferenceValue(),
					new Data(4),
					new TagBlockArray(typeof(TagBlock1_0)),
				});
			}
			public class TagBlock1_0 : TagBlock
			{
				public TagBlock1_0() : base(8, 4)
				{
				}
			}
		}
		public class TagBlock0_2 : TagBlock
		{
			public TagBlock0_2() : base(8, 4)
			{
			}
		}
		public class TagBlock0_3 : TagBlock
		{
			public TagBlock0_3() : base(8, 4)
			{
			}
		}
		public class TagBlock0_4 : TagBlock
		{
			public TagBlock0_4() : base(8, 4)
			{
			}
		}
		public class TagBlock0_5 : TagBlock
		{
			public TagBlock0_5() : base(8, 4)
			{
			}
		}
		public class TagBlock0_6 : TagBlock
		{
			public TagBlock0_6() : base(8, 4)
			{
			}
		}
		public class TagBlock0_7 : TagBlock
		{
			public TagBlock0_7() : base(8, 4)
			{
			}
		}
		public class TagBlock0_8 : TagBlock
		{
			public TagBlock0_8() : base(8, 4)
			{
			}
		}
	}
}
