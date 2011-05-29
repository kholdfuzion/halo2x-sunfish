using System.Collections.Generic;
using System;
using System.Text;
using System.IO;

namespace Sunfish.TagStructures
{
	public class bitm : TagBlock
	{
		public bitm() : base("bitm", 76)
		{
			Values = InitializeValues(new Value[]
			{
				new Data(60),
				new TagBlockArray(typeof(TagBlock0_0)),
				new TagBlockArray(typeof(TagBlock0_1)),
			});
		}

        public class BitmapRawBlock : RawBlock
        {
            public BitmapRawBlock(RawBlockAddress addressValue, RawBlockLength lengthValue) : base(addressValue, lengthValue) { }
        }

		public class TagBlock0_0 : TagBlock
		{
			public TagBlock0_0() : base(60, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new Data(52),
					new TagBlockArray(typeof(TagBlock1_0)),
				});
			}
			public class TagBlock1_0 : TagBlock
			{
				public TagBlock1_0() : base(32, 4)
				{
				}
			}
		}
		public class TagBlock0_1 : TagBlock
		{
			public TagBlock0_1() : base(116, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new Data(28),
                    new RawBlockAddress(typeof(BitmapRawBlock)),
                    new RawBlockAddress(typeof(BitmapRawBlock)),
                    new RawBlockAddress(typeof(BitmapRawBlock)),
                    new RawBlockAddress(typeof(BitmapRawBlock)),
                    new RawBlockAddress(typeof(BitmapRawBlock)),
                    new RawBlockAddress(typeof(BitmapRawBlock)),
                    new RawBlockLength(),
                    new RawBlockLength(),
                    new RawBlockLength(),
                    new RawBlockLength(),
                    new RawBlockLength(),
                    new RawBlockLength(),
					new TagIdentifier(),
				}, true);
			}
		}
	}
}
