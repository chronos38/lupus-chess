using System.Collections.Generic;
using System.Linq;
using Lupus.Chess.Interface;

namespace Lupus.Chess.Algorithm.Evaluation
{
	public class PawnStructure : AbstractStrategy
	{
		private const int Value = 10;

		public override int Compute(Field field, ICollection<IPiece> pieces)
		{
			var result = 0;
			var pawns = pieces.Where(p => p.Piece == PieceType.Pawn);
			var enumerable = pawns as IPiece[] ?? pawns.ToArray();

			result += (from pawn in enumerable
				let left = field.GetPiece(Move.Left(pawn.Position))
				where
					left != null && left.Side == pawn.Side && pawn.Position.Rank != (pawn.Side == Side.White ? 2 : 7) &&
					left.Piece == PieceType.Pawn
				select Value).Sum();

			result += (from pawn in enumerable
				let right = field.GetPiece(Move.Right(pawn.Position))
				where
					right != null && right.Side == pawn.Side && pawn.Position.Rank != (pawn.Side == Side.White ? 2 : 7) &&
					right.Piece == PieceType.Pawn
				select Value).Sum();

			return result;
		}
	}
}
