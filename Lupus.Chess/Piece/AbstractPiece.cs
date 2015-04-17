using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Lupus.Chess.Interface;

namespace Lupus.Chess.Piece
{
	[Serializable]
	public abstract class AbstractPiece : IPiece
	{
		private Side _side = Side.None;
		private PieceType _piece = PieceType.Abstract;
		private Position _position = new Position {File = 'A', Rank = 1};

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

		public virtual void Move(Field field, Position next)
		{
			var previous = Position;
			field.Remove(next);
			Position = next;
			field.History.Add(new Move
			{
				From = (Position) previous.Clone(),
				To = (Position) next.Clone(),
				Side = Side,
				Piece = Piece
			});
		}

		public bool TryMove(Field field, Position position)
		{
			if (!ValidateMove(field, position)) return false;
			Move(field, position);
			return true;
		}

		public bool ValidateMove(Field field, Position position)
		{
			return AllowedPositions(field).Contains(position);
		}

		public abstract object Clone();

		public abstract ICollection<Position> AllowedPositions(Field field);

		public virtual ICollection<Move> AllowedMoves(Field field)
		{
			return AllowedPositions(field).Select(p => new Move {From = Position, To = p, Side = Side, Piece = Piece}).ToList();
		}

		public static bool Equals(IPiece lhs, IPiece rhs)
		{
			return lhs.Position == rhs.Position
			       && lhs.Side == rhs.Side
			       && lhs.Piece == rhs.Piece;
		}

		protected static ICollection<Position> FindPositions(Field field, Side side, Position position, Direction direction)
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
