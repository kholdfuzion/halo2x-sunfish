using System.Collections.Generic;
using System;
using System.Text;
using System.IO;

namespace Sunfish.TagStructures
{
	public class wgit : TagBlock
	{
		public wgit() : base("wgit", 104)
		{
			Values = InitializeValues(new Value[]
			{
				new Data(24),
				new TagReference(),
				new TagBlockArray(typeof(TagBlock0_0)),
				new Data(4),
				new StringReference(),
				new TagBlockArray(typeof(TagBlock0_1)),
				new TagBlockArray(typeof(TagBlock0_2)),
			});
		}
		public class TagBlock0_0 : TagBlock
		{
			public TagBlock0_0() : base(76, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new Data(4),
					new TagBlockArray(typeof(TagBlock1_0)),
					new TagBlockArray(typeof(TagBlock1_1)),
					new Data(8),
					new TagBlockArray(typeof(TagBlock1_2)),
					new TagBlockArray(typeof(TagBlock1_3)),
					new TagBlockArray(typeof(TagBlock1_4)),
					new Data(16),
					new TagBlockArray(typeof(TagBlock1_5)),
				});
			}
			public class TagBlock1_0 : TagBlock
			{
				public TagBlock1_0() : base(60, 4)
				{
					Values = InitializeValues(new Value[]
					{
						new Data(36),
						new TagReference(),
						new Data(4),
						new StringReference(),
					});
				}
			}
			public class TagBlock1_1 : TagBlock
			{
				public TagBlock1_1() : base(24, 4)
				{
				}
			}
			public class TagBlock1_2 : TagBlock
			{
				public TagBlock1_2() : base(44, 4)
				{
					Values = InitializeValues(new Value[]
					{
						new Data(36),
						new StringReference(),
					});
				}
			}
			public class TagBlock1_3 : TagBlock
			{
				public TagBlock1_3() : base(56, 4)
				{
					Values = InitializeValues(new Value[]
					{
						new Data(24),
						new TagReference(),
						new Data(12),
						new StringReference(),
					});
				}
			}
			public class TagBlock1_4 : TagBlock
			{
				public TagBlock1_4() : base(76, 4)
				{
					Values = InitializeValues(new Value[]
					{
						new Data(12),
						new TagBlockArray(typeof(TagBlock2_0)),
						new TagBlockArray(typeof(TagBlock2_1)),
						new Data(36),
						new StringReference(),
						new StringReference(),
						new StringReference(),
					});
				}
				public class TagBlock2_0 : TagBlock
				{
					public TagBlock2_0() : base(32, 4)
					{
					}
				}
				public class TagBlock2_1 : TagBlock
				{
					public TagBlock2_1() : base(32, 4)
					{
					}
				}
			}
			public class TagBlock1_5 : TagBlock
			{
				public TagBlock1_5() : base(24, 4)
				{
					Values = InitializeValues(new Value[]
					{
						new Data(4),
						new TagReference(),
					});
				}
			}
		}
		public class TagBlock0_1 : TagBlock
		{
			public TagBlock0_1() : base(12, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new StringReference(),
					new TagBlockArray(typeof(TagBlock1_0)),
				});
			}
			public class TagBlock1_0 : TagBlock
			{
				public TagBlock1_0() : base(4, 4)
				{
					Values = InitializeValues(new Value[]
					{
						new StringReference(),
					});
				}
			}
		}
		public class TagBlock0_2 : TagBlock
		{
			public TagBlock0_2() : base(8, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new TagReference(),
				});
			}
		}
	}
}
