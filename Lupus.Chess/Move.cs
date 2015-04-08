using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lupus.Chess.Interface;

namespace Lupus.Chess
{
	[Serializable]
	public class Move
	{
		public Position From { get; set; }
		public Position To { get; set; }
		public PieceType Piece { get; set; }
		public Side Side { get; set; }
	}
}
