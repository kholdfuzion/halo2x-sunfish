using System.Collections.Generic;
using System;
using System.Text;
using System.IO;

namespace Sunfish.TagStructures
{
	public class jmad : TagBlock
	{
		public jmad() : base("jmad", 188)
		{
			Values = InitializeValues(new Value[]
			{
				new TagReference(),
				new Data(4),
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
				new Data(80),
				new TagBlockArray(typeof(TagBlock0_10)),
				new TagBlockArray(typeof(TagBlock0_11)),
			});
		}

        public class AnimationRawBlock : RawBlock
        {
            public AnimationRawBlock(RawBlockAddress addressValue, RawBlockLength lengthValue) : base(addressValue, lengthValue) { }
        }

		public class TagBlock0_0 : TagBlock
		{
			public TagBlock0_0() : base(32, 4)
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
					new TagReference(),
				});
			}
		}
		public class TagBlock0_2 : TagBlock
		{
			public TagBlock0_2() : base(12, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new TagReference(),
				});
			}
		}
		public class TagBlock0_3 : TagBlock
		{
			public TagBlock0_3() : base(28, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new StringReference(),
				});
			}
		}
		public class TagBlock0_4 : TagBlock
		{
			public TagBlock0_4() : base(108, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new StringReference(),
					new Data(72),
					new TagBlockArray(typeof(TagBlock1_0)),
					new TagBlockArray(typeof(TagBlock1_1)),
					new TagBlockArray(typeof(TagBlock1_2)),
					new TagBlockArray(typeof(TagBlock1_3)),
				});
			}
			public class TagBlock1_0 : TagBlock
			{
				public TagBlock1_0() : base(4, 4)
				{
				}
			}
			public class TagBlock1_1 : TagBlock
			{
				public TagBlock1_1() : base(8, 4)
				{
					Values = InitializeValues(new Value[]
					{
						new Data(4),
						new StringReference(),
					});
				}
			}
			public class TagBlock1_2 : TagBlock
			{
				public TagBlock1_2() : base(4, 4)
				{
				}
			}
			public class TagBlock1_3 : TagBlock
			{
				public TagBlock1_3() : base(28, 4)
				{
				}
			}
		}
		public class TagBlock0_5 : TagBlock
		{
			public TagBlock0_5() : base(20, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new StringReference(),
					new TagBlockArray(typeof(TagBlock1_0)),
					new TagBlockArray(typeof(TagBlock1_1)),
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
					public TagBlock2_0() : base(52, 4)
					{
						Values = InitializeValues(new Value[]
						{
							new StringReference(),
							new TagBlockArray(typeof(TagBlock3_0)),
							new TagBlockArray(typeof(TagBlock3_1)),
							new TagBlockArray(typeof(TagBlock3_2)),
							new TagBlockArray(typeof(TagBlock3_3)),
							new TagBlockArray(typeof(TagBlock3_4)),
							new TagBlockArray(typeof(TagBlock3_5)),
						});
					}
					public class TagBlock3_0 : TagBlock
					{
						public TagBlock3_0() : base(8, 4)
						{
							Values = InitializeValues(new Value[]
							{
								new StringReference(),
							});
						}
					}
					public class TagBlock3_1 : TagBlock
					{
						public TagBlock3_1() : base(8, 4)
						{
							Values = InitializeValues(new Value[]
							{
								new StringReference(),
							});
						}
					}
					public class TagBlock3_2 : TagBlock
					{
						public TagBlock3_2() : base(12, 4)
						{
							Values = InitializeValues(new Value[]
							{
								new StringReference(),
								new TagBlockArray(typeof(TagBlock4_0)),
							});
						}
						public class TagBlock4_0 : TagBlock
						{
							public TagBlock4_0() : base(8, 4)
							{
								Values = InitializeValues(new Value[]
								{
									new TagBlockArray(typeof(TagBlock5_0)),
								});
							}
							public class TagBlock5_0 : TagBlock
							{
								public TagBlock5_0() : base(4, 4)
								{
								}
							}
						}
					}
					public class TagBlock3_3 : TagBlock
					{
						public TagBlock3_3() : base(20, 4)
						{
							Values = InitializeValues(new Value[]
							{
								new StringReference(),
								new StringReference(),
								new Data(4),
								new TagBlockArray(typeof(TagBlock4_0)),
							});
						}
						public class TagBlock4_0 : TagBlock
						{
							public TagBlock4_0() : base(20, 4)
							{
								Values = InitializeValues(new Value[]
								{
									new StringReference(),
									new StringReference(),
									new StringReference(),
								});
							}
						}
					}
					public class TagBlock3_4 : TagBlock
					{
						public TagBlock3_4() : base(4, 4)
						{
						}
					}
					public class TagBlock3_5 : TagBlock
					{
						public TagBlock3_5() : base(4, 4)
						{
						}
					}
				}
				public class TagBlock2_1 : TagBlock
				{
					public TagBlock2_1() : base(8, 4)
					{
						Values = InitializeValues(new Value[]
						{
							new StringReference(),
							new StringReference(),
						});
					}
				}
			}
			public class TagBlock1_1 : TagBlock
			{
				public TagBlock1_1() : base(8, 4)
				{
					Values = InitializeValues(new Value[]
					{
						new StringReference(),
						new StringReference(),
					});
				}
			}
		}
		public class TagBlock0_6 : TagBlock
		{
			public TagBlock0_6() : base(40, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new StringReference(),
					new Data(4),
					new StringReference(),
					new Data(12),
					new StringReference(),
				});
			}
		}
		public class TagBlock0_7 : TagBlock
		{
			public TagBlock0_7() : base(20, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new StringReference(),
					new Data(8),
					new StringReference(),
				});
			}
		}
		public class TagBlock0_8 : TagBlock
		{
			public TagBlock0_8() : base(32, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new TagReference(),
					new TagBlockArray(typeof(TagBlock1_0)),
					new TagBlockArray(typeof(TagBlock1_1)),
				});
			}
			public class TagBlock1_0 : TagBlock
			{
				public TagBlock1_0() : base(2, 4)
				{
				}
			}
			public class TagBlock1_1 : TagBlock
			{
				public TagBlock1_1() : base(4, 4)
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
					new StringReference(),
					new StringReference(),
				});
			}
		}
		public class TagBlock0_10 : TagBlock
		{
			public TagBlock0_10() : base(20, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new TagIdentifier(),
                    new RawBlockLength(),
                    new RawBlockAddress(typeof(AnimationRawBlock)),
				}, true);
			}
		}
		public class TagBlock0_11 : TagBlock
		{
			public TagBlock0_11() : base(24, 4)
			{
			}
		}
	}
}
