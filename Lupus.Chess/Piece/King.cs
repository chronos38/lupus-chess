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
					Moved = false,
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
					Moved = false,
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

		internal King(Side side, Position position, bool moved)
		{
			Moved = moved;
			Piece = PieceType.King;
			Side = side;
			Position = position;
		}

		public override object Clone()
		{
			return new King
			{
				Moved = Moved,
				Piece = PieceType.King,
				Position = (Position) Position.Clone(),
				Side = Side
			};
		}

		public override ICollection<Position> AllowedPositions(Field field)
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
			// Check if king have moved
			if (Moved) return CastlingSide.None;
			// Check if king is in check
			var underAttack = field.UnderAttack(Side == Side.White ? Side.Black : Side.White).ToList();
			if (underAttack.Contains(Position)) return CastlingSide.None;
			// Search for unmoved rooks
			var pieces = Side == Side.White ? field.WhitePieces : field.BlackPieces;
			var rooks = (from piece in pieces where piece.Piece == PieceType.Rook && !((Rook) piece).Moved select piece).ToList();
			// If there are no rooks available than castling is disallowed
			if (!rooks.Any()) return CastlingSide.None;
			// Check if all fields between rook and king are free and not under attack
			var result = CastlingSide.Both;
			foreach (var position in rooks.Select(rook => rook.Position))
			{
				var pos = position;

				if (position.File == 'A')
				{
					for (var i = 0; i < 3; i++)
					{
						pos = Chess.Move.Right(pos);
						if (field.IsFree(pos) == Side.None && !underAttack.Contains(pos)) continue;
						if (result == CastlingSide.Both) result = rooks.Count == 1 ? CastlingSide.Queen : CastlingSide.King;
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
					if (result == CastlingSide.Both) result = rooks.Count == 1 ? CastlingSide.King : CastlingSide.Queen;
					if (result == CastlingSide.King) return CastlingSide.None;
					if (result == CastlingSide.Queen) break;
				}
			}

			return result;
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
			return field.UnderAttack(Side == Side.White ? Side.Black : Side.White).Contains(Position);
		}

		private bool IsInCheck(Field field, Position position)
		{
			return field.UnderAttack(Side == Side.White ? Side.Black : Side.White).Contains(position);
		}
	}
}
