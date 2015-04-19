using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using Lupus.Chess.Interface;

namespace Lupus.Chess.Piece
{
	public class Queen : AbstractPiece
	{
		public static Queen Black
		{
			get
			{
				return new Queen
				{
					Side = Side.Black,
					Piece = PieceType.Queen,
					Position = new Position
					{
						File = 'D',
						Rank = 8
					}
				};
			}
		}

		public static Queen White
		{
			get
			{
				return new Queen
				{
					Side = Side.White,
					Piece = PieceType.Queen,
					Position = new Position
					{
						File = 'D',
						Rank = 1
					}
				};
			}
		}

		internal Queen()
		{
			Piece = PieceType.Queen;
		}

		internal Queen(Side side, Position position)
		{
			Piece = PieceType.Queen;
			Side = side;
			Position = position;
		}

		internal Queen(Side side, Position position, bool moved)
		{
			Piece = PieceType.Queen;
			Side = side;
			Position = position;
		}

		public override object Clone()
		{
			return new Queen
			{
				Piece = PieceType.Queen,
				Position = (Position)Position.Clone(),
				Side = Side
			};
		}

		public override IEnumerable<Position> AllowedPositions(Field field)
		{
			var result = new List<Position>();
			result.AddRange(FindPositions(field, Side, Position, Direction.Down));
			result.AddRange(FindPositions(field, Side, Position, Direction.Left));
			result.AddRange(FindPositions(field, Side, Position, Direction.LowerLeft));
			result.AddRange(FindPositions(field, Side, Position, Direction.LowerRight));
			result.AddRange(FindPositions(field, Side, Position, Direction.Right));
			result.AddRange(FindPositions(field, Side, Position, Direction.Up));
			result.AddRange(FindPositions(field, Side, Position, Direction.UpperLeft));
			result.AddRange(FindPositions(field, Side, Position, Direction.UpperRight));
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
