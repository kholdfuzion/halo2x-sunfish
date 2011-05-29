using System.Collections.Generic;
using System;
using System.Text;
using System.IO;

namespace Sunfish.TagStructures
{
	public class snd : TagBlock
	{
		public snd() : base("snd!", 20)
		{
			Values = InitializeValues(new Value[]
			{
			});
		}
	}
}
