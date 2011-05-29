using System.Collections.Generic;
using System;
using System.Text;
using System.IO;

namespace Sunfish.TagStructures
{
	public class bipd : TagBlock
	{
		public bipd() : base("bipd", 788)
		{
            List<Value> values = new List<Value>(new unit().Values);
            values.AddRange(new Value[]{
                    new Data(112),
                    new TagReference(),
                    new Data(20),
                    new StringReference(),
                    new StringReference(),
                    new Data(4),
                    new TagBlockArray(typeof(TagBlock0_0)),
                    new TagBlockArray(typeof(TagBlock0_1)),
                    new TagBlockArray(typeof(TagBlock0_2)),
                    new Data(92),
                    new TagBlockArray(typeof(TagBlock0_3)),
                    new TagReference(),
                    new TagReference(),
            });
            Values = InitializeValues(values.ToArray());
        }

        public class TagBlock0_0 : TagBlock
        {
            public TagBlock0_0()
                : base(128, 4)
            {
                Values = InitializeValues(new Value[]
				{
                    new StringReference(),
				});
            }
        }

        public class TagBlock0_1 : TagBlock
        {
            public TagBlock0_1()
                : base(80, 4)
            {
                Values = InitializeValues(new Value[]
				{
                    new StringReference(),
				});
            }
        }

        public class TagBlock0_2 : TagBlock
        {
            public TagBlock0_2()
                : base(128, 4)
            {
                Values = InitializeValues(new Value[]
				{
                    new StringReference(),
				});
            }
        }

        public class TagBlock0_3 : TagBlock
        {
            public TagBlock0_3()
                : base(4, 4)
            {
                Values = InitializeValues(new Value[]
				{
                    new StringReference(),
				});
            }
        }
	}
}
