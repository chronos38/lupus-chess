using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Lupus.Chess.Interface;

namespace Lupus.Chess.Piece
{
	[Serializable]
	public class King : AbstractPiece
	{
		public static King Black
		{
			get
			{
				return new King
				{
					Side = Side.Black,
					Piece = PieceType.King,
					Position = new Position
					{
						File = 'E',
						Rank = 8
					}
				};
			}
		}

		public static King White
		{
			get
			{
				return new King
				{
					Side = Side.White,
					Piece = PieceType.King,
					Position = new Position
					{
						File = 'E',
						Rank = 1
					}
				};
			}
		}

		internal King()
		{
			Piece = PieceType.King;
		}

		internal King(Side side, Position position)
		{
			Piece = PieceType.King;
			Side = side;
			Position = position;
		}

		public override void Move(Field field, Move move)
		{
			switch (move.CastlingSide)
			{
				case CastlingSide.King:
				case CastlingSide.Queen:
					Chess.Move.Castling(field, this, move.CastlingSide);
					break;
				default:
					base.Move(field, move);
					break;
			}
		}

		public override object Clone()
		{
			return new King
			{
				Piece = PieceType.King,
				Position = (Position) Position.Clone(),
				Side = Side
			};
		}

		public override IEnumerable<Move> AllowedMoves(Field field)
		{
			var result = new List<Move>();
			var positions = AllowedPositions(field);
			AddCastling(result, CanUseCastling(field));
			result.AddRange(positions.Select(p => new Move {From = Position, To = p, Side = Side, Piece = Piece}).ToArray());
			return result;
		}

		public override IEnumerable<Position> AllowedPositions(Field field)
		{
			var result = new Collection<Position>();
			var position = Chess.Move.Direction(Position, Direction.Down);
			if (position.Validate() && field.IsFree(position, Side)) result.Add(position);
			position = Chess.Move.Direction(Position, Direction.Left);
			if (position.Validate() && field.IsFree(position, Side)) result.Add(position);
			position = Chess.Move.Direction(Position, Direction.LowerLeft);
			if (position.Validate() && field.IsFree(position, Side)) result.Add(position);
			position = Chess.Move.Direction(Position, Direction.LowerRight);
			if (position.Validate() && field.IsFree(position, Side)) result.Add(position);
			position = Chess.Move.Direction(Position, Direction.Right);
			if (position.Validate() && field.IsFree(position, Side)) result.Add(position);
			position = Chess.Move.Direction(Position, Direction.Up);
			if (position.Validate() && field.IsFree(position, Side)) result.Add(position);
			position = Chess.Move.Direction(Position, Direction.UpperLeft);
			if (position.Validate() && field.IsFree(position, Side)) result.Add(position);
			position = Chess.Move.Direction(Position, Direction.UpperRight);
			if (position.Validate() && field.IsFree(position, Side)) result.Add(position);
			return result;
		}

		public CastlingSide CanUseCastling(Field field)
		{
			var pieces = field[Side];
			// Check if king have moved
			if (PastMoves.Any(m => m.Piece == PieceType.King && m.Side == Side)) return CastlingSide.None;
			// If king is in check, castling is not allowed
			if (IsInCheck(field)) return CastlingSide.None;
			var rank = Side == Side.White ? 1 : 8;
			var result = CastlingSide.Both;
			var queen = new Position {File = 'A', Rank = rank};
			var king = new Position {File = 'H', Rank = rank};
			// Check if queen side rook has moved
			if (
				PastMoves.Any(
					p => p.Side == Side && p.Piece == PieceType.Rook && p.From == queen))
			{
				result = CastlingSide.King;
				queen = null;
			}
			// Check if king side rook has moved
			if (
				PastMoves.Any(
					p => p.Side == Side && p.Piece == PieceType.Rook && p.From == king))
			{
				if (result == CastlingSide.King) return CastlingSide.None;
				result = CastlingSide.Queen;
				king = null;
			}
			// Select unmoved rooks
			var rooks = (from p in pieces where p.Piece == PieceType.Rook && (p.Position == queen || p.Position == king) select p);
			// If there are no rooks available than castling is disallowed
			if (!rooks.Any()) return CastlingSide.None;
			// Check if all fields between rook and king are free and not under attack
			var underAttack = field.UnderAttack(Chess.Move.InvertSide(Side));
			foreach (var position in rooks.Select(rook => rook.Position))
			{
				var pos = position;

				if (position.File == 'A')
				{
					for (var i = 0; i < 3; i++)
					{
						pos = Chess.Move.Right(pos);
						if (field.IsFree(pos) == Side.None && !underAttack.Contains(pos)) continue;
						if (result == CastlingSide.Both) result = rooks.Count() == 1 ? CastlingSide.Queen : CastlingSide.King;
						if (result == CastlingSide.Queen) return CastlingSide.None;
						if (result == CastlingSide.King) break;
					}

					continue;
				}
				if (position.File != 'H') continue;
				for (var i = 0; i < 2; i++)
				{
					pos = Chess.Move.Left(pos);
					if (field.IsFree(pos) == Side.None && !underAttack.Contains(pos)) continue;
					if (result == CastlingSide.Both) result = rooks.Count() == 1 ? CastlingSide.King : CastlingSide.Queen;
					if (result == CastlingSide.King) return CastlingSide.None;
					if (result == CastlingSide.Queen) break;
				}
			}

			return result;
		}

		public static void AddCastling(ICollection<Move> moves, CastlingSide castling)
		{
			switch (castling)
			{
				case CastlingSide.Both:
					moves.Add(new Move { CastlingSide = CastlingSide.King });
					moves.Add(new Move { CastlingSide = CastlingSide.Queen });
					break;

				case CastlingSide.King:
					moves.Add(new Move { CastlingSide = CastlingSide.King });
					break;

				case CastlingSide.Queen:
					moves.Add(new Move { CastlingSide = CastlingSide.Queen });
					break;
			}
		}

		public static IEnumerable<IPiece> StartPieces()
		{
			return new Collection<IPiece>
			{
				White,
				Black
			};
		}

		public bool IsInCheck(Field field)
		{
			return IsInCheck(field, Position, Chess.Move.InvertSide(Side));
		}

		public static bool IsInCheck(Field field, Position kingPosition, Side attackingSide)
		{
			return field.UnderAttack(attackingSide).Contains(kingPosition);
		}
	}
}
