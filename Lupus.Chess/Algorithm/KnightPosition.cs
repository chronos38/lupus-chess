using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lupus.Chess.Interface.Algorithm;

namespace Lupus.Chess.Algorithm
{
	public class KnightPosition : IStrategy
	{
		private const int Value = 20;

		public int Execute(Field field, Side side)
		{
			var pieces = side == Side.White ? field.WhitePieces : field.BlackPieces;
			return pieces.Where(p => p.Piece == PieceType.Knight).Select(p => p.AllowedPositions(field).Count*Value).Sum();
		}
	}
}
