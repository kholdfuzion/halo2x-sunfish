using System.Collections.Generic;
using System;
using System.Text;
using System.IO;

namespace Sunfish.TagStructures
{
    public class unit : TagBlock
    {
        public unit()
            : base("unit", 312)
        {
            List<Value> values = new List<Value>(new obje().Values);
            values.AddRange(new Value[] { 
                
                new Data(24),
                new StringReference(),
                new StringReference(),
                new Data(12),
                new TagBlockArray(typeof(TagBlock0_0)),
                new Data(68),
                new TagReference(),
                new Data(28),
                new StringReference(),
                new StringReference(),
                new StringReference(),
                new TagReference(),
                new TagReference(),
                new TagReference(),
                new TagReference(),
                new TagReference(),
                new TagReference(),
                new Data(4),
                new TagBlockArray(typeof(TagBlock0_1)),
                new TagBlockArray(typeof(TagBlock0_2)),
                new TagBlockArray(typeof(TagBlock0_3)),
                new Data(8),
                new TagBlockArray(typeof(TagBlock0_4)),
                new TagBlockArray(typeof(TagBlock0_5)),
                new TagBlockArray(typeof(TagBlock0_6)),
                new Data(28),
            });
            Values = values.ToArray();
        }

        public class TagBlock0_0 : TagBlock
        {
            public TagBlock0_0()
                : base(8, 4)
            {
                Values = InitializeValues(new Value[]
				{
                    new TagReference(),
				});
            }
        }

        public class TagBlock0_1 : TagBlock
        {
            public TagBlock0_1()
                : base(16, 4)
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
                : base(8, 4)
            {
                Values = InitializeValues(new Value[]
				{
                    new TagReference(),
				});
            }
        }

        public class TagBlock0_3 : TagBlock
        {
            public TagBlock0_3()
                : base(24, 4)
            {
                Values = InitializeValues(new Value[]
				{
                    new Data(12),
                    new TagReference(),
				});
            }
        }

        public class TagBlock0_4 : TagBlock
        {
            public TagBlock0_4()
                : base(8, 4)
            { }
        }

        public class TagBlock0_5 : TagBlock
        {
            public TagBlock0_5()
                : base(8, 4)
            {
                Values = InitializeValues(new Value[]
				{
                    new TagReference(),
				});
            }
        }

        public class TagBlock0_6 : TagBlock
        {
            public TagBlock0_6()
                : base(176, 4)
            {
                Values = InitializeValues(new Value[]
				{
                    new Data(4),
                    new StringReference(),
                    new StringReference(),
                    new StringReference(),
                    new StringReference(),
                    new StringReference(),
                    new StringReference(),
                    new Data(68),
                    new StringReference(),
                    new StringReference(),
                    new Data(12),
                    new TagBlockArray(typeof(TagBlock1_0)),
                    new TagBlockArray(typeof(TagBlock1_1)),
                    new StringReference(),
                    new Data(8),
                    new TagReference(),
                    new Data(16),
                    new StringReference(),
				});
            }

            public class TagBlock1_0 : TagBlock
            {
                public TagBlock1_0()
                    : base(8, 4)
                {
                    Values = InitializeValues(new Value[]
				    {
                        new TagReference(),
				    });
                }
            }

            public class TagBlock1_1 : TagBlock
            {
                public TagBlock1_1()
                    : base(8, 4)
                {
                    Values = InitializeValues(new Value[]
				    {
                        new TagReference(),
				    });
                }
            }
        }
    }
}
