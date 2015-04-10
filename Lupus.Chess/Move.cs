using System;
using Lupus.Chess.Exception;
using Lupus.Chess.Piece;

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
			return new Move
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

		public static Position Direction(Position position, Direction direction)
		{
			switch (direction)
			{
				case Chess.Direction.LowerLeft:
					return LowerLeft(position);
				case Chess.Direction.Down:
					return Down(position);
				case Chess.Direction.LowerRight:
					return LowerRight(position);
				case Chess.Direction.Left:
					return Left(position);
				case Chess.Direction.Right:
					return Right(position);
				case Chess.Direction.UpperLeft:
					return UpperLeft(position);
				case Chess.Direction.Up:
					return Up(position);
				case Chess.Direction.UpperRight:
					return UpperRight(position);
				default:
					return null;
			}
		}

		public static void Castling(Field field, King king, Rook rook, CastlingSide side)
		{
			var allowedSide = king.CanUseCastling(field);
			if (king.Side != rook.Side && !(allowedSide == CastlingSide.Both || allowedSide == side))
				throw new ChessCastlingException(king.Side, "King cannot use castling.");

			switch (side)
			{
				case CastlingSide.King:
					switch (king.Side)
					{
						case Side.White:
							rook.Move(field, new Position {File = 'F', Rank = 1});
							king.Move(field, new Position {File = 'G', Rank = 1});
							break;
						case Side.Black:
							rook.Move(field, new Position {File = 'F', Rank = 8});
							king.Move(field, new Position {File = 'G', Rank = 8});
							break;
					}
					break;
				case CastlingSide.Queen:
					switch (king.Side)
					{
						case Side.White:
							rook.Move(field, new Position {File = 'D', Rank = 1});
							king.Move(field, new Position {File = 'C', Rank = 1});
							break;
						case Side.Black:
							rook.Move(field, new Position {File = 'D', Rank = 8});
							king.Move(field, new Position {File = 'C', Rank = 8});
							break;
					}
					break;
				default:
					throw new ChessCastlingException(king.Side,
						"Got invalid castling side. Valid values are CastlingSide.King and CastlingSide.Queen.");
			}
		}
	}
}
