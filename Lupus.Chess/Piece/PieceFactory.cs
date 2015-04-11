using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lupus.Chess.Interface;

namespace Lupus.Chess.Piece
{
	public static class PieceFactory
	{
		public static IPiece Create(PieceType type)
		{
			switch (type)
			{
				case PieceType.Bishop:
					return new Bishop(Side.White, new Position { File = 'D', Rank = 5 });
				case PieceType.King:
					return new King(Side.White, new Position { File = 'F', Rank = 5 });
				case PieceType.Knight:
					return new Knight(Side.White, new Position { File = 'D', Rank = 4 });
				case PieceType.Pawn:
					return new Pawn(Side.White, new Position { File = 'F', Rank = 2 });
				case PieceType.Queen:
					return new Queen(Side.White, new Position { File = 'D', Rank = 4 });
				case PieceType.Rook:
					return new Rook(Side.White, new Position { File = 'D', Rank = 5 });
				default:
					throw new NotSupportedException();
			}
		}

		public static IPiece Create(PieceType type, Side side)
		{
			switch (type)
			{
				case PieceType.Bishop:
					return new Bishop(side, new Position {File = 'D', Rank = 5});
				case PieceType.King:
					return new King(side, new Position { File = 'F', Rank = 5 });
				case PieceType.Knight:
					return new Knight(side, new Position { File = 'D', Rank = 4 });
				case PieceType.Pawn:
					return new Pawn(side, new Position { File = 'F', Rank = 2 });
				case PieceType.Queen:
					return new Queen(side, new Position { File = 'D', Rank = 4 });
				case PieceType.Rook:
					return new Rook(side, new Position { File = 'D', Rank = 5 });
				default:
					throw new NotSupportedException();
			}
		}

		public static IPiece Create(PieceType type, Side side, Position position)
		{
			switch (type)
			{
				case PieceType.Bishop:
					return new Bishop(side, position);
				case PieceType.King:
					return new King(side, position);
				case PieceType.Knight:
					return new Knight(side, position);
				case PieceType.Pawn:
					return new Pawn(side, position);
				case PieceType.Queen:
					return new Queen(side, position);
				case PieceType.Rook:
					return new Rook(side, position);
				default:
					throw new NotSupportedException();
			}
		}

		public static IPiece Create(PieceType type, Side side, Position position, bool moved)
		{
			switch (type)
			{
				case PieceType.Bishop:
					return new Bishop(side, position, moved);
				case PieceType.King:
					return new King(side, position, moved);
				case PieceType.Knight:
					return new Knight(side, position, moved);
				case PieceType.Pawn:
					return new Pawn(side, position, moved);
				case PieceType.Queen:
					return new Queen(side, position, moved);
				case PieceType.Rook:
					return new Rook(side, position, moved);
				default:
					throw new NotSupportedException();
			}
		}
	}
}
