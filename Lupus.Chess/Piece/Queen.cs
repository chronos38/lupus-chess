using System;
using System.Collections.Generic;
using System.Linq;
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
				return new Queen()
				{
					Side = Side.Black,
					Piece = PieceType.King,
					Position = new Position()
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
				return new Queen()
				{
					Side = Side.White,
					Piece = PieceType.King,
					Position = new Position()
					{
						File = 'D',
						Rank = 1
					}
				};
			}
		}

		public override object Clone()
		{
			return new Queen()
			{
				Piece = PieceType.Queen,
				Position = Position,
				Side = Side
			};
		}

		public override IEnumerable<Position> AllowedPositions(Field field)
		{
			throw new NotImplementedException();
		}

		public override IEnumerable<IPiece> StartPieces()
		{
			throw new NotImplementedException();
		}
	}
}
