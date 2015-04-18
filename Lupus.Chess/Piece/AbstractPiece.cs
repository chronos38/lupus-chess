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

		public virtual void Move(Field field, Move move)
		{
			if (Side != move.Side || Piece != move.Piece || Position != move.From) throw new ChessMoveException(move);
			field.Remove(move.To);
			Position = move.To;
			field.History.Add(move);
		}

		public bool TryMove(Field field, Move move)
		{
			try
			{
				if (!AllowedPositions(field).Contains(move.To)) return false;
				Move(field, move);
				return true;
			}
			catch (System.Exception)
			{
				return false;
			}
		}

		public bool ValidateMove(Field field, Move move)
		{
			return AllowedMoves(field).Contains(move);
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
