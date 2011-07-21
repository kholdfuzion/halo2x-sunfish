using System.Collections.Generic;
using System;
using System.Text;
using System.IO;

namespace Sunfish.TagStructures
{
	public class tdtl : TagBlock
	{
		public tdtl() : base("tdtl", 112)
		{
			Values = InitializeValues(new Value[]
			{
				new Data(4),
				new StringReferenceValue(),
				new Data(96),
				new TagBlockArray(typeof(TagBlock0_0)),
			});
		}
		public class TagBlock0_0 : TagBlock
		{
			public TagBlock0_0() : base(236, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new Data(28),
					new TagReference(),
					new TagReference(),
					new Data(8),
					new ByteArray(),
					new ByteArray(),
					new Data(4),
					new ByteArray(),
					new Data(132),
					new TagBlockArray(typeof(TagBlock1_0)),
					new ByteArray(),
					new ByteArray(),
				});
			}
			public class TagBlock1_0 : TagBlock
			{
				public TagBlock1_0() : base(56, 4)
				{
					Values = InitializeValues(new Value[]
					{
						new Data(16),
						new ByteArray(),
						new ByteArray(),
						new ByteArray(),
						new ByteArray(),
						new ByteArray(),
					});
				}
			}
		}
	}
}
