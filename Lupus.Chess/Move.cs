using System;
using System.Linq;
using Lupus.Chess.Exception;
using Lupus.Chess.Interface;
using Lupus.Chess.Piece;

namespace Lupus.Chess
{
	[Serializable]
	public class Move : ICloneable
	{
		private PieceType _piece = PieceType.Abstract;
		private Side _side = Side.None;
		private CastlingSide _castlingSide = CastlingSide.None;

		public Position From { get; set; }
		public Position To { get; set; }

		public PieceType Piece
		{
			get { return _piece; }
			set { _piece = value; }
		}

		public Side Side
		{
			get { return _side; }
			set { _side = value; }
		}

		public CastlingSide CastlingSide
		{
			get { return _castlingSide; }
			set { _castlingSide = value; }
		}

		public IPiece Captured { get; set; }

		public object Clone()
		{
			return new Move
			{
				From = From != null ? (Position) From.Clone() : null,
				To = To != null ? (Position) To.Clone() : null,
				Piece = Piece,
				Side = Side,
				CastlingSide = CastlingSide
			};
		}

		protected bool Equals(Move other)
		{
			return (Equals(From, other.From) && Equals(To, other.To) && Piece == other.Piece && Side == other.Side &&
			        CastlingSide == other.CastlingSide);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			return obj.GetType() == GetType() && Equals((Move) obj);
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
			return Equals(lhs, rhs);
		}

		public static bool operator !=(Move lhs, Move rhs)
		{
			return !(lhs == rhs);
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

		public static void Castling(Field field, King king, CastlingSide side)
		{
			var pieces = field[king.Side];
			var rank = king.Side == Side.White ? 1 : 8;
			var position = side == CastlingSide.King
				? new Position {File = 'H', Rank = rank}
				: new Position {File = 'A', Rank = rank};
			var rook = (Rook) (from p in pieces where p.Position == position select p).First();
			var allowedSide = king.CanUseCastling(field);
			if (allowedSide != CastlingSide.Both && allowedSide != side)
				throw new ChessCastlingException(king.Side, "King cannot use castling.");

			switch (side)
			{
				case CastlingSide.King:
					switch (king.Side)
					{
						case Side.White:
							rook.Position = new Position {File = 'F', Rank = 1};
							king.Position = new Position {File = 'G', Rank = 1};
							break;
						case Side.Black:
							rook.Position = new Position {File = 'F', Rank = 8};
							king.Position = new Position {File = 'G', Rank = 8};
							break;
					}
					break;
				case CastlingSide.Queen:
					switch (king.Side)
					{
						case Side.White:
							rook.Position = new Position {File = 'D', Rank = 1};
							king.Position = new Position {File = 'C', Rank = 1};
							break;
						case Side.Black:
							rook.Position = new Position {File = 'D', Rank = 8};
							king.Position = new Position {File = 'C', Rank = 8};
							break;
					}
					break;
				default:
					throw new ChessCastlingException(king.Side,
						"Got invalid castling side. Valid values are CastlingSide.King and CastlingSide.Queen.");
			}
		}

		public static void UndoCastling(Field field, King king, CastlingSide side)
		{
			switch (side)
			{
				case CastlingSide.King:
					switch (king.Side)
					{
						case Side.White:
							field[new Position {File = 'F', Rank = 1}].Position = new Position {File = 'H', Rank = 1};
							field[new Position {File = 'G', Rank = 1}].Position = new Position {File = 'E', Rank = 1};
							break;
						case Side.Black:
							field[new Position {File = 'F', Rank = 1}].Position = new Position {File = 'H', Rank = 8};
							field[new Position {File = 'G', Rank = 1}].Position = new Position {File = 'E', Rank = 8};
							break;
					}
					break;
				case CastlingSide.Queen:
					switch (king.Side)
					{
						case Side.White:
							field[new Position {File = 'D', Rank = 1}].Position = new Position {File = 'A', Rank = 1};
							field[new Position {File = 'C', Rank = 1}].Position = new Position {File = 'E', Rank = 1};
							break;
						case Side.Black:
							field[new Position {File = 'D', Rank = 1}].Position = new Position {File = 'A', Rank = 8};
							field[new Position {File = 'C', Rank = 1}].Position = new Position {File = 'E', Rank = 8};
							break;
					}
					break;
			}
		}

		public static bool TryCastling(Field field, King king, CastlingSide side)
		{
			try
			{
				Castling(field, king, side);
				return true;
			}
			catch (System.Exception)
			{
				return false;
			}
		}

		public static Side InvertSide(Side side)
		{
			switch (side)
			{
				case Side.Black: return Side.White;
				case Side.White: return Side.Black;
				default: throw new ArgumentException("Side should be either Side.White or Wide.Black");
			}
		}

		public static void Do(Field field, Move move)
		{
			switch (move.CastlingSide)
			{
				case CastlingSide.King:
				case CastlingSide.Queen:
					var king = (King) field[move.Side].First(p => p.Piece == PieceType.King);
					Castling(field, king, move.CastlingSide);
					break;
				default:
					var piece = field[move.From];
					piece.Move(field, move);
					break;
			}
		}

		public static void Undo(Field field, Move move)
		{
			switch (move.CastlingSide)
			{
				case CastlingSide.King:
				case CastlingSide.Queen:
					UndoCastling(field, (King) field[move.Side].First(p => p.Piece == PieceType.King), move.CastlingSide);
					break;
				default:
					if (move.Captured != null) field.Add(move.Captured);
					field[move.To].Position = move.From;
					break;
			}
		}
	}
}
