using System.Collections.Generic;
using System;
using System.Text;
using System.IO;

namespace Sunfish.TagStructures
{
	public class effe : TagBlock
	{
		public effe() : base("effe", 48)
		{
			Values = InitializeValues(new Value[]
			{
				new Data(12),
				new TagBlockArray(typeof(TagBlock0_0)),
				new TagBlockArray(typeof(TagBlock0_1)),
				new TagReference(),
			});
		}
		public class TagBlock0_0 : TagBlock
		{
			public TagBlock0_0() : base(4, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new StringReference(),
				});
			}
		}
		public class TagBlock0_1 : TagBlock
		{
			public TagBlock0_1() : base(56, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new Data(24),
					new TagBlockArray(typeof(TagBlock1_0)),
					new TagBlockArray(typeof(TagBlock1_1)),
					new TagBlockArray(typeof(TagBlock1_2)),
					new TagBlockArray(typeof(TagBlock1_3)),
				});
			}
			public class TagBlock1_0 : TagBlock
			{
				public TagBlock1_0() : base(56, 4)
				{
					Values = InitializeValues(new Value[]
					{
						new Data(12),
						new TagReference(),
					});
				}
			}
			public class TagBlock1_1 : TagBlock
			{
				public TagBlock1_1() : base(60, 4)
				{
					Values = InitializeValues(new Value[]
					{
						new TagReference(),
						new Data(4),
						new ByteArray(),
						new ByteArray(),
						new ByteArray(),
						new ByteArray(),
						new ByteArray(),
						new ByteArray(),
					});
				}
			}
			public class TagBlock1_2 : TagBlock
			{
				public TagBlock1_2() : base(20, 4)
				{
				}
			}
			public class TagBlock1_3 : TagBlock
			{
				public TagBlock1_3() : base(56, 4)
				{
					Values = InitializeValues(new Value[]
					{
						new TagReference(),
						new Data(40),
						new TagBlockArray(typeof(TagBlock2_0)),
					});
				}
				public class TagBlock2_0 : TagBlock
				{
					public TagBlock2_0() : base(184, 4)
					{
						Values = InitializeValues(new Value[]
						{
							new TagReference(),
							new Data(8),
							new ByteArray(),
							new Data(8),
							new ByteArray(),
							new Data(8),
							new ByteArray(),
							new Data(8),
							new ByteArray(),
							new Data(8),
							new ByteArray(),
							new Data(8),
							new ByteArray(),
							new Data(8),
							new ByteArray(),
							new Data(12),
							new ByteArray(),
							new Data(8),
							new ByteArray(),
						});
					}
				}
			}
		}
	}
}
