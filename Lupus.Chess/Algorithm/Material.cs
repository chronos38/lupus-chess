using System.Collections.Generic;
using System.Linq;
using Lupus.Chess.Interface;
using Lupus.Chess.Interface.Algorithm;

namespace Lupus.Chess.Algorithm
{
	public class Material : IStrategy
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

		public int Execute(Field field, Side side)
		{
			switch (side)
			{
				case Side.Black:
					return Compute(field.BlackPieces) - Compute(field.WhitePieces);
				case Side.White:
					return Compute(field.WhitePieces) - Compute(field.BlackPieces);
			}

			return 0;
		}

		private static int Compute(ICollection<IPiece> pieces)
		{
			var bishop = pieces.Count(p => p.Piece == PieceType.Bishop) >= 2 ? 50 : 0;
			return bishop + (from piece in pieces select LookupTable[piece.Piece]).Sum();
		}
	}
}
