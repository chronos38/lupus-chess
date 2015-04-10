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
			// TODO: Add checkmate
			var result = new Collection<Position>();
			var position = Chess.Move.Direction((Position) Position.Clone(), Direction.Down);
			if (position.Validate() && field.IsFree(position, Side)) result.Add(position);
			position = Chess.Move.Direction((Position) Position.Clone(), Direction.Left);
			if (position.Validate() && field.IsFree(position, Side)) result.Add(position);
			position = Chess.Move.Direction((Position) Position.Clone(), Direction.LowerLeft);
			if (position.Validate() && field.IsFree(position, Side)) result.Add(position);
			position = Chess.Move.Direction((Position) Position.Clone(), Direction.LowerRight);
			if (position.Validate() && field.IsFree(position, Side)) result.Add(position);
			position = Chess.Move.Direction((Position) Position.Clone(), Direction.Right);
			if (position.Validate() && field.IsFree(position, Side)) result.Add(position);
			position = Chess.Move.Direction((Position) Position.Clone(), Direction.Up);
			if (position.Validate() && field.IsFree(position, Side)) result.Add(position);
			position = Chess.Move.Direction((Position) Position.Clone(), Direction.UpperLeft);
			if (position.Validate() && field.IsFree(position, Side)) result.Add(position);
			position = Chess.Move.Direction((Position) Position.Clone(), Direction.UpperRight);
			if (position.Validate() && field.IsFree(position, Side)) result.Add(position);
			return result;
		}

		public CastlingSide CanUseCastling(Field field)
		{
			// King has not moved
			if (Moved) return CastlingSide.None;
			var pieces = Side == Side.White ? field.WhitePieces : field.BlackPieces;
			// The choosen rooks have not moved
			var rooks = (from piece in pieces where piece.Piece == PieceType.Rook && !piece.Moved select piece);
			// If there are none than castling is not allowed
			if (!rooks.Any()) return CastlingSide.None;
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
	}
}
