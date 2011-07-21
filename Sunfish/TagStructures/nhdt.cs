using System.Collections.Generic;
using System;
using System.Text;
using System.IO;

namespace Sunfish.TagStructures
{
	public class nhdt : TagBlock
	{
		public nhdt() : base("nhdt", 40)
		{
			Values = InitializeValues(new Value[]
			{
				new TagReference(),
				new TagBlockArray(typeof(TagBlock0_0)),
				new TagBlockArray(typeof(TagBlock0_1)),
				new Data(8),
				new TagBlockArray(typeof(TagBlock0_2)),
			});
		}
		public class TagBlock0_0 : TagBlock
		{
			public TagBlock0_0() : base(100, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new StringReferenceValue(),
					new Data(28),
					new TagReference(),
					new TagReference(),
					new Data(40),
					new TagBlockArray(typeof(TagBlock1_0)),
				});
			}
			public class TagBlock1_0 : TagBlock
			{
				public TagBlock1_0() : base(104, 4)
				{
					Values = InitializeValues(new Value[]
					{
						new Data(4),
						new StringReferenceValue(),
						new StringReferenceValue(),
						new Data(4),
						new ByteArray(),
						new StringReferenceValue(),
						new StringReferenceValue(),
						new Data(4),
						new ByteArray(),
						new StringReferenceValue(),
						new StringReferenceValue(),
						new Data(4),
						new ByteArray(),
						new StringReferenceValue(),
						new StringReferenceValue(),
						new Data(4),
						new ByteArray(),
						new StringReferenceValue(),
						new StringReferenceValue(),
						new Data(4),
						new ByteArray(),
					});
				}
			}
		}
		public class TagBlock0_1 : TagBlock
		{
			public TagBlock0_1() : base(84, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new StringReferenceValue(),
					new Data(28),
					new TagReference(),
					new StringReferenceValue(),
					new Data(32),
					new TagBlockArray(typeof(TagBlock1_0)),
				});
			}
			public class TagBlock1_0 : TagBlock
			{
				public TagBlock1_0() : base(104, 4)
				{
					Values = InitializeValues(new Value[]
					{
						new Data(4),
						new StringReferenceValue(),
						new StringReferenceValue(),
						new Data(4),
						new ByteArray(),
						new StringReferenceValue(),
						new StringReferenceValue(),
						new Data(4),
						new ByteArray(),
						new StringReferenceValue(),
						new StringReferenceValue(),
						new Data(4),
						new ByteArray(),
						new StringReferenceValue(),
						new StringReferenceValue(),
						new Data(4),
						new ByteArray(),
						new StringReferenceValue(),
						new StringReferenceValue(),
						new Data(4),
						new ByteArray(),
					});
				}
			}
		}
		public class TagBlock0_2 : TagBlock
		{
			public TagBlock0_2() : base(80, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new StringReferenceValue(),
					new Data(28),
					new TagReference(),
					new TagReference(),
					new TagReference(),
					new TagReference(),
				});
			}
		}
	}
}
