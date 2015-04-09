using System;
using System.Collections.Generic;
using System.Linq;
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

		public void Move(Field field, Position position)
		{
			if (!TryMove(field, position))
				throw new ChessMoveException(new Move()
				{
					From = Position,
					To = position,
					Piece = Piece,
					Side = Side
				});
		}

		public bool TryMove(Field field, Position position)
		{
			if (!ValidateMove(field, position)) return false;
			Position = position;
			return true;
		}

		public bool ValidateMove(Field field, Position position)
		{
			return AllowedPositions(field).Contains(position);
		}

		public abstract object Clone();

		public abstract IEnumerable<Position> AllowedPositions(Field field);

		public abstract IEnumerable<IPiece> StartPieces();
	}
}
