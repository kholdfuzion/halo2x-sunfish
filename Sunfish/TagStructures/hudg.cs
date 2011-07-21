using System.Collections.Generic;
using System;
using System.Text;
using System.IO;

namespace Sunfish.TagStructures
{
	public class hudg : TagBlock
	{
		public hudg() : base("hudg", 1160)
		{
			Values = InitializeValues(new Value[]
			{
				new Data(72),
				new TagReference(),
				new TagReference(),
				new Data(44),
				new TagReference(),
				new TagReference(),
				new TagReference(),
				new TagBlockArray(typeof(TagBlock0_0)),
				new Data(32),
				new TagReference(),
				new Data(80),
				new TagReference(),
				new TagBlockArray(typeof(TagBlock0_1)),
				new Data(460),
				new TagReference(),
				new Data(128),
				new TagReference(),
				new Data(8),
				new TagReference(),
				new Data(96),
				new TagReference(),
				new TagBlockArray(typeof(TagBlock0_2)),
				new TagBlockArray(typeof(TagBlock0_3)),
				new TagBlockArray(typeof(TagBlock0_4)),
				new TagBlockArray(typeof(TagBlock0_5)),
				new TagBlockArray(typeof(TagBlock0_6)),
				new TagReference(),
				new TagReference(),
				new StringReferenceValue(),
				new StringReferenceValue(),
				new TagReference(),
				new TagReference(),
				new TagReference(),
				new TagReference(),
				new TagReference(),
				new TagReference(),
				new TagReference(),
				new TagReference(),
				new TagReference(),
			});
		}
		public class TagBlock0_0 : TagBlock
		{
			public TagBlock0_0() : base(16, 4)
			{
			}
		}
		public class TagBlock0_1 : TagBlock
		{
			public TagBlock0_1() : base(104, 4)
			{
			}
		}
		public class TagBlock0_2 : TagBlock
		{
			public TagBlock0_2() : base(28, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new TagReference(),
					new TagReference(),
					new Data(4),
					new TagReference(),
				});
			}
		}
		public class TagBlock0_3 : TagBlock
		{
			public TagBlock0_3() : base(36, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new TagReference(),
					new TagReference(),
					new Data(12),
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
					new TagReference(),
					new TagReference(),
				});
			}
		}
		public class TagBlock0_5 : TagBlock
		{
			public TagBlock0_5() : base(24, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new TagReference(),
					new Data(8),
					new TagReference(),
				});
			}
		}
		public class TagBlock0_6 : TagBlock
		{
			public TagBlock0_6() : base(20, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new StringReferenceValue(),
				});
			}
		}
	}
}
