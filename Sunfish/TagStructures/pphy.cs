using System.Collections.Generic;
using System;
using System.Text;
using System.IO;

namespace Sunfish.TagStructures
{
	public class pphy : TagBlock
	{
		public pphy() : base("pphy", 64)
		{
			Values = InitializeValues(new Value[]
			{
			});
		}
	}
}
