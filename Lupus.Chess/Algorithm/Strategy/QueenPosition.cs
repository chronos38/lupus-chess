using System.Collections.Generic;
using System.Linq;
using Lupus.Chess.Interface;

namespace Lupus.Chess.Algorithm.Strategy
{
	public class QueenPosition : AbstractStrategy
	{
		private const int Value = 1;

		public override int Compute(Field field, ICollection<IPiece> pieces)
		{
			return pieces.Where(p => p.Piece == PieceType.Queen).Select(p => p.AllowedPositions(field).Count*Value).Sum();
		}
	}
}
