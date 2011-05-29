using System.Collections.Generic;
using System;
using System.Text;
using System.IO;

namespace Sunfish.TagStructures
{
	public class MGS2 : TagBlock
	{
		public MGS2() : base("MGS2", 16)
		{
			Values = InitializeValues(new Value[]
			{
				new Data(8),
				new TagBlockArray(typeof(TagBlock0_0)),
			});
		}
		public class TagBlock0_0 : TagBlock
		{
			public TagBlock0_0() : base(152, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new Data(4),
					new TagReference(),
					new Data(4),
					new ByteArray(),
					new ByteArray(),
					new ByteArray(),
					new ByteArray(),
					new ByteArray(),
					new TagBlockArray(typeof(TagBlock1_0)),
					new Data(32),
					new TagBlockArray(typeof(TagBlock1_1)),
				});
			}
			public class TagBlock1_0 : TagBlock
			{
				public TagBlock1_0() : base(28, 4)
				{
					Values = InitializeValues(new Value[]
					{
						new ByteArray(),
						new ByteArray(),
					});
				}
			}
			public class TagBlock1_1 : TagBlock
			{
				public TagBlock1_1() : base(8, 4)
				{
				}
			}
		}
	}
}
