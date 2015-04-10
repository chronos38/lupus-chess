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
					Moved = false,
					Side = Side.Black,
					Piece = PieceType.King,
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
					Moved = false,
					Side = Side.White,
					Piece = PieceType.King,
					Position = new Position
					{
						File = 'D',
						Rank = 1
					}
				};
			}
		}

		public override object Clone()
		{
			return new Queen
			{
				Moved = Moved,
				Piece = PieceType.Queen,
				Position = (Position)Position.Clone(),
				Side = Side
			};
		}

		public override IEnumerable<Position> AllowedPositions(Field field)
		{
			var result = new List<Position>();
			result.AddRange(FindPositions(field, Side, Position, 1));
			result.AddRange(FindPositions(field, Side, Position, 2));
			result.AddRange(FindPositions(field, Side, Position, 3));
			result.AddRange(FindPositions(field, Side, Position, 4));
			result.AddRange(FindPositions(field, Side, Position, 6));
			result.AddRange(FindPositions(field, Side, Position, 7));
			result.AddRange(FindPositions(field, Side, Position, 8));
			result.AddRange(FindPositions(field, Side, Position, 9));
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
