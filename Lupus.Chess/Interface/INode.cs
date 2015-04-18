using System.Collections.Generic;

namespace Lupus.Chess.Interface
{
	public interface INode : ICollection<INode>
	{
		/// <summary>
		/// The field of this node.
		/// </summary>
		Field Field { get; }
		/// <summary>
		/// Evaluation value of this node.
		/// </summary>
		long Value { get; set; }
		/// <summary>
		/// true if one of the kings is captured, otherwise false.
		/// </summary>
		bool Terminal { get; set; }
		/// <summary>
		/// Computes all allowed moves.
		/// Complexity is O(n) for n the number of pieces on the field.
		/// </summary>
		/// <returns>Enumerator containing all allowed moves.</returns>
		IEnumerable<Move> AllowedMoves();
		/// <summary>
		/// Computes all moves for given side.
		/// Complexity is O(n) for n the number of pieces for the given side on the field.
		/// </summary>
		/// <param name="fromSide">Either Side.White or Side.Black.</param>
		/// <returns>Enumerator containing all allowed moves.</returns>
		IEnumerable<Move> AllowedMoves(Side fromSide);
		/// <summary>
		/// Computes all possible caputres.
		/// </summary>
		/// <param name="fromSide">Determines that can execute the captures.</param>
		/// <returns>All capture moves.</returns>
		IEnumerable<Move> AvailableCaptures(Side fromSide);
	}
}
