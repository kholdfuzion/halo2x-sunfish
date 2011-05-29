using System.Collections.Generic;
using System;
using System.Text;
using System.IO;

namespace Sunfish.TagStructures
{
    public class sfx : TagBlock
    {
        public sfx()
            : base("sfx+", 8)
        {
            Values = InitializeValues(new Value[] { 
                new TagBlockArray(typeof(TagBlock0_0)),
            });
        }
        public class TagBlock0_0 : TagBlock
        {
            public TagBlock0_0()
                : base(56, 4)
            {
                Values = InitializeValues(new Value[] {                     
                    new StringReference(),
                    new Data(20),
                    new TagBlockArray(typeof(TagBlock1_0)),
                    new TagBlockArray(typeof(TagBlock1_1)),
                    new TagBlockArray(typeof(TagBlock1_2)),
                    new TagBlockArray(typeof(TagBlock1_3)),
                });
            }

            public class TagBlock1_0 : TagBlock
            {
                public TagBlock1_0()
                    : base(72, 4)
                { }
            }

            public class TagBlock1_1 : TagBlock
            {
                public TagBlock1_1()
                    : base(48, 4)
                { }
            }

            public class TagBlock1_2 : TagBlock
            {
                public TagBlock1_2()
                    : base(64, 4)
                { }
            }

            public class TagBlock1_3 : TagBlock
            {
                public TagBlock1_3()
                    : base(40, 4)
                {
                    Values = InitializeValues(new Value[] 
                    {   
                        new TagReference(),
                        new TagBlockArray(typeof(TagBlock2_0)),
                        new Data(16),
                        new TagBlockArray(typeof(TagBlock2_1)),
                    });
                }

                public class TagBlock2_0 : TagBlock
                {
                    public TagBlock2_0()
                        : base(16, 4)
                    {
                        Values = InitializeValues(new Value[] 
                        {   
                            new TagReference(),
                        });
                    }
                }

                public class TagBlock2_1 : TagBlock
                {
                    public TagBlock2_1()
                        : base(20, 4)
                    {
                        Values = InitializeValues(new Value[] 
                        {   
                            new TagBlockArray(typeof(TagBlock3_0)),
                            new TagBlockArray(typeof(TagBlock3_1)),
                        });
                    }

                    public class TagBlock3_0 : TagBlock
                    {
                        public TagBlock3_0()
                            : base(28, 4)
                        {
                            Values = InitializeValues(new Value[] 
                            {   
                                new TagBlockArray(typeof(TagBlock4_0)),   
                                new TagBlockArray(typeof(TagBlock4_1)),   
                                new TagBlockArray(typeof(TagBlock4_2)),
                            });
                        }

                        public class TagBlock4_0 : TagBlock
                        {
                            public TagBlock4_0()
                                : base(16, 4)
                            {
                                Values = InitializeValues(new Value[] 
                                {   
                                    new Data(4),
                                    new ByteArray(),
                                });
                            }
                        }

                        public class TagBlock4_1 : TagBlock
                        {
                            public TagBlock4_1()
                                : base(4, 4)
                            { }
                        }

                        public class TagBlock4_2 : TagBlock
                        {
                            public TagBlock4_2()
                                : base(4, 4)
                            { }
                        }

                    }

                    public class TagBlock3_1 : TagBlock
                    {
                        public TagBlock3_1()
                            : base(16, 4)
                        {
                            Values = InitializeValues(new Value[] {                                 
                                new Data(4),
                                new ByteArray(),
                            });
                        }
                    }
                }
            }
        }
    }
}