using System.Collections.Generic;
using System;
using System.Text;
using System.IO;

namespace Sunfish.TagStructures
{
	public class DECR : TagBlock
	{
		public DECR() : base("DECR", 108)
		{
			Values = InitializeValues(new Value[]
			{
				new TagBlockArray(typeof(TagBlock0_0)),
				new Data(8),
				new TagBlockArray(typeof(TagBlock0_1)),
				new TagBlockArray(typeof(TagBlock0_2)),
				new TagBlockArray(typeof(TagBlock0_3)),
				new TagBlockArray(typeof(TagBlock0_4)),
				new Data(8),
                new RawBlockAddress(typeof(DecoraterRawBlock)),
                new RawBlockLength(),
				new Data(8),
				new TagBlockArray(typeof(TagBlock0_5)),
				new TagIdentifier(),
			}, true);
		}

        public class DecoraterRawBlock : RawBlock
        {
            public DecoraterRawBlock(RawBlockAddress addressValue, RawBlockLength lengthValue) : base(addressValue, lengthValue) { }
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
			public TagBlock0_1() : base(20, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new StringReferenceValue(),
					new Data(8),
					new TagBlockArray(typeof(TagBlock1_0)),
				});
			}
			public class TagBlock1_0 : TagBlock
			{
				public TagBlock1_0() : base(40, 4)
				{
					Values = InitializeValues(new Value[]
					{
						new StringReferenceValue(),
					});
				}
			}
		}
		public class TagBlock0_2 : TagBlock
		{
			public TagBlock0_2() : base(8, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new StringReferenceValue(),
				});
			}
		}
		public class TagBlock0_3 : TagBlock
		{
			public TagBlock0_3() : base(56, 4)
			{
			}
		}
		public class TagBlock0_4 : TagBlock
		{
			public TagBlock0_4() : base(2, 4)
			{
			}
		}
		public class TagBlock0_5 : TagBlock
		{
			public TagBlock0_5() : base(16, 4)
			{
			}
		}
	}
}
