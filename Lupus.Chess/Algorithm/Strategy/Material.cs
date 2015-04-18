using System.Collections.Generic;
using System.Linq;
using Lupus.Chess.Interface;

namespace Lupus.Chess.Algorithm.Strategy
{
	public class Material : AbstractStrategy
	{
		private static readonly IDictionary<PieceType, int> LookupTable = new Dictionary<PieceType, int>
		{
			{ PieceType.King, 20000 },
			{ PieceType.Queen, 1000 },
			{ PieceType.Bishop, 350 },
			{ PieceType.Knight, 350 },
			{ PieceType.Rook, 525 },
			{ PieceType.Pawn, 100 }
		};

		public override int Compute(Field field, ICollection<IPiece> pieces)
		{
			var bishop = pieces.Count(p => p.Piece == PieceType.Bishop) >= 2 ? 50 : 0;
			return bishop + (from piece in pieces select LookupTable[piece.Piece]).Sum();
		}
	}
}
