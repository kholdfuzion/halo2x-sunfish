using System.Collections.Generic;
using System;
using System.Text;
using System.IO;

namespace Sunfish.TagStructures
{
	public class sily : TagBlock
	{
		public sily() : base("sily", 36)
		{
			Values = InitializeValues(new Value[]
			{
				new Data(8),
				new TagReference(),
				new StringReference(),
				new StringReference(),
				new StringReference(),
				new TagBlockArray(typeof(TagBlock0_0)),
			});
		}
		public class TagBlock0_0 : TagBlock
		{
			public TagBlock0_0() : base(12, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new Data(8),
					new StringReference(),
				});
			}
		}
	}
}
