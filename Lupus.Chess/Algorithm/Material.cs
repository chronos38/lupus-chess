using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lupus.Chess.Interface;
using Lupus.Chess.Interface.Algorithm;

namespace Lupus.Chess.Algorithm
{
	public class Material : IMaterial
	{
		private static readonly IDictionary<Piece, int> LookupTable = new Dictionary<Piece, int>
		{
			{ Piece.Queen, 10000 },
			{ Piece.Bishop, 350 },
			{ Piece.Knight, 350 },
			{ Piece.Rook, 525 },
			{ Piece.Pawn, 100 }
		};

		public int Execute(IField field)
		{
			return Execute(field.BlackPieces) - Execute(field.WhitePieces);
		}

		public int Execute(IField field, Side side)
		{
			return Execute(field)*(side == Side.Black ? 1 : -1);
		}

		public int Execute(IEnumerable<IPiece> pieces)
		{
			var bishopCount = 0;
			var result = 0;

			foreach (var piece in pieces)
			{
				switch (piece.Piece)
				{
					case Piece.King:
						continue;
					case Piece.Bishop:
						bishopCount += 1;
						break;
				}

				result += LookupTable[piece.Piece];
			}

			return result + (bishopCount >= 2 ? 50 : 0);
		}
	}
}
