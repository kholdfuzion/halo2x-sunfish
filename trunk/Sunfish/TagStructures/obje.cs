using System.Collections.Generic;
using System;
using System.Text;
using System.IO;

namespace Sunfish.TagStructures
{
	public class obje : TagBlock
	{
		public obje() : base("obje", 188)
		{
			Values = new Value[]
			{
				new Data(48),
				new StringReferenceValue(),
				new TagReference(),
				new TagReference(),
				new TagReference(),
				new TagReference(),
				new TagReference(),
				new TagBlockArray(typeof(TagBlock0_0)),
				new TagBlockArray(typeof(TagBlock0_1)),
				new Data(40),
				new TagBlockArray(typeof(TagBlock0_2)),
				new TagBlockArray(typeof(TagBlock0_3)),
				new TagBlockArray(typeof(TagBlock0_4)),
				new TagBlockArray(typeof(TagBlock0_5)),
				new TagBlockArray(typeof(TagBlock0_6)),
			};
		}
		public class TagBlock0_0 : TagBlock
		{
			public TagBlock0_0() : base(16, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new Data(4),
					new StringReferenceValue(),
				});
			}
		}
		public class TagBlock0_1 : TagBlock
		{
			public TagBlock0_1() : base(32, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new Data(4),
					new StringReferenceValue(),
					new StringReferenceValue(),
					new StringReferenceValue(),
					new Data(4),
					new ByteArray(),
					new StringReferenceValue(),
				});
			}
		}
		public class TagBlock0_2 : TagBlock
		{
			public TagBlock0_2() : base(24, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new TagReference(),
					new StringReferenceValue(),
					new Data(4),
					new StringReferenceValue(),
					new StringReferenceValue(),
				});
			}
		}
		public class TagBlock0_3 : TagBlock
		{
			public TagBlock0_3() : base(8, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new TagReference(),
				});
			}
		}
		public class TagBlock0_4 : TagBlock
		{
			public TagBlock0_4() : base(8, 4)
			{
			}
		}
		public class TagBlock0_5 : TagBlock
		{
			public TagBlock0_5() : base(16, 4)
			{
				Values = InitializeValues(new Value[]
				{
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
				public TagBlock1_1() : base(40, 4)
				{
				}
			}
		}
		public class TagBlock0_6 : TagBlock
		{
			public TagBlock0_6() : base(8, 4)
			{
                Values = InitializeValues(new Value[] { 
                    new Data(4),
                    new TagIdentifier(),
                });
			}
		}
	}
}
