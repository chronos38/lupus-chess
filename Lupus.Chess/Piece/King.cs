using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
			throw new NotImplementedException();
			// TODO: Add castling
			var result = new Collection<Position>();
			var position = Chess.Move.Direction((Position) Position.Clone(), 1);
			if (position.Validate() && field.IsFree(position, Side)) result.Add(position);
			position = Chess.Move.Direction((Position) Position.Clone(), 2);
			if (position.Validate() && field.IsFree(position, Side)) result.Add(position);
			position = Chess.Move.Direction((Position) Position.Clone(), 3);
			if (position.Validate() && field.IsFree(position, Side)) result.Add(position);
			position = Chess.Move.Direction((Position) Position.Clone(), 4);
			if (position.Validate() && field.IsFree(position, Side)) result.Add(position);
			position = Chess.Move.Direction((Position) Position.Clone(), 6);
			if (position.Validate() && field.IsFree(position, Side)) result.Add(position);
			position = Chess.Move.Direction((Position) Position.Clone(), 7);
			if (position.Validate() && field.IsFree(position, Side)) result.Add(position);
			position = Chess.Move.Direction((Position) Position.Clone(), 8);
			if (position.Validate() && field.IsFree(position, Side)) result.Add(position);
			position = Chess.Move.Direction((Position) Position.Clone(), 9);
			if (position.Validate() && field.IsFree(position, Side)) result.Add(position);
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
	}
}
