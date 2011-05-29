using System.Collections.Generic;
using System;
using System.Text;
using System.IO;

namespace Sunfish.TagStructures
{
	public class vehi : TagBlock
	{
		public vehi() : base("vehi", 768)
		{
            List<Value> values = new List<Value>(new unit().Values);
            values.AddRange(new Value[]{
                new Data(48),
                new StringReference(),
                new Data(72),
                new TagBlockArray(typeof(TagBlock0_0)),
                new Data(20),
                new TagReference(),
                new TagReference(),
                new TagReference(),
                new TagReference(),
                new TagReference(),
                new Data(60),
                new TagBlockArray(typeof(TagBlock0_1)),
                new TagBlockArray(typeof(TagBlock0_2)),
                new TagBlockArray(typeof(TagBlock0_3)),
            });
            Values = InitializeValues(values.ToArray());
        }

        public class TagBlock0_0 : TagBlock
        {
            public TagBlock0_0()
                : base(68, 4)
            { }
        }

        public class TagBlock0_1 : TagBlock
        {
            public TagBlock0_1()
                : base(76, 4)
            {
                Values = InitializeValues(new Value[]
				{
                    new StringReference(),
                    new Data(48),
                    new StringReference(),
				});
            }
        }

        public class TagBlock0_2 : TagBlock
        {
            public TagBlock0_2()
                : base(76, 4)
            {
                Values = InitializeValues(new Value[]
				{
                    new StringReference(),
                    new Data(56),
                    new StringReference(),
				});
            }
        }

        public class TagBlock0_3 : TagBlock
        {
            public TagBlock0_3()
                : base(672, 16)
            { }
        }
	}
}
