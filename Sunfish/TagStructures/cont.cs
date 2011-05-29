using System.Collections.Generic;
using System;
using System.Text;
using System.IO;

namespace Sunfish.TagStructures
{
	public class cont : TagBlock
	{
		public cont() : base("cont", 240)
		{
			Values = InitializeValues(new Value[]
			{
				new Data(48),
				new TagReference(),
				new Data(80),
				new TagReference(),
				new Data(88),
				new TagBlockArray(typeof(TagBlock0_0)),
			});
		}
		public class TagBlock0_0 : TagBlock
		{
			public TagBlock0_0() : base(64, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new Data(16),
					new TagReference(),
				});
			}
		}
	}
}
