using System.Collections.Generic;
using System;
using System.Text;
using System.IO;

namespace Sunfish.TagStructures
{
	public class mdlg : TagBlock
	{
		public mdlg() : base("mdlg", 8)
		{
			Values = InitializeValues(new Value[]
			{
				new TagBlockArray(typeof(TagBlock0_0)),
			});
		}
		public class TagBlock0_0 : TagBlock
		{
			public TagBlock0_0() : base(16, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new StringReference(),
					new TagBlockArray(typeof(TagBlock1_0)),
					new StringReference(),
				});
			}
			public class TagBlock1_0 : TagBlock
			{
				public TagBlock1_0() : base(16, 4)
				{
					Values = InitializeValues(new Value[]
					{
						new StringReference(),
						new TagReference(),
					});
				}
			}
		}
	}
}
