using System.Collections.Generic;
using System;
using System.Text;
using System.IO;

namespace Sunfish.TagStructures
{
	public class snmx : TagBlock
	{
		public snmx() : base("snmx", 88)
		{
			Values = InitializeValues(new Value[]
			{
			});
		}
	}
}
