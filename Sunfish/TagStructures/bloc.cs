using System.Collections.Generic;
using System;
using System.Text;
using System.IO;

namespace Sunfish.TagStructures
{
	public class bloc : TagBlock
	{
		public bloc() : base("bloc", 188 + 4)
		{
            Values = InitializeValues(new obje().Values);
		}
	}
}
