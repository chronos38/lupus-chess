using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Runtime.Remoting.Messaging;
using Lupus.Chess.Exception;
using Lupus.Chess.Interface;
using Lupus.Chess.Piece;

namespace Lupus.Chess
{
	[Serializable]
	public class Field : List<IPiece>, ICloneable
	{
		public override int GetHashCode()
		{
			unchecked
			{
				return this.Aggregate(19, (current, p) => current*31 + p.GetHashCode());
			}
		}

		public IEnumerable<IPiece> this[Side side]
		{
			get
			{
				switch (side)
				{
					case Side.White:
						return this.Where(p => p.Side == Side.White);
					case Side.Black:
						return this.Where(p => p.Side == Side.Black);
					case Side.Both:
						return this;
					default:
						return new List<IPiece>();
				}
			}
		}

		public IPiece this[Position position]
		{
			get { return this.FirstOrDefault(p => p.Position == position); }
		}

		public object Clone()
		{
			var clone = new Field();
			clone.AddRange(this.Select(p => (IPiece) p.Clone()));
			return clone;
		}

		/// <summary>
		/// Checks if the position is free from any piece.
		/// </summary>
		/// <param name="position">Position to check for.</param>
		/// <returns>Side which is occupying this position or undefined if it is free.</returns>
		public Side IsFree(Position position)
		{
			var p = this[position];
			return p == null ? Side.None : p.Side;
		}

		/// <summary>
		/// Checks if the position is occupied by a piece from the given side.
		/// </summary>
		/// <param name="position">Position to check for.</param>
		/// <param name="side">The side which pieces shoudl be taken into account.</param>
		/// <returns>true if on peice of given side stands on given position, otherwise false.</returns>
		public bool IsFree(Position position, Side side)
		{
			switch (side)
			{
				case Side.Black:
					return this[Side.Black].All(p => p.Position != position);
				case Side.White:
					return this[Side.White].All(p => p.Position != position);
				case Side.Both:
					return this.All(p => p.Position != position);
				default:
					return true;
			}
		}

		/// <summary>
		/// Gets all fields that are under attack from the given side.
		/// </summary>
		/// <param name="fromSide">Side to search for.</param>
		/// <returns>All positions that are under attack from given side.</returns>
		public IEnumerable<Position> UnderAttack(Side fromSide)
		{
			switch (fromSide)
			{
				case Side.Black:
					return (from p in this[Side.Black] from pos in p.AllowedPositions(this) select pos);
				case Side.White:
					return (from p in this[Side.White] from pos in p.AllowedPositions(this) select pos);
				default:
					return new Position[] {};
			}
		}

		public IPiece GetPiece(Position position)
		{
			return this[position];
		}

		public void Remove(Position position)
		{
			Remove(this[position]);
		}

		public void ExecuteMove(Move move)
		{
			var piece = this[move.Side].FirstOrDefault(p => p.Piece == move.Piece && p.Position == move.From);
			if (piece != null) piece.Move(this, move);
			throw new ChessMoveException(move, "Piece does not exist.");
		}

		protected bool Equals(Field other)
		{
			if (Equals(other, null)) return false;
			if (Count != other.Count) return false;
			return Count == this.Intersect(other, new LambdaComparer<IPiece>(AbstractPiece.Equals)).Count();
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType()) return false;
			return Equals((Field)obj);
		}

		public static bool operator ==(Field lhs, Field rhs)
		{
			return Equals(lhs, rhs);
		}

		public static bool operator !=(Field lhs, Field rhs)
		{
			return !(lhs == rhs);
		}

		public static Field Start()
		{
			var result = new Field();
			result.AddRange(Pawn.StartPieces());
			result.AddRange(Knight.StartPieces());
			result.AddRange(Bishop.StartPieces());
			result.AddRange(Rook.StartPieces());
			result.AddRange(Queen.StartPieces());
			result.AddRange(King.StartPieces());
			return result;
		}
	}
}
