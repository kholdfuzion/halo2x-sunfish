using System.Collections.Generic;
using System;
using System.Text;
using System.IO;

namespace Sunfish.TagStructures
{
	public class ligh : TagBlock
	{
		public ligh() : base("ligh", 228)
		{
			Values = InitializeValues(new Value[]
			{
				new Data(120),
				new TagReference(),
				new Data(16),
				new TagReference(),
				new Data(4),
				new TagReference(),
				new Data(32),
				new TagBlockArray(typeof(TagBlock0_0)),
				new TagBlockArray(typeof(TagBlock0_1)),
				new TagBlockArray(typeof(TagBlock0_2)),
				new TagReference(),
			});
		}
		public class TagBlock0_0 : TagBlock
		{
			public TagBlock0_0() : base(76, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new ByteArray(),
				});
			}
		}
		public class TagBlock0_1 : TagBlock
		{
			public TagBlock0_1() : base(188, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new ByteArray(),
				});
			}
		}
		public class TagBlock0_2 : TagBlock
		{
			public TagBlock0_2() : base(16, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new ByteArray(),
					new ByteArray(),
				});
			}
		}
	}
}
