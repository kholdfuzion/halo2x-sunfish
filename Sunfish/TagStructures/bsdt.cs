using System.Collections.Generic;
using System;
using System.Text;
using System.IO;

namespace Sunfish.TagStructures
{
	public class bsdt : TagBlock
	{
		public bsdt() : base("bsdt", 32)
		{
			Values = InitializeValues(new Value[]
			{
				new Data(4),
				new TagReference(),
				new TagReference(),
				new TagBlockArray(typeof(TagBlock0_0)),
			});
		}
		public class TagBlock0_0 : TagBlock
		{
			public TagBlock0_0() : base(56, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new TagReference(),
					new Data(40),
					new TagBlockArray(typeof(TagBlock1_0)),
				});
			}
			public class TagBlock1_0 : TagBlock
			{
				public TagBlock1_0() : base(184, 4)
				{
					Values = InitializeValues(new Value[]
					{
						new TagReference(),
						new Data(8),
						new ByteArray(),
						new Data(8),
						new ByteArray(),
						new Data(8),
						new ByteArray(),
						new Data(8),
						new ByteArray(),
						new Data(8),
						new ByteArray(),
						new Data(8),
						new ByteArray(),
						new Data(8),
						new ByteArray(),
						new Data(12),
						new ByteArray(),
						new Data(8),
						new ByteArray(),
					});
				}
			}
		}
	}
}
