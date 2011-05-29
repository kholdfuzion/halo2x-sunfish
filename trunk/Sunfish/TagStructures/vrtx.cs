using System.Collections.Generic;
using System;
using System.Text;
using System.IO;

namespace Sunfish.TagStructures
{
	public class vrtx : TagBlock
	{
		public vrtx() : base("vrtx", 12)
		{
			Values = InitializeValues(new Value[]
			{
				new Data(4),
				new TagBlockArray(typeof(TagBlock0_0)),
			});
		}
		public class TagBlock0_0 : TagBlock
		{
			public TagBlock0_0() : base(28, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new Data(4),
					new TagBlockArray(typeof(TagBlock1_0)),
					new ByteArray(),
				});
			}
			public class TagBlock1_0 : TagBlock
			{
				public TagBlock1_0() : base(2, 4)
				{
				}
			}
		}
	}
}
