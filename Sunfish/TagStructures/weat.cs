using System.Collections.Generic;
using System;
using System.Text;
using System.IO;

namespace Sunfish.TagStructures
{
	public class weat : TagBlock
	{
		public weat() : base("weat", 176)
		{
			Values = InitializeValues(new Value[]
			{
				new TagBlockArray(typeof(TagBlock0_0)),
				new TagBlockArray(typeof(TagBlock0_1)),
			});
		}

        public class WeatherRawBlock : RawBlock
        {
            public WeatherRawBlock(RawBlockAddress addressValue, RawBlockLength lengthValue) : base(addressValue, lengthValue) { }
        }


		public class TagBlock0_0 : TagBlock
		{
			public TagBlock0_0() : base(140, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new TagReference(),
					new Data(56),
                    new RawBlockAddress(typeof(WeatherRawBlock)),
                    new RawBlockLength(),
					new TagBlockArray(typeof(TagBlock1_0)),
					new TagIdentifier(),
				}, true);
			}
			public class TagBlock1_0 : TagBlock
			{
				public TagBlock1_0() : base(16, 4)
				{
				}
			}
		}
		public class TagBlock0_1 : TagBlock
		{
			public TagBlock0_1() : base(936, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new TagReference(),
					new TagReference(),
					new TagReference(),
				});
			}
		}
	}
}
