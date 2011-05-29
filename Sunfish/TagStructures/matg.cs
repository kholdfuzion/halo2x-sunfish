using System.Collections.Generic;
using System;
using System.Text;
using System.IO;

namespace Sunfish.TagStructures
{
	public class matg : TagBlock
	{
		public matg() : base("matg", 644)
		{
			Values = InitializeValues(new Value[]
			{
				new Data(176),
				new TagBlockArray(typeof(TagBlock0_0)),
				new TagBlockArray(typeof(TagBlock0_1)),
				new TagBlockArray(typeof(TagBlock0_2)),
				new TagBlockArray(typeof(TagBlock0_3)),
				new TagBlockArray(typeof(TagBlock0_4)),
				new Data(8),
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
				new TagReference(),
				new TagBlockArray(typeof(TagBlock0_22)),
				new TagBlockArray(typeof(TagBlock0_23)),
				new TagReference(),
			});
		}
		public class TagBlock0_0 : TagBlock
		{
			public TagBlock0_0() : base(8, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new TagReference(),
				});
			}
		}
		public class TagBlock0_1 : TagBlock
		{
			public TagBlock0_1() : base(72, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new TagReference(),
				});
			}
		}
		public class TagBlock0_2 : TagBlock
		{
			public TagBlock0_2() : base(36, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new TagReference(),
					new TagReference(),
					new TagReference(),
					new TagReference(),
					new TagIdentifier(),
				});
			}
		}
		public class TagBlock0_3 : TagBlock
		{
			public TagBlock0_3() : base(360, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new Data(96),
					new TagReference(),
					new StringReference(),
					new Data(180),
					new TagBlockArray(typeof(TagBlock1_0)),
				});
			}
			public class TagBlock1_0 : TagBlock
			{
				public TagBlock1_0() : base(12, 4)
				{
				}
			}
		}
		public class TagBlock0_4 : TagBlock
		{
			public TagBlock0_4() : base(8, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new TagBlockArray(typeof(TagBlock1_0)),
				});
			}
			public class TagBlock1_0 : TagBlock
			{
				public TagBlock1_0() : base(12, 4)
				{
					Values = InitializeValues(new Value[]
					{
						new StringReference(),
						new TagBlockArray(typeof(TagBlock2_0)),
					});
				}
				public class TagBlock2_0 : TagBlock
				{
					public TagBlock2_0() : base(8, 4)
					{
						Values = InitializeValues(new Value[]
						{
							new StringReference(),
						});
					}
				}
			}
		}
		public class TagBlock0_5 : TagBlock
		{
			public TagBlock0_5() : base(8, 4)
			{
				Values = InitializeValues(new Value[]
				{
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
					new TagReference(),
				});
			}
		}
		public class TagBlock0_7 : TagBlock
		{
			public TagBlock0_7() : base(128, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new Data(116),
					new TagBlockArray(typeof(TagBlock1_0)),
				});
			}
			public class TagBlock1_0 : TagBlock
			{
				public TagBlock1_0() : base(4, 4)
				{
				}
			}
		}
		public class TagBlock0_8 : TagBlock
		{
			public TagBlock0_8() : base(644, 4)
			{
			}
		}
		public class TagBlock0_9 : TagBlock
		{
			public TagBlock0_9() : base(44, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new Data(4),
					new TagReference(),
					new Data(16),
					new TagReference(),
					new TagReference(),
				});
			}
		}
		public class TagBlock0_10 : TagBlock
		{
			public TagBlock0_10() : base(264, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new TagReference(),
					new TagReference(),
					new TagReference(),
					new TagReference(),
					new TagReference(),
					new TagReference(),
					new TagReference(),
					new TagReference(),
					new TagReference(),
					new Data(16),
					new TagBlockArray(typeof(TagBlock1_0)),
					new TagReference(),
					new TagReference(),
					new TagReference(),
					new Data(84),
					new TagReference(),
					new Data(44),
					new TagReference(),
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
		public class TagBlock0_11 : TagBlock
		{
			public TagBlock0_11() : base(152, 4)
			{
				Values = InitializeValues(new Value[]
				{
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
					new Data(8),
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
		}
		public class TagBlock0_12 : TagBlock
		{
			public TagBlock0_12() : base(152, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new TagReference(),
				});
			}
		}
		public class TagBlock0_13 : TagBlock
		{
			public TagBlock0_13() : base(152, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new TagReference(),
				});
			}
		}
		public class TagBlock0_14 : TagBlock
		{
			public TagBlock0_14() : base(152, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new TagReference(),
					new TagReference(),
					new Data(8),
					new TagReference(),
					new TagReference(),
					new TagReference(),
				});
			}
		}
		public class TagBlock0_15 : TagBlock
		{
			public TagBlock0_15() : base(284, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new TagReference(),
					new Data(168),
					new TagReference(),
					new Data(12),
					new TagReference(),
					new TagReference(),
					new Data(16),
					new TagReference(),
					new TagReference(),
					new TagReference(),
					new TagReference(),
					new TagReference(),
					new TagReference(),
					new TagReference(),
				});
			}
		}
		public class TagBlock0_16 : TagBlock
		{
			public TagBlock0_16() : base(188, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new TagReference(),
					new TagReference(),
					new Data(160),
					new TagReference(),
					new StringReference(),
				});
			}
		}
		public class TagBlock0_17 : TagBlock
		{
			public TagBlock0_17() : base(104, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new Data(16),
					new TagReference(),
					new Data(12),
					new TagReference(),
					new TagReference(),
					new TagReference(),
					new TagReference(),
					new TagReference(),
				});
			}
		}
		public class TagBlock0_18 : TagBlock
		{
			public TagBlock0_18() : base(36, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new StringReference(),
					new StringReference(),
					new Data(20),
					new TagReference(),
				});
			}
		}
		public class TagBlock0_19 : TagBlock
		{
			public TagBlock0_19() : base(180, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new StringReference(),
					new StringReference(),
					new Data(8),
					new StringReference(),
					new StringReference(),
					new Data(16),
					new TagReference(),
					new TagReference(),
					new TagReference(),
					new TagReference(),
					new TagReference(),
					new TagReference(),
					new TagReference(),
					new TagReference(),
					new Data(8),
					new TagReference(),
					new TagReference(),
					new TagReference(),
					new TagReference(),
					new TagReference(),
					new TagReference(),
					new Data(12),
					new TagReference(),
				});
			}
		}
		public class TagBlock0_20 : TagBlock
		{
			public TagBlock0_20() : base(32, 4)
			{
			}
		}
		public class TagBlock0_21 : TagBlock
		{
			public TagBlock0_21() : base(12, 4)
			{
			}
		}
		public class TagBlock0_22 : TagBlock
		{
			public TagBlock0_22() : base(8, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new TagBlockArray(typeof(TagBlock1_0)),
				});
			}
			public class TagBlock1_0 : TagBlock
			{
				public TagBlock1_0() : base(264, 4)
				{
				}
			}
		}
		public class TagBlock0_23 : TagBlock
		{
			public TagBlock0_23() : base(24, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new TagBlockArray(typeof(TagBlock1_0)),
					new TagBlockArray(typeof(TagBlock1_1)),
					new TagBlockArray(typeof(TagBlock1_2)),
				});
			}
			public class TagBlock1_0 : TagBlock
			{
				public TagBlock1_0() : base(2884, 4)
				{
				}
			}
			public class TagBlock1_1 : TagBlock
			{
				public TagBlock1_1() : base(2896, 4)
				{
					Values = InitializeValues(new Value[]
					{
						new Data(8),
						new TagReference(),
					});
				}
			}
			public class TagBlock1_2 : TagBlock
			{
				public TagBlock1_2() : base(3172, 4)
				{
					Values = InitializeValues(new Value[]
					{
						new Data(4),
						new TagReference(),
					});
				}
			}
		}
	}
}
