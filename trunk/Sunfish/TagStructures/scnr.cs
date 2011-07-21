using System.Collections.Generic;
using System;
using System.Text;
using System.IO;

namespace Sunfish.TagStructures
{
	public class scnr : TagBlock
	{
		public scnr() : base("scnr", 992)
		{
			Values = InitializeValues(new Value[]
			{
				new Data(8),
				new TagBlockArray(typeof(TagBlock0_0)),
				new Data(16),
				new TagBlockArray(typeof(TagBlock0_1)),
				new Data(32),
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
				new TagBlockArray(typeof(TagBlock0_24)),
				new TagBlockArray(typeof(TagBlock0_25)),
				new TagBlockArray(typeof(TagBlock0_26)),
				new Data(8),
				new TagBlockArray(typeof(TagBlock0_27)),
				new TagBlockArray(typeof(TagBlock0_28)),
				new TagBlockArray(typeof(TagBlock0_29)),
				new TagBlockArray(typeof(TagBlock0_30)),
				new TagBlockArray(typeof(TagBlock0_31)),
				new TagBlockArray(typeof(TagBlock0_32)),
				new TagBlockArray(typeof(TagBlock0_33)),
				new TagBlockArray(typeof(TagBlock0_34)),
				new TagBlockArray(typeof(TagBlock0_35)),
				new TagBlockArray(typeof(TagBlock0_36)),
				new TagBlockArray(typeof(TagBlock0_37)),
				new TagBlockArray(typeof(TagBlock0_38)),
				new TagBlockArray(typeof(TagBlock0_39)),
				new Data(40),
				new ByteArray(),
				new ByteArray(),
				new TagBlockArray(typeof(TagBlock0_40)),
				new TagBlockArray(typeof(TagBlock0_41)),
				new TagBlockArray(typeof(TagBlock0_42)),
				new Data(8),
				new TagBlockArray(typeof(TagBlock0_43)),
				new TagBlockArray(typeof(TagBlock0_44)),
				new TagBlockArray(typeof(TagBlock0_45)),
				new TagBlockArray(typeof(TagBlock0_46)),
				new TagReference(),
				new TagReference(),
				new TagReference(),
				new TagBlockArray(typeof(TagBlock0_47)),
				new TagBlockArray(typeof(TagBlock0_48)),
				new Data(8),
				new TagBlockArray(typeof(TagBlock0_49)),
				new TagBlockArray(typeof(TagBlock0_50)),
				new TagBlockArray(typeof(TagBlock0_51)),
				new TagBlockArray(typeof(TagBlock0_52)),
				new TagBlockArray(typeof(TagBlock0_53)),
				new TagBlockArray(typeof(TagBlock0_54)),
				new TagBlockArray(typeof(TagBlock0_55)),
				new TagBlockArray(typeof(TagBlock0_56)),
				new Data(40),
				new TagBlockArray(typeof(TagBlock0_57)),
				new Data(128),
				new TagBlockArray(typeof(TagBlock0_58)),
				new TagReference(),
				new TagBlockArray(typeof(TagBlock0_59)),
				new TagBlockArray(typeof(TagBlock0_60)),
				new TagReference(),
				new TagBlockArray(typeof(TagBlock0_61)),
				new TagBlockArray(typeof(TagBlock0_62)),
				new TagBlockArray(typeof(TagBlock0_63)),
				new TagReference(),
				new Data(24),
				new TagBlockArray(typeof(TagBlock0_64)),
				new Data(8),
				new TagBlockArray(typeof(TagBlock0_65)),
				new Data(8),
				new TagBlockArray(typeof(TagBlock0_66)),
				new TagReference(),
				new Data(8),
				new TagBlockArray(typeof(TagBlock0_67)),
				new TagReference(),
				new TagBlockArray(typeof(TagBlock0_68)),
				new Data(8),
				new TagBlockArray(typeof(TagBlock0_69)),
				new TagBlockArray(typeof(TagBlock0_70)),
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
			public TagBlock0_1() : base(8, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new Data(4),
					new TagIdentifier(),
				});
			}
		}
		public class TagBlock0_2 : TagBlock
		{
			public TagBlock0_2() : base(36, 4)
			{
			}
		}
		public class TagBlock0_3 : TagBlock
		{
			public TagBlock0_3() : base(92, 4)
			{
			}
		}
		public class TagBlock0_4 : TagBlock
		{
			public TagBlock0_4() : base(40, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new TagReference(),
				});
			}
		}
		public class TagBlock0_5 : TagBlock
		{
			public TagBlock0_5() : base(84, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new Data(52),
					new StringReferenceValue(),
				});
			}
		}
		public class TagBlock0_6 : TagBlock
		{
			public TagBlock0_6() : base(40, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new TagReference(),
				});
			}
		}
		public class TagBlock0_7 : TagBlock
		{
			public TagBlock0_7() : base(84, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new Data(52),
					new StringReferenceValue(),
				});
			}
		}
		public class TagBlock0_8 : TagBlock
		{
			public TagBlock0_8() : base(40, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new TagReference(),
				});
			}
		}
		public class TagBlock0_9 : TagBlock
		{
			public TagBlock0_9() : base(56, 4)
			{
			}
		}
		public class TagBlock0_10 : TagBlock
		{
			public TagBlock0_10() : base(40, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new TagReference(),
				});
			}
		}
		public class TagBlock0_11 : TagBlock
		{
			public TagBlock0_11() : base(84, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new Data(52),
					new StringReferenceValue(),
				});
			}
		}
		public class TagBlock0_12 : TagBlock
		{
			public TagBlock0_12() : base(40, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new TagReference(),
				});
			}
		}
		public class TagBlock0_13 : TagBlock
		{
			public TagBlock0_13() : base(40, 4)
			{
			}
		}
		public class TagBlock0_14 : TagBlock
		{
			public TagBlock0_14() : base(72, 4)
			{
			}
		}
		public class TagBlock0_15 : TagBlock
		{
			public TagBlock0_15() : base(40, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new TagReference(),
				});
			}
		}
		public class TagBlock0_16 : TagBlock
		{
			public TagBlock0_16() : base(68, 4)
			{
			}
		}
		public class TagBlock0_17 : TagBlock
		{
			public TagBlock0_17() : base(40, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new TagReference(),
				});
			}
		}
		public class TagBlock0_18 : TagBlock
		{
			public TagBlock0_18() : base(80, 4)
			{
			}
		}
		public class TagBlock0_19 : TagBlock
		{
			public TagBlock0_19() : base(40, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new TagReference(),
				});
			}
		}
		public class TagBlock0_20 : TagBlock
		{
			public TagBlock0_20() : base(80, 4)
			{
			}
		}
		public class TagBlock0_21 : TagBlock
		{
			public TagBlock0_21() : base(40, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new TagReference(),
				});
			}
		}
		public class TagBlock0_22 : TagBlock
		{
			public TagBlock0_22() : base(108, 4)
			{
			}
		}
		public class TagBlock0_23 : TagBlock
		{
			public TagBlock0_23() : base(40, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new TagReference(),
				});
			}
		}
		public class TagBlock0_24 : TagBlock
		{
			public TagBlock0_24() : base(68, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new Data(40),
					new TagReference(),
					new Data(4),
					new TagReference(),
				});
			}
		}
		public class TagBlock0_25 : TagBlock
		{
			public TagBlock0_25() : base(52, 4)
			{
			}
		}
		public class TagBlock0_26 : TagBlock
		{
			public TagBlock0_26() : base(68, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new StringReferenceValue(),
					new Data(4),
					new StringReferenceValue(),
				});
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
			public TagBlock0_28() : base(144, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new Data(88),
					new TagReference(),
				});
			}
		}
		public class TagBlock0_29 : TagBlock
		{
			public TagBlock0_29() : base(156, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new Data(60),
					new TagReference(),
					new TagReference(),
					new TagReference(),
					new TagReference(),
					new TagReference(),
					new TagReference(),
				});
			}
		}
		public class TagBlock0_30 : TagBlock
		{
			public TagBlock0_30() : base(14, 4)
			{
			}
		}
		public class TagBlock0_31 : TagBlock
		{
			public TagBlock0_31() : base(16, 4)
			{
			}
		}
		public class TagBlock0_32 : TagBlock
		{
			public TagBlock0_32() : base(8, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new TagReference(),
				});
			}
		}
		public class TagBlock0_33 : TagBlock
		{
			public TagBlock0_33() : base(8, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new TagReference(),
				});
			}
		}
		public class TagBlock0_34 : TagBlock
		{
			public TagBlock0_34() : base(8, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new TagReference(),
				});
			}
		}
		public class TagBlock0_35 : TagBlock
		{
			public TagBlock0_35() : base(36, 4)
			{
			}
		}
		public class TagBlock0_36 : TagBlock
		{
			public TagBlock0_36() : base(116, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new Data(68),
					new StringReferenceValue(),
					new TagBlockArray(typeof(TagBlock1_0)),
				});
			}
			public class TagBlock1_0 : TagBlock
			{
				public TagBlock1_0() : base(100, 4)
				{
					Values = InitializeValues(new Value[]
					{
						new StringReferenceValue(),
						new Data(44),
						new StringReferenceValue(),
						new StringReferenceValue(),
					});
				}
			}
		}
		public class TagBlock0_37 : TagBlock
		{
			public TagBlock0_37() : base(56, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new Data(40),
					new TagBlockArray(typeof(TagBlock1_0)),
					new TagBlockArray(typeof(TagBlock1_1)),
				});
			}
			public class TagBlock1_0 : TagBlock
			{
				public TagBlock1_0() : base(32, 4)
				{
				}
			}
			public class TagBlock1_1 : TagBlock
			{
				public TagBlock1_1() : base(136, 4)
				{
					Values = InitializeValues(new Value[]
					{
						new Data(128),
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
		public class TagBlock0_38 : TagBlock
		{
			public TagBlock0_38() : base(24, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new StringReferenceValue(),
					new Data(4),
					new TagBlockArray(typeof(TagBlock1_0)),
					new TagBlockArray(typeof(TagBlock1_1)),
				});
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
				public TagBlock1_1() : base(16, 4)
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
					public TagBlock2_0() : base(4, 4)
					{
						Values = InitializeValues(new Value[]
						{
							new StringReferenceValue(),
						});
					}
				}
			}
		}
		public class TagBlock0_39 : TagBlock
		{
			public TagBlock0_39() : base(8, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new TagReference(),
				});
			}
		}
		public class TagBlock0_40 : TagBlock
		{
			public TagBlock0_40() : base(40, 4)
			{
			}
		}
		public class TagBlock0_41 : TagBlock
		{
			public TagBlock0_41() : base(40, 4)
			{
			}
		}
		public class TagBlock0_42 : TagBlock
		{
			public TagBlock0_42() : base(8, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new TagReference(),
				});
			}
		}
		public class TagBlock0_43 : TagBlock
		{
			public TagBlock0_43() : base(128, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new TagBlockArray(typeof(TagBlock1_0)),
				});
			}
			public class TagBlock1_0 : TagBlock
			{
				public TagBlock1_0() : base(48, 4)
				{
					Values = InitializeValues(new Value[]
					{
						new Data(32),
						new TagBlockArray(typeof(TagBlock2_0)),
					});
				}
				public class TagBlock2_0 : TagBlock
				{
					public TagBlock2_0() : base(76, 4)
					{
					}
				}
			}
		}
		public class TagBlock0_44 : TagBlock
		{
			public TagBlock0_44() : base(56, 4)
			{
			}
		}
		public class TagBlock0_45 : TagBlock
		{
			public TagBlock0_45() : base(64, 4)
			{
			}
		}
		public class TagBlock0_46 : TagBlock
		{
			public TagBlock0_46() : base(36, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new StringReferenceValue(),
				});
			}
		}
		public class TagBlock0_47 : TagBlock
		{
			public TagBlock0_47() : base(68, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new Data(16),
					new TagReference(),
					new TagReference(),
				});
			}
		}
		public class TagBlock0_48 : TagBlock
		{
			public TagBlock0_48() : base(24, 4)
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
				public TagBlock1_0() : base(8, 4)
				{
				}
			}
			public class TagBlock1_1 : TagBlock
			{
				public TagBlock1_1() : base(8, 4)
				{
				}
			}
			public class TagBlock1_2 : TagBlock
			{
				public TagBlock1_2() : base(8, 4)
				{
				}
			}
		}
		public class TagBlock0_49 : TagBlock
		{
			public TagBlock0_49() : base(8, 4)
			{
			}
		}
		public class TagBlock0_50 : TagBlock
		{
			public TagBlock0_50() : base(2, 4)
			{
			}
		}
		public class TagBlock0_51 : TagBlock
		{
			public TagBlock0_51() : base(20, 4)
			{
			}
		}
		public class TagBlock0_52 : TagBlock
		{
			public TagBlock0_52() : base(124, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new Data(84),
					new TagBlockArray(typeof(TagBlock1_0)),
					new TagBlockArray(typeof(TagBlock1_1)),
					new TagBlockArray(typeof(TagBlock1_2)),
					new TagBlockArray(typeof(TagBlock1_3)),
					new TagBlockArray(typeof(TagBlock1_4)),
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
				public TagBlock1_1() : base(8, 4)
				{
				}
			}
			public class TagBlock1_2 : TagBlock
			{
				public TagBlock1_2() : base(12, 4)
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
			public class TagBlock1_3 : TagBlock
			{
				public TagBlock1_3() : base(4, 4)
				{
				}
			}
			public class TagBlock1_4 : TagBlock
			{
				public TagBlock1_4() : base(20, 4)
				{
					Values = InitializeValues(new Value[]
					{
						new Data(12),
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
		}
		public class TagBlock0_53 : TagBlock
		{
			public TagBlock0_53() : base(48, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new Data(40),
					new TagBlockArray(typeof(TagBlock1_0)),
				});
			}
			public class TagBlock1_0 : TagBlock
			{
				public TagBlock1_0() : base(56, 4)
				{
				}
			}
		}
		public class TagBlock0_54 : TagBlock
		{
			public TagBlock0_54() : base(100, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new Data(32),
					new TagReference(),
					new TagReference(),
				});
			}
		}
		public class TagBlock0_55 : TagBlock
		{
			public TagBlock0_55() : base(72, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new Data(32),
					new TagReference(),
				});
			}
		}
		public class TagBlock0_56 : TagBlock
		{
			public TagBlock0_56() : base(136, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new Data(32),
					new TagReference(),
					new Data(36),
					new TagReference(),
					new Data(16),
					new StringReferenceValue(),
				});
			}
		}
		public class TagBlock0_57 : TagBlock
		{
			public TagBlock0_57() : base(52, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new TagReference(),
					new TagBlockArray(typeof(TagBlock1_0)),
					new TagBlockArray(typeof(TagBlock1_1)),
					new Data(4),
					new TagBlockArray(typeof(TagBlock1_2)),
					new TagBlockArray(typeof(TagBlock1_3)),
					new TagBlockArray(typeof(TagBlock1_4)),
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
				public TagBlock1_2() : base(12, 4)
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
				public TagBlock1_4() : base(4, 4)
				{
				}
			}
		}
		public class TagBlock0_58 : TagBlock
		{
			public TagBlock0_58() : base(96, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new Data(72),
					new TagBlockArray(typeof(TagBlock1_0)),
					new TagBlockArray(typeof(TagBlock1_1)),
					new TagBlockArray(typeof(TagBlock1_2)),
				});
			}
			public class TagBlock1_0 : TagBlock
			{
				public TagBlock1_0() : base(16, 4)
				{
				}
			}
			public class TagBlock1_1 : TagBlock
			{
				public TagBlock1_1() : base(48, 4)
				{
					Values = InitializeValues(new Value[]
					{
						new StringReferenceValue(),
					});
				}
			}
			public class TagBlock1_2 : TagBlock
			{
				public TagBlock1_2() : base(48, 4)
				{
					Values = InitializeValues(new Value[]
					{
						new StringReferenceValue(),
					});
				}
			}
		}
		public class TagBlock0_59 : TagBlock
		{
			public TagBlock0_59() : base(76, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new Data(52),
					new StringReferenceValue(),
				});
			}
		}
		public class TagBlock0_60 : TagBlock
		{
			public TagBlock0_60() : base(40, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new TagReference(),
				});
			}
		}
		public class TagBlock0_61 : TagBlock
		{
			public TagBlock0_61() : base(244, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new StringReferenceValue(),
					new Data(204),
					new TagReference(),
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
						new StringReferenceValue(),
					});
				}
			}
		}
		public class TagBlock0_62 : TagBlock
		{
			public TagBlock0_62() : base(16, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new StringReferenceValue(),
					new TagReference(),
				});
			}
		}
		public class TagBlock0_63 : TagBlock
		{
			public TagBlock0_63() : base(132, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new Data(12),
					new TagBlockArray(typeof(TagBlock1_0)),
					new Data(20),
					new TagReference(),
				});
			}
			public class TagBlock1_0 : TagBlock
			{
				public TagBlock1_0() : base(28, 4)
				{
				}
			}
		}
		public class TagBlock0_64 : TagBlock
		{
			public TagBlock0_64() : base(8, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new TagReference(),
				});
			}
		}
		public class TagBlock0_65 : TagBlock
		{
			public TagBlock0_65() : base(16, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new TagReference(),
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
		public class TagBlock0_66 : TagBlock
		{
			public TagBlock0_66() : base(24, 4)
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
				public TagBlock1_0() : base(2896, 4)
				{
					Values = InitializeValues(new Value[]
					{
						new Data(8),
						new TagReference(),
					});
				}
			}
			public class TagBlock1_1 : TagBlock
			{
				public TagBlock1_1() : base(3172, 4)
				{
					Values = InitializeValues(new Value[]
					{
						new Data(4),
						new TagReference(),
					});
				}
			}
		}
		public class TagBlock0_67 : TagBlock
		{
			public TagBlock0_67() : base(8, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new TagReference(),
				});
			}
		}
		public class TagBlock0_68 : TagBlock
		{
			public TagBlock0_68() : base(24, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new StringReferenceValue(),
					new StringReferenceValue(),
					new StringReferenceValue(),
					new ByteArray(),
				});
			}
		}
		public class TagBlock0_69 : TagBlock
		{
			public TagBlock0_69() : base(36, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new Data(16),
					new ByteArray(),
					new StringReferenceValue(),
				});
			}
		}
		public class TagBlock0_70 : TagBlock
		{
			public TagBlock0_70() : base(4, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new TagIdentifier(),
				});
			}
		}
	}
}
