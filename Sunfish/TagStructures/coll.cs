using System.Collections.Generic;
using System;
using System.Text;
using System.IO;

namespace Sunfish.TagStructures
{
	public class coll : TagBlock
	{
		public coll() : base("coll", 52)
		{
			Values = InitializeValues(new Value[]
			{
				new Data(20),
				new TagBlockArray(typeof(TagBlock0_0)),
				new TagBlockArray(typeof(TagBlock0_1)),
				new TagBlockArray(typeof(TagBlock0_2)),
				new TagBlockArray(typeof(TagBlock0_3)),
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
				public TagBlock1_0() : base(20, 4)
				{
					Values = InitializeValues(new Value[]
					{
						new StringReference(),
						new TagBlockArray(typeof(TagBlock2_0)),
						new TagBlockArray(typeof(TagBlock2_1)),
					});
				}
				public class TagBlock2_0 : TagBlock
				{
					public TagBlock2_0() : base(68, 4)
					{
						Values = InitializeValues(new Value[]
						{
							new Data(4),
							new TagBlockArray(typeof(TagBlock3_0)),
							new TagBlockArray(typeof(TagBlock3_1)),
							new TagBlockArray(typeof(TagBlock3_2)),
							new TagBlockArray(typeof(TagBlock3_3)),
							new TagBlockArray(typeof(TagBlock3_4)),
							new TagBlockArray(typeof(TagBlock3_5)),
							new TagBlockArray(typeof(TagBlock3_6)),
							new TagBlockArray(typeof(TagBlock3_7)),
						});
					}
					public class TagBlock3_0 : TagBlock
					{
						public TagBlock3_0() : base(8, 8)
						{
						}
					}
					public class TagBlock3_1 : TagBlock
					{
						public TagBlock3_1() : base(16, 16)
						{
						}
					}
					public class TagBlock3_2 : TagBlock
					{
						public TagBlock3_2() : base(4, 16)
						{
						}
					}
					public class TagBlock3_3 : TagBlock
					{
						public TagBlock3_3() : base(4, 4)
						{
						}
					}
					public class TagBlock3_4 : TagBlock
					{
						public TagBlock3_4() : base(16, 16)
						{
						}
					}
					public class TagBlock3_5 : TagBlock
					{
						public TagBlock3_5() : base(8, 8)
						{
						}
					}
					public class TagBlock3_6 : TagBlock
					{
						public TagBlock3_6() : base(12, 4)
						{
						}
					}
					public class TagBlock3_7 : TagBlock
					{
						public TagBlock3_7() : base(16, 16)
						{
						}
					}
				}
				public class TagBlock2_1 : TagBlock
				{
					public TagBlock2_1() : base(112, 4)
					{
						Values = InitializeValues(new Value[]
						{
							new Data(48),
							new TagIdentifier(),
							new Data(48),
							new ByteArray(),
						});
					}
				}
			}
		}
		public class TagBlock0_2 : TagBlock
		{
			public TagBlock0_2() : base(20, 4)
			{
			}
		}
		public class TagBlock0_3 : TagBlock
		{
			public TagBlock0_3() : base(12, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new StringReference(),
				});
			}
		}
	}
}
