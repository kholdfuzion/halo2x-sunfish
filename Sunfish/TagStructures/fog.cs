using System.Collections.Generic;
using System;
using System.Text;
using System.IO;

namespace Sunfish.TagStructures
{
	public class fog : TagBlock
	{
		public fog() : base("fog ", 96)
		{
			Values = InitializeValues(new Value[]
			{
				new Data(4),
				new StringReferenceValue(),
				new Data(40),
				new TagBlockArray(typeof(TagBlock0_0)),
				new TagReference(),
				new TagReference(),
				new Data(8),
				new TagReference(),
				new TagReference(),
			});
		}
		public class TagBlock0_0 : TagBlock
		{
			public TagBlock0_0() : base(52, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new Data(44),
					new TagReference(),
				});
			}
		}
	}
}
