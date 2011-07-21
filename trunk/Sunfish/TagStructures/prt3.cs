using System.Collections.Generic;
using System;
using System.Text;
using System.IO;

namespace Sunfish.TagStructures
{
	public class prt3 : TagBlock
	{
		public prt3() : base("prt3", 188)
		{
			Values = InitializeValues(new Value[]
			{
				new Data(12),
				new TagReference(),
				new TagBlockArray(typeof(TagBlock0_0)),
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
				new TagReference(),
				new TagReference(),
				new TagBlockArray(typeof(TagBlock0_1)),
				new TagBlockArray(typeof(TagBlock0_2)),
				new TagBlockArray(typeof(TagBlock0_3)),
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
				});
			}
		}
		public class TagBlock0_1 : TagBlock
		{
			public TagBlock0_1() : base(4, 4)
			{
				Values = InitializeValues(new Value[]
				{
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
					new TagReference(),
					new Data(40),
					new TagBlockArray(typeof(TagBlock1_0)),
				});
			}
			public class TagBlock1_0 : TagBlock
			{
				public TagBlock1_0() : base(184, 4)
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
		public class TagBlock0_3 : TagBlock
		{
			public TagBlock0_3() : base(124, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new TagIdentifier(),
					new TagBlockArray(typeof(TagBlock1_0)),
					new TagBlockArray(typeof(TagBlock1_1)),
					new TagBlockArray(typeof(TagBlock1_2)),
					new TagBlockArray(typeof(TagBlock1_3)),
					new TagBlockArray(typeof(TagBlock1_4)),
					new TagBlockArray(typeof(TagBlock1_5)),
					new TagBlockArray(typeof(TagBlock1_6)),
					new TagBlockArray(typeof(TagBlock1_7)),
					new TagBlockArray(typeof(TagBlock1_8)),
					new TagBlockArray(typeof(TagBlock1_9)),
					new TagBlockArray(typeof(TagBlock1_10)),
					new TagBlockArray(typeof(TagBlock1_11)),
					new TagBlockArray(typeof(TagBlock1_12)),
					new TagBlockArray(typeof(TagBlock1_13)),
					new Data(8),
				});
			}
			public class TagBlock1_0 : TagBlock
			{
				public TagBlock1_0() : base(12, 4)
				{
					Values = InitializeValues(new Value[]
					{
						new TagIdentifier(),
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
				public TagBlock1_2() : base(16, 4)
				{
				}
			}
			public class TagBlock1_3 : TagBlock
			{
				public TagBlock1_3() : base(8, 4)
				{
				}
			}
			public class TagBlock1_4 : TagBlock
			{
				public TagBlock1_4() : base(2, 4)
				{
				}
			}
			public class TagBlock1_5 : TagBlock
			{
				public TagBlock1_5() : base(2, 4)
				{
				}
			}
			public class TagBlock1_6 : TagBlock
			{
				public TagBlock1_6() : base(10, 4)
				{
				}
			}
			public class TagBlock1_7 : TagBlock
			{
				public TagBlock1_7() : base(20, 4)
				{
					Values = InitializeValues(new Value[]
					{
						new StringReferenceValue(),
						new StringReferenceValue(),
						new Data(4),
						new ByteArray(),
					});
				}
			}
			public class TagBlock1_8 : TagBlock
			{
				public TagBlock1_8() : base(4, 4)
				{
				}
			}
			public class TagBlock1_9 : TagBlock
			{
				public TagBlock1_9() : base(2, 4)
				{
				}
			}
			public class TagBlock1_10 : TagBlock
			{
				public TagBlock1_10() : base(4, 4)
				{
				}
			}
			public class TagBlock1_11 : TagBlock
			{
				public TagBlock1_11() : base(4, 4)
				{
				}
			}
			public class TagBlock1_12 : TagBlock
			{
				public TagBlock1_12() : base(12, 4)
				{
				}
			}
			public class TagBlock1_13 : TagBlock
			{
				public TagBlock1_13() : base(4, 4)
				{
				}
			}
		}
	}
}
