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
		/// The move that leads to the field.
		/// </summary>
		Move Move { get; }
		/// <summary>
		/// Allowed moves from this node on.
		/// </summary>
		IEnumerable<Move> AllowedMoves { get; } 
		/// <summary>
		/// Evaluation value of this node.
		/// </summary>
		long Value { get; set; }
		/// <summary>
		/// Tree depth of this node.
		/// </summary>
		int Depth { get; set; }
		/// <summary>
		/// true if one of the kings is captured, otherwise false.
		/// </summary>
		bool Terminal { get; set; }
	}
}
