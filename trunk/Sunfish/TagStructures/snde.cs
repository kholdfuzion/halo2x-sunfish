using System.Collections.Generic;
using System;
using System.Text;
using System.IO;

namespace Sunfish.TagStructures
{
	public class snde : TagBlock
	{
		public snde() : base("snde", 72)
		{
			Values = InitializeValues(new Value[]
			{
			});
		}
	}
}
