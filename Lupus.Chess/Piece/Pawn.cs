using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lupus.Chess.Interface;

namespace Lupus.Chess.Piece
{
	public class Pawn : AbstractPiece
	{
		public override object Clone()
		{
			return new Pawn()
			{
				Piece = Piece,
				Position = (Position) Position.Clone(),
				Side = Side
			};
		}

		public override IEnumerable<Position> AllowedPositions(Field field)
		{
			throw new NotImplementedException();
		}

		public override IEnumerable<IPiece> StartPieces()
		{
			var result = new Collection<IPiece>();
			var line = new Position()
			{
				File = 'A',
				Rank = 2
			};

			for (var j = 0; j < 2; j++)
			{
				for (var i = 0; i < 8; i++)
				{
					result.Add(new Pawn()
					{
						Piece = PieceType.Pawn,
						Position = line,
						Side = j == 0 ? Side.White : Side.Black
					});

					line.File = (char) (line.File + 1);
				}

				line = new Position()
				{
					File = 'A',
					Rank = 7
				};
			}

			return result;
		}
	}
}
