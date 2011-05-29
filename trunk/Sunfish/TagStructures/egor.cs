using System.Collections.Generic;
using System;
using System.Text;
using System.IO;

namespace Sunfish.TagStructures
{
	public class egor : TagBlock
	{
		public egor() : base("egor", 144)
		{
			Values = InitializeValues(new Value[]
			{
				new Data(64),
				new TagReference(),
				new Data(64),
				new TagBlockArray(typeof(TagBlock0_0)),
			});
		}
		public class TagBlock0_0 : TagBlock
		{
			public TagBlock0_0() : base(172, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new Data(164),
					new TagBlockArray(typeof(TagBlock1_0)),
				});
			}
			public class TagBlock1_0 : TagBlock
			{
				public TagBlock1_0() : base(92, 4)
				{
				}
			}
		}
	}
}
