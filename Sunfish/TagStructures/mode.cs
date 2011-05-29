using System.Collections.Generic;
using System;
using System.Text;
using System.IO;

namespace Sunfish.TagStructures
{
	public class mode : TagBlock
	{
		public mode() : base("mode", 132)
		{
			Values = InitializeValues(new Value[]
			{
				new StringReference(),
				new Data(16),
				new TagBlockArray(typeof(TagBlock0_0)),
				new TagBlockArray(typeof(TagBlock0_1)),
				new TagBlockArray(typeof(TagBlock0_2)),
				new TagBlockArray(typeof(TagBlock0_3)),
				new TagBlockArray(typeof(TagBlock0_4)),
				new Data(12),
				new TagBlockArray(typeof(TagBlock0_5)),
				new Data(8),
				new TagBlockArray(typeof(TagBlock0_6)),
				new TagBlockArray(typeof(TagBlock0_7)),
				new Data(12),
				new TagBlockArray(typeof(TagBlock0_8)),
				new TagBlockArray(typeof(TagBlock0_9)),
			});
		}

        public class ModelRawBlock : RawBlock
        {
            public ModelRawBlock(RawBlockAddress addressValue, RawBlockLength lengthValue) : base(addressValue, lengthValue) { }
        }

        public class UnknownRawBlock : RawBlock
        {
            public UnknownRawBlock(RawBlockAddress addressValue, RawBlockLength lengthValue) : base(addressValue, lengthValue) { }
        }


		public class TagBlock0_0 : TagBlock
		{
			public TagBlock0_0() : base("bounding box", 56, 4)
			{
			}
		}
		public class TagBlock0_1 : TagBlock
		{
			public TagBlock0_1() : base("region", 16, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new StringReference(),
					new Data(4),
					new TagBlockArray(typeof(TagBlock1_0)),
				});
			}
			public class TagBlock1_0 : TagBlock
			{
				public TagBlock1_0() : base("permutation", 16, 4)
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
			public TagBlock0_2() : base("section", 92, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new Data(48),
					new TagBlockArray(typeof(TagBlock1_0)),
                    new RawBlockAddress(typeof(ModelRawBlock)),
                    new RawBlockLength(),
					new Data(8),
					new TagBlockArray(typeof(TagBlock1_1)),
					new TagIdentifier(),
				}, true);
			}
			public class TagBlock1_0 : TagBlock
			{
				public TagBlock1_0() : base(88, 4)
				{
				}
			}
			public class TagBlock1_1 : TagBlock
			{
				public TagBlock1_1() : base("resource", 16, 4)
				{
				}
			}
		}
		public class TagBlock0_3 : TagBlock
		{
			public TagBlock0_3() : base(4, 4)
			{
			}
		}
		public class TagBlock0_4 : TagBlock
		{
			public TagBlock0_4() : base(12, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new Data(4),
					new TagBlockArray(typeof(TagBlock1_0)),
				});
			}
			public class TagBlock1_0 : TagBlock
			{
				public TagBlock1_0() : base(16, 4)
				{
				}
			}
		}
		public class TagBlock0_5 : TagBlock
		{
			public TagBlock0_5() : base("node", 96, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new StringReference(),
				});
			}
		}
		public class TagBlock0_6 : TagBlock
		{
			public TagBlock0_6() : base("marker group", 12, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new StringReference(),
					new TagBlockArray(typeof(TagBlock1_0)),
				});
			}
			public class TagBlock1_0 : TagBlock
			{
				public TagBlock1_0() : base("marker", 36, 4)
				{
				}
			}
		}
		public class TagBlock0_7 : TagBlock
		{
			public TagBlock0_7() : base("shader", 32, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new TagReference(),
					new TagReference(),
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
		public class TagBlock0_8 : TagBlock
		{
			public TagBlock0_8() : base(88, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new Data(20),
					new TagBlockArray(typeof(TagBlock1_0)),
					new TagBlockArray(typeof(TagBlock1_1)),
					new Data(16),
                    new RawBlockAddress(typeof(UnknownRawBlock)),
                    new RawBlockLength(),
                    new Data(8),
					new TagBlockArray(typeof(TagBlock1_2)),
					new TagIdentifier(),
				}, true);
			}
			public class TagBlock1_0 : TagBlock
			{
				public TagBlock1_0() : base(12, 4)
				{
					Values = InitializeValues(new Value[]
					{
						new Data(4),
						new TagBlockArray(typeof(TagBlock2_0)),
					});
				}
				public class TagBlock2_0 : TagBlock
				{
					public TagBlock2_0() : base(8, 4)
					{
					}
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
		}
		public class TagBlock0_9 : TagBlock
		{
			public TagBlock0_9() : base(8, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new TagBlockArray(typeof(TagBlock1_0)),
				});
			}
			public class TagBlock1_0 : TagBlock
			{
				public TagBlock1_0() : base(16, 4)
				{
					Values = InitializeValues(new Value[]
					{
						new TagBlockArray(typeof(TagBlock2_0)),
						new TagBlockArray(typeof(TagBlock2_1)),
					});
				}
				public class TagBlock2_0 : TagBlock
				{
					public TagBlock2_0() : base(8, 4)
					{
					}
				}
				public class TagBlock2_1 : TagBlock
				{
					public TagBlock2_1() : base(8, 4)
					{
					}
				}
			}
		}
	}
}
