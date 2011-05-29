using System.Collections.Generic;
using System;
using System.Text;
using System.IO;

namespace Sunfish.TagStructures
{
	public class mach : TagBlock
	{
		public mach() : base("mach", 308)
		{
            Values = InitializeValues(new devi().Values);
		}
	}
}
