using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Runtime.Remoting.Messaging;
using Lupus.Chess.Interface;
using Lupus.Chess.Piece;

namespace Lupus.Chess
{
	[Serializable]
	public class Field : ICloneable
	{
		public override int GetHashCode()
		{
			unchecked
			{
				return ((WhitePieces != null ? WhitePieces.GetHashCode() : 0)*397) ^ (BlackPieces != null ? BlackPieces.GetHashCode() : 0);
			}
		}

		public IList<Move> History { get; set; } 
		public ICollection<IPiece> WhitePieces { get; set; }
		public ICollection<IPiece> BlackPieces { get; set; }

		public ICollection<IPiece> this[Side side]
		{
			get
			{
				switch (side)
				{
					case Side.White:
						return WhitePieces;
					case Side.Black:
						return BlackPieces;
					default:
						throw new ArgumentException("Side should be either Side.White or Wide.Black");
				}
			}
		}

		public IPiece this[Position position]
		{
			get { return WhitePieces.Concat(BlackPieces).FirstOrDefault(p => p.Position == position); }
		}

		public object Clone()
		{
			return new Field()
			{
				WhitePieces = (from piece in WhitePieces select (IPiece) piece.Clone()).ToList(),
				BlackPieces = (from piece in BlackPieces select (IPiece) piece.Clone()).ToList()
			};
		}

		/// <summary>
		/// Checks if the position is free from any piece.
		/// </summary>
		/// <param name="position">Position to check for.</param>
		/// <returns>Side which is occupying this position or undefined if it is free.</returns>
		public Side IsFree(Position position)
		{
			var piece = (from p in WhitePieces.Concat(BlackPieces) where p.Position == position select p).FirstOrDefault();
			return piece == null ? Side.None : piece.Side;
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
					return BlackPieces.All(piece => piece.Position != position);
				case Side.White:
					return WhitePieces.All(piece => piece.Position != position);
				case Side.Both:
					return WhitePieces.Concat(BlackPieces).All(piece => piece.Position != position);
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
					return
						(from blackPiece in BlackPieces from positions in blackPiece.AllowedPositions(this) select positions).ToList();
				case Side.White:
					return
						(from whitePiece in WhitePieces from positions in whitePiece.AllowedPositions(this) select positions).ToList();
				default:
					return new Position[] {};
			}
		}

		public IPiece GetPiece(Position position)
		{
			return WhitePieces.Concat(BlackPieces).FirstOrDefault(p => p.Position == position);
		}

		public void Remove(Position position)
		{
			var piece = WhitePieces.Concat(BlackPieces).FirstOrDefault(p => p.Position == position);
			if (piece == null) return;

			switch (piece.Side)
			{
				case Side.White:
					WhitePieces.Remove(piece);
					break;
				case Side.Black:
					BlackPieces.Remove(piece);
					break;
			}
		}

		protected bool Equals(Field other)
		{
			if (Equals(other, null)) return false;
			if (WhitePieces.Count != other.WhitePieces.Count) return false;
			if (BlackPieces.Count != other.BlackPieces.Count) return false;
			var white = WhitePieces.Intersect(other.WhitePieces, new LambdaComparer<IPiece>(AbstractPiece.Equals)).ToList();
			var black = BlackPieces.Intersect(other.BlackPieces, new LambdaComparer<IPiece>(AbstractPiece.Equals)).ToList();
			return white.Count == WhitePieces.Count && black.Count == other.BlackPieces.Count;
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

		public static Field Create()
		{
			return new Field
			{
				BlackPieces = new Collection<IPiece>(),
				WhitePieces = new Collection<IPiece>(),
				History = new List<Move>()
			};
		}

		public static Field Start()
		{
			var result = new Field();
			var pieces = new List<IPiece>();
			pieces.AddRange(Pawn.StartPieces());
			pieces.AddRange(Knight.StartPieces());
			pieces.AddRange(Bishop.StartPieces());
			pieces.AddRange(Rook.StartPieces());
			pieces.AddRange(Queen.StartPieces());
			pieces.AddRange(King.StartPieces());
			result.WhitePieces = (from piece in pieces where piece.Side == Side.White select piece).ToList();
			result.BlackPieces = (from piece in pieces where piece.Side == Side.Black select piece).ToList();
			result.History = new List<Move>();
			return result;
		}
	}
}
