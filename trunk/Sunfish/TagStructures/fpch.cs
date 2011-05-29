using System.Collections.Generic;
using System;
using System.Text;
using System.IO;

namespace Sunfish.TagStructures
{
	public class fpch : TagBlock
	{
		public fpch() : base("fpch", 80)
		{
			Values = InitializeValues(new Value[]
			{
				new Data(20),
				new TagReference(),
			});
		}
	}
}
