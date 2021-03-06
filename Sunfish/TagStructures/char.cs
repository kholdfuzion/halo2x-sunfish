using System.Collections.Generic;
using System;
using System.Text;
using System.IO;

namespace Sunfish.TagStructures
{
	public class @char : TagBlock
	{
		public @char() : base("char", 236)
		{
			Values = InitializeValues(new Value[]
			{
				new Data(4),
				new TagReference(),
				new TagReference(),
				new TagReference(),
				new TagReference(),
				new TagReference(),
				new TagBlockArray(typeof(TagBlock0_0)),
				new TagBlockArray(typeof(TagBlock0_1)),
				new TagBlockArray(typeof(TagBlock0_2)),
				new TagBlockArray(typeof(TagBlock0_3)),
				new TagBlockArray(typeof(TagBlock0_4)),
				new TagBlockArray(typeof(TagBlock0_5)),
				new TagBlockArray(typeof(TagBlock0_6)),
				new TagBlockArray(typeof(TagBlock0_7)),
				new TagBlockArray(typeof(TagBlock0_8)),
				new TagBlockArray(typeof(TagBlock0_9)),
				new TagBlockArray(typeof(TagBlock0_10)),
				new TagBlockArray(typeof(TagBlock0_11)),
				new TagBlockArray(typeof(TagBlock0_12)),
				new TagBlockArray(typeof(TagBlock0_13)),
				new TagBlockArray(typeof(TagBlock0_14)),
				new TagBlockArray(typeof(TagBlock0_15)),
				new TagBlockArray(typeof(TagBlock0_16)),
				new TagBlockArray(typeof(TagBlock0_17)),
				new TagBlockArray(typeof(TagBlock0_18)),
				new TagBlockArray(typeof(TagBlock0_19)),
				new TagBlockArray(typeof(TagBlock0_20)),
				new TagBlockArray(typeof(TagBlock0_21)),
				new TagBlockArray(typeof(TagBlock0_22)),
				new TagBlockArray(typeof(TagBlock0_23)),
			});
		}

		public class TagBlock0_0 : TagBlock
		{
			public TagBlock0_0() : base(12, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new StringReferenceValue(),
					new Data(4),
					new StringReferenceValue(),
				});
			}
		}
		public class TagBlock0_1 : TagBlock
		{
			public TagBlock0_1() : base(12, 4)
			{
			}
		}
		public class TagBlock0_2 : TagBlock
		{
			public TagBlock0_2() : base(112, 4)
			{
			}
		}
		public class TagBlock0_3 : TagBlock
		{
			public TagBlock0_3() : base(52, 4)
			{
			}
		}
		public class TagBlock0_4 : TagBlock
		{
			public TagBlock0_4() : base(52, 4)
			{
			}
		}
		public class TagBlock0_5 : TagBlock
		{
			public TagBlock0_5() : base(80, 4)
			{
			}
		}
		public class TagBlock0_6 : TagBlock
		{
			public TagBlock0_6() : base(36, 4)
			{
			}
		}
		public class TagBlock0_7 : TagBlock
		{
			public TagBlock0_7() : base(40, 4)
			{
			}
		}
		public class TagBlock0_8 : TagBlock
		{
			public TagBlock0_8() : base(8, 4)
			{
			}
		}
		public class TagBlock0_9 : TagBlock
		{
			public TagBlock0_9() : base(16, 4)
			{
			}
		}
		public class TagBlock0_10 : TagBlock
		{
			public TagBlock0_10() : base(64, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new Data(56),
					new TagReference(),
				});
			}
		}
		public class TagBlock0_11 : TagBlock
		{
			public TagBlock0_11() : base(20, 4)
			{
			}
		}
		public class TagBlock0_12 : TagBlock
		{
			public TagBlock0_12() : base(64, 4)
			{
			}
		}
		public class TagBlock0_13 : TagBlock
		{
			public TagBlock0_13() : base(76, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new Data(68),
					new TagReference(),
				});
			}
		}
		public class TagBlock0_14 : TagBlock
		{
			public TagBlock0_14() : base(20, 4)
			{
			}
		}
		public class TagBlock0_15 : TagBlock
		{
			public TagBlock0_15() : base(36, 4)
			{
			}
		}
		public class TagBlock0_16 : TagBlock
		{
			public TagBlock0_16() : base(12, 4)
			{
			}
		}
		public class TagBlock0_17 : TagBlock
		{
			public TagBlock0_17() : base(8, 4)
			{
			}
		}
		public class TagBlock0_18 : TagBlock
		{
			public TagBlock0_18() : base(16, 4)
			{
			}
		}
		public class TagBlock0_19 : TagBlock
		{
			public TagBlock0_19() : base(12, 4)
			{
			}
		}
		public class TagBlock0_20 : TagBlock
		{
			public TagBlock0_20() : base(204, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new Data(4),
					new TagReference(),
					new Data(184),
					new TagReference(),
				});
			}
		}
		public class TagBlock0_21 : TagBlock
		{
			public TagBlock0_21() : base(16, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new TagReference(),
					new TagBlockArray(typeof(TagBlock1_0)),
				});
			}
			public class TagBlock1_0 : TagBlock
			{
				public TagBlock1_0() : base(64, 4)
				{
				}
			}
		}
		public class TagBlock0_22 : TagBlock
		{
			public TagBlock0_22() : base(60, 4)
			{
			}
		}
		public class TagBlock0_23 : TagBlock
		{
			public TagBlock0_23() : base(180, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new TagReference(),
					new TagReference(),
				});
			}
		}
	}
}
