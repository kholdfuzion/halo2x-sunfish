using System.Collections.Generic;
using System;
using System.Text;
using System.IO;

namespace Sunfish.TagStructures
{
	public class adlg : TagBlock
	{
		public adlg() : base("adlg", 44)
		{
			Values = InitializeValues(new Value[]
			{
				new TagBlockArray(typeof(TagBlock0_0)),
				new TagBlockArray(typeof(TagBlock0_1)),
				new Data(12),
				new TagBlockArray(typeof(TagBlock0_2)),
				new TagBlockArray(typeof(TagBlock0_3)),
			});
		}
		public class TagBlock0_0 : TagBlock
		{
			public TagBlock0_0() : base(96, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new StringReference(),
					new StringReference(),
					new Data(68),
					new StringReference(),
					new TagBlockArray(typeof(TagBlock1_0)),
				});
			}
			public class TagBlock1_0 : TagBlock
			{
				public TagBlock1_0() : base(12, 4)
				{
					Values = InitializeValues(new Value[]
					{
						new StringReference(),
					});
				}
			}
		}
		public class TagBlock0_1 : TagBlock
		{
			public TagBlock0_1() : base(64, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new Data(4),
					new StringReference(),
					new Data(32),
					new StringReference(),
					new Data(4),
					new StringReference(),
				});
			}
		}
		public class TagBlock0_2 : TagBlock
		{
			public TagBlock0_2() : base(4, 4)
			{
			}
		}
		public class TagBlock0_3 : TagBlock
		{
			public TagBlock0_3() : base(4, 4)
			{
			}
		}
	}
}
