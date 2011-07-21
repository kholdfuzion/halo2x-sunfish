using System.Collections.Generic;
using System;
using System.Text;
using System.IO;

namespace Sunfish.TagStructures
{
    public class item : TagBlock
    {
        public item()
            : base("item", 188 + 112)
        {
            List<Value> values = new List<Value>(new obje().Values);
            values.AddRange(new Value[]
			{
				new Data(16),
				new StringReferenceValue(),
				new StringReferenceValue(),
				new StringReferenceValue(),
				new StringReferenceValue(),
				new StringReferenceValue(),
				new StringReferenceValue(),
				new StringReferenceValue(),
				new StringReferenceValue(),
				new StringReferenceValue(),
				new StringReferenceValue(),
				new TagReference(),
				new TagReference(),
				new TagBlockArray(typeof(TagBlock0_0)),
				new TagReference(),
				new Data(8),
				new TagReference(),
				new TagReference(),
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
    }
}
