using System.Collections.Generic;
using Lupus.Chess.Exception;
using Lupus.Chess.Interface;

namespace Lupus.Chess.Piece
{
	public abstract class AbstractPiece : IPiece
	{
		public Side Side { get; private set; }
		public PieceType Piece { get; private set; }
		public Position Position { get; private set; }

		public void Move(Position position)
		{
			if (!TryMove(position))
				throw new ChessMoveException(new Move()
				{
					From = Position,
					To = position,
					Piece = Piece,
					Side = Side
				});
		}

		public bool TryMove(Position position)
		{
			if (!ValidateMove(position)) return false;
			Position = position;
			return true;
		}

		public abstract ICollection<Position> AllowedPositions();

		protected abstract bool ValidateMove(Position position);
	}
}
