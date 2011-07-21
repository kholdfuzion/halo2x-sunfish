using System.Collections.Generic;
using System;
using System.Text;
using System.IO;

namespace Sunfish.TagStructures
{
	public class fx : TagBlock
	{
		public fx() : base("<fx>", 28)
		{
			Values = InitializeValues(new Value[]
			{
				new Data(12),
				new TagBlockArray(typeof(TagBlock0_0)),
				new TagBlockArray(typeof(TagBlock0_1)),
			});
		}
		public class TagBlock0_0 : TagBlock
		{
			public TagBlock0_0() : base(16, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new StringReferenceValue(),
					new ByteArray(),
				});
			}
		}
		public class TagBlock0_1 : TagBlock
		{
			public TagBlock0_1() : base(12, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new TagBlockArray(typeof(TagBlock1_0)),
					new StringReferenceValue(),
				});
			}
			public class TagBlock1_0 : TagBlock
			{
				public TagBlock1_0() : base(24, 4)
				{
					Values = InitializeValues(new Value[]
					{
						new StringReferenceValue(),
						new Data(12),
						new TagBlockArray(typeof(TagBlock2_0)),
					});
				}
				public class TagBlock2_0 : TagBlock
				{
					public TagBlock2_0() : base(16, 4)
					{
					}
				}
			}
		}
	}
}
