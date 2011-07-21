using System.Collections.Generic;
using System;
using System.Text;
using System.IO;

namespace Sunfish.TagStructures
{
	public class hlmt : TagBlock
	{
		public hlmt() : base("hlmt", 252)
		{
			Values = InitializeValues(new Value[]
			{
				new TagReference(),
				new TagReference(),
				new TagReference(),
				new TagReference(),
				new TagReference(),
				new Data(40),
				new TagBlockArray(typeof(TagBlock0_0)),
				new TagBlockArray(typeof(TagBlock0_1)),
				new TagBlockArray(typeof(TagBlock0_2)),
				new TagBlockArray(typeof(TagBlock0_3)),
				new TagBlockArray(typeof(TagBlock0_4)),
				new TagBlockArray(typeof(TagBlock0_5)),
				new Data(4),
				new TagBlockArray(typeof(TagBlock0_6)),
				new TagReference(),
				new TagReference(),
				new Data(4),
				new StringReferenceValue(),
				new Data(76),
				new TagReference(),
				new StringReferenceValue(),
			});
		}
		public class TagBlock0_0 : TagBlock
		{
			public TagBlock0_0() : base(56, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new StringReferenceValue(),
					new Data(16),
					new TagBlockArray(typeof(TagBlock1_0)),
					new TagBlockArray(typeof(TagBlock1_1)),
					new Data(8),
					new StringReferenceValue(),
					new TagReference(),
				});
			}
			public class TagBlock1_0 : TagBlock
			{
				public TagBlock1_0() : base(20, 4)
				{
					Values = InitializeValues(new Value[]
					{
						new StringReferenceValue(),
						new Data(4),
						new TagBlockArray(typeof(TagBlock2_0)),
					});
				}
				public class TagBlock2_0 : TagBlock
				{
					public TagBlock2_0() : base(32, 4)
					{
						Values = InitializeValues(new Value[]
						{
							new StringReferenceValue(),
							new Data(8),
							new TagBlockArray(typeof(TagBlock3_0)),
							new StringReferenceValue(),
						});
					}
					public class TagBlock3_0 : TagBlock
					{
						public TagBlock3_0() : base(24, 4)
						{
							Values = InitializeValues(new Value[]
							{
								new StringReferenceValue(),
								new Data(4),
								new TagReference(),
								new StringReferenceValue(),
							});
						}
					}
				}
			}
			public class TagBlock1_1 : TagBlock
			{
				public TagBlock1_1() : base(16, 4)
				{
					Values = InitializeValues(new Value[]
					{
						new StringReferenceValue(),
						new StringReferenceValue(),
						new TagReference(),
					});
				}
			}
		}
		public class TagBlock0_1 : TagBlock
		{
			public TagBlock0_1() : base(20, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new StringReferenceValue(),
					new Data(8),
					new StringReferenceValue(),
				});
			}
		}
		public class TagBlock0_2 : TagBlock
		{
			public TagBlock0_2() : base(248, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new Data(4),
					new StringReferenceValue(),
					new Data(116),
					new TagReference(),
					new TagReference(),
					new Data(4),
					new StringReferenceValue(),
					new Data(16),
					new TagReference(),
					new TagReference(),
					new TagReference(),
					new TagBlockArray(typeof(TagBlock1_0)),
					new TagBlockArray(typeof(TagBlock1_1)),
					new Data(12),
					new TagBlockArray(typeof(TagBlock1_2)),
					new TagBlockArray(typeof(TagBlock1_3)),
					new TagReference(),
					new TagReference(),
				});
			}
			public class TagBlock1_0 : TagBlock
			{
				public TagBlock1_0() : base(56, 4)
				{
					Values = InitializeValues(new Value[]
					{
						new StringReferenceValue(),
						new Data(8),
						new TagBlockArray(typeof(TagBlock2_0)),
						new Data(28),
						new StringReferenceValue(),
					});
				}
				public class TagBlock2_0 : TagBlock
				{
					public TagBlock2_0() : base(80, 4)
					{
						Values = InitializeValues(new Value[]
						{
							new Data(12),
							new TagReference(),
							new TagReference(),
							new StringReferenceValue(),
							new Data(4),
							new StringReferenceValue(),
							new StringReferenceValue(),
							new Data(4),
							new TagReference(),
							new StringReferenceValue(),
							new StringReferenceValue(),
							new StringReferenceValue(),
							new Data(4),
							new StringReferenceValue(),
						});
					}
				}
			}
			public class TagBlock1_1 : TagBlock
			{
				public TagBlock1_1() : base(16, 4)
				{
				}
			}
			public class TagBlock1_2 : TagBlock
			{
				public TagBlock1_2() : base(20, 4)
				{
					Values = InitializeValues(new Value[]
					{
						new StringReferenceValue(),
					});
				}
			}
			public class TagBlock1_3 : TagBlock
			{
				public TagBlock1_3() : base(20, 4)
				{
					Values = InitializeValues(new Value[]
					{
						new StringReferenceValue(),
						new StringReferenceValue(),
						new StringReferenceValue(),
					});
				}
			}
		}
		public class TagBlock0_3 : TagBlock
		{
			public TagBlock0_3() : base(28, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new StringReferenceValue(),
				});
			}
		}
		public class TagBlock0_4 : TagBlock
		{
			public TagBlock0_4() : base(16, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new StringReferenceValue(),
					new Data(4),
					new TagBlockArray(typeof(TagBlock1_0)),
				});
			}
			public class TagBlock1_0 : TagBlock
			{
				public TagBlock1_0() : base(8, 4)
				{
					Values = InitializeValues(new Value[]
					{
						new StringReferenceValue(),
					});
				}
			}
		}
		public class TagBlock0_5 : TagBlock
		{
			public TagBlock0_5() : base(92, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new StringReferenceValue(),
				});
			}
		}
		public class TagBlock0_6 : TagBlock
		{
			public TagBlock0_6() : base(20, 4)
			{
			}
		}
	}
}
