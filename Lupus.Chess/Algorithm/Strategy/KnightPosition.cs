using System;
using System.Collections.Generic;
using System.Linq;
using Lupus.Chess.Interface;

namespace Lupus.Chess.Algorithm.Strategy
{
	public class KnightPosition : AbstractStrategy
	{
		private const float Value = 2;

		public override int Compute(Field field, ICollection<IPiece> pieces)
		{
			return
				(int)
					Math.Round(pieces.Where(p => p.Piece == PieceType.Knight).Select(p => p.AllowedPositions(field).Count()*Value).Sum());
		}
	}
}
