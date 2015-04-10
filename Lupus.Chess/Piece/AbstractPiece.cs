using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Lupus.Chess.Exception;
using Lupus.Chess.Interface;

namespace Lupus.Chess.Piece
{
	[Serializable]
	public abstract class AbstractPiece : IPiece
	{
		private Side _side = Side.None;
		private PieceType _piece = PieceType.Abstract;
		private Position _position = new Position {File = 'A', Rank = 1};
		private bool _moved;

		public Side Side
		{
			get { return _side; }
			internal set { _side = value; }
		}

		public PieceType Piece
		{
			get { return _piece; }
			internal set { _piece = value; }
		}

		public Position Position
		{
			get { return _position; }
			internal set { _position = value; }
		}

		public bool Moved
		{
			get { return _moved; }
			internal set { _moved = value; }
		}

		public void Move(Field field, Position position)
		{
			if (!position.Validate())
				throw new ChessMoveException(new Move
				{
					From = Position,
					To = position,
					Piece = Piece,
					Side = Side
				});
			Position = position;
		}

		public bool TryMove(Field field, Position position)
		{
			if (!ValidateMove(field, position)) return false;
			Position = position;
			Moved = true;
			return true;
		}

		public bool ValidateMove(Field field, Position position)
		{
			return AllowedPositions(field).Contains(position);
		}

		public abstract object Clone();

		public abstract IEnumerable<Position> AllowedPositions(Field field);

		protected static IEnumerable<Position> FindPositions(Field field, Side side, Position position, Direction direction)
		{
			var pos = position;
			var result = new Collection<Position>();

			while (true)
			{
				pos = Chess.Move.Direction(pos, direction);
				var free = field.IsFree(pos);

				if (!pos.Validate()) break;
				if (free == Side.None) result.Add(pos);
				else
				{
					if (free != side) result.Add(pos);
					break;
				}
			}

			return result;
		}
	}
}
