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
			IPiece piece;
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
			IPiece piece;
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
			IPiece piece;
			switch (type)
			{
				case PieceType.Bishop:
					piece = new Bishop(side, position);
					return piece;
				case PieceType.King:
					piece = new King(side, position);
					return piece;
				case PieceType.Knight:
					piece = new Knight(side, position);
					return piece;
				case PieceType.Pawn:
					piece = new Pawn(side, position);
					return piece;
				case PieceType.Queen:
					piece = new Queen(side, position);
					return piece;
				case PieceType.Rook:
					piece = new Rook(side, position);
					return piece;
				default:
					throw new NotSupportedException();
			}
		}

		public static IPiece Create(PieceType type, Side side, Position position, bool moved)
		{
			IPiece piece;
			switch (type)
			{
				case PieceType.Bishop:
					piece = new Bishop(side, position, moved);
					return piece;
				case PieceType.King:
					piece = new King(side, position, moved);
					return piece;
				case PieceType.Knight:
					piece = new Knight(side, position, moved);
					return piece;
				case PieceType.Pawn:
					piece = new Pawn(side, position, moved);
					return piece;
				case PieceType.Queen:
					piece = new Queen(side, position, moved);
					return piece;
				case PieceType.Rook:
					piece = new Rook(side, position, moved);
					return piece;
				default:
					throw new NotSupportedException();
			}
		}
	}
}
