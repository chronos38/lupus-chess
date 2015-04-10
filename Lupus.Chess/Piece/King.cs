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

		public bool Moved { get; protected set; }

		public override bool TryMove(Field field, Position position)
		{
			Moved = true;
			return base.TryMove(field, position);
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

		public override IEnumerable<Position> AllowedPositions(Field field)
		{
			var result = new Collection<Position>();
			var position = Chess.Move.Direction(Position, Direction.Down);
			if (position.Validate() && field.IsFree(position, Side) && !IsCheckmate(field, position)) result.Add(position);
			position = Chess.Move.Direction(Position, Direction.Left);
			if (position.Validate() && field.IsFree(position, Side) && !IsCheckmate(field, position)) result.Add(position);
			position = Chess.Move.Direction(Position, Direction.LowerLeft);
			if (position.Validate() && field.IsFree(position, Side) && !IsCheckmate(field, position)) result.Add(position);
			position = Chess.Move.Direction(Position, Direction.LowerRight);
			if (position.Validate() && field.IsFree(position, Side) && !IsCheckmate(field, position)) result.Add(position);
			position = Chess.Move.Direction(Position, Direction.Right);
			if (position.Validate() && field.IsFree(position, Side) && !IsCheckmate(field, position)) result.Add(position);
			position = Chess.Move.Direction(Position, Direction.Up);
			if (position.Validate() && field.IsFree(position, Side) && !IsCheckmate(field, position)) result.Add(position);
			position = Chess.Move.Direction(Position, Direction.UpperLeft);
			if (position.Validate() && field.IsFree(position, Side) && !IsCheckmate(field, position)) result.Add(position);
			position = Chess.Move.Direction(Position, Direction.UpperRight);
			if (position.Validate() && field.IsFree(position, Side) && !IsCheckmate(field, position)) result.Add(position);
			return result;
		}

		public CastlingSide CanUseCastling(Field field)
		{
			// Check if king have moved
			if (Moved) return CastlingSide.None;
			// Check if king is in check
			if (IsCheckmate(field, Position)) return CastlingSide.None;
			var pieces = Side == Side.White ? field.WhitePieces : field.BlackPieces;
			// Search for unmoved rooks
			var rooks = (from piece in pieces where piece.Piece == PieceType.Rook && !((Rook) piece).Moved select piece);
			// If there are no rooks available than castling is disallowed
			if (!rooks.Any()) return CastlingSide.None;
			// Check if all fields between rook and king are free
			throw new NotImplementedException();
		}

		public static IEnumerable<IPiece> StartPieces()
		{
			return new Collection<IPiece>
			{
				White,
				Black
			};
		}

		private bool IsCheckmate(Field field, Position position)
		{
			return field.UnderAttack(Side == Side.White ? Side.Black : Side.White).Contains(position);
		}
	}
}
