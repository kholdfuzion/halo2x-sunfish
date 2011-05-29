using System.Collections.Generic;
using System;
using System.Text;
using System.IO;

namespace Sunfish.TagStructures
{
	public class jpt : TagBlock
	{
		public jpt() : base("jpt!", 200)
		{
			Values = InitializeValues(new Value[]
			{
				new Data(80),
				new StringReference(),
				new StringReference(),
				new Data(20),
				new TagBlockArray(typeof(TagBlock0_0)),
				new Data(52),
                new TagReference(),
			});
		}
		public class TagBlock0_0 : TagBlock
		{
            public TagBlock0_0()
                : base(76, 4)
            {
                Values = InitializeValues(new Value[] {                     
                    new Data(40),
                    new ByteArray(),
                    new Data(4),
                    new ByteArray(),
                    new StringReference(),
                    new Data(4),
                    new TagBlockArray(typeof(TagBlock1_0)),
                });
            }

            public class TagBlock1_0 : TagBlock
            {
                public TagBlock1_0()
                    : base(12, 4)
                { }
            }
		}
	}
}
