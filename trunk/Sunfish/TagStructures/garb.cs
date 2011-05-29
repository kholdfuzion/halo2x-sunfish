using System.Collections.Generic;
using System;
using System.Text;
using System.IO;

namespace Sunfish.TagStructures
{
	public class garb : TagBlock
	{
		public garb() : base("garb", 468)
		{
			Values = InitializeValues(new item().Values);
		}
	}
}
