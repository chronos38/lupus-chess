using System;
using System.Collections.Generic;
using System.Deployment.Internal;

namespace Lupus.Chess.Interface
{
	public interface IPiece : ICloneable
	{
		Side Side { get; }
		PieceType Piece { get; }
		Position Position { get; }
		bool Moved { get; }
		/// <summary>
		/// This method is intended for internal usage only. Do not use it.
		/// </summary>
		/// <param name="field"></param>
		/// <param name="position"></param>
		void Move(Field field, Position position);
		/// <summary>
		/// Tries to move the piece to the supplied position.
		/// </summary>
		/// <param name="field">The current field.</param>
		/// <param name="position">The position to move the piece to.</param>
		/// <returns>true if the move was successful, otherwise false.</returns>
		bool TryMove(Field field, Position position);
		ICollection<Position> AllowedPositions(Field field);
		bool ValidateMove(Field field, Position position);
	}
}
