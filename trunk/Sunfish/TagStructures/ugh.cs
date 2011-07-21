using System.Collections.Generic;
using System;
using System.Text;
using System.IO;

namespace Sunfish.TagStructures
{
	public class ugh : TagBlock
	{
		public ugh() : base("ugh!", 88)
		{
			Values = InitializeValues(new Value[]
			{
				new TagBlockArray(typeof(TagBlock0_0)),
				new TagBlockArray(typeof(TagBlock0_1)),
				new TagBlockArray(typeof(TagBlock0_2)),
				new TagBlockArray(typeof(TagBlock0_3)),
				new TagBlockArray(typeof(TagBlock0_4)),
				new TagBlockArray(typeof(TagBlock0_5)),
				new TagBlockArray(typeof(TagBlock0_6)),
				new ByteArray(),
				new TagBlockArray(typeof(TagBlock0_7)),
				new TagBlockArray(typeof(TagBlock0_8)),
				new TagBlockArray(typeof(TagBlock0_9)),
			});
		}

        public class SoundRawBlock : RawBlock
        {
            public SoundRawBlock(RawBlockAddress addressValue, RawBlockLength lengthValue) : base(addressValue, lengthValue) { }

            protected override int length
            {
                get { return BitConverter.ToInt32(dataRef, lengthOffset) & 0x7FFFFFFF; }
                set
                {
                    byte[] setBytes = BitConverter.GetBytes(value);
                    dataRef[lengthOffset] = setBytes[0];
                    dataRef[lengthOffset + 1] = setBytes[1];
                    dataRef[lengthOffset + 2] = setBytes[2];
                    dataRef[lengthOffset + 3] = setBytes[3];
                }
            }
        }

        public class UnknownRawBlock : RawBlock
        {
            public UnknownRawBlock(RawBlockAddress addressValue, RawBlockLength lengthValue) : base(addressValue, lengthValue) { }
        }


		public class TagBlock0_0 : TagBlock
		{
			public TagBlock0_0() : base(56, 4)
			{
			}
		}
		public class TagBlock0_1 : TagBlock
		{
			public TagBlock0_1() : base(20, 4)
			{
			}
		}
		public class TagBlock0_2 : TagBlock
		{
			public TagBlock0_2() : base(4, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new StringReferenceValue(),
				});
			}
		}
		public class TagBlock0_3 : TagBlock
		{
			public TagBlock0_3() : base(10, 4)
			{
			}
		}
		public class TagBlock0_4 : TagBlock
		{
			public TagBlock0_4() : base(12, 4)
			{
			}
		}
		public class TagBlock0_5 : TagBlock
		{
			public TagBlock0_5() : base(16, 4)
			{
			}
		}
		public class TagBlock0_6 : TagBlock
		{
			public TagBlock0_6() : base(52, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new Data(20),
					new TagBlockArray(typeof(TagBlock1_0)),
					new TagBlockArray(typeof(TagBlock1_1)),
				});
            }
            public class TagBlock1_0 : TagBlock
            {
                public TagBlock1_0()
                    : base(72, 4)
                {
                }
            }
            public class TagBlock1_1 : TagBlock
            {
                public TagBlock1_1()
                    : base(48, 4)
                {
                }
            }
		}
		public class TagBlock0_7 : TagBlock
		{
			public TagBlock0_7() : base(12, 4)
            {
                Values = InitializeValues(new Value[]
				{
                    new RawBlockAddress(typeof(SoundRawBlock)),
                    new RawBlockLength(),
				}, true);
			}
		}
		public class TagBlock0_8 : TagBlock
		{
			public TagBlock0_8() : base(28, 4)
			{
				Values = InitializeValues(new Value[]
				{
					new TagBlockArray(typeof(TagBlock1_0)),
					new TagBlockArray(typeof(TagBlock1_1)),
				});
			}
			public class TagBlock1_0 : TagBlock
			{
				public TagBlock1_0() : base(16, 4)
				{
				}
			}
			public class TagBlock1_1 : TagBlock
			{
				public TagBlock1_1() : base(4, 4)
				{
				}
			}
		}
		public class TagBlock0_9 : TagBlock
		{
			public TagBlock0_9() : base(44, 4)
			{
				Values = InitializeValues(new Value[]
				{
                    new Data(8),
                    new RawBlockAddress(typeof(UnknownRawBlock)),
                    new RawBlockLength(),
					new Data(8),
					new TagBlockArray(typeof(TagBlock1_0)),
					new TagIdentifier(),
				}, true);
			}
			public class TagBlock1_0 : TagBlock
			{
				public TagBlock1_0() : base(16, 4)
				{
				}
			}
		}
	}
}
