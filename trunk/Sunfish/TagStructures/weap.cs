using System.Collections.Generic;
using System;
using System.Text;
using System.IO;

namespace Sunfish.TagStructures
{
    public class weap : TagBlock
    {
        public weap()
            : base("weap", 796)
        {
            List<Value> values = new List<Value>(new item().Values);
            values.AddRange(new Value[]{
                new Data(20),
                new TagReference(),
                new TagReference(),
                new Data(28),
                new TagReference(),
                new TagReference(),
                new TagReference(),
                new TagReference(),
                new TagReference(),
                new TagReference(),
                new Data(32),
                new TagReference(),
                new TagReference(),
                new TagReference(),
                new TagReference(),
                new TagReference(),
                new TagReference(),
                new TagReference(),
                new TagReference(),
                new Data(72),                
                new TagReference(),
                new TagReference(),
                new Data(16),
                new TagReference(),
                new TagReference(),
                new TagReference(),
                new Data(8),
                new StringReference(),
                new StringReference(),
                new StringReference(),
                new Data(24),
                new TagBlockArray(typeof(TagBlock0_0)),
                new TagReference(),
                new TagBlockArray(typeof(TagBlock0_1)),
                new TagBlockArray(typeof(TagBlock0_2)),
                new TagBlockArray(typeof(TagBlock0_3)),
                new TagBlockArray(typeof(TagBlock0_4)),
                new Data(24),
                new TagReference(),
                new TagReference(),
                new TagReference(),
            });
            Values = InitializeValues(values.ToArray());
        }

        public class TagBlock0_0 : TagBlock
        {
            public TagBlock0_0()
                : base(16, 4)
            {
                Values = InitializeValues(new Value[]
				{
                    new TagReference(),
                    new TagReference(),
				});
            }
        }

        public class TagBlock0_1 : TagBlock
        {
            public TagBlock0_1()
                : base(8, 4)
            {
                Values = InitializeValues(new Value[]
				{
                    new Data(4),
                    new TagIdentifier(),
				});
            }
        }

        public class TagBlock0_2 : TagBlock
        {
            public TagBlock0_2()
                : base(92, 4)
            {
                Values = InitializeValues(new Value[]
				{
                    new Data(52),
                    new TagReference(),
                    new TagReference(),
                    new TagReference(),
                    new TagReference(),
                    new TagBlockArray(typeof(TagBlock1_0)),
                });
            }

            public class TagBlock1_0 : TagBlock
            {
                public TagBlock1_0()
                    : base(12, 4)
                {
                    Values = InitializeValues(new Value[]
				{
                        new Data(4),
                        new TagReference(),
                });
                }
            }
        }

        public class TagBlock0_3 : TagBlock
        {
            public TagBlock0_3()
                : base(64, 4)
            {
                Values = InitializeValues(new Value[]
				{
                    new Data(48),
                    new TagReference(),
                    new TagReference(),
				});
            }
        }

        public class TagBlock0_4 : TagBlock
        {
            public TagBlock0_4()
                : base(236, 4)
            {
                Values = InitializeValues(new Value[]
				{ new Data(48),
                    new StringReference(),
                    new Data(88),
                    new TagReference(),
                    new TagReference(),
                    new Data(72),
                    new TagBlockArray(typeof(TagBlock1_0)),
                });
            }

            public class TagBlock1_0 : TagBlock
            {
                public TagBlock1_0()
                    : base(52, 4)
                {
                    Values = InitializeValues(new Value[]
				    {
                        new Data(4),
                        new TagReference(),
                        new TagReference(),
                        new TagReference(),
                        new TagReference(),
                        new TagReference(),
                        new TagReference(),
                    });
                }
            }
        }
    }
}