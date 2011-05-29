using System.Collections.Generic;
using System;
using System.Text;
using System.IO;

namespace Sunfish.TagStructures
{
	public class lens : TagBlock
	{
		public lens() : base("lens", 100)
		{
			Values = InitializeValues(new Value[]
			{
				new Data(32),
				new TagReference(),
				new Data(24),
				new TagBlockArray(typeof(TagBlock0_0)),
				new Data(4),
				new TagBlockArray(typeof(TagBlock0_1)),
				new TagBlockArray(typeof(TagBlock0_2)),
				new TagBlockArray(typeof(TagBlock0_3)),
			});
		}
		public class TagBlock0_0 : TagBlock
		{
			public TagBlock0_0() : base(48, 4)
			{
			}
		}
		public class TagBlock0_1 : TagBlock
		{
			public TagBlock0_1() : base(8, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new ByteArray(),
				});
			}
		}
		public class TagBlock0_2 : TagBlock
		{
			public TagBlock0_2() : base(8, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new ByteArray(),
				});
			}
		}
		public class TagBlock0_3 : TagBlock
		{
			public TagBlock0_3() : base(8, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new ByteArray(),
				});
			}
		}
	}
}
