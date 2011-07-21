using System.Collections.Generic;
using System;
using System.Text;
using System.IO;

namespace Sunfish.TagStructures
{
	public class PRTM : TagBlock
	{
		public PRTM() : base("PRTM", 220)
		{
			Values = InitializeValues(new Value[]
			{
				new Data(24),
				new TagReference(),
				new Data(8),
				new ByteArray(),
				new Data(8),
				new ByteArray(),
				new Data(8),
				new ByteArray(),
				new Data(8),
				new ByteArray(),
				new TagReference(),
				new TagReference(),
				new TagBlockArray(typeof(TagBlock0_0)),
				new TagBlockArray(typeof(TagBlock0_1)),
				new TagBlockArray(typeof(TagBlock0_2)),
				new TagBlockArray(typeof(TagBlock0_3)),
				new TagBlockArray(typeof(TagBlock0_4)),
                new Data(8),
                new RawBlockAddress(typeof(ParticleModelRawBlock)),
                new RawBlockLength(),
				new Data(8),
				new TagBlockArray(typeof(TagBlock0_5)),
				new TagIdentifier(),
			}, true);
		}

        public class ParticleModelRawBlock : RawBlock
        {
            public ParticleModelRawBlock(RawBlockAddress addressValue, RawBlockLength lengthValue) : base(addressValue, lengthValue) { }
        }


		public class TagBlock0_0 : TagBlock
		{
			public TagBlock0_0() : base(4, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new StringReferenceValue(),
				});
			}
		}
		public class TagBlock0_1 : TagBlock
		{
			public TagBlock0_1() : base(56, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new TagReference(),
					new Data(40),
					new TagBlockArray(typeof(TagBlock1_0)),
				});
			}
			public class TagBlock1_0 : TagBlock
			{
				public TagBlock1_0() : base(184, 4)
				{
					Values = InitializeValues(new Value[]
					{
						new TagReference(),
						new Data(8),
						new ByteArray(),
						new Data(8),
						new ByteArray(),
						new Data(8),
						new ByteArray(),
						new Data(8),
						new ByteArray(),
						new Data(8),
						new ByteArray(),
						new Data(8),
						new ByteArray(),
						new Data(8),
						new ByteArray(),
						new Data(12),
						new ByteArray(),
						new Data(8),
						new ByteArray(),
					});
				}
			}
		}
		public class TagBlock0_2 : TagBlock
		{
			public TagBlock0_2() : base(8, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new StringReferenceValue(),
				});
			}
		}
		public class TagBlock0_3 : TagBlock
		{
			public TagBlock0_3() : base(56, 4)
			{
			}
		}
		public class TagBlock0_4 : TagBlock
		{
			public TagBlock0_4() : base(2, 4)
			{
			}
		}
		public class TagBlock0_5 : TagBlock
		{
			public TagBlock0_5() : base(16, 4)
			{
			}
		}
	}
}
