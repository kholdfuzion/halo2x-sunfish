using System.Collections.Generic;
using System;
using System.Text;
using System.IO;

namespace Sunfish.TagStructures
{
	public class scen : TagBlock
	{
		public scen() : base("scen", 188 + 8)
		{
			Values = InitializeValues(new obje().Values);
		}
	}
}
