using System;
using System.Collections.Generic;
using System.Deployment.Internal;

namespace Lupus.Chess.Interface
{
	public interface IPiece : ICloneable
	{
		/// <summary>
		/// Either Side.White or Side.Black.
		/// </summary>
		Side Side { get; }
		/// <summary>
		/// What kind of piece is this instance.
		/// </summary>
		PieceType Piece { get; }
		Position Position { get; }
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
		/// <summary>
		/// Returns all allowed moves for this instance.
		/// </summary>
		/// <param name="field">The current field.</param>
		/// <returns>Collection containing all legal moves.</returns>
		ICollection<Move> AllowedMoves(Field field);
		/// <summary>
		/// Returns all possible positions from this instance.
		/// </summary>
		/// <param name="field">The current field.</param>
		/// <returns>Collection containing all legal positions.</returns>
		ICollection<Position> AllowedPositions(Field field);
		/// <summary>
		/// Validates a move if it is possible.
		/// </summary>
		/// <param name="field">The current field.</param>
		/// <param name="position">The position to validate.</param>
		/// <returns>true if the move is valid, otherwise false.</returns>
		bool ValidateMove(Field field, Position position);
	}
}
