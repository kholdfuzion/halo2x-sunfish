using System.Collections.Generic;
using System;
using System.Text;
using System.IO;

namespace Sunfish.TagStructures
{
	public class ssce : TagBlock
	{
		public ssce() : base("ssce", 204)
		{
            Values = InitializeValues(new obje().Values);
		}
	}
}
