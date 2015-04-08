using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
				return new King()
				{
					Side = Side.Black,
					Piece = PieceType.King,
					Position = new Position()
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
				return new King()
				{
					Side = Side.Black,
					Piece = PieceType.King,
					Position = new Position()
					{
						File = 'E',
						Rank = 1
					}
				};
			}
		}

		public override object Clone()
		{
			return new King()
			{
				Piece = PieceType.King,
				Position = (Position)Position.Clone(),
				Side = Side
			};
		}

		public override ICollection<Position> AllowedPositions(Field field)
		{
			// TODO: If position is in checkmate than do not add it
			var result = new Collection<Position>();
			var position = Chess.Move.Direction(Position, 1);
			if (position.Validate()) result.Add(position);
			position = Chess.Move.Direction(Position, 2);
			if (position.Validate()) result.Add(position);
			position = Chess.Move.Direction(Position, 3);
			if (position.Validate()) result.Add(position);
			position = Chess.Move.Direction(Position, 4);
			if (position.Validate()) result.Add(position);
			position = Chess.Move.Direction(Position, 6);
			if (position.Validate()) result.Add(position);
			position = Chess.Move.Direction(Position, 7);
			if (position.Validate()) result.Add(position);
			position = Chess.Move.Direction(Position, 8);
			if (position.Validate()) result.Add(position);
			position = Chess.Move.Direction(Position, 9);
			if (position.Validate()) result.Add(position);
			return result;
		}
	}
}
