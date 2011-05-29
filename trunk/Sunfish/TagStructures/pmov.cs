using System.Collections.Generic;
using System;
using System.Text;
using System.IO;

namespace Sunfish.TagStructures
{
	public class pmov : TagBlock
	{
		public pmov() : base("pmov", 20)
		{
			Values = InitializeValues(new Value[]
			{
				new TagReference(),
				new Data(4),
				new TagBlockArray(typeof(TagBlock0_0)),
			});
		}
		public class TagBlock0_0 : TagBlock
		{
			public TagBlock0_0() : base(20, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new Data(4),
					new TagBlockArray(typeof(TagBlock1_0)),
				});
			}
			public class TagBlock1_0 : TagBlock
			{
				public TagBlock1_0() : base(20, 4)
				{
					Values = InitializeValues(new Value[]
					{
						new Data(12),
						new ByteArray(),
					});
				}
			}
		}
	}
}
