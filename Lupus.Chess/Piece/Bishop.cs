using System.Collections.Generic;
using System.Collections.ObjectModel;
using Lupus.Chess.Interface;

namespace Lupus.Chess.Piece
{
	public class Bishop : AbstractPiece
	{
		public static IEnumerable<Bishop> White
		{
			get
			{
				return new Collection<Bishop>
				{
					new Bishop
					{
						Moved = false,
						Piece = PieceType.Bishop,
						Side = Side.White,
						Position = new Position
						{
							File = 'C',
							Rank = 1
						}
					},
					new Bishop
					{
						Moved = false,
						Piece = PieceType.Bishop,
						Side = Side.White,
						Position = new Position
						{
							File = 'F',
							Rank = 1
						}
					}
				};
			}
		}

		public static IEnumerable<Bishop> Black
		{
			get
			{
				return new Collection<Bishop>
				{
					new Bishop
					{
						Moved = false,
						Piece = PieceType.Bishop,
						Side = Side.Black,
						Position = new Position
						{
							File = 'C',
							Rank = 8
						}
					},
					new Bishop
					{
						Moved = false,
						Piece = PieceType.Bishop,
						Side = Side.Black,
						Position = new Position
						{
							File = 'F',
							Rank = 8
						}
					}
				};
			}
		}

		public override object Clone()
		{
			return new Bishop
			{
				Moved = Moved,
				Piece = PieceType.Bishop,
				Position = (Position) Position.Clone(),
				Side = Side
			};
		}

		public override IEnumerable<Position> AllowedPositions(Field field)
		{
			var result = new List<Position>();
			result.AddRange(FindPositions(field, Side, Position, 1));
			result.AddRange(FindPositions(field, Side, Position, 3));
			result.AddRange(FindPositions(field, Side, Position, 7));
			result.AddRange(FindPositions(field, Side, Position, 9));
			return result;
		}

		public static IEnumerable<IPiece> StartPieces()
		{
			var result = new List<IPiece>();
			result.AddRange(White);
			result.AddRange(Black);
			return result;
		}
	}
}
