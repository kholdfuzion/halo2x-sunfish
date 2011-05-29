using System.Collections.Generic;
using System;
using System.Text;
using System.IO;

namespace Sunfish.TagStructures
{
	public class deca : TagBlock
	{
		public deca() : base("deca", 172)
		{
			Values = InitializeValues(new Value[]
			{
				new Data(8),
				new TagReference(),
				new Data(120),
				new TagReference(),
				new Data(20),
			});
		}
	}
}
