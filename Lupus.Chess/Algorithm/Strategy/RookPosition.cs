using System;
using System.Collections.Generic;
using System.Linq;
using Lupus.Chess.Interface;

namespace Lupus.Chess.Algorithm.Strategy
{
	public class RookPosition : AbstractStrategy
	{
		private const int Rank = 1;
		private const int Value = 1;

		public override int Compute(Field field, IEnumerable<IPiece> pieces)
		{
			var rooks = pieces.Where(p => p.Piece == PieceType.Rook).ToArray();
			return rooks.Select(r => r.AllowedPositions(field).Count() * Value).Sum() *
				   rooks.Select(r => r.Side == Side.White ? (r.Position.Rank - 1) * Rank : Math.Abs(r.Position.Rank - 8) * Rank)
				       .Sum();
		}
	}
}
