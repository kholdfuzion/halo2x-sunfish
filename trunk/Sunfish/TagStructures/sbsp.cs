using System.Collections.Generic;
using System;
using System.Text;
using System.IO;

namespace Sunfish.TagStructures
{
	public class sbsp : TagBlock
	{
		public sbsp() : base("sbsp", 588)
		{
			Values = InitializeValues(new Value[]
			{
				new Data(28),
				new TagBlockArray(typeof(TagBlock0_0)),
				new TagBlockArray(typeof(TagBlock0_1)),
				new Data(16),
				new TagBlockArray(typeof(TagBlock0_2)),
				new Data(24),
				new TagBlockArray(typeof(TagBlock0_3)),
				new ByteArray(),
				new TagBlockArray(typeof(TagBlock0_4)),
				new TagBlockArray(typeof(TagBlock0_5)),
				new Data(24),
				new TagBlockArray(typeof(TagBlock0_6)),
				new TagBlockArray(typeof(TagBlock0_7)),
				new TagBlockArray(typeof(TagBlock0_8)),
				new TagBlockArray(typeof(TagBlock0_9)),
				new TagBlockArray(typeof(TagBlock0_10)),
				new TagBlockArray(typeof(TagBlock0_11)),
				new TagBlockArray(typeof(TagBlock0_12)),
				new TagBlockArray(typeof(TagBlock0_13)),
				new TagBlockArray(typeof(TagBlock0_14)),
				new Data(8),
				new TagBlockArray(typeof(TagBlock0_15)),
				new TagBlockArray(typeof(TagBlock0_16)),
				new ByteArray(),
				new TagBlockArray(typeof(TagBlock0_17)),
				new TagBlockArray(typeof(TagBlock0_18)),
				new TagBlockArray(typeof(TagBlock0_19)),
				new TagBlockArray(typeof(TagBlock0_20)),
				new Data(44),
				new TagBlockArray(typeof(TagBlock0_21)),
				new TagBlockArray(typeof(TagBlock0_22)),
				new TagBlockArray(typeof(TagBlock0_23)),
				new TagBlockArray(typeof(TagBlock0_24)),
				new TagBlockArray(typeof(TagBlock0_25)),
				new Data(112),
				new TagBlockArray(typeof(TagBlock0_26)),
				new TagReference(),
				new ByteArray(),
				new Data(28),
				new ByteArray(),
				new TagBlockArray(typeof(TagBlock0_27)),
				new TagBlockArray(typeof(TagBlock0_28)),
				new TagBlockArray(typeof(TagBlock0_29)),
				new TagBlockArray(typeof(TagBlock0_30)),
				new TagBlockArray(typeof(TagBlock0_31)),
				new TagBlockArray(typeof(TagBlock0_32)),
			});
		}

        public class MeshRawBlock : RawBlock
        {
            public MeshRawBlock(RawBlockAddress addressValue, RawBlockLength lengthValue) : base(addressValue, lengthValue) { }
        }

        public class PermutationsRawBlock : RawBlock
        {
            public PermutationsRawBlock(RawBlockAddress addressValue, RawBlockLength lengthValue) : base(addressValue, lengthValue) { }
        }
        
        public class WaterRawBlock : RawBlock
        {
            public WaterRawBlock(RawBlockAddress addressValue, RawBlockLength lengthValue) : base(addressValue, lengthValue) { }
        }

        public class DecoraterRawBlock : RawBlock
        {
            public DecoraterRawBlock(RawBlockAddress addressValue, RawBlockLength lengthValue) : base(addressValue, lengthValue) { }
        }



		public class TagBlock0_0 : TagBlock
		{
			public TagBlock0_0() : base(20, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new TagReference(),
					new Data(4),
					new TagReference(),
				});
			}
		}
		public class TagBlock0_1 : TagBlock
		{
			public TagBlock0_1() : base(68, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new TagBlockArray(typeof(TagBlock1_0)),
					new TagBlockArray(typeof(TagBlock1_1)),
					new TagBlockArray(typeof(TagBlock1_2)),
					new TagBlockArray(typeof(TagBlock1_3)),
					new TagBlockArray(typeof(TagBlock1_4)),
					new TagBlockArray(typeof(TagBlock1_5)),
					new TagBlockArray(typeof(TagBlock1_6)),
					new TagBlockArray(typeof(TagBlock1_7)),
				});
			}
			public class TagBlock1_0 : TagBlock
			{
				public TagBlock1_0() : base(8, 4)
				{
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
				public TagBlock1_2() : base(4, 4)
				{
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
				public TagBlock1_4() : base(16, 4)
				{
				}
			}
			public class TagBlock1_5 : TagBlock
			{
				public TagBlock1_5() : base(8, 4)
				{
				}
			}
			public class TagBlock1_6 : TagBlock
			{
				public TagBlock1_6() : base(12, 4)
				{
				}
			}
			public class TagBlock1_7 : TagBlock
			{
				public TagBlock1_7() : base(16, 4)
				{
				}
			}
		}
		public class TagBlock0_2 : TagBlock
		{
			public TagBlock0_2() : base(8, 4)
			{
			}
		}
		public class TagBlock0_3 : TagBlock
		{
			public TagBlock0_3() : base(8, 4)
			{
			}
		}
		public class TagBlock0_4 : TagBlock
		{
			public TagBlock0_4() : base(36, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new Data(28),
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
			public TagBlock0_5() : base(24, 4)
			{
			}
		}
		public class TagBlock0_6 : TagBlock
		{
			public TagBlock0_6() : base(136, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new Data(32),
					new TagReference(),
					new Data(36),
					new TagReference(),
					new Data(16),
					new StringReference(),
				});
			}
		}
		public class TagBlock0_7 : TagBlock
		{
			public TagBlock0_7() : base(24, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new Data(16),
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
		public class TagBlock0_8 : TagBlock
		{
			public TagBlock0_8() : base(36, 4)
			{
			}
		}
		public class TagBlock0_9 : TagBlock
		{
			public TagBlock0_9() : base(176, 4)
			{
				Values = InitializeValues(new Value[]
				{
                    new Data(40),
                    new RawBlockAddress(typeof(MeshRawBlock)),
                    new RawBlockLength(),
					new Data(8),
					new TagBlockArray(typeof(TagBlock1_0)),
					new TagIdentifier(),
					new Data(64),
					new TagBlockArray(typeof(TagBlock1_1)),
					new TagBlockArray(typeof(TagBlock1_2)),
					new Data(4),
					new TagBlockArray(typeof(TagBlock1_3)),
					new Data(8),
					new ByteArray(),
				}, true);
			}
			public class TagBlock1_0 : TagBlock
			{
				public TagBlock1_0() : base(16, 4)
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
						new TagIdentifier(),
					});
				}
			}
			public class TagBlock1_2 : TagBlock
			{
				public TagBlock1_2() : base(2, 4)
				{
				}
			}
			public class TagBlock1_3 : TagBlock
			{
				public TagBlock1_3() : base(2, 4)
				{
				}
			}
		}
		public class TagBlock0_10 : TagBlock
		{
			public TagBlock0_10() : base(32, 4)
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
		public class TagBlock0_11 : TagBlock
		{
			public TagBlock0_11() : base(2, 4)
			{
			}
		}
		public class TagBlock0_12 : TagBlock
		{
			public TagBlock0_12() : base(24, 16)
			{
			}
		}
		public class TagBlock0_13 : TagBlock
		{
			public TagBlock0_13() : base(24, 16)
			{
			}
		}
		public class TagBlock0_14 : TagBlock
		{
			public TagBlock0_14() : base(116, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new TagBlockArray(typeof(TagBlock1_0)),
					new TagBlockArray(typeof(TagBlock1_1)),
					new TagBlockArray(typeof(TagBlock1_2)),
					new TagBlockArray(typeof(TagBlock1_3)),
					new Data(8),
					new TagBlockArray(typeof(TagBlock1_4)),
					new TagBlockArray(typeof(TagBlock1_5)),
					new TagBlockArray(typeof(TagBlock1_6)),
					new TagBlockArray(typeof(TagBlock1_7)),
					new Data(36),
					new TagBlockArray(typeof(TagBlock1_8)),
				});
			}
			public class TagBlock1_0 : TagBlock
			{
				public TagBlock1_0() : base(8, 4)
				{
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
				public TagBlock1_2() : base(4, 4)
				{
				}
			}
			public class TagBlock1_3 : TagBlock
			{
				public TagBlock1_3() : base(20, 16)
				{
				}
			}
			public class TagBlock1_4 : TagBlock
			{
				public TagBlock1_4() : base(12, 16)
				{
				}
			}
			public class TagBlock1_5 : TagBlock
			{
				public TagBlock1_5() : base(28, 4)
				{
					Values = InitializeValues(new Value[]
					{
						new Data(12),
						new TagBlockArray(typeof(TagBlock2_0)),
						new TagBlockArray(typeof(TagBlock2_1)),
					});
				}
				public class TagBlock2_0 : TagBlock
				{
					public TagBlock2_0() : base(16, 4)
					{
					}
				}
				public class TagBlock2_1 : TagBlock
				{
					public TagBlock2_1() : base(4, 4)
					{
					}
				}
			}
			public class TagBlock1_6 : TagBlock
			{
				public TagBlock1_6() : base(20, 4)
				{
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
				public TagBlock1_8() : base(72, 4)
				{
					Values = InitializeValues(new Value[]
					{
						new Data(16),
						new TagBlockArray(typeof(TagBlock2_0)),
						new TagBlockArray(typeof(TagBlock2_1)),
						new Data(8),
						new TagBlockArray(typeof(TagBlock2_2)),
						new TagBlockArray(typeof(TagBlock2_3)),
						new Data(8),
						new TagBlockArray(typeof(TagBlock2_4)),
					});
				}
				public class TagBlock2_0 : TagBlock
				{
					public TagBlock2_0() : base(34, 16)
					{
					}
				}
				public class TagBlock2_1 : TagBlock
				{
					public TagBlock2_1() : base(66, 16)
					{
					}
				}
				public class TagBlock2_2 : TagBlock
				{
					public TagBlock2_2() : base(8, 4)
					{
					}
				}
				public class TagBlock2_3 : TagBlock
				{
					public TagBlock2_3() : base(4, 4)
					{
					}
				}
				public class TagBlock2_4 : TagBlock
				{
					public TagBlock2_4() : base(8, 4)
					{
						Values = InitializeValues(new Value[]
						{
							new TagBlockArray(typeof(TagBlock3_0)),
						});
					}
					public class TagBlock3_0 : TagBlock
					{
						public TagBlock3_0() : base(12, 16)
						{
						}
					}
				}
			}
		}
		public class TagBlock0_15 : TagBlock
		{
			public TagBlock0_15() : base(100, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new Data(32),
					new TagReference(),
					new TagReference(),
				});
			}
		}
		public class TagBlock0_16 : TagBlock
		{
			public TagBlock0_16() : base(72, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new Data(32),
					new TagReference(),
				});
			}
		}
		public class TagBlock0_17 : TagBlock
		{
			public TagBlock0_17() : base(60, 16)
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
			public TagBlock0_19() : base(20, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new TagReference(),
					new TagReference(),
				});
			}
		}
		public class TagBlock0_20 : TagBlock
		{
			public TagBlock0_20() : base(104, 4)
			{
			}
		}
		public class TagBlock0_21 : TagBlock
		{
			public TagBlock0_21() : base(200, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new Data(24),
					new TagBlockArray(typeof(TagBlock1_0)),
                    new Data(8),
                    new RawBlockAddress(typeof(PermutationsRawBlock)),
                    new RawBlockLength(),
					new Data(8),
					new TagBlockArray(typeof(TagBlock1_1)),
					new TagIdentifier(),
					new Data(8),
					new TagBlockArray(typeof(TagBlock1_2)),
					new TagBlockArray(typeof(TagBlock1_3)),
					new Data(20),
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
					new TagBlockArray(typeof(TagBlock1_14)),
				}, true);
			}
			public class TagBlock1_0 : TagBlock
			{
				public TagBlock1_0() : base(40, 4)
				{
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
				public TagBlock1_2() : base(64, 4)
				{
					Values = InitializeValues(new Value[]
					{
						new Data(40),
						new ByteArray(),
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
				public TagBlock1_4() : base(8, 8)
				{
				}
			}
			public class TagBlock1_5 : TagBlock
			{
				public TagBlock1_5() : base(16, 16)
				{
				}
			}
			public class TagBlock1_6 : TagBlock
			{
				public TagBlock1_6() : base(4, 4)
				{
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
				public TagBlock1_8() : base(16, 16)
				{
				}
			}
			public class TagBlock1_9 : TagBlock
			{
				public TagBlock1_9() : base(8, 8)
				{
				}
			}
			public class TagBlock1_10 : TagBlock
			{
				public TagBlock1_10() : base(12, 4)
				{
				}
			}
			public class TagBlock1_11 : TagBlock
			{
				public TagBlock1_11() : base(16, 16)
				{
				}
			}
			public class TagBlock1_12 : TagBlock
			{
				public TagBlock1_12() : base(112, 4)
				{
					Values = InitializeValues(new Value[]
					{
						new Data(100),
						new ByteArray(),
					});
				}
			}
			public class TagBlock1_13 : TagBlock
			{
				public TagBlock1_13() : base(8, 4)
				{
				}
			}
			public class TagBlock1_14 : TagBlock
			{
				public TagBlock1_14() : base(8, 4)
				{
				}
			}
		}
		public class TagBlock0_22 : TagBlock
		{
			public TagBlock0_22() : base(88, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new Data(80),
					new StringReference(),
				});
			}
		}
		public class TagBlock0_23 : TagBlock
		{
			public TagBlock0_23() : base(20, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new Data(4),
					new TagBlockArray(typeof(TagBlock1_0)),
					new TagBlockArray(typeof(TagBlock1_1)),
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
				public TagBlock1_1() : base(4, 4)
				{
				}
			}
		}
		public class TagBlock0_24 : TagBlock
		{
			public TagBlock0_24() : base(20, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new Data(4),
					new TagBlockArray(typeof(TagBlock1_0)),
					new TagBlockArray(typeof(TagBlock1_1)),
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
				public TagBlock1_1() : base(4, 4)
				{
				}
			}
		}
		public class TagBlock0_25 : TagBlock
		{
			public TagBlock0_25() : base(20, 4)
			{
			}
		}
		public class TagBlock0_26 : TagBlock
		{
			public TagBlock0_26() : base(88, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new Data(64),
					new TagBlockArray(typeof(TagBlock1_0)),
					new TagBlockArray(typeof(TagBlock1_1)),
					new TagBlockArray(typeof(TagBlock1_2)),
				});
			}
			public class TagBlock1_0 : TagBlock
			{
				public TagBlock1_0() : base(72, 4)
				{
					Values = InitializeValues(new Value[]
					{
						new Data(40),
						new TagBlockArray(typeof(TagBlock2_0)),
						new TagBlockArray(typeof(TagBlock2_1)),
						new Data(8),
						new TagBlockArray(typeof(TagBlock2_2)),
					});
				}
				public class TagBlock2_0 : TagBlock
				{
					public TagBlock2_0() : base(4, 4)
					{
					}
				}
				public class TagBlock2_1 : TagBlock
				{
					public TagBlock2_1() : base(4, 4)
					{
					}
				}
				public class TagBlock2_2 : TagBlock
				{
					public TagBlock2_2() : base(4, 4)
					{
					}
				}
			}
			public class TagBlock1_1 : TagBlock
			{
				public TagBlock1_1() : base(56, 4)
				{
					Values = InitializeValues(new Value[]
					{
						new Data(32),
						new TagBlockArray(typeof(TagBlock2_0)),
						new TagBlockArray(typeof(TagBlock2_1)),
						new TagBlockArray(typeof(TagBlock2_2)),
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
					public TagBlock2_1() : base(4, 4)
					{
					}
				}
				public class TagBlock2_2 : TagBlock
				{
					public TagBlock2_2() : base(4, 4)
					{
					}
				}
			}
			public class TagBlock1_2 : TagBlock
			{
				public TagBlock1_2() : base(64, 4)
				{
					Values = InitializeValues(new Value[]
					{
						new Data(32),
						new TagBlockArray(typeof(TagBlock2_0)),
						new Data(8),
						new TagBlockArray(typeof(TagBlock2_1)),
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
					public TagBlock2_1() : base(4, 4)
					{
					}
				}
			}
		}
		public class TagBlock0_27 : TagBlock
		{
			public TagBlock0_27() : base(32, 4)
			{
			}
		}
		public class TagBlock0_28 : TagBlock
		{
			public TagBlock0_28() : base(172, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new TagReference(),
					new TagBlockArray(typeof(TagBlock1_0)),
                    new RawBlockAddress(typeof(WaterRawBlock)),
                    new RawBlockLength(),
					new Data(8),
					new TagBlockArray(typeof(TagBlock1_1)),
				}, true);
			}
			public class TagBlock1_0 : TagBlock
			{
				public TagBlock1_0() : base(72, 4)
				{
					Values = InitializeValues(new Value[]
					{
						new Data(40),
						new ByteArray(),
					});
				}
			}
			public class TagBlock1_1 : TagBlock
			{
				public TagBlock1_1() : base(16, 4)
				{
				}
			}
		}
		public class TagBlock0_29 : TagBlock
		{
			public TagBlock0_29() : base(16, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new TagBlockArray(typeof(TagBlock1_0)),
					new TagBlockArray(typeof(TagBlock1_1)),
				});
			}
			public class TagBlock1_0 : TagBlock
			{
				public TagBlock1_0() : base(14, 4)
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
		public class TagBlock0_30 : TagBlock
		{
			public TagBlock0_30() : base(52, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new Data(12),
					new TagBlockArray(typeof(TagBlock1_0)),
					new TagBlockArray(typeof(TagBlock1_1)),
					new TagBlockArray(typeof(TagBlock1_2)),
					new ByteArray(),
					new ByteArray(),
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
				public TagBlock1_1() : base(4, 4)
				{
				}
			}
			public class TagBlock1_2 : TagBlock
			{
				public TagBlock1_2() : base(4, 4)
				{
				}
			}
		}
		public class TagBlock0_31 : TagBlock
		{
			public TagBlock0_31() : base(92, 16)
			{
			}
		}
		public class TagBlock0_32 : TagBlock
		{
			public TagBlock0_32() : base(48, 16)
			{
				Values = InitializeValues(new Value[]
				{
					new Data(16),
					new TagBlockArray(typeof(TagBlock1_0)),
					new TagBlockArray(typeof(TagBlock1_1)),
					new TagBlockArray(typeof(TagBlock1_2)),
					new TagBlockArray(typeof(TagBlock1_3)),
				});
			}
			public class TagBlock1_0 : TagBlock
			{
				public TagBlock1_0() : base(44, 4)
				{
					Values = InitializeValues(new Value[]
					{
                        new RawBlockAddress(typeof(DecoraterRawBlock)),
                        new RawBlockLength(),
						new Data(8),
						new TagBlockArray(typeof(TagBlock2_0)),
						new TagIdentifier(),
						new Data(8),
						new TagBlockArray(typeof(TagBlock2_1)),
					}, true);
				}
				public class TagBlock2_0 : TagBlock
				{
					public TagBlock2_0() : base(16, 4)
					{
					}
				}
				public class TagBlock2_1 : TagBlock
				{
					public TagBlock2_1() : base(40, 4)
					{
					}
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
				public TagBlock1_2() : base(24, 4)
				{
				}
			}
			public class TagBlock1_3 : TagBlock
			{
				public TagBlock1_3() : base(68, 16)
				{
				}
			}
		}
	}
}
