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

		internal Bishop()
		{
			Piece = PieceType.Bishop;
		}

		internal Bishop(Side side, Position position)
		{
			Piece = PieceType.Bishop;
			Side = side;
			Position = position;
		}

		public override object Clone()
		{
			return new Bishop
			{
				Piece = PieceType.Bishop,
				Position = (Position) Position.Clone(),
				Side = Side
			};
		}

		public override IEnumerable<Position> AllowedPositions(Field field)
		{
			var result = new List<Position>();
			result.AddRange(FindPositions(field, Side, Position, Direction.LowerLeft));
			result.AddRange(FindPositions(field, Side, Position, Direction.LowerRight));
			result.AddRange(FindPositions(field, Side, Position, Direction.UpperLeft));
			result.AddRange(FindPositions(field, Side, Position, Direction.UpperRight));
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
