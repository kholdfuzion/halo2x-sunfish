using System.Collections.Generic;
using System;
using System.Text;
using System.IO;

namespace Sunfish.TagStructures
{
	public class wigl : TagBlock
	{
		public wigl() : base("wigl", 452)
		{
			Values = InitializeValues(new Value[]
			{
				new Data(136),
				new TagBlockArray(typeof(TagBlock0_0)),
				new TagReference(),
				new TagReference(),
				new TagReference(),
				new TagReference(),
				new TagReference(),
				new TagReference(),
				new TagReference(),
				new TagReference(),
				new TagReference(),
				new TagReference(),
				new TagReference(),
				new TagReference(),
				new TagReference(),
				new TagReference(),
				new TagReference(),
				new TagReference(),
				new TagReference(),
				new TagReference(),
				new TagBlockArray(typeof(TagBlock0_1)),
				new Data(8),
				new TagBlockArray(typeof(TagBlock0_2)),
				new TagBlockArray(typeof(TagBlock0_3)),
				new TagReference(),
				new TagReference(),
				new TagReference(),
				new TagBlockArray(typeof(TagBlock0_4)),
				new Data(88),
				new TagReference(),
			});
		}
		public class TagBlock0_0 : TagBlock
		{
			public TagBlock0_0() : base(40, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new StringReferenceValue(),
					new Data(4),
					new TagReference(),
					new StringReferenceValue(),
					new StringReferenceValue(),
					new StringReferenceValue(),
					new StringReferenceValue(),
					new TagBlockArray(typeof(TagBlock1_0)),
				});
			}
			public class TagBlock1_0 : TagBlock
			{
				public TagBlock1_0() : base(24, 4)
				{
					Values = InitializeValues(new Value[]
					{
						new Data(8),
						new StringReferenceValue(),
						new StringReferenceValue(),
						new StringReferenceValue(),
						new StringReferenceValue(),
					});
				}
			}
		}
		public class TagBlock0_1 : TagBlock
		{
			public TagBlock0_1() : base(44, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new Data(8),
					new TagBlockArray(typeof(TagBlock1_0)),
					new Data(4),
					new TagBlockArray(typeof(TagBlock1_1)),
					new Data(8),
					new TagBlockArray(typeof(TagBlock1_2)),
				});
			}
			public class TagBlock1_0 : TagBlock
			{
				public TagBlock1_0() : base(20, 4)
				{
				}
			}
			public class TagBlock1_1 : TagBlock
			{
				public TagBlock1_1() : base(20, 4)
				{
				}
			}
			public class TagBlock1_2 : TagBlock
			{
				public TagBlock1_2() : base(20, 4)
				{
				}
			}
		}
		public class TagBlock0_2 : TagBlock
		{
			public TagBlock0_2() : base(16, 4)
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
		public class TagBlock0_3 : TagBlock
		{
			public TagBlock0_3() : base(8, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new TagReference(),
				});
			}
		}
		public class TagBlock0_4 : TagBlock
		{
			public TagBlock0_4() : base(4, 4)
			{
			}
		}
	}
}
