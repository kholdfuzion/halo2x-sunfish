using System.Collections.Generic;
using System;
using System.Text;
using System.IO;

namespace Sunfish.TagStructures
{
	public class spk : TagBlock
	{
		public spk() : base("spk!", 40)
		{
			Values = InitializeValues(new Value[]
			{
			});
		}
	}
}
