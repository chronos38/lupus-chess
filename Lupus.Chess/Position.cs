using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lupus.Chess
{
	[Serializable]
	public class Position
	{
		public char File { get; set; }
		public int Rank { get; set; }

		public bool Validate()
		{
			if (File.ToString(CultureInfo.InvariantCulture).IndexOfAny(new[] {'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H'}) == -1)
				return false;
			return Rank >= 1 && Rank <= 8;
		}
	}
}
