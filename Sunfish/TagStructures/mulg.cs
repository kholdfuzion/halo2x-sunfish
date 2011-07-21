using System.Collections.Generic;
using System;
using System.Text;
using System.IO;

namespace Sunfish.TagStructures
{
	public class mulg : TagBlock
	{
		public mulg() : base("mulg", 16)
		{
			Values = InitializeValues(new Value[]
			{
				new TagBlockArray(typeof(TagBlock0_0)),
				new TagBlockArray(typeof(TagBlock0_1)),
			});
		}
		public class TagBlock0_0 : TagBlock
		{
			public TagBlock0_0() : base(32, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new TagReference(),
					new TagReference(),
					new TagBlockArray(typeof(TagBlock1_0)),
					new TagReference(),
				});
			}
			public class TagBlock1_0 : TagBlock
			{
				public TagBlock1_0() : base(12, 4)
				{
				}
			}
		}
		public class TagBlock0_1 : TagBlock
		{
			public TagBlock0_1() : base(1384, 4)
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
					new Data(40),
					new TagBlockArray(typeof(TagBlock1_0)),
					new TagBlockArray(typeof(TagBlock1_1)),
					new TagBlockArray(typeof(TagBlock1_2)),
					new TagBlockArray(typeof(TagBlock1_3)),
					new TagReference(),
					new TagBlockArray(typeof(TagBlock1_4)),
					new TagBlockArray(typeof(TagBlock1_5)),
					new TagBlockArray(typeof(TagBlock1_6)),
					new TagBlockArray(typeof(TagBlock1_7)),
					new TagBlockArray(typeof(TagBlock1_8)),
					new TagBlockArray(typeof(TagBlock1_9)),
					new Data(8),
					new TagBlockArray(typeof(TagBlock1_10)),
					new Data(8),
					new TagBlockArray(typeof(TagBlock1_11)),
					new TagBlockArray(typeof(TagBlock1_12)),
					new TagBlockArray(typeof(TagBlock1_13)),
					new Data(32),
					new TagReference(),
					new TagReference(),
					new Data(1040),
					new TagBlockArray(typeof(TagBlock1_14)),
					new TagBlockArray(typeof(TagBlock1_15)),
					new TagReference(),
					new TagReference(),
					new TagReference(),
					new TagReference(),
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
			public class TagBlock1_1 : TagBlock
			{
				public TagBlock1_1() : base(8, 4)
				{
					Values = InitializeValues(new Value[]
					{
						new TagReference(),
					});
				}
			}
			public class TagBlock1_2 : TagBlock
			{
				public TagBlock1_2() : base(8, 4)
				{
					Values = InitializeValues(new Value[]
					{
						new TagReference(),
					});
				}
			}
			public class TagBlock1_3 : TagBlock
			{
				public TagBlock1_3() : base(8, 4)
				{
					Values = InitializeValues(new Value[]
					{
						new TagReference(),
					});
				}
			}
			public class TagBlock1_4 : TagBlock
			{
				public TagBlock1_4() : base(8, 4)
				{
					Values = InitializeValues(new Value[]
					{
						new TagReference(),
					});
				}
			}
			public class TagBlock1_5 : TagBlock
			{
				public TagBlock1_5() : base(168, 4)
				{
					Values = InitializeValues(new Value[]
					{
						new Data(12),
						new StringReferenceValue(),
						new Data(4),
						new StringReferenceValue(),
						new Data(4),
						new StringReferenceValue(),
						new Data(36),
						new TagReference(),
						new TagReference(),
						new TagReference(),
						new TagReference(),
						new TagReference(),
						new TagReference(),
						new TagReference(),
						new TagReference(),
						new TagReference(),
						new TagBlockArray(typeof(TagBlock2_0)),
					});
				}
				public class TagBlock2_0 : TagBlock
				{
					public TagBlock2_0() : base(80, 4)
					{
						Values = InitializeValues(new Value[]
						{
							new Data(4),
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
				}
			}
			public class TagBlock1_6 : TagBlock
			{
				public TagBlock1_6() : base(168, 4)
				{
					Values = InitializeValues(new Value[]
					{
						new Data(12),
						new StringReferenceValue(),
						new Data(4),
						new StringReferenceValue(),
						new Data(4),
						new StringReferenceValue(),
						new Data(36),
						new TagReference(),
						new TagReference(),
						new TagReference(),
						new TagReference(),
						new TagReference(),
						new TagReference(),
						new TagReference(),
						new TagReference(),
						new TagReference(),
						new TagBlockArray(typeof(TagBlock2_0)),
					});
				}
				public class TagBlock2_0 : TagBlock
				{
					public TagBlock2_0() : base(80, 4)
					{
						Values = InitializeValues(new Value[]
						{
							new Data(4),
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
				}
			}
			public class TagBlock1_7 : TagBlock
			{
				public TagBlock1_7() : base(168, 4)
				{
					Values = InitializeValues(new Value[]
					{
						new Data(12),
						new StringReferenceValue(),
						new Data(4),
						new StringReferenceValue(),
						new Data(4),
						new StringReferenceValue(),
						new Data(36),
						new TagReference(),
						new TagReference(),
						new TagReference(),
						new TagReference(),
						new TagReference(),
						new TagReference(),
						new TagReference(),
						new TagReference(),
						new TagReference(),
						new TagBlockArray(typeof(TagBlock2_0)),
					});
				}
				public class TagBlock2_0 : TagBlock
				{
					public TagBlock2_0() : base(80, 4)
					{
						Values = InitializeValues(new Value[]
						{
							new Data(4),
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
				}
			}
			public class TagBlock1_8 : TagBlock
			{
				public TagBlock1_8() : base(168, 4)
				{
					Values = InitializeValues(new Value[]
					{
						new Data(12),
						new StringReferenceValue(),
						new Data(4),
						new StringReferenceValue(),
						new Data(4),
						new StringReferenceValue(),
						new Data(36),
						new TagReference(),
						new TagReference(),
						new TagReference(),
						new TagReference(),
						new TagReference(),
						new TagReference(),
						new TagReference(),
						new TagReference(),
						new TagReference(),
						new TagBlockArray(typeof(TagBlock2_0)),
					});
				}
				public class TagBlock2_0 : TagBlock
				{
					public TagBlock2_0() : base(80, 4)
					{
						Values = InitializeValues(new Value[]
						{
							new Data(4),
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
				}
			}
			public class TagBlock1_9 : TagBlock
			{
				public TagBlock1_9() : base(168, 4)
				{
					Values = InitializeValues(new Value[]
					{
						new Data(12),
						new StringReferenceValue(),
						new Data(4),
						new StringReferenceValue(),
						new Data(4),
						new StringReferenceValue(),
						new Data(36),
						new TagReference(),
						new TagReference(),
						new TagReference(),
						new TagReference(),
						new TagReference(),
						new TagReference(),
						new TagReference(),
						new TagReference(),
						new TagReference(),
						new TagBlockArray(typeof(TagBlock2_0)),
					});
				}
				public class TagBlock2_0 : TagBlock
				{
					public TagBlock2_0() : base(80, 4)
					{
						Values = InitializeValues(new Value[]
						{
							new Data(4),
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
				}
			}
			public class TagBlock1_10 : TagBlock
			{
				public TagBlock1_10() : base(168, 4)
				{
					Values = InitializeValues(new Value[]
					{
						new Data(12),
						new StringReferenceValue(),
						new Data(4),
						new StringReferenceValue(),
						new Data(4),
						new StringReferenceValue(),
						new Data(36),
						new TagReference(),
						new TagReference(),
						new TagReference(),
						new TagReference(),
						new TagReference(),
						new TagReference(),
						new TagReference(),
						new TagReference(),
						new TagReference(),
						new TagBlockArray(typeof(TagBlock2_0)),
					});
				}
				public class TagBlock2_0 : TagBlock
				{
					public TagBlock2_0() : base(80, 4)
					{
						Values = InitializeValues(new Value[]
						{
							new Data(4),
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
				}
			}
			public class TagBlock1_11 : TagBlock
			{
				public TagBlock1_11() : base(168, 4)
				{
					Values = InitializeValues(new Value[]
					{
						new Data(12),
						new StringReferenceValue(),
						new Data(4),
						new StringReferenceValue(),
						new Data(4),
						new StringReferenceValue(),
						new Data(36),
						new TagReference(),
						new TagReference(),
						new TagReference(),
						new TagReference(),
						new TagReference(),
						new TagReference(),
						new TagReference(),
						new TagReference(),
						new TagReference(),
						new TagBlockArray(typeof(TagBlock2_0)),
					});
				}
				public class TagBlock2_0 : TagBlock
				{
					public TagBlock2_0() : base(80, 4)
					{
						Values = InitializeValues(new Value[]
						{
							new Data(4),
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
				}
			}
			public class TagBlock1_12 : TagBlock
			{
				public TagBlock1_12() : base(168, 4)
				{
					Values = InitializeValues(new Value[]
					{
						new Data(12),
						new StringReferenceValue(),
						new Data(4),
						new StringReferenceValue(),
						new Data(4),
						new StringReferenceValue(),
						new Data(36),
						new TagReference(),
						new TagReference(),
						new TagReference(),
						new TagReference(),
						new TagReference(),
						new TagReference(),
						new TagReference(),
						new TagReference(),
						new TagReference(),
						new TagBlockArray(typeof(TagBlock2_0)),
					});
				}
				public class TagBlock2_0 : TagBlock
				{
					public TagBlock2_0() : base(80, 4)
					{
						Values = InitializeValues(new Value[]
						{
							new Data(4),
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
				}
			}
			public class TagBlock1_13 : TagBlock
			{
				public TagBlock1_13() : base(168, 4)
				{
					Values = InitializeValues(new Value[]
					{
						new Data(12),
						new StringReferenceValue(),
						new Data(4),
						new StringReferenceValue(),
						new Data(4),
						new StringReferenceValue(),
						new Data(36),
						new TagReference(),
						new TagReference(),
						new TagReference(),
						new TagReference(),
						new TagReference(),
						new TagReference(),
						new TagReference(),
						new TagReference(),
						new TagReference(),
						new TagBlockArray(typeof(TagBlock2_0)),
					});
				}
				public class TagBlock2_0 : TagBlock
				{
					public TagBlock2_0() : base(80, 4)
					{
						Values = InitializeValues(new Value[]
						{
							new Data(4),
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
				}
			}
			public class TagBlock1_14 : TagBlock
			{
				public TagBlock1_14() : base(352, 4)
				{
					Values = InitializeValues(new Value[]
					{
						new Data(192),
						new TagReference(),
						new Data(20),
						new TagReference(),
						new TagReference(),
						new TagReference(),
						new StringReferenceValue(),
						new StringReferenceValue(),
					});
				}
			}
			public class TagBlock1_15 : TagBlock
			{
				public TagBlock1_15() : base(28, 4)
				{
					Values = InitializeValues(new Value[]
					{
						new Data(8),
						new StringReferenceValue(),
						new StringReferenceValue(),
					});
				}
			}
		}
	}
}
