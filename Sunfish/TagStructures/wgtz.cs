using System.Collections.Generic;
using System;
using System.Text;
using System.IO;

namespace Sunfish.TagStructures
{
	public class wgtz : TagBlock
	{
		public wgtz() : base("wgtz", 32)
		{
			Values = InitializeValues(new Value[]
			{
				new TagReference(),
				new TagBlockArray(typeof(TagBlock0_0)),
				new TagReference(),
				new TagReference(),
			});
		}
		public class TagBlock0_0 : TagBlock
		{
			public TagBlock0_0() : base(8, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new TagReference(),
				});
			}
		}
	}
}
