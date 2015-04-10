using System.Collections.Generic;
using System.Collections.ObjectModel;
using Lupus.Chess.Interface;

namespace Lupus.Chess.Piece
{
	public class Rook : AbstractPiece
	{
		public static IEnumerable<Rook> White
		{
			get
			{
				return new Collection<Rook>
				{
					new Rook
					{
						Moved = false,
						Piece = PieceType.Rook,
						Side = Side.White,
						Position = new Position
						{
							File = 'A',
							Rank = 1
						}
					},
					new Rook
					{
						Moved = false,
						Piece = PieceType.Rook,
						Side = Side.White,
						Position = new Position
						{
							File = 'H',
							Rank = 1
						}
					}
				};
			}
		}

		public static IEnumerable<Rook> Black
		{
			get
			{
				return new Collection<Rook>
				{
					new Rook
					{
						Moved = false,
						Piece = PieceType.Rook,
						Side = Side.Black,
						Position = new Position
						{
							File = 'A',
							Rank = 8
						}
					},
					new Rook
					{
						Moved = false,
						Piece = PieceType.Rook,
						Side = Side.Black,
						Position = new Position
						{
							File = 'H',
							Rank = 8
						}
					}
				};
			}
		}

		internal Rook()
		{
			Piece = PieceType.Rook;
		}

		internal Rook(Side side, Position position)
		{
			Piece = PieceType.Rook;
			Side = side;
			Position = position;
		}

		internal Rook(Side side, Position position, bool moved)
		{
			Moved = moved;
			Piece = PieceType.Rook;
			Side = side;
			Position = position;
		}

		public override object Clone()
		{
			return new Rook
			{
				Moved = Moved,
				Piece = PieceType.Rook,
				Position = (Position) Position.Clone(),
				Side = Side
			};
		}

		public override IEnumerable<Position> AllowedPositions(Field field)
		{
			var result = new List<Position>();
			result.AddRange(FindPositions(field, Side, Position, Direction.Down));
			result.AddRange(FindPositions(field, Side, Position, Direction.Left));
			result.AddRange(FindPositions(field, Side, Position, Direction.Right));
			result.AddRange(FindPositions(field, Side, Position, Direction.Up));
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
