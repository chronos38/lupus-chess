using System;
using System.Collections.Generic;
using Lupus.Chess.Exception;
using Lupus.Chess.Interface;

namespace Lupus.Chess.Piece
{
	[Serializable]
	public abstract class AbstractPiece : IPiece
	{
		public Side Side { get; protected set; }
		public PieceType Piece { get; protected set; }
		public Position Position { get; protected set; }

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
