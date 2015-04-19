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
		/// <summary>
		/// Position of this piece.
		/// </summary>
		Position Position { get; set; }
		/// <summary>
		/// Inject the previous moves for allowed moves computation.
		/// </summary>
		History PastMoves { get; set; }
		/// <summary>
		/// This method is intended for internal usage only. Do not use it.
		/// </summary>
		/// <param name="field">The current field.</param>
		/// <param name="move">The move to execute.</param>
		void Move(Field field, Move move);
		/// <summary>
		/// Tries to execute the given move.
		/// </summary>
		/// <param name="field">The current field.</param>
		/// <param name="move">The move to execute.</param>
		/// <returns>true if move was successful, otherwise false.</returns>
		bool TryMove(Field field, Move move);
		/// <summary>
		/// Returns all allowed moves for this instance.
		/// </summary>
		/// <param name="field">The current field.</param>
		/// <returns>Collection containing all legal moves.</returns>
		IEnumerable<Move> AllowedMoves(Field field);
		/// <summary>
		/// Returns all possible positions from this instance.
		/// </summary>
		/// <param name="field">The current field.</param>
		/// <returns>Collection containing all legal positions.</returns>
		IEnumerable<Position> AllowedPositions(Field field);
		/// <summary>
		/// Validates a move if it is possible.
		/// </summary>
		/// <param name="field">The current field.</param>
		/// <param name="move">The move to validate.</param>
		/// <returns>true if the move is valid, otherwise false.</returns>
		bool ValidateMove(Field field, Move move);
	}
}
