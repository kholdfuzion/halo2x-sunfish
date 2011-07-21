using System.Collections.Generic;
using System;
using System.Text;
using System.IO;

namespace Sunfish.TagStructures
{
	public class lsnd : TagBlock
	{
		public lsnd() : base("lsnd", 44)
		{
			Values = InitializeValues(new Value[]
			{
				new Data(20),
				new TagReference(),
				new TagBlockArray(typeof(TagBlock0_0)),
				new TagBlockArray(typeof(TagBlock0_1)),
			});
		}
		public class TagBlock0_0 : TagBlock
		{
			public TagBlock0_0() : base(88, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new StringReferenceValue(),
					new Data(16),
					new TagReference(),
					new TagReference(),
					new TagReference(),
					new TagReference(),
					new TagReference(),
					new Data(4),
					new TagReference(),
					new TagReference(),
				});
			}
		}
		public class TagBlock0_1 : TagBlock
		{
			public TagBlock0_1() : base(52, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new StringReferenceValue(),
					new TagReference(),
				});
			}
		}
	}
}
