using System.Collections.Generic;
using System;
using System.Text;
using System.IO;

namespace Sunfish.TagStructures
{
	public class ltmp : TagBlock
	{
		public ltmp() : base("ltmp", 260)
		{
			Values = InitializeValues(new Value[]
			{
				new Data(128),
				new TagBlockArray(typeof(TagBlock0_0)),
			});
		}

        public class LightmapRawBlock : RawBlock
        {
            public LightmapRawBlock(RawBlockAddress addressValue, RawBlockLength lengthValue) : base(addressValue, lengthValue) { }
        }

		public class TagBlock0_0 : TagBlock
		{
			public TagBlock0_0() : base(104, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new Data(8),
					new TagBlockArray(typeof(TagBlock1_0)),
					new TagBlockArray(typeof(TagBlock1_1)),
					new TagReference(),
					new TagBlockArray(typeof(TagBlock1_2)),
					new TagBlockArray(typeof(TagBlock1_3)),
					new TagBlockArray(typeof(TagBlock1_4)),
					new TagBlockArray(typeof(TagBlock1_5)),
					new TagBlockArray(typeof(TagBlock1_6)),
					new TagBlockArray(typeof(TagBlock1_7)),
					new TagBlockArray(typeof(TagBlock1_8)),
					new TagBlockArray(typeof(TagBlock1_9)),
					new TagBlockArray(typeof(TagBlock1_10)),
				});
			}
			public class TagBlock1_0 : TagBlock
			{
				public TagBlock1_0() : base(1024, 64)
				{
				}
			}
			public class TagBlock1_1 : TagBlock
			{
				public TagBlock1_1() : base(1024, 64)
				{
				}
			}
			public class TagBlock1_2 : TagBlock
			{
				public TagBlock1_2() : base(84, 4)
				{
					Values = InitializeValues(new Value[]
					{
						new Data(64),
						new TagIdentifier(),
					});
				}
			}
			public class TagBlock1_3 : TagBlock
			{
				public TagBlock1_3() : base(4, 4)
				{
				}
			}
			public class TagBlock1_4 : TagBlock
			{
				public TagBlock1_4() : base(84, 4)
				{
					Values = InitializeValues(new Value[]
					{
						new Data(64),
						new TagIdentifier(),
					});
				}
			}
			public class TagBlock1_5 : TagBlock
			{
				public TagBlock1_5() : base(220, 4)
				{
				}
			}
			public class TagBlock1_6 : TagBlock
			{
				public TagBlock1_6() : base(56, 4)
				{
					Values = InitializeValues(new Value[]
					{
                        new Data(12),
                        new RawBlockAddress(typeof(LightmapRawBlock)),
                        new RawBlockLength(),
						new Data(8),
						new TagBlockArray(typeof(TagBlock2_0)),
						new TagIdentifier(),
					}, true);
				}
				public class TagBlock2_0 : TagBlock
				{
					public TagBlock2_0() : base(16, 4)
					{
					}
				}
			}
			public class TagBlock1_7 : TagBlock
			{
				public TagBlock1_7() : base(4, 4)
				{
				}
			}
			public class TagBlock1_8 : TagBlock
			{
				public TagBlock1_8() : base(12, 4)
				{
					Values = InitializeValues(new Value[]
					{
						new Data(4),
						new TagBlockArray(typeof(TagBlock2_0)),
					});
				}
				public class TagBlock2_0 : TagBlock
				{
					public TagBlock2_0() : base(4, 4)
					{
					}
				}
			}
			public class TagBlock1_9 : TagBlock
			{
				public TagBlock1_9() : base(12, 4)
				{
				}
			}
			public class TagBlock1_10 : TagBlock
			{
				public TagBlock1_10() : base(12, 4)
				{
					Values = InitializeValues(new Value[]
					{
						new Data(4),
						new TagBlockArray(typeof(TagBlock2_0)),
					});
				}
				public class TagBlock2_0 : TagBlock
				{
					public TagBlock2_0() : base(4, 4)
					{
					}
				}
			}
		}
	}
}
