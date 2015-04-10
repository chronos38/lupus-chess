using System;
using System.Collections.Generic;

namespace Lupus.Chess.Interface
{
	public interface IPiece : ICloneable
	{
		Side Side { get; }
		PieceType Piece { get; }
		Position Position { get; }
		bool Moved { get; }
		void Move(Field field, Position position);
		bool TryMove(Field field, Position position);
		IEnumerable<Position> AllowedPositions(Field field);
		bool ValidateMove(Field field, Position position);
	}
}
