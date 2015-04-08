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
		private static readonly IDictionary<PieceType, int> LookupTable = new Dictionary<PieceType, int>
		{
			{ PieceType.Queen, 10000 },
			{ PieceType.Bishop, 350 },
			{ PieceType.Knight, 350 },
			{ PieceType.Rook, 525 },
			{ PieceType.Pawn, 100 }
		};

		public int Execute(IField field, Side side)
		{
			switch (side)
			{
				case Side.Black:
					return Execute(field.BlackPieces) - Execute(field.WhitePieces);
				case Side.White:
					return Execute(field.WhitePieces) - Execute(field.BlackPieces);
			}

			return 0;
		}

		public int Execute(IEnumerable<IPiece> pieces)
		{
			var bishopCount = 0;
			var result = 0;

			foreach (var piece in pieces)
			{
				switch (piece.Piece)
				{
					case PieceType.King:
						continue;
					case PieceType.Bishop:
						bishopCount += 1;
						break;
				}

				result += LookupTable[piece.Piece];
			}

			return result + (bishopCount >= 2 ? 50 : 0);
		}
	}
}
