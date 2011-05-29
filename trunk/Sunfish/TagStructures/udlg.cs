using System.Collections.Generic;
using System;
using System.Text;
using System.IO;

namespace Sunfish.TagStructures
{
	public class udlg : TagBlock
	{
		public udlg() : base("udlg", 24)
		{
			Values = InitializeValues(new Value[]
			{
				new TagReference(),
				new Data(4),
				new TagBlockArray(typeof(TagBlock0_0)),
				new StringReference(),
			});
		}
		public class TagBlock0_0 : TagBlock
		{
			public TagBlock0_0() : base(16, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new Data(4),
					new StringReference(),
					new TagReference(),
				});
			}
		}
	}
}
