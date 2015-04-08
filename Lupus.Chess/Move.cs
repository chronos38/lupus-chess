using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lupus.Chess.Interface;

namespace Lupus.Chess
{
	[Serializable]
	public class Move : ICloneable
	{
		public Position From { get; set; }
		public Position To { get; set; }
		public PieceType Piece { get; set; }
		public Side Side { get; set; }

		public object Clone()
		{
			return new Move()
			{
				From = (Position) From.Clone(),
				To = (Position) To.Clone(),
				Piece = Piece,
				Side = Side
			};
		}

		protected bool Equals(Move other)
		{
			return Equals(From, other.From) && Equals(To, other.To) && Piece == other.Piece && Side == other.Side;
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != GetType()) return false;
			return Equals((Move)obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int hashCode = (From != null ? From.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (To != null ? To.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (int)Piece;
				hashCode = (hashCode * 397) ^ (int)Side;
				return hashCode;
			}
		}

		public static bool operator ==(Move lhs, Move rhs)
		{
			return ReferenceEquals(null, lhs) ? ReferenceEquals(null, rhs) : lhs.Equals(rhs);
		}

		public static bool operator !=(Move lhs, Move rhs)
		{
			return ReferenceEquals(null, lhs) ? !ReferenceEquals(null, rhs) : !lhs.Equals(rhs);
		}

		public static Position UpperLeft(Position position)
		{
			return new Position()
			{
				File = (char)(position.File - 1),
				Rank = position.Rank + 1
			};
		}

		public static Position UpperRight(Position position)
		{
			return new Position()
			{
				File = (char)(position.File + 1),
				Rank = position.Rank + 1
			};
		}

		public static Position LowerLeft(Position position)
		{
			return new Position()
			{
				File = (char)(position.File - 1),
				Rank = position.Rank - 1
			};
		}

		public static Position LowerRight(Position position)
		{
			return new Position()
			{
				File = (char)(position.File + 1),
				Rank = position.Rank - 1
			};
		}

		public static Position Left(Position position)
		{
			return new Position()
			{
				File = (char)(position.File - 1),
				Rank = position.Rank
			};
		}

		public static Position Right(Position position)
		{
			return new Position()
			{
				File = (char)(position.File + 1),
				Rank = position.Rank
			};
		}

		public static Position Up(Position position)
		{
			return new Position()
			{
				File = position.File,
				Rank = position.Rank + 1
			};
		}

		public static Position Down(Position position)
		{
			return new Position()
			{
				File = position.File,
				Rank = position.Rank - 1
			};
		}

		public static Position Direction(Position position, int direction)
		{
			switch (direction)
			{
				case 1:
					return LowerLeft(position);
				case 2:
					return Down(position);
				case 3:
					return LowerRight(position);
				case 4:
					return Left(position);
				case 6:
					return Right(position);
				case 7:
					return UpperLeft(position);
				case 8:
					return Up(position);
				case 9:
					return UpperRight(position);
				default:
					return null;
			}
		}
	}
}
