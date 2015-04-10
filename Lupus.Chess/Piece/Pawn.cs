using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Lupus.Chess.Interface;

namespace Lupus.Chess.Piece
{
	public class Pawn : AbstractPiece
	{
		public static IEnumerable<Pawn> White
		{
			get
			{
				var result = new Collection<Pawn>();
				var position = new Position
				{
					File = 'A',
					Rank = 2
				};

				for (var i = 0; i < 8; i++)
				{
					result.Add(new Pawn
					{
						Moved = false,
						Piece = PieceType.Pawn,
						Position = (Position) position.Clone(),
						Side = Side.White
					});

					position.File = (char) (position.File + 1);
				}

				return result;
			}
		}

		public static IEnumerable<Pawn> Black
		{
			get
			{
				var result = new Collection<Pawn>();
				var position = new Position
				{
					File = 'A',
					Rank = 7
				};

				for (var i = 0; i < 8; i++)
				{
					result.Add(new Pawn
					{
						Moved = false,
						Piece = PieceType.Pawn,
						Position = (Position) position.Clone(),
						Side = Side.Black
					});

					position.File = (char) (position.File + 1);
				}

				return result;
			}
		}

		public override object Clone()
		{
			return new Pawn
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

		public static IEnumerable<IPiece> StartPieces()
		{
			var result = new List<IPiece>();
			result.AddRange(White);
			result.AddRange(Black);
			return result;
		}
	}
}
